using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashLoader : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		//PlayerPrefs.SetFloat ("curVol", 1.0f);
		StartCoroutine ("Countdown");
	}
	
	private IEnumerator Countdown(){
		yield return new WaitForSeconds (7);
		//Application.LoadLevel ("main menu");
		SceneManager.LoadScene("Main Menu");
	}
}
