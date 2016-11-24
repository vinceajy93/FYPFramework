using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TutorialSceneScript : MonoBehaviour
{

	//for animation of the ship
	[SerializeField]
	private GameObject ship_anim;
	[SerializeField]
	private GameObject[] Panels;

	[SerializeField]
	private Image[] turrets;
	[SerializeField]
	private Image[] Gesture;

	//Non-serialized fields
	private static float setTime = 2.0f;
	//time before the ship starts flying off the screen
	private float tick;
	private bool shipAtOriginalPos = false;

	private GameObject GO;
	[SerializeField]
	private GameObject[] bullet;

	//countdown timer for animation
	private float countdownTimer;
	private static float countdownTresher = 0.5f;

	//boolean(s) for animations
	private bool Boo;

	// Use this for initialization
	void Start ()
	{
		//Initializations
		countdownTimer = 0f;
		Boo = false;

		//set all panels to active for any start codes to work
		foreach (GameObject panels in Panels) {
			panels.SetActive (true);
		}

		tick = 0;

		//set all panels to inactive 
		foreach (GameObject panels in Panels) {
			if (panels.name == "tut_2_Panel")
				continue;
			panels.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		tick += Time.deltaTime;

		//do the animation for the plane_anim when the tick is higher than the set time
		if (tick > setTime) {
			ship_anim.gameObject.transform.Translate (2.0f, 0, 0);
			if (ship_anim.transform.localPosition.x >= 510 && !shipAtOriginalPos) {
				ship_anim.transform.localPosition = new Vector2 (-600, ship_anim.transform.localPosition.y);
				shipAtOriginalPos = true;
			}
			//reset the tick back to 0 when the ship is back to its original position
			else if (ship_anim.transform.localPosition.x >= -235 && shipAtOriginalPos) {
				tick = 0;
				ship_anim.transform.localPosition = new Vector2 (-235, ship_anim.transform.localPosition.y);
				shipAtOriginalPos = false;
			}
		}

		//Update the countdownTimer 
		countdownTimer += Time.deltaTime;

		if (countdownTimer > countdownTresher) {
			if (Boo)
				Boo = false;
			else
				Boo = true;

			//Reset the countdown timer 
			countdownTimer = 0f;	
		}

		TutorialAnimations (Boo);
	}

	void TutorialAnimations(bool boo){

		//tutorial 1 panel
		if (Panels [0].activeSelf) {
			if (boo) {	
				Gesture [0].transform.Translate (2.5f, 0f, 0f);
				turrets [0].transform.Translate (1.5f, 0f, 0f);
			} else {
				Gesture [0].transform.Translate (-2.5f, 0f, 0f);
				turrets [0].transform.Translate (-1.5f, 0f, 0f);
			}
		} else if (Panels [1].activeSelf) {

			bool tempBool = false;

			if (boo) {	
				bullet [0].SetActive (true);
				Gesture [1].transform.position = new Vector3 (Gesture [1].transform.position.x, Gesture [1].transform.position.y + .5f, 0f);
			} else {
				Gesture [1].transform.position = new Vector3 (Gesture [1].transform.position.x, Gesture [1].transform.position.y - .5f, 0f);
			}

			if (bullet [0].activeSelf) {
				bullet [0].transform.Translate (0f, 2f, 0f);
			}

			if (bullet [0].transform.position.y >= 200f && tempBool == false) {
				tempBool = true;
				bullet [0].transform.position = (turrets [1].transform.position + new Vector3 (0, 1.5f, 0));
			}	
		} else if (Panels [2].activeSelf) {
			if (boo) {	
				Gesture [2].transform.position = new Vector3 (Gesture [2].transform.position.x, Gesture [2].transform.position.y + 1f, 0f);
				Gesture [3].transform.position = new Vector3 (Gesture [3].transform.position.x, Gesture [3].transform.position.y - 1f, 0f);
				bullet [1].SetActive (true);
				bullet [2].SetActive (false);
			} else {
				Gesture [2].transform.position = new Vector3 (Gesture [2].transform.position.x, Gesture [2].transform.position.y -1f, 0f);
				Gesture [3].transform.position = new Vector3 (Gesture [3].transform.position.x, Gesture [3].transform.position.y + 1f, 0f);
				bullet [1].SetActive (false);
				bullet [2].SetActive (true);
			}
		} else if (Panels [4].activeSelf) {
			if (boo) {	
				turrets [2].transform.position = new Vector3 (turrets [2].transform.position.x +2f, turrets [2].transform.position.y, 0f);
				turrets [3].transform.position = new Vector3 (turrets [3].transform.position.x -2f, turrets [3].transform.position.y, 0f);
			} else {
				turrets [2].transform.position = new Vector3 (turrets [2].transform.position.x -2f, turrets [2].transform.position.y, 0f);
				turrets [3].transform.position = new Vector3 (turrets [3].transform.position.x +2f, turrets [3].transform.position.y, 0f);
			}
		}


	}

	public void TogglePanelFunction ()
	{
		//Set GameObject to be the current selected one by player
		GO = EventSystem.current.currentSelectedGameObject;

		switch (GO.name) {
		case "Tutorial_1":
			foreach (GameObject panels in Panels) {
				if (panels.name == "tut_1_Panel")
					panels.SetActive (true);
				else
					panels.SetActive (false);
			}
			break;
		case "Tutorial_2":
			foreach (GameObject panels in Panels) {
				if (panels.name == "tut_2_Panel")
					panels.SetActive (true);
				else
					panels.SetActive (false);
			}
			break;
		case "Tutorial_3":
			foreach (GameObject panels in Panels) {
				if (panels.name == "tut_3_Panel")
					panels.SetActive (true);
				else
					panels.SetActive (false);
			}
			break;
		case "Tutorial_4":
			foreach (GameObject panels in Panels) {
				if (panels.name == "tut_4_Panel")
					panels.SetActive (true);
				else
					panels.SetActive (false);
			}
			break;
		case "Tutorial_5":
			foreach (GameObject panels in Panels) {
				if (panels.name == "tut_5_Panel")
					panels.SetActive (true);
				else
					panels.SetActive (false);
			}
			break;
		case "Tutorial_6":
			foreach (GameObject panels in Panels) {
				if (panels.name == "tut_6_Panel")
					panels.SetActive (true);
				else
					panels.SetActive (false);
			}
			break;
		case "Tutorial_7":
			foreach (GameObject panels in Panels) {
				if (panels.name == "tut_7_Panel")
					panels.SetActive (true);
				else
					panels.SetActive (false);
			}
			break;
		}
	}
}
