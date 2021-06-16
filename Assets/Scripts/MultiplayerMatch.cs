﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MultiplayerMatch : MonoBehaviour {

    public static MultiplayerMatch instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(instance.gameObject);
    }

    [SerializeField]
	private NetworkManager networkManager;

	public bool isHost = false;

	// Use this for initialization
	void Start () {	
		SceneManager.sceneLoaded += OnSceneLoaded;

		networkManager.StartMatchMaker ();
	}
	
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (scene.name == "Battle") {
			SceneManager.sceneLoaded -= OnSceneLoaded;
			if (this.isHost) {
				networkManager.StartHost ();
                Debug.Log("Host started");
			} else {
				networkManager.StartClient ();
			}
		}
	}

	public void CreateMatch() {
		networkManager.matchMaker.CreateMatch ("match", 2, true, string.Empty, string.Empty, string.Empty, 0, 0, OnMatchCreate);
        Debug.Log("well. It started");
	}

	void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
		this.isHost = true;
		SceneManager.LoadScene ("Battle");
        Debug.Log("Match Created");
	}

	public void JoinMatch() {
		networkManager.matchMaker.ListMatches (0, 1, string.Empty, true, 0, 0, OnMatchList);
	}

	void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
		networkManager.matchMaker.JoinMatch (matches [0].networkId, string.Empty, string.Empty, string.Empty, 0, 0, OnMatchJoined);
	}

	void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo) {
		SceneManager.LoadScene ("Battle");
	}
    public void EndMatch()
    {
        networkManager.StopHost();
    }
}
