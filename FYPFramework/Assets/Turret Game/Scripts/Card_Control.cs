using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Card_Control : MonoBehaviour {
	private Mode_Control mcontrol;
	private PauseScript _pauseScript;

	// Card_Menu Needed Material
	private GameObject card_menu;

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
		if (mcontrol.game_mode_Single) {
			card_menu = GameObject.FindGameObjectWithTag ("Card_P1");

			// Indivial Card
			name_card_left = "Card/Icon_" + PlayerPrefs.GetString ("S_Card_Left", "Card_Health");
			name_card_middle = "Card/Icon_" + PlayerPrefs.GetString ("S_Card_Middle", "Card_Health");
			name_card_right = "Card/Icon_" + PlayerPrefs.GetString ("S_Card_Right", "Card_Health");

			Card_Left = Instantiate (Resources.Load (name_card_left)) as GameObject;
			Card_Left.transform.SetParent (card_menu.transform, false);
			Card_Left.transform.position = card_menu.transform.GetChild (0).position;
			Card_Left.transform.localScale = card_menu.transform.GetChild (0).localScale;
			Card_Left.tag = "Card_P1";

			Card_Middle = Instantiate (Resources.Load (name_card_middle)) as GameObject;
			Card_Middle.transform.SetParent (card_menu.transform, false);
			Card_Middle.transform.position = card_menu.transform.GetChild (1).position;
			Card_Middle.transform.localScale = card_menu.transform.GetChild (1).localScale;
			Card_Middle.tag = "Card_P1";

			Card_Right = Instantiate (Resources.Load (name_card_right)) as GameObject;
			Card_Right.transform.SetParent (card_menu.transform, false);
			Card_Right.transform.position = card_menu.transform.GetChild (2).position;
			Card_Right.transform.localScale = card_menu.transform.GetChild (2).localScale;
			Card_Right.tag = "Card_P1";
		} else {
			// Menu P1
			card_menu = GameObject.FindGameObjectWithTag ("Card_P1");

			// Indivial Card
			name_card_left = "Card/Icon_" + PlayerPrefs.GetString ("LM_Card_Left_P1", "Card_Health");
			name_card_middle = "Card/Icon_" + PlayerPrefs.GetString ("LM_Card_Middle_P1", "Card_Health");
			name_card_right = "Card/Icon_" + PlayerPrefs.GetString ("LM_Card_Right_P1", "Card_Health");

			Card_Left = Instantiate (Resources.Load (name_card_left)) as GameObject;
			Card_Left.transform.SetParent (card_menu.transform, false);
			Card_Left.transform.position = card_menu.transform.GetChild (0).position;
			Card_Left.transform.localScale = card_menu.transform.GetChild (0).localScale;
			Card_Left.tag = "Card_P1";

			Card_Middle = Instantiate (Resources.Load (name_card_middle)) as GameObject;
			Card_Middle.transform.SetParent (card_menu.transform, false);
			Card_Middle.transform.position = card_menu.transform.GetChild (1).position;
			Card_Middle.transform.localScale = card_menu.transform.GetChild (1).localScale;
			Card_Middle.tag = "Card_P1";
	
			Card_Right = Instantiate (Resources.Load (name_card_right)) as GameObject;
			Card_Right.transform.SetParent (card_menu.transform, false);
			Card_Right.transform.position = card_menu.transform.GetChild (2).position;
			Card_Right.transform.localScale = card_menu.transform.GetChild (2).localScale;
			Card_Right.tag = "Card_P1";


			// Menu P2
			card_menu = GameObject.FindGameObjectWithTag ("Card_P2");

			// Indivial Card
			name_card_left = "Card/Icon_" + PlayerPrefs.GetString ("LM_Card_Left_P2", "Card_Health");
			name_card_middle = "Card/Icon_" + PlayerPrefs.GetString ("LM_Card_Middle_P2", "Card_Health");
			name_card_right = "Card/Icon_" + PlayerPrefs.GetString ("LM_Card_Right_P2", "Card_Health");

			Card_Left = Instantiate (Resources.Load (name_card_left)) as GameObject;
			Card_Left.transform.SetParent (card_menu.transform, false);
			Card_Left.transform.position = card_menu.transform.GetChild (0).position;
			Card_Left.transform.rotation = card_menu.transform.GetChild (0).rotation;
			Card_Left.transform.localScale = card_menu.transform.GetChild (0).localScale;
			Card_Left.tag = "Card_P2";

			Card_Middle = Instantiate (Resources.Load (name_card_middle)) as GameObject;
			Card_Middle.transform.SetParent (card_menu.transform, false);
			Card_Middle.transform.position = card_menu.transform.GetChild (1).position;
			Card_Middle.transform.rotation = card_menu.transform.GetChild (1).rotation;
			Card_Middle.transform.localScale = card_menu.transform.GetChild (1).localScale;
			Card_Middle.tag = "Card_P2";
		
			Card_Right = Instantiate (Resources.Load (name_card_right)) as GameObject;
			Card_Right.transform.SetParent (card_menu.transform, false);
			Card_Right.transform.position = card_menu.transform.GetChild (2).position;
			Card_Right.transform.rotation = card_menu.transform.GetChild (2).rotation;
			Card_Right.transform.localScale = card_menu.transform.GetChild (2).localScale;
			Card_Right.tag = "Card_P2";
		}
	}

	void Update () {
		
	}
}
