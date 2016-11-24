using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Overlay_Control : MonoBehaviour
{

	public GameObject panel;
	private Text p1OverlayText, p2OverlayText;
	[SerializeField]
	private Text roundTimer_P1, roundTimer_P2;
	private float setTimerTime = 0.5f; //testing set 0.5f, actual 3.0f
	public float countdownTimer, inGameCountdownTimer, nextroundTimer;
	//change this time for debugging
	public bool PanelisActive = true;
	private bool P1Touched, P2Touched, roundEnded = false;

	//non serialized field
	private PauseScript _PauseScript;
	private Level_Control _Level_Control;
	private HealthManager _HealthManager;
	private RoundsIndicator _roundsIndicator;
	private string tempP1Text, tempP2Text;
	private int roundsPassed;

	// Use this for initialization
	void Start ()
	{
		//get the numberof rounds passed form player pref then save it to an int
		if(PlayerPrefs.HasKey("roundsPassed"))
			roundsPassed = PlayerPrefs.GetInt("roundsPassed");

		//Set the overlay panel to be active if its inactive
		if (!panel.activeSelf)
			panel.SetActive (true);
		_roundsIndicator = GetComponent<RoundsIndicator> ();

		//start the timer
		countdownTimer = setTimerTime;
		inGameCountdownTimer = PlayerPrefs.GetInt ("time");
		_PauseScript = GetComponent<PauseScript> ();

		//initialize the health manager script
		_HealthManager = GetComponent<HealthManager>();

		//initialize the level control script
		_Level_Control = GetComponent<Level_Control>();

		//single player
		if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
			p1OverlayText = GameObject.Find ("P1 Timer").GetComponent<Text> ();
			p1OverlayText.text = ("Place your finger here!");

			//set the time for the round to be player setting
			roundTimer_P1.text = inGameCountdownTimer.ToString ();
			roundTimer_P1.GetComponent<Text> ().enabled = false;

		} else {
			//multiplayer
			p1OverlayText = GameObject.Find ("P1 OverlayText").GetComponent<Text> ();
			p2OverlayText = GameObject.Find ("P2 OverlayText").GetComponent<Text> ();

			p1OverlayText.text = ("Round: "  + roundsPassed + "\n Player 1 place finger here!");
			p2OverlayText.text = ("Round: "  + roundsPassed + "\n Player 2 place finger here!");

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

		//start the next round timer when the round concluded 
		if (nextroundTimer != 0.0f) {
			//Set the overlay text to display who won and next round starting time
			p1OverlayText.text = (tempP1Text + " round begins in: " + nextroundTimer.ToString("f0"));
			p2OverlayText.text = (tempP2Text + " round begins in: " + nextroundTimer.ToString ("f0"));

			//decrease/start the timer with time.deltatime
			nextroundTimer -= Time.deltaTime;

			//Set Panel to false when timer hits 0
			if (nextroundTimer <= 0) {
				//Add 1 to the current roundsPassed count, then update it in the player prefs
				roundsPassed += 1;

				PlayerPrefs.SetInt ("roundsPassed", roundsPassed);
				var tempRound = PlayerPrefs.GetInt ("rounds");

				//if player neither players have hit the amount of rounds require to win the game, send them back to loadout selection
				if (PlayerPrefs.GetInt ("roundWon_P1") < tempRound && PlayerPrefs.GetInt ("roundWon_P2") < tempRound) {
					_Level_Control.loadLoadout2pSelect ();
				} else {
					//goes to win page
					_Level_Control.loadLoseWinScene();
				}

			}
		}

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
			//Set the ingamecountdowntimer to not go under 0
			if (inGameCountdownTimer < 0)
				inGameCountdownTimer = 0;
			
			roundTimer_P1.text = inGameCountdownTimer.ToString ("f0");

			if (!gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
				float roundTimer_P2_x = roundTimer_P2.transform.localScale.x;
				float roundTimer_P2_y = roundTimer_P2.transform.localScale.y;

				//p2 roundtimer resizing
				if (roundTimer_P2_y >= 0.7f)
					roundTimer_P2.transform.localScale = new Vector2 (roundTimer_P2_x -= Time.deltaTime, roundTimer_P2_y -= Time.deltaTime);
				
				roundTimer_P2.text = roundTimer_P1.text;
			}

			//when player is dead before round end
			if (_HealthManager.P1Health.CurrentVal <= 0 && roundEnded == false) {
				//P1 loses
				roundEnded = true;

				if (PlayerPrefs.GetInt ("roundWon_P1") < PlayerPrefs.GetInt ("rounds")) {
					_roundsIndicator.roundWon_P1 = PlayerPrefs.GetInt ("roundWon_P1") + 1;
					PlayerPrefs.SetInt ("roundWon_P1", _roundsIndicator.roundWon_P1);

					//display round winner then change back to loadout select screen after intended time
					panel.SetActive (true);
					tempP1Text = ("YOU WON!");
					tempP2Text = ("YOU LOSE!");

					nextroundTimer = 3.0f; //setTimerTime;
				}
				//when the player won the number of rounds set
				else {
					nextroundTimer = 3.0f; //setTimerTime;
					_Level_Control.loadLoseWinScene ();
				}
			} else if (_HealthManager.P2Health.CurrentVal <= 0 && roundEnded == false) {
				//P2 loses
				roundEnded = true;

				//plus the rounds if not end of rounds set
				if (PlayerPrefs.GetInt ("roundWon_P2") < PlayerPrefs.GetInt ("rounds")) {
					_roundsIndicator.roundWon_P1 = PlayerPrefs.GetInt ("roundWon_P2") + 1;
					PlayerPrefs.SetInt ("roundWon_P2", _roundsIndicator.roundWon_P1);

					//display round winner then change back to loadout select screen after intended time
					panel.SetActive (true);
					tempP1Text = ("YOU LOSE!");
					tempP2Text = ("YOU WIN!");

					nextroundTimer = 3.0f; //setTimerTime;

				}
				//when the player won the number of rounds set
				else {
					nextroundTimer = 3.0f; //setTimerTime;
					_Level_Control.loadLoseWinScene ();
				}
			}
				
			//when the round has ended
			if ( (int)inGameCountdownTimer == 0.0f && roundEnded == false) {
				roundEnded = true;

				//check which player has more health, that player wins the round

				//if player 1 health > player 2
				if (_HealthManager.P1Health.CurrentVal > _HealthManager.P2Health.CurrentVal) {
					//plus the rounds if not end of rounds set
					if (PlayerPrefs.GetInt ("roundWon_P1") < PlayerPrefs.GetInt ("rounds")) {
						_roundsIndicator.roundWon_P1 = PlayerPrefs.GetInt ("roundWon_P1") + 1;
						PlayerPrefs.SetInt ("roundWon_P1", _roundsIndicator.roundWon_P1);

						//display round winner then change back to loadout select screen after intended time
						panel.SetActive (true);
						tempP1Text = ("YOU WON!");
						tempP2Text = ("YOU LOSE!");

						nextroundTimer = 3.0f; //setTimerTime;
					}
					//when the player won the number of rounds set
					else {
						nextroundTimer = 3.0f; //setTimerTime;
						_Level_Control.loadLoseWinScene ();
					}

				}
				//if player 2 health > player 1
				else if (_HealthManager.P2Health.CurrentVal > _HealthManager.P1Health.CurrentVal) {
					//plus the rounds if not end of rounds set
					if (PlayerPrefs.GetInt ("roundWon_P2") < PlayerPrefs.GetInt ("rounds")) {
						_roundsIndicator.roundWon_P1 = PlayerPrefs.GetInt ("roundWon_P2") + 1;
						PlayerPrefs.SetInt ("roundWon_P2", _roundsIndicator.roundWon_P1);

						//display round winner then change back to loadout select screen after intended time
						panel.SetActive (true);
						tempP1Text = ("YOU LOSE!");
						tempP2Text = ("YOU WIN!");

						nextroundTimer = 3.0f; //setTimerTime;

					}
					//when the player won the number of rounds set
					else {
						nextroundTimer = 3.0f; //setTimerTime;
						_Level_Control.loadLoseWinScene ();
					}

				}
				//same health at the end of the round
				else {
					//ends the round, no player gets a score
					panel.SetActive(true);
					tempP1Text = ("DRAW!");
					tempP2Text = ("DRAW!");
					nextroundTimer = 3.0f;
				}
			}
		}
			
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
				p1OverlayText.GetComponent<Text> ().fontSize = 120;
				p1OverlayText.text = countdownTimer.ToString ("f0");
			}

			//multiplayer
			else {
				//change the font size
				p1OverlayText.GetComponent<Text> ().fontSize = 120;
				p2OverlayText.GetComponent<Text> ().fontSize = 120;

				p1OverlayText.text = countdownTimer.ToString ("f0");
				p2OverlayText.text = p1OverlayText.text;
			}
		} else {
			//single player
			if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
				//Set booleans back to false
				P1Touched = false;

				//Set font size back to 50
				p1OverlayText.GetComponent<Text> ().fontSize = 50;

				//Set roundtimer text to visible
				roundTimer_P1.GetComponent<Text> ().enabled = true;
			} 

			// multiplayer
			else {
				//Set booleans back to false
				P1Touched = false;
				P2Touched = false;

				//Set font size back to 50
				p1OverlayText.GetComponent<Text> ().fontSize = 50;
				p2OverlayText.GetComponent<Text> ().fontSize = 50;

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
						p1OverlayText.text = ("waiting for player 2!");
					}
					//multiplayer
					else {
						//player 1 touched
						if (touchPosition.y < 0) {
							P1Touched = true;
							p1OverlayText.text = ("waiting for player 2!");
						}
								
						//player 2 touched
						if (touchPosition.y > 0) {
							P2Touched = true;
							p2OverlayText.text = ("waiting for player 1!");
						}
					}

					break;
					
				case TouchPhase.Ended: // Release touch from screen

						//single player
					if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
						p1OverlayText.text = ("Place finger here to start!");

						if (P1Touched) {
						} else { //not touching 
							P1Touched = false;
						}
					}
						//multiplayer
						else {
						p1OverlayText.text = ("Player 1 place finger here!");
						p2OverlayText.text = ("Player 2 place finger here!");

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
		}/* else {
			if (Input.GetMouseButtonDown (0)) {
				P1Touched = true;
				P2Touched = true;
			}
		}*/
	}
}
