using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashAnimSwitcher : MonoBehaviour {

	[HideInInspector]protected GameObject CompanyLogoAndName;
	[HideInInspector]protected GameObject GameLogoAndName;

	// Use this for initialization
	void Start () {
		//find and set active game objects
		CompanyLogoAndName = GameObject.Find ("Company LnN");
		GameLogoAndName = GameObject.Find ("Game LnN");

		//Set Game logo and name to false when start
		GameLogoAndName.SetActive (false);
		StartCoroutine ("Countdown");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.W)) {
			Debug.Log ("test");
		}
	}


	IEnumerator Countdown(){
		yield return new WaitForSeconds (3);
		CompanyLogoAndName.SetActive (false);
		GameLogoAndName.SetActive (true);
	}
}
