using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour {

	public enum ObjectsHealth{
		player1 =1,
		player2,
		wall1,
		wall2

	};
		
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
		}

	}
}
