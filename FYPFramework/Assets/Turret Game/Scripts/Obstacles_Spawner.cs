using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Obstacles_Spawner : MonoBehaviour
{
	private LevelSelectScript _LevelSelectScript;

	//Obstacle GameObject
	[SerializeField]
	private GameObject[] obstacles_Prefab;
	public GameObject[] ArrayGO;
	public int[] ArrayGO_Health;

	//variables used to calclualte the distance between two boundaries
	[SerializeField]
	private GameObject bulletbound_P1, bulletbound_P2;

	//center of the two points of bullet boundaries (used to check for the middle of obstacles spawner);
	private Vector3 BoundforObstaclesSpawn_center;
	//counter that checks how many obstacles have been spawned at any players side
	public int ObstacleCount_P1, ObstacleCount_P2;

	//number of obstacles that is parsed from players choice
	private int numberof_Obstacles;

	//health of the obstacles
	private static int obstacles_health = 5;

	private int randObstacle;

	Vector2 local_sprite_size;
	Vector2 obstacle_world_size;
	Vector2 sprite_size;

	GameObject Obstacle_Spawn;

	private float countdowntimer;
	private static float countdownTreshhold = 10.0f;
	private int i;
	private int obs_no;

	bool isRespawning = false;
	// Use this for initialization
	void Start ()
	{
		i = 0;
		countdowntimer = 0f;

		numberof_Obstacles = PlayerPrefs.GetInt ("no_obstacles");
		ArrayGO = new GameObject[numberof_Obstacles];
		ArrayGO_Health = new int[numberof_Obstacles];

		ObstacleCount_P1 = 0;
		ObstacleCount_P2 = 0;

		obs_no = 0;
		//Random the obstacles that will spawn
		if (PlayerPrefs.GetInt ("Selected_Stage") == 1)
			randObstacle = Random.Range (0, 2);
		else if (PlayerPrefs.GetInt ("Selected_Stage") == 2)
			randObstacle = Random.Range (2, 4);


		Obstacle_Spawn = obstacles_Prefab [randObstacle];//Resources.Load ("OBSTY TEST") as GameObject;

		sprite_size = Obstacle_Spawn.GetComponent<SpriteRenderer> ().sprite.rect.size;
		local_sprite_size = sprite_size / Obstacle_Spawn.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		obstacle_world_size = local_sprite_size;
		obstacle_world_size.x *= Obstacle_Spawn.transform.lossyScale.x;
		obstacle_world_size.y *= Obstacle_Spawn.transform.lossyScale.y;

		//Calculate the middle position between player 1 and player 2 obstacles spawn point 
		BoundforObstaclesSpawn_center = 0.5f * (bulletbound_P2.transform.position - bulletbound_P1.transform.position);

		spawnObstacle ();
	}

	void Update(){

		if (!isRespawning) {
			isRespawning = true;
			//spawnObstacle ();
		}


		//total number of obstacles spawned
		obs_no = ObstacleCount_P1 + ObstacleCount_P2;

		if (obs_no < i) {
			//Update the countdownTimer
			countdowntimer += Time.deltaTime;
		}

		//If timer above trreshhold, deduct I to spawn obstacle
		if (countdowntimer > countdownTreshhold) {
			i = obs_no;
			countdowntimer = 0;
			isRespawning = false;
		}

	}

	void spawnObstacle(){

		//for i is lesser than number of obstacles set by player
		for (i = i; i < numberof_Obstacles; i++)
		{

			float TempFloatX = Random.Range (-4.67f, 4.67f); //hard coded - needs to change 
			//within p1 bounds
			if (ObstacleCount_P1 < (numberof_Obstacles * 0.5f)) {
				ObstacleCount_P1++;

				// Force the random position to be within the boundary of player and center
				float TempFloatY = Random.Range (obstacle_world_size.y * 0.5f, BoundforObstaclesSpawn_center.y - obstacle_world_size.y * 0.5f);

				//Adds the obstacle to a go list and to a health list 
				GameObject go = Instantiate (Obstacle_Spawn, new Vector3 (TempFloatX, TempFloatY, 0), Quaternion.identity) as GameObject;
				go.tag = "P1_Obstacle";
				go.name = "Obstacle" + (i + 1);
				ArrayGO [i] = go;
				ArrayGO_Health [i] = obstacles_health;

//				foreach (GameObject ea_GO in ArrayGO) {
//					if (ea_GO != null) {
//						if (ea_GO != ArrayGO [i]) {
//							while (ArrayGO [i].GetComponent<Renderer> ().bounds.Intersects (ea_GO.GetComponent<Renderer> ().bounds)) {
//								TempFloatY = Random.Range (obstacle_world_size.y * 0.5f, BoundforObstaclesSpawn_center.y - obstacle_world_size.y * 0.5f);
//								ArrayGO [i].transform.position = new Vector3 (ArrayGO [i].transform.position.x, TempFloatY, ArrayGO [i].transform.position.z);
//							}
//						}
//					}
//				}

			} else if (ObstacleCount_P2 < (numberof_Obstacles * 0.5f)) {
				ObstacleCount_P2++;
				float TempFloatY = Random.Range (BoundforObstaclesSpawn_center.y + obstacle_world_size.y * 0.5f, bulletbound_P2.transform.position.y - obstacle_world_size.y * 0.5f);
				//Adds the obstacle to a go list and to a health list 
				//obstacles_Prefab.tag = ("Player2");
				ArrayGO [i] = Obstacle_Spawn;
				ArrayGO_Health [i] = obstacles_health;
				GameObject go = Instantiate (Obstacle_Spawn, new Vector3 (TempFloatX, TempFloatY, 0), Quaternion.identity) as GameObject;
				go.tag = "P2_Obstacle";
				go.name = "Obstacle" + (i + 1);

//				foreach (GameObject ea_GO in ArrayGO) {
//					if (ea_GO != null) {
//						if (ea_GO != ArrayGO [i]) {
//							while (ArrayGO [i].GetComponent<Renderer> ().bounds.Intersects (ea_GO.GetComponent<Renderer> ().bounds)) {
//								TempFloatY = Random.Range (obstacle_world_size.y * 0.5f, BoundforObstaclesSpawn_center.y - obstacle_world_size.y * 0.5f);
//								ArrayGO [i].transform.position = new Vector3 (ArrayGO [i].transform.position.x, TempFloatY, ArrayGO [i].transform.position.z);
//							}
//						}
//					}
//				}
			}
		}
	}

}

