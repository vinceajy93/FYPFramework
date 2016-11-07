using UnityEngine;
using System.Collections;

public class Mode_Control : MonoBehaviour {

	// True - Single, False - Multiplayer
	public bool game_mode_Single;
	public bool move_player_P1 = true;
	public bool move_player_P2 = true;
	public bool card_menu_P1 = false;
	public bool card_menu_P2 = false;

	private string turret_name_P1;
	private string turret_name_P2;
	private string turret_name_E;

	private GameObject Spawn_P1;
	private GameObject Spawn_P2;
	private GameObject Spawn_E;
	// Use this for initialization
	void Start () {
		//single player mode 
		if (GameObject.Find ("Top Camera") == null && GameObject.Find ("Bottom Camera") == null) {
			game_mode_Single = true;

			if (PlayerPrefs.HasKey ("S_P1_T"))
				turret_name_P1 = "Turret/Turret_" + PlayerPrefs.GetInt("LM_P1_T").ToString();
			else
				turret_name_P1 = "Turret/Turret_" + "5";
			
			Spawn_P1 = GameObject.FindGameObjectWithTag ("Player1");
			GameObject clone_P1 = Instantiate(Resources.Load(turret_name_P1)) as GameObject;
			clone_P1.transform.position = Spawn_P1.transform.position + clone_P1.transform.position;
			clone_P1.transform.rotation = Spawn_P1.transform.rotation;
			clone_P1.layer = LayerMask.NameToLayer("Player 1");
			clone_P1.transform.SetParent(Spawn_P1.transform);
			clone_P1.tag = "Player1";

			if (PlayerPrefs.HasKey ("S_E_T"))
				turret_name_E = "Turret/Enemy_" + PlayerPrefs.GetInt("LM_E_T").ToString();
			else 
				turret_name_E = "Turret/Enemy_" + "1";
			
			Spawn_E = GameObject.FindGameObjectWithTag ("Enemy");
			GameObject clone_E = Instantiate(Resources.Load(turret_name_E)) as GameObject;
			clone_E.transform.position = Spawn_E.transform.position - clone_E.transform.position;
			clone_E.transform.rotation = Spawn_E.transform.rotation;
			clone_E.layer = LayerMask.NameToLayer("Enemy");
			clone_E.transform.SetParent(Spawn_E.transform);
			clone_E.tag = "Enemy";
		}
		//multiplayer mode 
		else {
			game_mode_Single = false;

			if (PlayerPrefs.HasKey ("LM_P1_T"))
				turret_name_P1 = "Turret/Turret_" + PlayerPrefs.GetInt("LM_P1_T").ToString();
			else
				turret_name_P1 = "Turret/Turret_" + "1";
			
			Spawn_P1 = GameObject.FindGameObjectWithTag ("Player1");
			GameObject clone_P1 = Instantiate(Resources.Load(turret_name_P1)) as GameObject;
			clone_P1.transform.position = Spawn_P1.transform.position + clone_P1.transform.position;
			clone_P1.transform.rotation = Spawn_P1.transform.rotation;
			clone_P1.layer = LayerMask.NameToLayer("Player 1");
			clone_P1.transform.SetParent(Spawn_P1.transform);
			clone_P1.tag = "Player1";

			if (PlayerPrefs.HasKey ("LM_P2_T"))
				turret_name_P2 = "Turret/Turret_" + PlayerPrefs.GetInt("LM_P2_T").ToString();
			else
				turret_name_P2 = "Turret/Turret_" + "2";
			
			Spawn_P2 = GameObject.FindGameObjectWithTag ("Player2");
			GameObject clone_P2 = Instantiate(Resources.Load(turret_name_P2)) as GameObject;
			clone_P2.transform.position = Spawn_P2.transform.position - clone_P2.transform.position;
			clone_P2.transform.rotation = Spawn_P2.transform.rotation;
			clone_P2.layer = LayerMask.NameToLayer("Player 2");
			clone_P2.transform.SetParent(Spawn_P2.transform);
			clone_P2.tag = "Player2";
		}
	}
}
