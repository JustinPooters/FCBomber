using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerMovement : NetworkBehaviour {

	[SerializeField]
	private float speed;

	[SerializeField]
	private Animator animatorOne;

    [SerializeField]
    private Animator animatorTwo;

    public override void OnStartLocalPlayer() {
	    GetComponent<SpriteRenderer> ().color = Color.blue;
    }

    private PlayerLife pLife;

    private void Start()
    {
        pLife = this.GetComponent<PlayerLife>();
    }

    void FixedUpdate () {
		if (this.isLocalPlayer) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
        
            Vector2 currentVelocity = gameObject.GetComponent<Rigidbody2D> ().velocity;

			float newVelocityX = 0f;
			if (moveHorizontal < 0 && currentVelocity.x <= 0) {
				newVelocityX = -speed;
				animatorOne.SetInteger ("DirectionX", -1);
			} else if (moveHorizontal > 0 && currentVelocity.x >= 0) {
				newVelocityX = speed;
				animatorOne.SetInteger ("DirectionX", 1);
			} else {
				animatorOne.SetInteger ("DirectionX", 0);
			}
            
            float newVelocityY = 0f;
			if (moveVertical < 0 && currentVelocity.y <= 0) {
				newVelocityY = -speed;
				animatorOne.SetInteger ("DirectionY", -1);
			} else if (moveVertical > 0 && currentVelocity.y >= 0) {
				newVelocityY = speed;
				animatorOne.SetInteger ("DirectionY", 1);
			} else {
				animatorOne.SetInteger ("DirectionY", 0);
			}
            
            gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (newVelocityX, newVelocityY);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "FC")
        {
            Debug.Log("You won!"); //Insert eigen win conditie
            pLife.ShowGameOverScreen(true);
        }
        
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.transform.tag == "Explosion")
    //    {
    //        Debug.Log("dead motherfucker"); // :)

    //        //close network sessie/stuff here
    //        SceneManager.LoadScene(0);
    //    }
    //}

}
