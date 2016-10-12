using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadOut_Manager : MonoBehaviour {
	

	[SerializeField]
	private GameObject turretPanel_P1;
	[SerializeField]
	private GameObject turretPanel_P2;
	[SerializeField]
	private GameObject bulletPanel_P1;
	[SerializeField]
	private GameObject bulletPanel_P2;
	[SerializeField]
	private GameObject wallPanel_P1;
	[SerializeField]
	private GameObject wallPanel_P2;

	private GameObject P1_Selections;
	private GameObject P2_Selections;

	private string GO_Name;

	// Use this for initialization
	void Start () { 
		//Pass by referene from GameObjects
		//turretPanel_P1 = GameObject.Find("turretPanel_P1");

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
		//for debugging
		if (Input.GetKeyDown (KeyCode.W)) {

		}
	}

	public void togglePanels(){
		//Get the name of the GameObject(GO) and set it to GO_Name string
		GO_Name = EventSystem.current.currentSelectedGameObject.name;


		switch(GO_Name){
		case "Turret_Icon_P1":
			turretPanel_P1.SetActive (true);
			P1_Selections.SetActive (false);
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
			break;
		case "Confirm Button_P2":
			turretPanel_P2.SetActive (false);
			bulletPanel_P2.SetActive (false);
			wallPanel_P2.SetActive (false);
			P2_Selections.SetActive (true);
			break;
		}
	}
}
