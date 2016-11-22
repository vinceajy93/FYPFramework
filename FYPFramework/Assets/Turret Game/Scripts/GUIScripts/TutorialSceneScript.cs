using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialSceneScript : MonoBehaviour {

	//for animation of the ship
	[SerializeField]
	private GameObject ship_anim;
	[SerializeField]
	private GameObject[] Panels;
	//Non-serialized fields
	private static float setTime = 2.0f; //time before the ship starts flying off the screen 
	private float tick;
	private bool shipAtOriginalPos = false;

	// Use this for initialization
	void Start () {
		//set all panels to active for any start codes to work
		foreach(GameObject panels in Panels){
			panels.SetActive (true);
		}

		tick = 0;

		//set all panels to inactive 
		foreach(GameObject panels in Panels){
			if (panels.name == "tut_1_Panel")
				continue;
			panels.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () {
		tick += Time.deltaTime;

		//do the animation for the plane_anim when the tick is higher than the set time
		if(tick > setTime){
			ship_anim.gameObject.transform.Translate (2.0f, 0, 0);
			if (ship_anim.transform.localPosition.x >= 510 && !shipAtOriginalPos) {
				ship_anim.transform.localPosition = new Vector2 (-600, ship_anim.transform.localPosition.y);
				shipAtOriginalPos = true;
			}
			//reset the tick back to 0 when the ship is back to its original position
			else if(ship_anim.transform.localPosition.x >= -235 && shipAtOriginalPos){
				tick = 0;
				ship_anim.transform.localPosition = new Vector2 (-235, ship_anim.transform.localPosition.y);
				shipAtOriginalPos = false;
			}
		}
	}
}
