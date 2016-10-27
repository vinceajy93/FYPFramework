using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Overlay_Control : MonoBehaviour
{

	public GameObject panel;
	private Text p1Timer, p2Timer;
	[SerializeField]
	private Text roundTimer_P1, roundTimer_P2;
	private float setTimerTime = 0.5f; //testing set 0.5f, actual 3.0f
	public float countdownTimer, inGameCountdownTimer;
	//change this time for debugging
	public bool PanelisActive = true;
	private bool P1Touched, P2Touched = false;

	private PauseScript _PauseScript;
	private Level_Control _Level_Control;

	private HealthManager _HealthManager;

	// Use this for initialization
	void Start ()
	{

		//start the timer
		countdownTimer = setTimerTime;
		inGameCountdownTimer = PlayerPrefs.GetInt ("time");
		_PauseScript = GameObject.Find ("Scripts").GetComponent<PauseScript> ();


		//single player
		if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
			p1Timer = GameObject.Find ("P1 Timer").GetComponent<Text> ();
			p1Timer.text = ("Player 1 place finger here!");

			//set the time for the round to be player setting
			roundTimer_P1.text = inGameCountdownTimer.ToString ();
			roundTimer_P1.GetComponent<Text> ().enabled = false;

		} else {
			//multiplayer
			p1Timer = GameObject.Find ("P1 Timer").GetComponent<Text> ();
			p2Timer = GameObject.Find ("P2 Timer").GetComponent<Text> ();

			p1Timer.text = ("Player 1 place finger here!");
			p2Timer.text = ("Player 2 place finger here!");

			//set the time for the round to be player setting
			roundTimer_P1.text = inGameCountdownTimer.ToString ();
			roundTimer_P1.GetComponent<Text> ().enabled = false;

			roundTimer_P2.text = roundTimer_P1.text;
			roundTimer_P2.GetComponent<Text> ().enabled = false;

		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//start countdown timer only when finger is on screen
		CheckPress ();

		//bring the overlay back to game if game is paused
		if (_PauseScript.Paused == true) {
			PanelisActive = true;
		}
		//when game is not paused
		else {
			
			//animation of the timer//
			float roundTimer_P1_x = roundTimer_P1.transform.localScale.x;
			float roundTimer_P1_y = roundTimer_P1.transform.localScale.y;

			//p1 roundtimer resizing
			if (roundTimer_P1_y >= 0.7f) //either x or y will work
				roundTimer_P1.transform.localScale = new Vector2 (roundTimer_P1_x -= Time.deltaTime, roundTimer_P1_y -= Time.deltaTime);

			//update the time for roundTimers
			inGameCountdownTimer -= Time.deltaTime;
			roundTimer_P1.text = inGameCountdownTimer.ToString ("f0");

			if (!gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
				float roundTimer_P2_x = roundTimer_P2.transform.localScale.x;
				float roundTimer_P2_y = roundTimer_P2.transform.localScale.y;

				//p2 roundtimer resizing
				if (roundTimer_P2_y >= 0.7f)
					roundTimer_P2.transform.localScale = new Vector2 (roundTimer_P2_x -= Time.deltaTime, roundTimer_P2_y -= Time.deltaTime);
				
				roundTimer_P2.text = roundTimer_P1.text;
			}
			//when the round has ended
			if (inGameCountdownTimer <= 0) {
				//_Level_Control.loadLocal2P ();
				//for testing, show who win who lose first before going to loadout page
				//SceneManager.LoadScene ("Stage_Select");
			}
		}
			
		//single player
		if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
			if (P1Touched == true) {
				Debug.Log ("p1 timer" + countdownTimer);
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
				//Debug.Log ("entered");
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
			//single player
			if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
				//Set booleans back to false
				P1Touched = false;

				//Set font size back to 50
				p1Timer.GetComponent<Text> ().fontSize = 50;

				//Set roundtimer text to visible
				roundTimer_P1.GetComponent<Text> ().enabled = true;
			} 

			// multiplayer
			else {
				//Set booleans back to false
				P1Touched = false;
				P2Touched = false;

				//Set font size back to 50
				p1Timer.GetComponent<Text> ().fontSize = 50;
				p2Timer.GetComponent<Text> ().fontSize = 50;

				//Set roundtimer text to visible
				roundTimer_P1.GetComponent<Text> ().enabled = true;
				roundTimer_P2.GetComponent<Text> ().enabled = true;
			}

			//Deactivate the pause mode
			_PauseScript.Paused = false;

			//Set panel to inactive
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
