using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Overlay_Control : MonoBehaviour
{

	private GameObject panel;
	private Text p1Timer;
	private Text p2Timer;
	float countdownTimer = 0.5f; //change this time for debugging
	public bool PanelisActive = true;
	private bool P1Touched, P2Touched = false;

	// Use this for initialization
	void Start ()
	{
		panel = GameObject.Find ("panel overlay");

		//single player
		if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
			p1Timer = GameObject.Find ("P1 Timer").GetComponent<Text> ();
			p1Timer.text = ("Player 1 place finger here!");
		} else {
			//multiplayer
			p1Timer = GameObject.Find ("P1 Timer").GetComponent<Text> ();
			p2Timer = GameObject.Find ("P2 Timer").GetComponent<Text> ();

			p1Timer.text = ("Player 1 place finger here!");
			p2Timer.text = ("Player 2 place finger here!");
		}




	}
	
	// Update is called once per frame
	void Update ()
	{
		//start countdown timer only when finger is on screen
		CheckPress ();

		//single player
		if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
			if (P1Touched == true) {
			
				//countdown timer
				countdownTimer -= Time.deltaTime;
				//Set Panel to false when timer hits 0
				if (countdownTimer <= 0)
					PanelisActive = false;

				Panel ();
			}
			//multiplayer (2P)
		} else {
			if (P1Touched == true && P2Touched == true) {
				
				//countdown timer
				countdownTimer -= Time.deltaTime;
				//Set Panel to false when timer hits 0
				if (countdownTimer <= 0)
					PanelisActive = false;

				Panel ();
			}
		}
	}

	void Panel ()
	{
		if (PanelisActive == true) {
			//single player
			if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
				p1Timer.GetComponent<Text> ().fontSize = 120;
				p1Timer.text = countdownTimer.ToString ("f0");

			}

			//multiplayer
			else {
				//change the font size
				p1Timer.GetComponent<Text> ().fontSize = 120;
				p2Timer.GetComponent<Text> ().fontSize = 120;

				p1Timer.text = countdownTimer.ToString ("f0");
				p2Timer.text = p1Timer.text;
			}
		} else {
			panel.SetActive (false);
		}
				
	}

	void CheckPress ()
	{

		int nbTouches = Input.touchCount;

		if (nbTouches > 0) {
			for (int i = 0; i < nbTouches; i++) {
				Touch touch = Input.GetTouch (i);

				TouchPhase phase = touch.phase;

				switch (phase) {
				case TouchPhase.Began:
					Vector2 touchPosition = Camera.main.ScreenToWorldPoint (touch.position);

						//single player
					if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
						
						//player 1 touched
						P1Touched = true;
						p1Timer.text = ("waiting for player 2!");
					}
					//multiplayer
					else {
						//player 1 touched
						if (touchPosition.y < 0) {
							P1Touched = true;
							p1Timer.text = ("waiting for player 2!");
						}
								
						//player 2 touched
						if (touchPosition.y > 0) {
							P2Touched = true;
							p2Timer.text = ("waiting for player 1!");
						}
					}

					break;
					
				case TouchPhase.Ended: // Release touch from screen

						//single player
					if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
						p1Timer.text = ("Place finger here to start!");

						if (P1Touched) {
						} else { //not touching 
							P1Touched = false;
						}
					}
						//multiplayer
						else {
						p1Timer.text = ("Player 1 place finger here!");
						p2Timer.text = ("Player 2 place finger here!");

						if (P1Touched && P2Touched) {
						} else { //either one not touching 
							P1Touched = false;
							P2Touched = false;
						}
					}

					break;
				}
			}
		}

		if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
			if (Input.GetMouseButtonDown (0)) {
				P1Touched = true;
			}
		}
	}
}
