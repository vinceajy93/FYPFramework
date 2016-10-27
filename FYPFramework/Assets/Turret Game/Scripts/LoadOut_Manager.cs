using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadOut_Manager : MonoBehaviour {
	

	[SerializeField]
	private GameObject turretPanel_P1, turretPanel_P2, bulletPanel_P1, bulletPanel_P2, wallPanel_P1, wallPanel_P2;
	[SerializeField]
	private Image turretImageP1, turretImageP2, bulletImageP1, bulletImageP2, wallImageP1, wallImageP2, frameImage_P1;

	private GameObject P1_Selections, P2_Selections, GO;
	private Vector3 lastSelectedTurretPos_P1, lastSelectedBulletPos_P1;
	private Text fireRate_P1_txt, fireRate_P2_txt, Health_P1_txt, Health_P2_txt, DMG_P1_txt, DMG_P2_txt;
	private string[] fireRate_P1_str, fireRate_P2_str, Health_P1_str, Health_P2_str, DMG_P1_str, DMG_P2_str;

	// Use this for initialization
	void Start () { 
		//dont show the frame until the panels are active
		//frameImage_P1.gameObject.SetActive (false);
		//lastSelectedBulletPos_P1 = 

		//frameImage_P1.gameObject.transform.position = GameObject.Find("turret_1_P1").transform.position;
		//frameImage_P1.gameObject.transform.SetParent(GameObject.Find("turret_1_P1").transform);

		//Pass by referene from GameObjects
		P1_Selections = GameObject.FindWithTag("Player1");
		P2_Selections = GameObject.FindWithTag ("Player2");


		//Deactivate panels at start
		turretPanel_P1.SetActive (false);
		turretPanel_P2.SetActive (false);

		bulletPanel_P1.SetActive (false);
		bulletPanel_P2.SetActive (false);

		wallPanel_P1.SetActive (false);
		wallPanel_P2.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		//if (lastSelectedTurretPos_P1 == Vector3.zero && GameObject.Find ("turret_1_P1").gameObject.activeSelf == true)
			//Debug.Log ("enters here");
			//			frameImage_P1.gameObject.transform.position = GameObject.Find("turret_1_P1").transform.position;
	}

	//calls when a turret button is pressed
	public void turretSelect(){
		//Set GO to be the current selected game object by player
		GO = EventSystem.current.currentSelectedGameObject;

		if(GO.CompareTag("TurretChoice_P1")){
			//set the selected turret's image to turret image
			turretImageP1.transform.Find ("turret_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("turret_Preview").GetComponent<Image> ().sprite;
			//Set the temp string fireRate_P1_Str to get the current selected turret's speed text then split and store in an array
			//Save this Stat to playerPrefs to be use later
			fireRate_P1_str = GO.transform.Find("Stat").GetComponent<Text>().text.Split(' ');

			//set the frame image to be overlayed above the current selected turret
			lastSelectedTurretPos_P1 = GO.transform.position;
			//Set the position of the frame overlay to be at the position where the current selected object is
			frameImage_P1.gameObject.transform.position = lastSelectedTurretPos_P1;
			//move the frame overlay to be child of current pressed game object
			frameImage_P1.gameObject.transform.SetParent(GO.transform);

			//Set the turret stats
			//fireRate_P1 = 

		} 
		else if(GO.CompareTag("TurretChoice_P2")){
			//set the selected turret's image to turret image
			turretImageP2.transform.Find ("turret_Preview").GetComponent<Image> ().sprite = GO.transform.Find ("turret_Preview").GetComponent<Image> ().sprite;
		}



	}
	public void bulletSelect(){
	}
	public void wallSelect(){
	}

	public void togglePanels(){
		//Get the name of the GameObject(GO) and set it to GO_Name string
		GO = EventSystem.current.currentSelectedGameObject;

		switch(GO.name){
		case "Turret_Icon_P1":
			turretPanel_P1.SetActive (true);
			P1_Selections.SetActive (false);

			frameImage_P1.gameObject.SetActive (true);
			break;
		case "Bullet_Icon_P1":
			bulletPanel_P1.SetActive (true);
			P1_Selections.SetActive (false);
			break;
		case "Wall_Icon_P1":
			wallPanel_P1.SetActive (true);
			P1_Selections.SetActive (false);
			break;
		case "Turret_Icon_P2":
			turretPanel_P2.SetActive (true);
			P2_Selections.SetActive (false);
			break;
		case "Bullet_Icon_P2":
			bulletPanel_P2.SetActive (true);
			P2_Selections.SetActive (false);
			break;
		case "Wall_Icon_P2":
			wallPanel_P2.SetActive (true);
			P2_Selections.SetActive (false);
			break;
		case "Confirm Button_P1":
			//for testing
			turretPanel_P1.SetActive (false);
			bulletPanel_P1.SetActive (false);
			wallPanel_P1.SetActive (false);
			P1_Selections.SetActive (true);

			frameImage_P1.gameObject.SetActive (false);

			break;
		case "Confirm Button_P2":
			Debug.Log ("entered");
			turretPanel_P2.SetActive (false);
			bulletPanel_P2.SetActive (false);
			wallPanel_P2.SetActive (false);
			P2_Selections.SetActive (true);
			break;
		}
	}
}
