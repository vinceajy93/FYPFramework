using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Level_Control : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	//load the main menu scene
	public void loadMenu(){
		SceneManager.LoadScene ("Main Menu");
	}

	//load the play scene
	public void loadModeSelect(){
		SceneManager.LoadScene ("Mode Selection");
	}

	//load the campaign scene
	public void loadCampaign(){
		SceneManager.LoadScene ("Arcade");
	}
	//load the loadout scene
	public void loadLoadout(){
		SceneManager.LoadScene ("Loadout");
	}

	//load the logbook scene
	public void loadLogbook(){
		SceneManager.LoadScene ("Logbook");
	}

	//load the tutorial selection scene
	public void loadTutorialSelect(){
		SceneManager.LoadScene ("Tutorial Selection");
	}

	//load the options scene
	public void loadOptions(){
		SceneManager.LoadScene ("Options");
	}

	//load the Stage select scene
	public void loadStageSelect(){
		SceneManager.LoadScene ("Stage_Select");
	}

	//Local 2 - player
	public void loadLocal2P(){
		SceneManager.LoadScene ("Local_Multiplayer");
	}

	//loadout localplayer select
	public void loadLoadout2pSelect(){
		SceneManager.LoadScene ("Loadout Select");
	}

	//loadout localplayer select
	public void loadLoseWinScene(){
		SceneManager.LoadScene ("LoseWinScene");
	}

	//for testing purpose
	public void loadTest(){
		SceneManager.LoadScene ("test scene");
	}


}
