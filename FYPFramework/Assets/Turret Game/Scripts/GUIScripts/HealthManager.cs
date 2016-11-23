using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour {

	//for testing
	public int[] obstacle_health;

	public enum ObjectsHealth{
		player1 =1,
		player2,
		wall1,
		wall2,

		//for testing 
		obstacle1,
		obstacle2,
		obstacle3,
		obstacle4,
		obstacle5,
		obstacle6,
		obstacle7,
		obstacle8,
		obstacle9,
		obstacle10

	};

	//obstacles health

	public Stat P1Health;
    public Stat P2Health;

	[HideInInspector]
	public ObjectsHealth objHealth;

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
			if (obstacle_health[0] > 0)
				obstacle_health[0] -= damage;
			break;
		case ObjectsHealth.obstacle2:
			if (obstacle_health[1] > 0)
				obstacle_health[1] -= damage;
			break;
		case ObjectsHealth.obstacle3:
			if (obstacle_health[2] > 0)
				obstacle_health[2] -= damage;
			break;
		case ObjectsHealth.obstacle4:
			if (obstacle_health[3] > 0)
				obstacle_health[3] -= damage;
			break;
		case ObjectsHealth.obstacle5:
			if (obstacle_health[4] > 0)
				obstacle_health[4] -= damage;
			break;
		case ObjectsHealth.obstacle6:
			if (obstacle_health[5] > 0)
				obstacle_health[5] -= damage;
			break;
		case ObjectsHealth.obstacle7:
			if (obstacle_health[6] > 0)
				obstacle_health[6] -= damage;
			break;
		case ObjectsHealth.obstacle8:
			if (obstacle_health[7] > 0)
				obstacle_health[7] -= damage;
			break;
		case ObjectsHealth.obstacle9:
			if (obstacle_health[8] > 0)
				obstacle_health[8] -= damage;
			break;
		case ObjectsHealth.obstacle10:
			if (obstacle_health[9] > 0)
				obstacle_health[9] -= damage;
			break;
		}
	}

    public void AddHealth(int amount)
    {
        switch (objHealth)
        {
            case ObjectsHealth.player1:
                if (P1Health.CurrentVal == P1Health.MaxVal)
                {
                    P1Health.MaxVal += amount;
                }
                P1Health.CurrentVal += amount;
                break;
            case ObjectsHealth.player2:
                if (P2Health.CurrentVal == P2Health.MaxVal)
                {
                    P2Health.MaxVal += amount;
                }
                P2Health.CurrentVal += amount;
                break;
        }
    }
}
