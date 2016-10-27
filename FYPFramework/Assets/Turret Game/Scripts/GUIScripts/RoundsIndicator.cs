using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundsIndicator : MonoBehaviour {

	[SerializeField]
	private Image rounds_P1_1, rounds_P1_2, rounds_P1_3, rounds_P2_1, rounds_P2_2, rounds_P2_3;
	[SerializeField]
	private Sprite filledOrb;

	private int noOfRounds;

	// Use this for initialization
	void Start () {
		noOfRounds = PlayerPrefs.GetInt ("rounds");

		//displays the number of rounds set and number of wins
		//TODO: need to add how many rounds each player have achieved
		switch(noOfRounds){
		case 1:
			rounds_P1_2.gameObject.SetActive (false);
			rounds_P1_3.gameObject.SetActive (false);

			rounds_P2_2.gameObject.SetActive (false);
			rounds_P2_3.gameObject.SetActive (false);
			break;
		case 2:
			rounds_P1_3.gameObject.SetActive (false);
			rounds_P2_3.gameObject.SetActive (false);
			break;
		case 3:
			break;

		default:
			Debug.Log ("number of rounds exceeeded 3, no of rounds = " + noOfRounds);
			break;
		}
		//for testing
		rounds_P1_1.GetComponent<Image> ().sprite = filledOrb;
	}
}
