using UnityEngine;
using System.Collections;

public class Mode_Control : MonoBehaviour {

	// True - Single, False - Multiplayer
	public bool game_mode_Single;
	public bool move_player_P1 = true;
	public bool move_player_P2 = true;
	public bool card_menu_P1 = false;
	public bool card_menu_P2 = false;

	// Use this for initialization
	void Start () {
		//single player mode 
		if (GameObject.Find ("Top Camera") == null && GameObject.Find ("Bottom Camera") == null) {
			game_mode_Single = true;
		}
		//multiplayer mode 
		else {
			game_mode_Single = false; 
		}
	}
}
