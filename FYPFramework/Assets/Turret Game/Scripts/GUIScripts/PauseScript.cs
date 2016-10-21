using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseScript : MonoBehaviour
{

	[HideInInspector]
	public bool Paused;

	private bool pausePressed_P1 = false;
	private bool pausePressed_P2 = false;

	private string GO_Name;

	[SerializeField]
	private Button _Pause_P1, _Pause_P2;


	private Overlay_Control _OverlayControl;
	// Use this for initialization
	void Start ()
	{
		_OverlayControl = GetComponent<Overlay_Control> ();
		Paused = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//single player
		if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
			//hide the pause button when the overlay panel is active
			if (_OverlayControl.PanelisActive == true)
				_Pause_P1.transform.localScale = Vector3.zero;
			else
				_Pause_P1.transform.localScale = new Vector3 (0.4f, 0.4f, 1.0f);
			
			//Set panel active if paused button is pressed
			if (pausePressed_P1 == true) {
				Paused = true;
				//Reactivate the overlay panel
				_OverlayControl.panel.SetActive (true);

			}
		}
		//multiplayer 
		else {
			//hide the pause button when the overlay panel is active
			if (_OverlayControl.PanelisActive == true) {
				_Pause_P1.transform.localScale = Vector3.zero;
				_Pause_P2.transform.localScale = Vector3.zero;
			} else {
				_Pause_P1.transform.localScale = new Vector3 (0.4f, 0.4f, 1.0f);
				_Pause_P2.transform.localScale = new Vector3 (0.4f, 0.4f, 1.0f);
			}
	
			//if both players pressed the pause button
			if (pausePressed_P1 == true && pausePressed_P2 == true) {
				Paused = true;
				//Reactivate the overlay panel
				_OverlayControl.panel.SetActive (true);

			}

		}
	}
		
	//on pointer down (check if players is holding down pause button)
	public void isPress ()
	{

		GO_Name = EventSystem.current.currentSelectedGameObject.name;
		//single player 
		if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
			if (GO_Name == _Pause_P1.name)
				pausePressed_P1 = true;

		} else {
			//multiplayer
			if (GO_Name == _Pause_P1.name)
				pausePressed_P1 = true;

			if (GO_Name == _Pause_P2.name)
				pausePressed_P2 = true;


		}	
	}

	//on pointer up (reset pausePressed booleans when button released)
	public void isReleased ()
	{
		//if (GO_Name == _Pause_P1.name)
		pausePressed_P1 = false;

		//if (GO_Name == _Pause_P2.name)
		pausePressed_P2 = false;


	}
}
