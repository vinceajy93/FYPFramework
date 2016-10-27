using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Card_Control : MonoBehaviour {
	private Mode_Control mcontrol;
	private PauseScript _pauseScript;
	private HealthManager _HealthManager;

	// Canvas Varibles (Only usable in canvas)
	private GameObject MainCanvas;
	private float MainCanvas_Height;

	// Card_Menu Needed Material
	private GameObject card_button;
	private GameObject card_menu;
	private bool overSprite = false;

	private string name_card_left;
	private string name_card_middle;
	private string name_card_right;

	private GameObject Card_Left;
	private GameObject Card_Middle;
	private GameObject Card_Right;

	// Use this for initialization
	void Start () {
		mcontrol = GameObject.Find ("Scripts").GetComponent<Mode_Control> ();
		_pauseScript = GameObject.Find ("Scripts").GetComponent<PauseScript> ();
		_HealthManager = GameObject.Find("Scripts").GetComponent<HealthManager>();

		// Get Canvas as Gameobject
		MainCanvas = GameObject.Find("Canvas");
		MainCanvas_Height = MainCanvas.GetComponent<RectTransform> ().rect.height;

		if (mcontrol.game_mode_Single) {
			// Menu
			card_button = GameObject.Find ("Canvas/Card/Card_Button");
			card_menu = GameObject.Find ("Canvas/Card/Card_Menu");
			card_menu.SetActive (false);

			// Indivial Card
			name_card_left = "Card/" + "Card_Yoosung";
			name_card_middle = "Card/" + "Card_Jaehee";
			name_card_right = "Card/" + "Card_Zen";

			Card_Left = Instantiate (Resources.Load (name_card_left), Vector3.zero, Quaternion.identity) as GameObject;
			Card_Left.transform.SetParent (card_menu.transform, false);
			Card_Left.transform.position = card_menu.transform.GetChild(0).position;
			Card_Left.tag = "Card_P1";

			Card_Middle = Instantiate (Resources.Load (name_card_middle), Vector3.zero, Quaternion.identity) as GameObject;
			Card_Middle.transform.SetParent (card_menu.transform, false);
			Card_Middle.transform.position = card_menu.transform.GetChild(1).position;
			Card_Middle.tag = "Card_P1";

			Card_Right = Instantiate (Resources.Load (name_card_right), Vector3.zero, Quaternion.identity) as GameObject;
			Card_Right.transform.SetParent (card_menu.transform, false);
			Card_Right.transform.position = card_menu.transform.GetChild(2).position;
			Card_Right.tag = "Card_P1";
		} else {
			if (this.CompareTag("Card_P1")) {
				// Menu
				card_button = GameObject.Find ("Canvas/Card_P1/Card_Button");
				card_menu = GameObject.Find ("Canvas/Card_P1/Card_Menu");
				card_menu.SetActive (false);

				// Indivial Card
				name_card_left = "Card/" + "Card_707";
				name_card_middle = "Card/" + "Card_Jumin";
				name_card_right = "Card/" + "Card_Zen";

				Card_Left = Instantiate (Resources.Load (name_card_left), Vector3.zero, Quaternion.identity) as GameObject;
				Card_Left.transform.SetParent (card_menu.transform, false);
				Card_Left.transform.position = card_menu.transform.GetChild(0).position;
				Card_Left.tag = "Card_P1";

				Card_Middle = Instantiate (Resources.Load (name_card_middle), Vector3.zero, Quaternion.identity) as GameObject;
				Card_Middle.transform.SetParent (card_menu.transform, false);
				Card_Middle.transform.position = card_menu.transform.GetChild(1).position;
				Card_Middle.tag = "Card_P1";

				Card_Right = Instantiate (Resources.Load (name_card_right), Vector3.zero, Quaternion.identity) as GameObject;
				Card_Right.transform.SetParent (card_menu.transform, false);
				Card_Right.transform.position = card_menu.transform.GetChild(2).position;
				Card_Right.tag = "Card_P1";
			} else {
				// Menu
				card_button = GameObject.Find ("Canvas/Card_P2/Card_Button");
				card_menu = GameObject.Find ("Canvas/Card_P2/Card_Menu");
				card_menu.SetActive (false);

				// Indivial Card
				name_card_left = "Card/" + "Card_Jaehee";
				name_card_middle = "Card/" + "Card_Yoosung";
				name_card_right = "Card/" + "Card_Zen";

				Card_Left = Instantiate (Resources.Load (name_card_left), Vector3.zero, Quaternion.identity) as GameObject;
				Card_Left.transform.SetParent (card_menu.transform, false);
				Card_Left.transform.position = card_menu.transform.GetChild(0).position;
				Card_Left.tag = "Card_P2";

				Card_Middle = Instantiate (Resources.Load (name_card_middle), Vector3.zero, Quaternion.identity) as GameObject;
				Card_Middle.transform.SetParent (card_menu.transform, false);
				Card_Middle.transform.position = card_menu.transform.GetChild(1).position;
				Card_Middle.tag = "Card_P2";

				Card_Right = Instantiate (Resources.Load (name_card_right), Vector3.zero, Quaternion.identity) as GameObject;
				Card_Right.transform.SetParent (card_menu.transform, false);
				Card_Right.transform.position = card_menu.transform.GetChild(2).position;
				Card_Right.tag = "Card_P2";
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (mcontrol.game_mode_Single) { // Single Player
			if (!Card_Left.activeSelf && !Card_Middle.activeSelf && !Card_Right.activeSelf) {
				card_menu.SetActive (false);
				mcontrol.card_menu_P1 = false;
				card_button.SetActive (false);
			}

			if (mcontrol.card_menu_P1) {
				if (Input.GetMouseButtonDown (0)) {
					if (!RectTransformUtility.RectangleContainsScreenPoint (card_menu.GetComponent<RectTransform> (), Input.mousePosition)) {
						card_menu.SetActive (false);
						mcontrol.card_menu_P1 = false;
						card_button.SetActive (true);
					}
				}
			}
		} else { // Multiplayer
			if (!Card_Left.activeSelf && !Card_Middle.activeSelf && !Card_Right.activeSelf) {
				card_menu.SetActive (false);
				card_button.SetActive (false);

				if (this.CompareTag ("Card_P1"))
					mcontrol.card_menu_P1 = false;
				else
					mcontrol.card_menu_P2 = false;
			}

			if (this.CompareTag("Card_P1") && mcontrol.card_menu_P1) {
				int nbTouches = Input.touchCount;

				if (nbTouches > 0) {
					for (int i = 0; i < nbTouches; i++) {
						Touch touch = Input.GetTouch (i);

						TouchPhase phase = touch.phase;

						if (phase == TouchPhase.Began && touch.position.y < (Screen.height / 2)) {
							if (!RectTransformUtility.RectangleContainsScreenPoint (card_menu.GetComponent<RectTransform> (), Input.mousePosition)) {
								card_menu.SetActive (false);
								mcontrol.card_menu_P1 = false;
								card_button.SetActive (true);
							}
						}
					}
				}
			}

			if (this.CompareTag("Card_P2") && mcontrol.card_menu_P2) {
				int nbTouches = Input.touchCount;

				if (nbTouches > 0) {
					for (int i = 0; i < nbTouches; i++) {
						Touch touch = Input.GetTouch (i);

						TouchPhase phase = touch.phase;

						if (phase == TouchPhase.Began && touch.position.y > (Screen.height / 2)) {
							if (!RectTransformUtility.RectangleContainsScreenPoint (card_menu.GetComponent<RectTransform> (), Input.mousePosition)) {
								card_menu.SetActive (false);
								mcontrol.card_menu_P2 = false;
								card_button.SetActive (true);
							}
						}
					}
				}
			}
		}
	}

	public void Card_Down (Button button) {
		if (!_pauseScript.Paused) {
			if (button.CompareTag ("Card_P1")) {
				if (mcontrol.card_menu_P1) {
					mcontrol.card_menu_P1 = false;
					card_menu.SetActive (false);
					card_button.SetActive (true);
				} else {
					mcontrol.card_menu_P1 = true;
					card_menu.SetActive (true);
					card_button.SetActive (false);
				}

			} 

			if (button.CompareTag ("Card_P2")) {
				if (mcontrol.card_menu_P2) {
					mcontrol.card_menu_P2 = false;
					card_menu.SetActive (false);
					card_button.SetActive (true);
				} else {
					mcontrol.card_menu_P2 = true;
					card_menu.SetActive (true);
					card_button.SetActive (false);
				}
			}
		}
	}

	public void DisableMenu (bool Player_1) {
		if (Player_1) {
			mcontrol.card_menu_P1 = false;
		} else {
			mcontrol.card_menu_P2 = false;
		}

		if (!Card_Left.activeSelf && !Card_Middle.activeSelf && !Card_Right.activeSelf) {
			card_button.SetActive (false);
		} else {
			card_button.SetActive (true);
		}
		card_menu.SetActive (false);
	}
}
