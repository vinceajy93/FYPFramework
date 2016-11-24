﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

//this script is used for testing
public class Loadout_MultiScript : MonoBehaviour
{
	//Panels
	[SerializeField] 
	private GameObject[] Panels = null;

	//Display Images and frames
	[SerializeField]
	private Image[] DisplayImages, CardImages, FrameImages;

	//Sprites for confirm button
	[SerializeField]
	private Sprite[] ConfirmSprites = null;

	//Buttons for confirmation of player selection
	[SerializeField]
	private Button[] ConfirmButtons = null;

	//Texts used for displaying costs and stats
	[SerializeField]
	private Text[] DisplayTexts;

	//player selections
	[SerializeField]
	private GameObject[] PlayerSelections;

	//Alert text
	[SerializeField]
	Text[] AlertText;
	//Cards text
	[SerializeField]
	private Text[] CardText;

	//Stats Texts
	[SerializeField]
	private Text[] StatText;

	//For character heads
	[SerializeField]
	private Sprite[] headSprites;
	[SerializeField]
	private Image[] headImages;

	[SerializeField]
	private Text[] upgradePanelCreds;

	//Non - serialized fields//

	//check tags
	string prevTag;
	//GameObject used to store and check tag
	private GameObject _TempGO;

	//Check player confirmation
	private bool isConfirmed_P1, isConfirmed_P2;

	//creds used to spend on purchasing/ upgradng
	private int Creds_P1, Creds_P2;

	//temp go_cost to store the cost of the individual objects
	int[] tempGO_Cost;

	//GameObject to store current gameobject selected by player
	private GameObject GO;
	private Level_Control _Level_Control;

	//Restricts number of cards
	private int P1CardsCount = 0;
	private int P2CardsCount = 0;

	//timer for flashing alert
	float countdownTimer_Alert;
	static float tresholdTime_Alert = 0.5f;
	private Vector3 AlertTextScale;

	string currentStat;
	string nextStat, nextnextStat;

	string[] tempStringSplit;

	//string of stats
	//E.g. turret_1_2:[0], turret_1_3:[1], turret_2_2:[2], turret_2_3:[3] , etc
	public float[] turretStat;
	public float[] bulletStat;
	public float[] wallStat;

	int roundsPassed;
	int currentRound;

