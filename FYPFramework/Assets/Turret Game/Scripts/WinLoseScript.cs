using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinLoseScript : MonoBehaviour {

	int no_of_rounds_played;
	int p1_wons, p2_wons, p1_loses, p2_loses, p1_draws, p2_draws;

	[SerializeField]
	private Text[] statisticText;

	[SerializeField]
	private Image[] Images;
	[SerializeField]
	private Sprite[] Sprites;

	//TODO: no of bullets fired, total game time
	// Use this for initialization
	void Start () {
		//caching
		no_of_rounds_played = PlayerPrefs.GetInt ("rounds");
		p1_wons = PlayerPrefs.GetInt ("roundWon_P1");
		p2_wons = PlayerPrefs.GetInt ("roundWon_P2");
		p1_loses = p2_wons;
		p2_loses = p1_wons;
		p1_draws = no_of_rounds_played - (p1_wons + p2_wons);
		p2_draws = p1_draws;

		//Set image of winner/ loser

		if (p1_wons > p2_wons) {
			//P1 win
			Images [0].sprite = Sprites [0];
			Images [1].sprite = Sprites [1];

			Images [0].SetNativeSize ();
			Images [1].SetNativeSize ();
		}
			
		else if(p2_wons > p1_wons){
			//P2 win
			Images [0].sprite = Sprites [1];
			Images [1].sprite = Sprites [0];

			Images [0].SetNativeSize ();
			Images [1].SetNativeSize ();
		}
		else if(p1_wons == p2_wons){
			Images [0].sprite = Sprites [1];
			Images [1].sprite = Sprites [1];

			Images [0].SetNativeSize ();
			Images [1].SetNativeSize ();
		}
		//Display the informations
		statisticText[0].text = statisticText[0].text + " " + p1_wons;
		statisticText[1].text = statisticText[1].text + " " + p1_loses;
		statisticText[2].text = statisticText[2].text + " " + p1_draws;
		statisticText[3].text = statisticText[3].text + " " + no_of_rounds_played;

		statisticText[4].text = statisticText[4].text + " " + p2_wons;
		statisticText[5].text = statisticText[5].text + " " + p2_loses;
		statisticText[6].text = statisticText[6].text + " " + p2_draws;
		statisticText[7].text = statisticText[7].text + " " + no_of_rounds_played;
	}
}
