using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour {
	//debug
	public Text debugTextP1;
	public Text debugTextP2;

	public enum ObjectsHealth{
		player1 =1,
		player2,
		wall1,
		wall2

	};

	//health variables
	public int health_p1 = 5;
	public int health_p2 = 5;

	private Text _p1HealthText;
	private Text _p2HealthText;

	public ObjectsHealth objHealth;
	// Use this for initialization
	void Start (){
		_p1HealthText = GameObject.Find ("Txt_Player1Health").GetComponent<Text> ();
		_p2HealthText = GameObject.Find ("Txt_Player2Health").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		//display the health of objects
		_p1HealthText.text = (health_p1.ToString ()); //player 1 health (Number)
		_p2HealthText.text = (health_p2.ToString ()); //player 2 health (Number)
	}
	public void ApplyDamage(int damage){

		switch(objHealth){
		case ObjectsHealth.player1:
			if (health_p1 > 0)
				health_p1 -= damage;
			break;
		case ObjectsHealth.player2:
			if (health_p2 > 0)
				health_p2 -= damage;
			break;
		case ObjectsHealth.wall1:
			break;
		case ObjectsHealth.wall2:
			break;
		}

	}
}