	// Use this for initialization
	void Start ()
	{
		currentStat = null;
		nextStat = null;
		nextnextStat = null;

		//Alert Text Scale
		AlertTextScale = new Vector3 (0.2f, 0.2f, 1.0f);

		//Cheat Method (set buletStat to turretStat's stat)
		bulletStat = turretStat;

		//Set initial stats display
		//P1 Stattext display
		StatText [0].text = (Panels [0].transform.Find ("Grid/turret_1_P1/Stat").GetComponent<Text> ().text);
		StatText [1].text = (Panels [1].transform.Find ("Grid/bullet_1_P1/Stat").GetComponent<Text> ().text);
		StatText [2].text = (Panels [2].transform.Find ("Grid/wall_1_P1/Stat").GetComponent<Text> ().text);

		StatText [3].text = (Panels [4].transform.Find ("Grid/turret_1_P2/Stat").GetComponent<Text> ().text);
		StatText [4].text = (Panels [5].transform.Find ("Grid/bullet_1_P2/Stat").GetComponent<Text> ().text);
		StatText [5].text = (Panels [6].transform.Find ("Grid/wall_1_P2/Stat").GetComponent<Text> ().text);

		//Set all panels active first to do some start codes
		foreach (GameObject _Panels in Panels) {
			_Panels.SetActive (true);
		}

		//Set all Texts with text "Selected" to false (for cards)
		Text[] TempTexts;
		TempTexts = GameObject.FindObjectsOfType<Text> ();

		foreach (Text selText in TempTexts) {

			if (selText.text == "SELECTED")
				selText.gameObject.SetActive (false);
		}

		// --Initializations -- //
		GO = null;

		//P1 cred
		Creds_P1 = 5;
		DisplayTexts [4].text = "Creds: " + Creds_P1;
		//P2 cred
		Creds_P2 = 5;
		DisplayTexts [5].text = "Creds: " + Creds_P2;

		//gameobject
		_TempGO = new GameObject ();
		_TempGO.transform.SetParent (this.transform);
		//Temp GameObject
		Transform tempGO_Transform;

		//string
		prevTag = null;

		//P1 Turret Frame
		tempGO_Transform = GameObject.Find ("turret_1_P1").transform;
		FrameImages [0].gameObject.transform.position = tempGO_Transform.position;
		FrameImages [0].gameObject.transform.SetParent (tempGO_Transform);

		//P1 Bullet Frame
		tempGO_Transform = GameObject.Find ("bullet_1_P1").transform;
		FrameImages [1].gameObject.transform.position = tempGO_Transform.position;
		FrameImages [1].gameObject.transform.SetParent (tempGO_Transform);

		//P1 Wall Frame
		tempGO_Transform = GameObject.Find ("wall_1_P1").transform;
		FrameImages [2].gameObject.transform.position = tempGO_Transform.position;
		FrameImages [2].gameObject.transform.SetParent (tempGO_Transform);

		//P2 Turret Frame
		tempGO_Transform = GameObject.Find ("turret_1_P2").transform;
		FrameImages [3].gameObject.transform.position = tempGO_Transform.position;
		FrameImages [3].gameObject.transform.SetParent (tempGO_Transform);

		//P2 Bullet Frame
		tempGO_Transform = GameObject.Find ("bullet_1_P2").transform;
		FrameImages [4].gameObject.transform.position = tempGO_Transform.position;
		FrameImages [4].gameObject.transform.SetParent (tempGO_Transform);

		//P2 Wall Frame
		tempGO_Transform = GameObject.Find ("wall_1_P2").transform;
		FrameImages [5].gameObject.transform.position = tempGO_Transform.position;
		FrameImages [5].gameObject.transform.SetParent (tempGO_Transform);

		_Level_Control = GetComponent<Level_Control> ();

		//Set all panels to false
		foreach (GameObject _Panels in Panels) {
			_Panels.SetActive (false);
		}

		//Set default playerPrefs
		PlayerPrefs.SetFloat ("M_P1_FireRate", 1f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		roundsPassed = PlayerPrefs.GetInt("roundsPassed");
		if (roundsPassed > currentRound) {
			Creds_P1 += 5;
			Creds_P2 += 5;
			currentRound = roundsPassed;
		}
		upgradePanelCreds [0].text = "Creds: " + Creds_P1;
		upgradePanelCreds [1].text = "Creds: " + Creds_P2;

		//Update the countdownTimer
		countdownTimer_Alert += Time.deltaTime;

		//Lock the upgrade button if not enough credits

		//Do actions when timer hit treshhold time
		if (countdownTimer_Alert > tresholdTime_Alert) {

			if (AlertText [0].gameObject.transform.localScale == AlertTextScale)
				AlertText [0].transform.localScale = Vector3.zero;
			else
				AlertText [0].transform.localScale = AlertTextScale;

			if (AlertText [1].gameObject.transform.localScale == AlertTextScale)
				AlertText [1].transform.localScale = Vector3.zero;
			else
				AlertText [1].transform.localScale = AlertTextScale;

			countdownTimer_Alert = 0f;
		}


		if (Panels [3].activeSelf) {
			//update the card texts
			CardText [0].text = ("Cards selected: " + P1CardsCount + "/3");
		}

		if (Panels [7].activeSelf) {
			//update the card texts
			CardText [1].text = ("Cards selected: " + P2CardsCount + "/3");
		}

		//make confirm button uninteractable unless 3 cards are chosen
		if (P1CardsCount < 3) {
			ConfirmButtons [0].interactable = false;
			ConfirmButtons [0].GetComponent<Image> ().sprite = ConfirmSprites [0];
			AlertText [0].gameObject.SetActive (true);
		} else {
			ConfirmButtons [0].interactable = true;
			ConfirmButtons [0].GetComponent<Image> ().sprite = ConfirmSprites [1];
			AlertText [0].gameObject.SetActive (false);
		}

		if (P2CardsCount < 3) {
			ConfirmButtons [1].interactable = false;
			ConfirmButtons [1].GetComponent<Image> ().sprite = ConfirmSprites [0];
			AlertText [1].gameObject.SetActive (true);
		} else {
			ConfirmButtons [1].interactable = true;
			ConfirmButtons [1].GetComponent<Image> ().sprite = ConfirmSprites [1];
			AlertText [1].gameObject.SetActive (false);
		}
	}

	//Called when turret button is pressed
	public void SelectTurretFunction ()
	{
		//Set GameObject to be the current selected one by player
		GO = EventSystem.current.currentSelectedGameObject;

		//temp variables
		string[] FireRate = null;
		string[] tempString = GO.name.Split ('_'); 
		int tempInt = int.Parse (tempString [1]) - 1;
		Vector3 lastSelectedPos;

		//If tag belongs to player1
		if (GO.CompareTag ("TurretChoice_P1")) {
			//set the selected image to new image
			DisplayImages [0].transform.Find ("turret_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("turret_Preview").GetComponent<Image> ().sprite;
			//Set the temp string to get current selected speed text then split and store in an array
			FireRate = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			//Save this Stat to playerPrefs to be use later
			PlayerPrefs.SetFloat ("M_P1_FireRate", float.Parse (FireRate [1]));
			//set the frame image to be the current selected
			lastSelectedPos = GO.transform.position;
			//Set pilot potrait to match with turret 
			headImages [0].sprite = headSprites [tempInt];
			//Set position of frame at current selected object pos
			FrameImages [0].gameObject.transform.position = lastSelectedPos;
			//move the frame overlay to be child of current pressed game object
			FrameImages [0].gameObject.transform.SetParent (GO.transform);
		}
		//If tag belongs to player2
		else if (GO.CompareTag ("TurretChoice_P2")) {
			//set the selected image to new image
			DisplayImages [3].transform.Find ("turret_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("turret_Preview").GetComponent<Image> ().sprite;
			//Set the temp string to get current selected speed text then split and store in an array
			FireRate = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			//Save this Stat to playerPrefs to be use later
			PlayerPrefs.SetFloat ("M_P2_FireRate", float.Parse (FireRate [1]));
			//set the frame image to be the current selected
			lastSelectedPos = GO.transform.position;
			//Set pilot potrait to match with turret 
			headImages [3].sprite = headSprites [tempInt];
			//Set position of frame at current selected object pos
			FrameImages [3].gameObject.transform.position = lastSelectedPos;
			//move the frame overlay to be child of current pressed game object
			FrameImages [3].gameObject.transform.SetParent (GO.transform);

		}

		//toggle stars function
		ToggleStars ();

	}

	public void SelectBulletFunction ()
	{
		//Set GameObject to be the current selected one by player
		GO = EventSystem.current.currentSelectedGameObject;

		//temp variables
		string[] DMG;
		Vector3 lastSelectedPos;

		//If tag belongs to player1
		if (GO.CompareTag ("BulletChoice_P1")) {
			//set the selected image to new image
			DisplayImages [1].transform.Find ("bullet_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("bullet_Preview").GetComponent<Image> ().sprite;
			//Set the temp string to get current selected speed text then split and store in an array
			DMG = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			//Save this Stat to playerPrefs to be use later
			PlayerPrefs.SetFloat ("M_P1_DMG", float.Parse (DMG [1]));
			//set the frame image to be the current selected
			lastSelectedPos = GO.transform.position;
			//Set position of frame at current selected object pos
			FrameImages [1].gameObject.transform.position = lastSelectedPos;
			//move the frame overlay to be child of current pressed game object
			FrameImages [1].gameObject.transform.SetParent (GO.transform);
		}
		//If tag belongs to player2
		else if (GO.CompareTag ("BulletChoice_P2")) {

			//set the selected turret's image to turret image
			DisplayImages [4].transform.Find ("bullet_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("bullet_Preview").GetComponent<Image> ().sprite;
			//Set the temp string to get current selected speed text then split and store in an array
			DMG = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			//Save this Stat to playerPrefs to be use later
			PlayerPrefs.SetFloat ("M_P2_DMG", float.Parse (DMG [1]));
			//set the frame image to be the current selected
			lastSelectedPos = GO.transform.position;
			//Set position of frame at current selected object pos
			FrameImages [4].gameObject.transform.position = lastSelectedPos;
			//move the frame overlay to be child of current pressed game object
			FrameImages [4].gameObject.transform.SetParent (GO.transform);

		}

		//toggle stars function
		ToggleStars ();
	}

	public void SelectWallFunction ()
	{
		//Set GameObject to be the current selected one by player
		GO = EventSystem.current.currentSelectedGameObject;

		//temp variables
		string[] Hp;
		Vector3 lastSelectedPos;

		//If tag belongs to player1
		if (GO.CompareTag ("WallChoice_P1")) {
			//set the selected image to new image
			DisplayImages [2].transform.Find ("Panel/wall_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("Panel/wall_Preview").GetComponent<Image> ().sprite;
			//Set the temp string to get current selected speed text then split and store in an array
			Hp = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			//Save this Stat to playerPrefs to be use later
			PlayerPrefs.SetFloat ("M_P1_HP", float.Parse (Hp [1]));
			//set the frame image to be the current selected
			lastSelectedPos = GO.transform.position;
			//Set position of frame at current selected object pos
			FrameImages [2].gameObject.transform.position = lastSelectedPos;
			//move the frame overlay to be child of current pressed game object
			FrameImages [2].gameObject.transform.SetParent (GO.transform);
		}
		//If tag belongs to player2
		else if (GO.CompareTag ("WallChoice_P2")) {

			//set the selected turret's image to turret image
			DisplayImages [5].transform.Find ("Panel/wall_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("Panel/wall_Preview").GetComponent<Image> ().sprite;
			//Set the temp string to get current selected speed text then split and store in an array
			Hp = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			//Save this Stat to playerPrefs to be use later
			PlayerPrefs.SetFloat ("M_P2_HP", float.Parse (Hp [1]));
			//set the frame image to be the current selected
			lastSelectedPos = GO.transform.position;
			//Set position of frame at current selected object pos
			FrameImages [5].gameObject.transform.position = lastSelectedPos;
			//move the frame overlay to be child of current pressed game object
			FrameImages [5].gameObject.transform.SetParent (GO.transform);

		}

		//toggle stars function
		ToggleStars ();
	}

	public void SelectCardFunction ()
	{
		//Set GameObject to be the current selected one by player
		GO = EventSystem.current.currentSelectedGameObject;

		GameObject textGO_P1, textGO_P2 = null;



		//If tag belongs to player1
		if (GO.CompareTag ("CardChoice_P1")) {

			Sprite GoSprite = GO.transform.Find ("card_Preview").GetComponent<Image> ().sprite;
			textGO_P1 = GO.transform.Find ("Text").gameObject;
			//check if current card is selected, if not select it. if is, unselect it
			if (!textGO_P1.activeSelf) {
				//not selected
				if (P1CardsCount < 3) {
					//not selected
					P1CardsCount++;
					textGO_P1.SetActive (true);
					GO.transform.Find ("card_Preview").GetComponent<Image> ().color = new Color (0.278f, 0.278f, 0.278f, 1.0f);

					//Debug.Log(GO.transform.Find ("Card_Name").GetComponent<Text> ().text);

					//set the image of the card
					for (int i = 0; i < 3; i++) {
						if (CardImages [i].GetComponent<Image> ().sprite == null) {
							CardImages [i].gameObject.SetActive (true);
							CardImages [i].GetComponent<Image> ().sprite = GoSprite;
							break;
						} else {
							continue;
						}
					}
				}
			} else {
				P1CardsCount--;
				textGO_P1.SetActive (false);
				GO.transform.Find ("card_Preview").GetComponent<Image> ().color = Color.white;

				for (int i = 0; i < 3; i++) {
					if (CardImages [i].GetComponent<Image> ().sprite == GoSprite) {
						CardImages [i].gameObject.SetActive (false);
						CardImages [i].GetComponent<Image> ().sprite = null;
					} else {
						continue;
					}
				}
			}
		} else if (GO.CompareTag ("CardChoice_P2")) {

			Sprite GoSprite = GO.transform.Find ("card_Preview").GetComponent<Image> ().sprite;
			textGO_P2 = GO.transform.Find ("Text").gameObject;
			//check if current card is selected, if not select it. if is, unselect it
			if (!textGO_P2.activeSelf) {
				//not selected
				if (P2CardsCount < 3) {
					//not selected
					P2CardsCount++;
					textGO_P2.SetActive (true);
					GO.transform.Find ("card_Preview").GetComponent<Image> ().color = new Color (0.278f, 0.278f, 0.278f, 1.0f);

					//set the image of the card
					for (int i = 3; i < 6; i++) {
						if (CardImages [i].GetComponent<Image> ().sprite == null) {
							CardImages [i].gameObject.SetActive (true);
							CardImages [i].GetComponent<Image> ().sprite = GoSprite;
							break;
						} else {
							continue;
						}
					}
				}
			} else {
				P2CardsCount--;
				textGO_P2.SetActive (false);
				GO.transform.Find ("card_Preview").GetComponent<Image> ().color = Color.white;

				for (int i = 3; i < 6; i++) {
					if (CardImages [i].GetComponent<Image> ().sprite == GoSprite) {
						CardImages [i].gameObject.SetActive (false);
						CardImages [i].GetComponent<Image> ().sprite = null;
					} else {
						continue;
					}
				}
			}
			
		}	
	}

	public void UpgradeReturn ()
	{
		//P1
		if (_TempGO.CompareTag ("TurretChoice_P1")) {
			Panels [8].SetActive (false);
			Panels [0].SetActive (true);
		} else if (_TempGO.CompareTag ("BulletChoice_P1")) {
			Panels [8].SetActive (false);
			Panels [1].SetActive (true);
		} else if (_TempGO.CompareTag ("WallChoice_P1")) {
			Panels [8].SetActive (false);
			Panels [2].SetActive (true);
		}
		//P2
		if (_TempGO.CompareTag ("TurretChoice_P2")) {
			Panels [9].SetActive (false);
			Panels [4].SetActive (true);
		} else if (_TempGO.CompareTag ("BulletChoice_P2")) {
			Panels [9].SetActive (false);
			Panels [5].SetActive (true);
		} else if (_TempGO.CompareTag ("WallChoice_P2")) {
			Panels [9].SetActive (false);
			Panels [6].SetActive (true);
		}
	}

	public void UpgradeConfirm ()
	{
		//P1
		if (_TempGO.CompareTag ("TurretChoice_P1")) {
			//Check if enough Creds to buy, if yes, do deduction and upgrade, else nothing
			if ((Creds_P1 - 1) >= 0) {
				Creds_P1--;

				//Update the Creds text 
				DisplayTexts [4].text = "Creds: " + Creds_P1;
				Panels [8].transform.Find ("Stats").GetComponent<Text> ().text = "SPD: " + nextStat + " >> " + nextnextStat;
				//Change the level
				//if(GO.transform.Find("Level").GetComponent<Text>().text != null)
			}
		} else if (_TempGO.CompareTag ("BulletChoice_P1")) {
			if ((Creds_P1 - 1) >= 0) {
				Creds_P1--;

				//Update the Creds text 
				DisplayTexts [4].text = "Creds: " + Creds_P1;
				Panels [8].transform.Find ("Stats").GetComponent<Text> ().text = "DMG: " + currentStat + " >> " + nextStat;

			}
		} else if (_TempGO.CompareTag ("WallChoice_P1")) {
			if ((Creds_P1 - 1) >= 0) {
				Creds_P1--;

				//Update the Creds text 
				DisplayTexts [4].text = "Creds: " + Creds_P1;
				Panels [8].transform.Find ("Stats").GetComponent<Text> ().text = "HP: " + currentStat + " >> " + nextStat;
			}
		}
		//P2
		if (_TempGO.CompareTag ("TurretChoice_P2")) {
			if ((Creds_P2 - 1) >= 0) {
				Creds_P2--;

				//Update the Creds text 
				DisplayTexts [5].text = "Creds: " + Creds_P2;
				Panels [9].transform.Find ("Stats").GetComponent<Text> ().text = "SPD: " + currentStat + " >> " + nextStat;

			}
		} else if (_TempGO.CompareTag ("BulletChoice_P2")) {
			if ((Creds_P2 - 1) >= 0) {
				Creds_P2--;

				//Update the Creds text 
				DisplayTexts [5].text = "Creds: " + Creds_P2;
				Panels [9].transform.Find ("Stats").GetComponent<Text> ().text = "DMG: " + currentStat + " >> " + nextStat;

			}
		} else if (_TempGO.CompareTag ("WallChoice_P2")) {
			if ((Creds_P2 - 1) >= 0) {
				Creds_P2--;

				//Update the Creds text 
				DisplayTexts [5].text = "Creds: " + Creds_P2;
				Panels [9].transform.Find ("Stats").GetComponent<Text> ().text = "HP: " + currentStat + " >> " + nextStat;

			}
		}
	}

	public void TogglePanels ()
	{
		//Set GameObject to be the current selected one by player
		GO = EventSystem.current.currentSelectedGameObject;

		//P1 Stat text display
		StatText [0].text = (FrameImages [0].transform.parent.gameObject.transform.Find ("Stat").GetComponent<Text> ().text);
		StatText [1].text = (FrameImages [1].transform.parent.gameObject.transform.Find ("Stat").GetComponent<Text> ().text);
		StatText [2].text = (FrameImages [2].transform.parent.gameObject.transform.Find ("Stat").GetComponent<Text> ().text);

		StatText [3].text = (FrameImages [3].transform.parent.gameObject.transform.Find ("Stat").GetComponent<Text> ().text);
		StatText [4].text = (FrameImages [4].transform.parent.gameObject.transform.Find ("Stat").GetComponent<Text> ().text);
		StatText [5].text = (FrameImages [5].transform.parent.gameObject.transform.Find ("Stat").GetComponent<Text> ().text);

		switch (GO.name) {
		case "Turret_Icon_P1":
			//Set tempGO tag to this tag
			_TempGO.tag = GO.tag;
			//Set corresponding panel to active
			Panels [0].SetActive (true);
			//set Player selections panel and buttons to inactive
			PlayerSelections [0].SetActive (false);
			ConfirmButtons [0].gameObject.SetActive (false);
			//Set corresponding frame image active
			FrameImages [0].gameObject.SetActive (true);
			break;
		case "Bullet_Icon_P1":
			//Set tempGO tag to this tag
			_TempGO.tag = GO.tag;
			//Set corresponding panel to active
			Panels [1].SetActive (true);
			//set Player selections panel and buttons to inactive
			PlayerSelections [0].SetActive (false);
			ConfirmButtons [0].gameObject.SetActive (false);
			//Set corresponding frame image active
			FrameImages [1].gameObject.SetActive (true);
			break;
		case "Wall_Icon_P1":
			//Set tempGO tag to this tag
			_TempGO.tag = GO.tag;
			//Set corresponding panel to active
			Panels [2].SetActive (true);
			//set Player selections panel and buttons to inactive
			PlayerSelections [0].SetActive (false);
			ConfirmButtons [0].gameObject.SetActive (false);
			//Set corresponding frame image active
			FrameImages [2].gameObject.SetActive (true);
			break;
		case "Card_Icon_P1":
			//Set tempGO tag to this tag
			_TempGO.tag = GO.tag;
			//Set corresponding panel to active
			Panels [3].SetActive (true);
			//set Player selections panel and buttons to inactive
			PlayerSelections [0].SetActive (false);
			ConfirmButtons [0].gameObject.SetActive (false);
			break;
		case "Turret_Icon_P2":
			//Set tempGO tag to this tag
			_TempGO.tag = GO.tag;
			//Set corresponding panel to active
			Panels [4].SetActive (true);
			//set Player selections panel and buttons to inactive
			PlayerSelections [1].SetActive (false);
			ConfirmButtons [1].gameObject.SetActive (false);
			//Set corresponding frame image active
			FrameImages [3].gameObject.SetActive (true);
			break;
		case "Bullet_Icon_P2":
			//Set tempGO tag to this tag
			_TempGO.tag = GO.tag;
			//Set corresponding panel to active
			Panels [5].SetActive (true);
			//set Player selections panel and buttons to inactive
			PlayerSelections [1].SetActive (false);
			ConfirmButtons [1].gameObject.SetActive (false);
			//Set corresponding frame image active
			FrameImages [4].gameObject.SetActive (true);
			break;
		case "Wall_Icon_P2":
			//Set tempGO tag to this tag
			_TempGO.tag = GO.tag;
			//Set corresponding panel to active
			Panels [6].SetActive (true);
			//set Player selections panel and buttons to inactive
			PlayerSelections [1].SetActive (false);
			ConfirmButtons [1].gameObject.SetActive (false);
			//Set corresponding frame image active
			FrameImages [5].gameObject.SetActive (true);
			break;
		case "Card_Icon_P2":
			//Set tempGO tag to this tag
			_TempGO.tag = GO.tag;
			//Set corresponding panel to active
			Panels [7].SetActive (true);
			//set Player selections panel and buttons to inactive
			PlayerSelections [1].SetActive (false);
			ConfirmButtons [1].gameObject.SetActive (false);
			break;
		case "Confirm Button_P1":
			//Set P1 selections panel and buttons active
			PlayerSelections [0].SetActive (true);
			ConfirmButtons [0].gameObject.SetActive (true);

			//Set all p1 panels and frames to false
			for (int i = 0; i < 4; i++) {
				Panels [i].SetActive (false);
				FrameImages [i].gameObject.SetActive (false);
			}
			break;
		case "Confirm Button_P2":
			//Set P1 selections panel and buttons active
			PlayerSelections [1].SetActive (true);
			ConfirmButtons [1].gameObject.SetActive (true);

			//Set all p1 panels and frames to false
			for (int i = 4; i < 8; i++) {
				Panels [i].SetActive (false);
				if (i < 5)
					FrameImages [i].gameObject.SetActive (false);
			}
			break;
		case "UCButton": //upgrade confirm button
			//P1
			if (_TempGO.CompareTag ("TurretChoice_P1")) {
				//Panels [8].SetActive (false);
				//Panels [0].SetActive (true);
				//Check if enough Creds to buy, if yes, do deduction and upgrade, else nothing
				if ((Creds_P1 - 1) > 0) {
					Creds_P1--;

					//Update the Creds text 
					DisplayTexts [4].text = "Creds: " + Creds_P1;

				}
			} else if (_TempGO.CompareTag ("BulletChoice_P1")) {
				Panels [8].SetActive (false);
				Panels [1].SetActive (true);
			} else if (_TempGO.CompareTag ("WallChoice_P1")) {
				Panels [8].SetActive (false);
				Panels [2].SetActive (true);
			}
			//P2
			if (_TempGO.CompareTag ("TurretChoice_P2")) {
				Panels [9].SetActive (false);
				Panels [4].SetActive (true);
			} else if (_TempGO.CompareTag ("BulletChoice_P2")) {
				Panels [9].SetActive (false);
				Panels [5].SetActive (true);
			} else if (_TempGO.CompareTag ("WallChoice_P2")) {
				Panels [9].SetActive (false);
				Panels [6].SetActive (true);
			}
			break;
		}
	}

	public void ToggleStars ()
	{
		//Get the GameObject(GO) and set it to GO_Name string
		GO = EventSystem.current.currentSelectedGameObject;

		//get the current Lv of the selected object and save to a temp int
		string[] tempString = GO.transform.Find ("Level").GetComponent<Text> ().text.Split (' ');
		int tempInt = int.Parse (tempString [1]);

		//temp string for lesser codes
		GameObject[] tempStarsGO = {
			GO.transform.Find ("selectedFrame/Stars/stars_1").gameObject,
			GO.transform.Find ("selectedFrame/Stars/stars_2").gameObject,
			GO.transform.Find ("selectedFrame/Stars/stars_3").gameObject
		};

		//switch case to display the number of stars according to the tempInt
		switch (tempInt) {
		case 1:
			tempStarsGO [0].SetActive (true);
			tempStarsGO [1].SetActive (false);
			tempStarsGO [2].SetActive (false);
			break;
		case 2:
			tempStarsGO [0].SetActive (true);
			tempStarsGO [1].SetActive (true);
			tempStarsGO [2].SetActive (false);
			break;
		case 3:
			tempStarsGO [0].SetActive (true);
			tempStarsGO [1].SetActive (true);
			tempStarsGO [2].SetActive (true);
			break;
		default:
			Debug.LogError ("number not in case: " + tempInt);
			return;
		}
	}

	public void UpgradeFunction ()
	{
		
		//Split GO tag name and store in string array
		string[] TempString = _TempGO.tag.Split ('_');
		//temp variables
		string[] stringPreview = { "turret_Preview", "bullet_Preview", "Panel/wall_Preview" };
		Sprite Displays = null;
		string[] tempTag = GO.tag.Split ('_');


		if (tempTag [0] != "Untagged")
			prevTag = tempTag [0];
		
		if (GO.transform.Find ("Stat") == null) {

			switch (prevTag) {
			case "TurretChoice":
				Displays = GO.transform.Find (stringPreview [0]).GetComponent<Image> ().sprite;

				if (prevTag == "P1")
					currentStat = "1";
				else
					currentStat = "1";
				break;
			case "BulletChoice":
				Displays = GO.transform.Find (stringPreview [1]).GetComponent<Image> ().sprite;

				if (prevTag == "P1")
					currentStat = "1";
				else
					currentStat = "1";
				break;
			case "WallChoice":
				Displays = GO.transform.Find (stringPreview [2]).GetComponent<Image> ().sprite;

				if (prevTag == "P1")
					currentStat = "5";
				else
					currentStat = "5";
				break;
			}

		} else {
			string[] tempString = null;
			tempString = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			currentStat = tempString [1];
		}

		switch (prevTag) {
		case "TurretChoice":
			Displays = GO.transform.Find (stringPreview [0]).GetComponent<Image> ().sprite;
			break;
		case "BulletChoice":
			Displays = GO.transform.Find (stringPreview [1]).GetComponent<Image> ().sprite;
			break;
		case "WallChoice":
			Displays = GO.transform.Find (stringPreview [2]).GetComponent<Image> ().sprite;
			break;
		}

		//Set all P1 panels false and upgrade panel P1 to true 
		if (TempString [1] == "P1") {
			
			// P1 set panels
			for (int i = 0; i < 4; i++) {
				Panels [i].SetActive (false);
			}
			Panels [8].SetActive (true);

			//Set image and native size
			DisplayImages [6].GetComponent<Image> ().sprite = Displays;
			DisplayImages [6].GetComponent<Image> ().SetNativeSize ();


			//Set the current and after upgrade stat

			for (int i = 0; i < turretStat.Length; i++) {
				//P1_Turret
				if (currentStat == turretStat [i].ToString () && prevTag == "TurretChoice") {
					//TODO: Add check if this belongs to which turret
					nextStat = turretStat [i + 1].ToString ();
					nextnextStat = turretStat [i + 2].ToString ();
					Panels [8].transform.Find ("Stats").GetComponent<Text> ().text = "SPD: " + currentStat + " >> " + nextStat;
				}
				//P1_Bullet
				else if (currentStat == bulletStat [i].ToString () && prevTag == "BulletChoice") {
					nextStat = turretStat [i + 1].ToString ();
					nextnextStat = turretStat [i + 2].ToString ();
					Panels [8].transform.Find ("Stats").GetComponent<Text> ().text = "DMG: " + currentStat + " >> " + nextStat;
				}
			}

			//P1_Wall
			for (int i = 0; i < wallStat.Length; i++) {
				if (currentStat == wallStat [i].ToString () && prevTag == "WallChoice") {
					nextStat = turretStat [i + 1].ToString ();
					nextnextStat = turretStat [i + 2].ToString ();
					Panels [8].transform.Find ("Stats").GetComponent<Text> ().text = "DMG: " + currentStat + " >> " + nextStat;
				}
			}
		} else if (TempString [1] == "P2") {
			//P2 set panels
			for (int i = 4; i < 7; i++) {
				Panels [i].SetActive (false);
			}
			Panels [9].SetActive (true);

			DisplayImages [7].GetComponent<Image> ().sprite = Displays;
			DisplayImages [7].GetComponent<Image> ().SetNativeSize ();

			//Set the current and after upgrade stat

			for (int i = 0; i < turretStat.Length; i++) {
				//P2_Turret
				if (currentStat == turretStat [i].ToString () && prevTag == "TurretChoice") {
					//TODO: Add check if this belongs to which turret
					nextStat = turretStat [i + 1].ToString ();
					nextnextStat = turretStat [i + 2].ToString ();
					Panels [9].transform.Find ("Stats").GetComponent<Text> ().text = "SPD: " + currentStat + " >> " + nextStat;
				}
				//P2_Bullet
				else if (currentStat == bulletStat [i].ToString () && prevTag == "BulletChoice") {
					nextStat = turretStat [i + 1].ToString ();
					nextnextStat = turretStat [i + 2].ToString ();
					Panels [9].transform.Find ("Stats").GetComponent<Text> ().text = "HP: " + currentStat + " >> " + nextStat;
				}
			}

			//P1_Wall
			for (int i = 0; i < wallStat.Length; i++) {
				if (currentStat == wallStat [i].ToString () && prevTag == "WallChoice") {
					nextStat = turretStat [i + 1].ToString ();
					nextnextStat = turretStat [i + 2].ToString ();
					Panels [9].transform.Find ("Stats").GetComponent<Text> ().text = "HP: " + currentStat + " >> " + nextStat;
				}
			}
		}

	}

	public void CheckConfirmationFunction ()
	{
		//Get the GameObject(GO) and set it to GO_Name string
		GO = EventSystem.current.currentSelectedGameObject;

		//Check for player 1 onfirmation
		if (GO.name == "confirmButton_P1" && !isConfirmed_P1) {
			isConfirmed_P1 = true;
			GO.gameObject.GetComponent<Image> ().sprite = ConfirmSprites [0];

			Component[] tempComponent;
			tempComponent = PlayerSelections [0].GetComponentsInChildren<Button> ();
			foreach (Button buttonComp in tempComponent) {
				buttonComp.interactable = false;
			}
		} else if (GO.name == "confirmButton_P1" && isConfirmed_P1) {
			isConfirmed_P1 = false;
			GO.gameObject.GetComponent<Image> ().sprite = ConfirmSprites [1];

			Component[] tempComponent;
			tempComponent = PlayerSelections [0].GetComponentsInChildren<Button> ();
			foreach (Button buttonComp in tempComponent) {
				buttonComp.interactable = true;
			}
		}

		//check for player 2 confirmation
		if (GO.name == "confirmButton_P2" && !isConfirmed_P2) {
			isConfirmed_P2 = true;
			GO.gameObject.GetComponent<Image> ().sprite = ConfirmSprites [0];

			Component[] tempComponent;
			tempComponent = PlayerSelections [1].GetComponentsInChildren<Button> ();
			foreach (Button buttonComp in tempComponent) {
				buttonComp.interactable = false;
			}
		} else if (GO.name == "confirmButton_P2" && isConfirmed_P2) {
			isConfirmed_P2 = false;
			GO.gameObject.GetComponent<Image> ().sprite = ConfirmSprites [2];
			Component[] tempComponent;
			tempComponent = PlayerSelections [1].GetComponentsInChildren<Button> ();
			foreach (Button buttonComp in tempComponent) {
				buttonComp.interactable = true;
			}
		}

		//goes to main gameplay after both player has confirmed their selection
		if (isConfirmed_P1 && isConfirmed_P2) {
			//Set the cards
			PlayerPrefs.SetString("LM_Card_Left_P1", CardImages[0].sprite.name);
			PlayerPrefs.SetString("LM_Card_Middle_P1", CardImages[1].sprite.name);
			PlayerPrefs.SetString("LM_Card_Right_P1", CardImages[2].sprite.name);

			PlayerPrefs.SetString("LM_Card_Left_P2", CardImages[3].sprite.name);
			PlayerPrefs.SetString("LM_Card_Middle_P2", CardImages[4].sprite.name);
			PlayerPrefs.SetString("LM_Card_Right_P2", CardImages[5].sprite.name);

			//Set the turrets
			tempStringSplit = DisplayImages[0].transform.Find("turret_Preview").GetComponent<Image>().sprite.name.Split(' ');

			PlayerPrefs.SetInt("LM_P1_T", int.Parse(tempStringSplit[1]));
			tempStringSplit = DisplayImages[3].transform.Find("turret_Preview").GetComponent<Image>().sprite.name.Split(' ');
			PlayerPrefs.SetInt("LM_P2_T", int.Parse(tempStringSplit[1]));

			//Set the bullets
			tempStringSplit = DisplayImages[1].transform.Find("bullet_Preview").GetComponent<Image>().sprite.name.Split(' ');
			PlayerPrefs.SetInt("LM_P1_B", int.Parse(tempStringSplit[1]));
			tempStringSplit = DisplayImages[4].transform.Find("bullet_Preview").GetComponent<Image>().sprite.name.Split(' ');
			PlayerPrefs.SetInt("LM_P2_B", int.Parse(tempStringSplit[1]));

			//reset all component to interactable 
			Component[] tempComponent;
			tempComponent = PlayerSelections [0].GetComponentsInChildren<Button> ();
			foreach (Button buttonComp in tempComponent) {
				buttonComp.interactable = true;
			}

			tempComponent = PlayerSelections [1].GetComponentsInChildren<Button> ();
			foreach (Button buttonComp in tempComponent) {
				buttonComp.interactable = true;
			}


			//Reset booleans
			isConfirmed_P1 = false;
			isConfirmed_P2 = false;

			//Load the local multi level
			_Level_Control.loadLocal2P ();
		}
	}
		
}
