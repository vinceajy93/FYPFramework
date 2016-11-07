using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundsIndicator : MonoBehaviour {

	[SerializeField]
	private Image rounds_P1_1, rounds_P1_2, rounds_P1_3, rounds_P2_1, rounds_P2_2, rounds_P2_3;
	[SerializeField]
	private Sprite filledOrb_P1, filledOrb_P2;

	//number of rounds set by the player
	private int noOfRounds;
	//number of rounds won individually by each player
	public int roundWon_P1, roundWon_P2;

	// Use this for initialization
	void Start () {
		noOfRounds = PlayerPrefs.GetInt ("rounds");


		//Set the number of rounds won to filled orb sprite
		switch(PlayerPrefs.GetInt("roundWon_P1")){
		case 1:
			rounds_P1_1.GetComponent<Image> ().sprite = filledOrb_P1;
			break;
		case 2:
			rounds_P1_1.GetComponent<Image> ().sprite = filledOrb_P1;
			rounds_P1_2.GetComponent<Image> ().sprite = filledOrb_P1;
			break;
		case 3:
			rounds_P1_1.GetComponent<Image> ().sprite = filledOrb_P1;
			rounds_P1_2.GetComponent<Image> ().sprite = filledOrb_P1;
			rounds_P1_3.GetComponent<Image> ().sprite = filledOrb_P1;
			break;
		}
		switch(PlayerPrefs.GetInt("roundWon_P2")){
		case 1:
			rounds_P2_1.GetComponent<Image> ().sprite = filledOrb_P2;
			break;
		case 2:
			rounds_P2_1.GetComponent<Image> ().sprite = filledOrb_P2;
			rounds_P2_2.GetComponent<Image> ().sprite = filledOrb_P2;
			break;
		case 3:
			rounds_P2_1.GetComponent<Image> ().sprite = filledOrb_P2;
			rounds_P2_2.GetComponent<Image> ().sprite = filledOrb_P2;
			rounds_P2_3.GetComponent<Image> ().sprite = filledOrb_P2;
			break;
		}

		//displays the number of rounds set and number of wins
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
	}
}
