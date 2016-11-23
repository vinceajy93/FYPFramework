using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOut_Manager : MonoBehaviour
{

	public static LoadOut_Manager instance;

	//Panels
	[SerializeField]
	private GameObject turretPanel_P1, turretPanel_P2, bulletPanel_P1, bulletPanel_P2, wallPanel_P1, wallPanel_P2, upgradePanel_P1, upgradePanel_P2, cardPanel_P1, cardPanel_P2;
	//Display Images and frames
	[SerializeField]
	private Image turretImageP1, turretImageP2, bulletImageP1, bulletImageP2, wallImageP1, wallImageP2, 
		frameImage_P1_turret, frameImage_P1_bullet, frameImage_P1_wall, frameImage_P2_turret, frameImage_P2_bullet, frameImage_P2_wall, upgradeImageDisplay_P1, upgradeImageDisplay_P2;

	[SerializeField]
	private Image[] cardImage_P1, cardImage_P2;
	//Sprites
	[SerializeField]
	private Sprite[] confirmed_sprite_P1, confirmed_sprite_P2;
	//Buttons
	[SerializeField]
	private Button confirmedButton_P1, confirmedButton_P2;
	//Texts
	[SerializeField]
	private Text upgradeStatDisplay_P1, upgradeCostDisplay_P1, upgradeStatDisplay_P2, upgradeCostDisplay_P2, creds_P1, creds_P2;
	//Non - serialized variables
	private GameObject P1_Selections, P2_Selections, GO;
	private Vector3 lastSelectedTurretPos_P1, lastSelectedBulletPos_P1, lastSelectedWallPos_P1, lastSelectedTurretPos_P2, lastSelectedBulletPos_P2,
		lastSelectedWallPos_P2;
	private Text fireRate_P1_txt, fireRate_P2_txt, Health_P1_txt, Health_P2_txt, DMG_P1_txt, DMG_P2_txt;
	private string[] fireRate_P1_str, fireRate_P2_str, Health_P1_str, Health_P2_str, DMG_P1_str, DMG_P2_str;

	//booleans used to chek if both players locked in their selections
	private bool isConfirmed_P1, isConfirmed_P2;
	private Level_Control _Level_Control;

	//creds used to spend on purchasing/ upgradng
	int credsAmt_P1, credsAmt_P2;
	//temp go_cost to store the cost of the individual objects
	int tempGO_Cost_P1, tempGO_Cost_P2;
	//TempGO stores the tag of GO and uses it to set which panel to be active
	GameObject tempGO;

	void Awake(){
		if (instance != null) {
			if (instance != this) {
				Destroy (this.gameObject); //change to remove component if not other scripts will be destroyed as well
			}
		} else {
			instance = this;
			DontDestroyOnLoad (this);
		}
	}

	// Use this for initialization
	void Start ()
	{ 
		//initialize the credits
		credsAmt_P1 = 5;
		credsAmt_P2 = 5;

		creds_P1.text = "CRDs: " + credsAmt_P1.ToString ();
		creds_P2.text = "CRDs: " + credsAmt_P2.ToString ();

		//initialize the tempGO gameobject
		tempGO = new GameObject ();
		tempGO.name = "TempGO";

		//Set relevant panels to true so that its child components can be accessed
		turretPanel_P1.SetActive (true);
		bulletPanel_P1.SetActive (true);
		wallPanel_P1.SetActive (true);
		cardPanel_P1.SetActive (true);

		turretPanel_P2.SetActive (true);
		bulletPanel_P2.SetActive (true);
		wallPanel_P2.SetActive (true);
		cardPanel_P2.SetActive (true);
			
		//Set the Image frame to be at the defualt selected turret/bullet/wall
		frameImage_P1_turret.gameObject.transform.position = GameObject.Find ("turret_1_P1").transform.position;
		frameImage_P1_turret.gameObject.transform.SetParent (GameObject.Find ("turret_1_P1").transform);

		frameImage_P1_bullet.gameObject.transform.position = GameObject.Find ("bullet_1_P1").transform.position;
		frameImage_P1_bullet.gameObject.transform.SetParent (GameObject.Find ("bullet_1_P1").transform);

		frameImage_P1_wall.gameObject.transform.position = GameObject.Find ("wall_1_P1").transform.position;
		frameImage_P1_wall.gameObject.transform.SetParent (GameObject.Find ("wall_1_P1").transform);

		frameImage_P2_turret.gameObject.transform.position = GameObject.Find ("turret_1_P2").transform.position;
		frameImage_P2_turret.gameObject.transform.SetParent (GameObject.Find ("turret_1_P2").transform);

		frameImage_P2_bullet.gameObject.transform.position = GameObject.Find ("bullet_1_P2").transform.position;
		frameImage_P2_bullet.gameObject.transform.SetParent (GameObject.Find ("bullet_1_P2").transform);

		frameImage_P2_wall.gameObject.transform.position = GameObject.Find ("wall_1_P2").transform.position;
		frameImage_P2_wall.gameObject.transform.SetParent (GameObject.Find ("wall_1_P2").transform);

		//Pass by referene from GameObjects
		P1_Selections = GameObject.FindWithTag ("Player1");
		P2_Selections = GameObject.FindWithTag ("Player2");


		//Deactivate panels at start
		turretPanel_P1.SetActive (false);
		bulletPanel_P1.SetActive (false);
		wallPanel_P1.SetActive (false);
		cardPanel_P1.SetActive (false);

		turretPanel_P2.SetActive (false);
		bulletPanel_P2.SetActive (false);
		wallPanel_P2.SetActive (false);
		cardPanel_P2.SetActive (false);

		upgradePanel_P1.SetActive (false);
		upgradePanel_P2.SetActive (false);

		_Level_Control = GetComponent<Level_Control> ();
	}

	//calls when a turret button is pressed
	public void turretSelect ()
	{
		//Set GO to be the current selected game object by player
		GO = EventSystem.current.currentSelectedGameObject;

		if (GO.CompareTag ("TurretChoice_P1")) {

			//set the selected turret's image to turret image
			turretImageP1.transform.Find ("turret_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("turret_Preview").GetComponent<Image> ().sprite;
			//Set the temp string fireRate_P1_Str to get the current selected turret's speed text then split and store in an array
			//Save this Stat to playerPrefs to be use later
			fireRate_P1_str = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			PlayerPrefs.SetInt ("Local_turretP1_Stat", int.Parse (fireRate_P1_str [1]));


			//set the frame image to be overlayed above the current selected turret
			lastSelectedTurretPos_P1 = GO.transform.position;
			//Set the position of the frame overlay to be at the position where the current selected object is
			frameImage_P1_turret.gameObject.transform.position = lastSelectedTurretPos_P1;
			//move the frame overlay to be child of current pressed game object
			frameImage_P1_turret.gameObject.transform.SetParent (GO.transform);

			//TODO: save selected turrets last position to player prefs, its upgrade level 

		} else if (GO.CompareTag ("TurretChoice_P2")) {

			//set the selected turret's image to turret image
			turretImageP2.transform.Find ("turret_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("turret_Preview").GetComponent<Image> ().sprite;

			//Set the temp string fireRate_P2_Str to get the current selected turret's speed text then split and store in an array
			//Save this Stat to playerPrefs to be use later
			fireRate_P2_str = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			PlayerPrefs.SetInt ("Local_turretP2_Stat", int.Parse (fireRate_P2_str [1]));


			//set the frame image to be overlayed above the current selected turret
			lastSelectedTurretPos_P2 = GO.transform.position;
			//Set the position of the frame overlay to be at the position where the current selected object is
			frameImage_P2_turret.gameObject.transform.position = lastSelectedTurretPos_P2;
			//move the frame overlay to be child of current pressed game object
			frameImage_P2_turret.gameObject.transform.SetParent (GO.transform);
			//TODO: save selected turrets last position to player prefs, its upgrade level 

		}

		//update the stars according to the level of the current selected object
		//toggleStars ();
	}

	public void bulletSelect ()
	{
		//Set GO to be the current selected game object by player
		GO = EventSystem.current.currentSelectedGameObject;

		if (GO.CompareTag ("BulletChoice_P1")) {
			//set the selected turret's image to turret image
			bulletImageP1.transform.Find ("bullet_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("bullet_Preview").GetComponent<Image> ().sprite;
			//Set the temp string fireRate_P1_Str to get the current selected turret's speed text then split and store in an array
			//Save this Stat to playerPrefs to be use later
			DMG_P1_str = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			PlayerPrefs.SetInt ("Local_bulletP1_Stat", int.Parse (DMG_P1_str [1]));


			//set the frame image to be overlayed above the current selected bullet
			lastSelectedBulletPos_P1 = GO.transform.position;
			//Set the position of the frame overlay to be at the position where the current selected object is
			frameImage_P1_bullet.gameObject.transform.position = lastSelectedBulletPos_P1;
			//move the frame overlay to be child of current pressed game object
			frameImage_P1_bullet.gameObject.transform.SetParent (GO.transform);

			//TODO: save selected bullets last position to player prefs, its upgrade level 
		} else if (GO.CompareTag ("BulletChoice_P2")) {
			//set the selected turret's image to turret image
			bulletImageP2.transform.Find ("bullet_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("bullet_Preview").GetComponent<Image> ().sprite;
			//Set the temp string fireRate_P1_Str to get the current selected turret's speed text then split and store in an array
			//Save this Stat to playerPrefs to be use later
			DMG_P2_str = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			PlayerPrefs.SetInt ("Local_bulletP2_Stat", int.Parse (DMG_P2_str [1]));


			//set the frame image to be overlayed above the current selected bullet
			lastSelectedBulletPos_P2 = GO.transform.position;
			//Set the position of the frame overlay to be at the position where the current selected object is
			frameImage_P2_bullet.gameObject.transform.position = lastSelectedBulletPos_P2;
			//move the frame overlay to be child of current pressed game object
			frameImage_P2_bullet.gameObject.transform.SetParent (GO.transform);

			//TODO: save selected bullets last position to player prefs, its upgrade level 
		} 

		//update the stars according to the level of the current selected object
		//toggleStars ();

	}

	public void wallSelect ()
	{
		//Set GO to be the current selected game object by player
		GO = EventSystem.current.currentSelectedGameObject;

		if (GO.CompareTag ("WallChoice_P1")) {
			//set the selected wall's image to wall image
			wallImageP1.transform.Find ("Panel/wall_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("Panel/wall_Preview").GetComponent<Image> ().sprite;
			//Set the temp string fireRate_P1_Str to get the current selected turret's speed text then split and store in an array
			//Save this Stat to playerPrefs to be use later
			Health_P1_str = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			PlayerPrefs.SetInt ("Local_wallP1_Stat", int.Parse (Health_P1_str [1]));


			//set the frame image to be overlayed above the current selected bullet
			lastSelectedWallPos_P1 = GO.transform.position;
			//Set the position of the frame overlay to be at the position where the current selected object is
			frameImage_P1_wall.gameObject.transform.position = lastSelectedWallPos_P1;
			//move the frame overlay to be child of current pressed game object
			frameImage_P1_wall.gameObject.transform.SetParent (GO.transform);

			//TODO: save selected bullets last position to player prefs, its upgrade level 
		} else if (GO.CompareTag ("WallChoice_P2")) {
			//set the selected turret's image to turret image
			wallImageP2.transform.Find ("wall_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("Panel/wall_Preview").GetComponent<Image> ().sprite;
			//Set the temp string health_P2 to get the current selected turret's speed text then split and store in an array
			//Save this Stat to playerPrefs to be use later
			Health_P2_str = GO.transform.Find ("Stat").GetComponent<Text> ().text.Split (' ');
			PlayerPrefs.SetInt ("Local_wallP2_Stat", int.Parse (Health_P2_str [1]));


			//set the frame image to be overlayed above the current selected bullet
			lastSelectedWallPos_P2 = GO.transform.position;
			//Set the position of the frame overlay to be at the position where the current selected object is
			frameImage_P2_wall.gameObject.transform.position = lastSelectedWallPos_P2;
			//move the frame overlay to be child of current pressed game object
			frameImage_P2_wall.gameObject.transform.SetParent (GO.transform);

			//TODO: save selected bullets last position to player prefs, its upgrade level 
		} 

		//update the stars according to the level of the current selected object
		//toggleStars ();
	}

	public void CardSelect ()
	{
		//Set GO to be the current selected game object by player
		GO = EventSystem.current.currentSelectedGameObject;

		Debug.Log (GO.name);
	}

	public void togglePanels ()
	{
		//Get the name of the GameObject(GO) and set it to GO_Name string
		GO = EventSystem.current.currentSelectedGameObject;
		switch (GO.name) {
		case "Turret_Icon_P1":
			tempGO.tag = GO.tag;
			turretPanel_P1.SetActive (true);
			P1_Selections.SetActive (false);
			confirmedButton_P1.gameObject.SetActive (false);
			frameImage_P1_turret.gameObject.SetActive (true);
			break;
		case "Bullet_Icon_P1":
			tempGO.tag = GO.tag;
			bulletPanel_P1.SetActive (true);
			P1_Selections.SetActive (false);
			confirmedButton_P1.gameObject.SetActive (false);
			frameImage_P1_bullet.gameObject.SetActive (true);
			break;
		case "Wall_Icon_P1":
			tempGO.tag = GO.tag;
			wallPanel_P1.SetActive (true);
			P1_Selections.SetActive (false);
			confirmedButton_P1.gameObject.SetActive (false);
			frameImage_P1_wall.gameObject.SetActive (true);
			break;
		case "Card_Icon_P1":
			cardPanel_P1.SetActive (true);
			P1_Selections.SetActive (false);
			confirmedButton_P1.gameObject.SetActive (false);
			break;
		case "Turret_Icon_P2":
			tempGO.tag = GO.tag;
			turretPanel_P2.SetActive (true);
			P2_Selections.SetActive (false);
			confirmedButton_P2.gameObject.SetActive (false);
			frameImage_P2_turret.gameObject.SetActive (true);
			break;
		case "Bullet_Icon_P2":
			tempGO.tag = GO.tag;
			bulletPanel_P2.SetActive (true);
			P2_Selections.SetActive (false);
			confirmedButton_P2.gameObject.SetActive (false);
			frameImage_P2_bullet.gameObject.SetActive (true);
			break;
		case "Wall_Icon_P2":
			tempGO.tag = GO.tag;
			wallPanel_P2.SetActive (true);
			P2_Selections.SetActive (false);
			confirmedButton_P2.gameObject.SetActive (false);
			frameImage_P2_wall.gameObject.SetActive (true);
			break;
		case "Card_Icon_P2":
			cardPanel_P2.SetActive (true);
			P2_Selections.SetActive (false);
			confirmedButton_P2.gameObject.SetActive (false);
			break;
		case "Confirm Button_P1":
			P1_Selections.SetActive (true);
			confirmedButton_P1.gameObject.SetActive (true);

			turretPanel_P1.SetActive (false);
			bulletPanel_P1.SetActive (false);
			wallPanel_P1.SetActive (false);
			cardPanel_P1.SetActive (false);

			//Set the frames to false
			frameImage_P1_turret.gameObject.SetActive (false);
			frameImage_P1_bullet.gameObject.SetActive (false);
			frameImage_P1_wall.gameObject.SetActive (false);
			break;
		case "Confirm Button_P2":
			P2_Selections.SetActive (true);
			confirmedButton_P2.gameObject.SetActive (true);

			turretPanel_P2.SetActive (false);
			bulletPanel_P2.SetActive (false);
			wallPanel_P2.SetActive (false);

			//Set the frames to false
			frameImage_P2_turret.gameObject.SetActive (false);
			frameImage_P2_bullet.gameObject.SetActive (false);
			frameImage_P2_wall.gameObject.SetActive (false);
			break;
		case "UCButton": //upgrade confirm button
			
			if (tempGO.CompareTag ("TurretChoice_P1")) {
				//set the upgrade panel p1 to false and turret panel to true 
				upgradePanel_P1.SetActive (false);
				turretPanel_P1.SetActive (true);
			} else if (tempGO.CompareTag ("BulletChoice_P1")) {
				//set the upgrade panel p1 to false and bullet panel to true 
				upgradePanel_P1.SetActive (false);
//				bulletPanel_P1.SetActive (true);
			} else if (tempGO.CompareTag ("WallChoice_P1")) {
				//set the upgrade panel p1 to false and wall panel to true 
				upgradePanel_P1.SetActive (false);
				wallPanel_P1.SetActive (true);
			}	

			if (tempGO.CompareTag ("TurretChoice_P2")) {
				//set the upgrade panel p1 to false and turret panel to true 
				upgradePanel_P2.SetActive (false);
				turretPanel_P2.SetActive (true);
			} else if (tempGO.CompareTag ("BulletChoice_P2")) {
				//set the upgrade panel p1 to false and bullet panel to true 
				upgradePanel_P2.SetActive (false);
				bulletPanel_P2.SetActive (true);
			} else if (tempGO.CompareTag ("WallChoice_P2")) {
				//set the upgrade panel p1 to false and wall panel to true 
				upgradePanel_P2.SetActive (false);
				wallPanel_P2.SetActive (true);
			}	
			break;
		case "UBButton": //upgrade back button
			if (tempGO.CompareTag ("TurretChoice_P1")) {
				//set the upgrade panel p1 to false and turret panel to true 
				upgradePanel_P1.SetActive (false);
				turretPanel_P1.SetActive (true);
			} else if (tempGO.CompareTag ("BulletChoice_P1")) {
				//set the upgrade panel p1 to false and bullet panel to true 
				upgradePanel_P1.SetActive (false);
				bulletPanel_P1.SetActive (true);
			} else if (tempGO.CompareTag ("WallChoice_P1")) {
				//set the upgrade panel p1 to false and wall panel to true 
				upgradePanel_P1.SetActive (false);
				wallPanel_P1.SetActive (true);
			}	

			if (tempGO.CompareTag ("TurretChoice_P2")) {
				//set the upgrade panel p1 to false and turret panel to true 
				upgradePanel_P2.SetActive (false);
				turretPanel_P2.SetActive (true);
			} else if (tempGO.CompareTag ("BulletChoice_P2")) {
				//set the upgrade panel p1 to false and bullet panel to true 
				upgradePanel_P2.SetActive (false);
				bulletPanel_P2.SetActive (true);
			} else if (tempGO.CompareTag ("WallChoice_P2")) {
				//set the upgrade panel p1 to false and wall panel to true 
				upgradePanel_P2.SetActive (false);
				wallPanel_P2.SetActive (true);
			}
			break;
		}
	}

	public void toggleStars ()
	{
		//Get the GameObject(GO) and set it to GO_Name string
		GO = EventSystem.current.currentSelectedGameObject;

		//get the current Lv of the selected object and save to a temp int
		int tempInt = 0;
		string[] tempString = GO.transform.Find ("Level").GetComponent<Text> ().text.Split (' ');
		tempInt = int.Parse (tempString [1]);

		//switch case to display the number of stars according to the tempInt
		switch (tempInt) {
		case 1:
			GO.transform.Find ("selectedFrame/Stars/stars_1").gameObject.SetActive (true);
			GO.transform.Find ("selectedFrame/Stars/stars_2").gameObject.SetActive (false);
			GO.transform.Find ("selectedFrame/Stars/stars_3").gameObject.SetActive (false);
			break;
		case 2:
			GO.transform.Find ("selectedFrame/Stars/stars_1").gameObject.SetActive (true);
			GO.transform.Find ("selectedFrame/Stars/stars_2").gameObject.SetActive (true);
			GO.transform.Find ("selectedFrame/Stars/stars_3").gameObject.SetActive (false);
			break;
		case 3:
			GO.transform.Find ("selectedFrame/Stars/stars_1").gameObject.SetActive (true);
			GO.transform.Find ("selectedFrame/Stars/stars_2").gameObject.SetActive (true);
			GO.transform.Find ("selectedFrame/Stars/stars_3").gameObject.SetActive (true);
			break;
		default:
			Debug.Log ("number not in case: " + tempInt);
			return;
		}
	}

	public void upgradeFunction ()
	{
		//hides the other panels, show the respective player's upgrade panel
		if (tempGO.CompareTag ("TurretChoice_P1") || tempGO.CompareTag ("BulletChoice_P1") || tempGO.CompareTag ("WallChoice_P1")) {
			turretPanel_P1.SetActive (false);
			bulletPanel_P1.SetActive (false);
			wallPanel_P1.SetActive (false);
			upgradePanel_P1.SetActive (true);
		} else if (tempGO.CompareTag ("TurretChoice_P2") || tempGO.CompareTag ("BulletChoice_P2") || tempGO.CompareTag ("WallChoice_P2")) {
			turretPanel_P2.SetActive (false);
			bulletPanel_P2.SetActive (false);
			wallPanel_P2.SetActive (false);
			upgradePanel_P2.SetActive (true);
		}

		Debug.Log ("wall via findchild: " + GO.transform.FindChild("Panel/wall_Preview"));
		Debug.Log ("wall via find: " + GO.transform.Find("Panel/wall_Preview"));
		Debug.Log (GO.name);

		//set the image of upgradable object, its stats and cost based on select
		if (GO.CompareTag ("TurretChoice_P1")) {
			upgradeImageDisplay_P1.GetComponent<Image> ().sprite = GO.transform.Find ("turret_Preview").GetComponent<Image> ().sprite;
		} else if (GO.CompareTag ("BulletChoice_P1")) {
			upgradeImageDisplay_P1.GetComponent<Image> ().sprite = GO.transform.Find ("bullet_Preview").GetComponent<Image> ().sprite;
		} else if (GO.CompareTag ("WallChoice_P1")) {
			Debug.Log ("works");
			//upgradeImageDisplay_P1.GetComponent<Image> ().sprite = GO.transform.Find ("wall_Preview").GetComponent<Image> ().sprite;
		}
		if (GO.CompareTag ("TurretChoice_P2")) {
			upgradeImageDisplay_P2.GetComponent<Image> ().sprite = GO.transform.Find ("turret_Preview").GetComponent<Image> ().sprite;
		} else if (GO.CompareTag ("BulletChoice_P2")) {
			upgradeImageDisplay_P2.GetComponent<Image> ().sprite = GO.transform.Find ("bullet_Preview").GetComponent<Image> ().sprite;
		} else if (GO.CompareTag ("WallChoice_P2")) {
			upgradeImageDisplay_P2.GetComponent<Image> ().sprite = GO.transform.Find ("wall_Preview").GetComponent<Image> ().sprite;
		}
			
		

	}

	public void checkConfirmation ()
	{
		//Get the GameObject(GO) and set it to GO_Name string
		GO = EventSystem.current.currentSelectedGameObject;

		//Check for player 1 confirmation
		if (GO.name == "confirmButton_P1" && !isConfirmed_P1) {
			isConfirmed_P1 = true;
			GO.gameObject.GetComponent<Image> ().sprite = confirmed_sprite_P1 [1];

			Component[] tempComponent;
			tempComponent = P1_Selections.GetComponentsInChildren<Button> ();
			foreach (Button buttonComp in tempComponent) {
				buttonComp.interactable = false;
			}
		} else if (GO.name == "confirmButton_P1" && isConfirmed_P1) {
			isConfirmed_P1 = false;
			GO.gameObject.GetComponent<Image> ().sprite = confirmed_sprite_P1 [0];

			Component[] tempComponent;
			tempComponent = P1_Selections.GetComponentsInChildren<Button> ();
			foreach (Button buttonComp in tempComponent) {
				buttonComp.interactable = true;
			}
		}

		//check for player 2 confirmation
		if (GO.name == "confirmButton_P2" && !isConfirmed_P2) {
			isConfirmed_P2 = true;
			GO.gameObject.GetComponent<Image> ().sprite = confirmed_sprite_P2 [1];

			Component[] tempComponent;
			tempComponent = P2_Selections.GetComponentsInChildren<Button> ();
			foreach (Button buttonComp in tempComponent) {
				buttonComp.interactable = false;
			}
		} else if (GO.name == "confirmButton_P2" && isConfirmed_P2) {
			isConfirmed_P2 = false;
			GO.gameObject.GetComponent<Image> ().sprite = confirmed_sprite_P2 [0];
			Component[] tempComponent;
			tempComponent = P2_Selections.GetComponentsInChildren<Button> ();
			foreach (Button buttonComp in tempComponent) {
				buttonComp.interactable = true;
			}
		}

		//goes to main gameplay after both player has confirmed their selection
		if (isConfirmed_P1 && isConfirmed_P2) {
			_Level_Control.loadLocal2P ();
		}
	}
}
