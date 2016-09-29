using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Overlay_Control : MonoBehaviour {


	public static Player_Control pControl;

	private GameObject panel;
	private Text p1Timer;
	private Text p2Timer;
	float countdownTimer = 3.0f;
	public bool PanelisActive = true;

	// Use this for initialization
	void Start () {
		//panel
		panel = GameObject.Find("panel overlay");

		if (GameObject.Find ("Top Camera") == null && GameObject.Find ("Bottom Camera") == null) {
			pControl.game_mode_Single = true;
		} else {
			p1Timer = GameObject.Find ("P1 Timer").GetComponent<Text> ();
			p2Timer = GameObject.Find ("P2 Timer").GetComponent<Text> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//countdown timer
		countdownTimer -= Time.deltaTime;
		//Set Panel to false when timer hits 0
		if (countdownTimer <= 0)
			PanelisActive = false;

		Panel ();

	}

	void Panel(){
		if (PanelisActive == true) {
			//set timer for text in panel
			p1Timer.text = countdownTimer.ToString("f0");
			p2Timer.text = p1Timer.text;

		} else {
			//Debug.Log ("works");
			panel.SetActive (false);
		}
	}
}
