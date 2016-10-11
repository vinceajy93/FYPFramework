using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour {

	//maximum number of obstacles in the game
	public int OBSTACLES_MAX = 10;

	//for testing
	public int obstacle1_health = 3 , obstacle2_health = 3, obstacle3_health = 3, obstacle4_health = 3;

	public enum ObjectsHealth{
		player1 =1,
		player2,
		wall1,
		wall2,

		//for testing 
		obstacle1,
		obstacle2,
		obstacle3,
		obstacle4

	};

	//obstacles health

	[SerializeField]
	private Stat P1Health;
	[SerializeField]
	private Stat P2Health;

	void Awake(){
		//single player
		if (gameObject.GetComponent<Mode_Control> ().game_mode_Single) {
			P1Health.Initialize ();
		}
		//multiplayer
		else {
			P1Health.Initialize ();
			P2Health.Initialize ();
		}

	}


	public ObjectsHealth objHealth;

	// Use this for initialization
	void Start (){
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void ApplyDamage(int damage){

		switch(objHealth){
		case ObjectsHealth.player1:
			if (P1Health.CurrentVal > 0)
				P1Health.CurrentVal -= damage;
			break;
		case ObjectsHealth.player2:
			if (P2Health.CurrentVal > 0)
				P2Health.CurrentVal -= damage;
			break;
		case ObjectsHealth.wall1:
			if (P1Health.CurrentVal > 0)
				P1Health.CurrentVal -= damage;
			break;
		case ObjectsHealth.wall2:
			if (P2Health.CurrentVal > 0)
				P2Health.CurrentVal -= damage;
			break;
		case ObjectsHealth.obstacle1:
			if (obstacle1_health > 0)
				obstacle1_health -= damage;
			break;
		case ObjectsHealth.obstacle2:
			if (obstacle2_health > 0)
				obstacle2_health -= damage;
			break;
		case ObjectsHealth.obstacle3:
			if (obstacle3_health > 0)
				obstacle3_health -= damage;
			break;
		case ObjectsHealth.obstacle4:
			if (obstacle4_health > 0)
				obstacle4_health -= damage;

			break;

		}

	}
}
