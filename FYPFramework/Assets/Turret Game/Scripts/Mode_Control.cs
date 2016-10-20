using UnityEngine;
using System.Collections;

public class Mode_Control : MonoBehaviour {

	// True - Single, False - Multiplayer
	public bool game_mode_Single;
	public bool move_player = true;

	// Use this for initialization
	void Start () {
		//single player mode 
		if (GameObject.Find ("Top Camera") == null && GameObject.Find ("Bottom Camera") == null) {
			game_mode_Single = true;
			Debug.Log ("Single");
		}
		//multiplayer mode 
		else {
			game_mode_Single = false; 
		}
	}

}
