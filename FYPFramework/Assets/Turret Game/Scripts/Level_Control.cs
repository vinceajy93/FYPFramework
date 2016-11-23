using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Level_Control : MonoBehaviour {

	private Slider _SFXSlider;
	private Toggle _SFXToggle;

	private float vol;

	// Use this for initialization
	void Start () {
		if (GameObject.Find ("Slider SFX") != null)
			_SFXSlider = GameObject.Find ("Slider SFX").GetComponent<Slider>();
		if (GameObject.Find ("Toggle SFX") != null)
			_SFXToggle = GameObject.Find ("Toggle SFX").GetComponent<Toggle>();
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

	public void SFXSliderFunction(){
		
		if (PlayerPrefs.HasKey ("vol"))
			vol = PlayerPrefs.GetFloat ("vol");
		else
			vol = 1.0f;

		vol = _SFXSlider.value;

		AudioListener.volume = vol;
		PlayerPrefs.SetFloat ("vol", vol);
	}

	public void SFXOnOffFunction(){
		if (PlayerPrefs.HasKey ("toggleVol"))
			vol = PlayerPrefs.GetInt("toggleVol");
		else
			_SFXToggle.isOn = true;

		if (_SFXToggle.isOn) {
			AudioListener.volume = PlayerPrefs.GetFloat ("vol");
			PlayerPrefs.SetInt("toggleVol", 1);
			_SFXSlider.interactable = true;
		}
		else {
			AudioListener.volume = 0f;
			PlayerPrefs.SetInt("toggleVol", 0);
			_SFXSlider.interactable = false;
		}
	}
	void Update(){
		if (_SFXSlider != null) {
			_SFXSlider.value = PlayerPrefs.GetFloat ("vol");
		}
			
	}
}
