using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelController : MonoBehaviour {

	public Text debugText;

	// Use this for initialization
	void Start () {
		debugText.text = ("Stage: " + SceneManager.GetActiveScene().name);
	}
	
	// Update is called once per frame
	void Update () {
	}

	//load the main menu scene
	public void loadMenu(){
		SceneManager.LoadScene ("Main Menu");
	}

	//load the play scene
	public void loadPlay(){
		SceneManager.LoadScene ("Mode Selection");
	}

	//load the campaign scene
	public void loadCampaign(){
		//SceneManager.LoadScene ("Loadout");
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
}
