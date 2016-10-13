using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseScript : MonoBehaviour {

	[HideInInspector]
	public bool Paused;

	private bool pausePressed_P1 = false;
	private bool pausePressed_P2 = false;

	private string GO_Name;

	public Button _Pause_P1;
	public Button _Pause_P2;

	private Overlay_Control _OverlayControl;
	// Use this for initialization
	void Start () {

		_OverlayControl = GetComponent<Overlay_Control> ();
		Paused = true;
	}
	
	// Update is called once per frame
	void Update () {

		//if both players pressed the pause button
		if (pausePressed_P1 == true && pausePressed_P2 == true) {
			Paused = true;
			//Reactivate the overlay panel
			_OverlayControl.panel.SetActive (true);

			//reset the timer
			_OverlayControl.countdownTimer = _OverlayControl.setTimerTime;

		}
	}
		
	//on pointer down (check if players is holding down pause button)
	public void isPress(){

		GO_Name = EventSystem.current.currentSelectedGameObject.name;
		//TODO: Added single player feature 

		//multiplayer
		if (GO_Name == _Pause_P1.name) 
			pausePressed_P1 = true;

		if (GO_Name == _Pause_P2.name) 
			pausePressed_P2 = true;
			
	}

	//on pointer up (reset pausePressed booleans when button released)
	public void isReleased(){
		//if (GO_Name == _Pause_P1.name)
			pausePressed_P1 = false;

		//if (GO_Name == _Pause_P2.name)
			pausePressed_P2 = false;
	}
}
