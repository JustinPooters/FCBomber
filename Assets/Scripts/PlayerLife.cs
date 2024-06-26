﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLife : NetworkBehaviour {

	[SerializeField]
	private int numberOfLives = 3;

	[SerializeField]
	private float invulnerabilityDuration = 2;

	private bool isInvulnerable = false;

	[SerializeField]
	private GameObject playerLifeImage;

	private List<GameObject> lifeImages;

	private GameObject gameOverPanel;
    

	private Vector2 initialPosition;
	private int initialNumberOfLives;

    public void ShowGameOverScreen(bool showGameOver)
    {
        this.gameOverPanel.SetActive(showGameOver);
    }
    
    IEnumerator FindGameOverPanel()
    {
        while(this.gameOverPanel == null)
        {
            this.gameOverPanel = GameObject.Find("GameOverPanel");
            yield return new WaitForSeconds(Time.deltaTime * 2);
        }
    }
	void Start() {	
		if (this.isLocalPlayer) {
			this.initialPosition = this.transform.position;
			this.initialNumberOfLives = this.numberOfLives;

            //this.gameOverPanel = GameObject.Find ("GameOverPanel");
            StartCoroutine(FindGameOverPanel());
            ShowGameOverScreen(false);



            GameObject playerLivesGrid = GameObject.Find ("PlayerLivesGrid");

			this.lifeImages = new List<GameObject> ();
            resetLife(playerLivesGrid);
        }
    }

	public void LoseLife() {
		if (!this.isInvulnerable && this.isLocalPlayer) {
			this.numberOfLives--;
			GameObject lifeImage = this.lifeImages [this.lifeImages.Count - 1];
			Destroy (lifeImage);
			this.lifeImages.RemoveAt (this.lifeImages.Count - 1);
            if (this.numberOfLives <=0) {
                //broken garbage
                //this.gameOverPanel.SetActive(true);
                MultiplayerMatch.instance.EndMatch();
                SceneManager.LoadScene("Title");
            }
			this.isInvulnerable = true;
			Invoke ("BecomeVulnerable", this.invulnerabilityDuration); 
		}
	}


	private void BecomeVulnerable() {
		this.isInvulnerable = false;
	}

	void Respawn()
    {
        this.numberOfLives = this.initialNumberOfLives;

        GameObject playerLivesGrid = GameObject.Find("PlayerLivesGrid");

        this.lifeImages = new List<GameObject>();
        resetLife(playerLivesGrid);

        this.transform.position = this.initialPosition;
    }

    private void resetLife(GameObject playerLivesGrid)
    {
        for (int lifeIndex = 0; lifeIndex < this.numberOfLives; ++lifeIndex)
        {
            GameObject lifeImage = Instantiate(playerLifeImage, playerLivesGrid.transform) as GameObject;
            this.lifeImages.Add(lifeImage);
        }
    }
}   