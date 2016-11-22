using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Obstacles_Spawner : MonoBehaviour
{
	private LevelSelectScript _LevelSelectScript;

	//Obstacle GameObject
	[SerializeField]
	private GameObject obstacles_Prefab;

	public GameObject[] ArrayGO;
	public int[] ArrayGO_Health;

	//variables used to calclualte the distance between two boundaries
	[SerializeField]
	private GameObject bulletbound_P1, bulletbound_P2;

	//center of the two points of bullet boundaries (used to check for the middle of obstacles spawner);
	private Vector3 BoundforObstaclesSpawn_center;
	//counter that checks how many obstacles have been spawned at any players side
	private int ObstacleCount_P1, ObstacleCount_P2;

	//number of obstacles that is parsed from players choice
	private int numberof_Obstacles;

	//health of the obstacles
	private static int obstacles_health = 5;

	// Use this for initialization
	void Start ()
	{
		numberof_Obstacles = PlayerPrefs.GetInt ("no_obstacles");
		ArrayGO = new GameObject[numberof_Obstacles];
		ArrayGO_Health = new int[numberof_Obstacles];

		ObstacleCount_P1 = 0;
		ObstacleCount_P2 = 0;

		GameObject Obstacle_Spawn = Resources.Load ("OBSTY TEST") as GameObject;

		Vector2 sprite_size = Obstacle_Spawn.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / Obstacle_Spawn.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector2 obstacle_world_size = local_sprite_size;
		obstacle_world_size.x *= Obstacle_Spawn.transform.lossyScale.x;
		obstacle_world_size.y *= Obstacle_Spawn.transform.lossyScale.y;

		//Calculate the middle position between player 1 and player 2 obstacles spawn point 
		BoundforObstaclesSpawn_center = 0.5f * (bulletbound_P2.transform.position - bulletbound_P1.transform.position);

		//for i is lesser than number of obstacles set by player
		for (int i = 0; i < numberof_Obstacles; i++) {

			float TempFloatX = Random.Range (-4.67f, 4.67f); //hard coded - needs to change 
			//within p1 bounds
			if (ObstacleCount_P1 < (numberof_Obstacles * 0.5f)) {
				ObstacleCount_P1++;

				// Force the random position to be within the boundary of player and center
				float TempFloatY = Random.Range (obstacle_world_size.y * 0.5f, BoundforObstaclesSpawn_center.y - obstacle_world_size.y * 0.5f);

				//Adds the obstacle to a go list and to a health list 
				GameObject go = Instantiate (Obstacle_Spawn, new Vector3 (TempFloatX, TempFloatY, 0), Quaternion.identity) as GameObject;
				go.tag = "Obstacle";
				ArrayGO [i] = go;
				ArrayGO_Health [i] = obstacles_health;

				foreach (GameObject ea_GO in ArrayGO) {
					if (ea_GO != null) {
						if (ea_GO != ArrayGO [i]) {
							while (ArrayGO [i].GetComponent<Renderer> ().bounds.Intersects (ea_GO.GetComponent<Renderer> ().bounds)) {
								TempFloatY = Random.Range (obstacle_world_size.y * 0.5f, BoundforObstaclesSpawn_center.y - obstacle_world_size.y * 0.5f);
								ArrayGO [i].transform.position = new Vector3 (ArrayGO [i].transform.position.x, TempFloatY, ArrayGO [i].transform.position.z);
							}
						}
					}
				}

			} else if (ObstacleCount_P2 < (numberof_Obstacles * 0.5f)) {
				ObstacleCount_P2++;

				float TempFloatY = Random.Range (BoundforObstaclesSpawn_center.y + obstacle_world_size.y * 0.5f, bulletbound_P2.transform.position.y - obstacle_world_size.y * 0.5f);
				//Adds the obstacle to a go list and to a health list 
				obstacles_Prefab.tag = ("Player2");
				ArrayGO [i] = Obstacle_Spawn;
				ArrayGO_Health [i] = obstacles_health;
				GameObject go = Instantiate (Obstacle_Spawn, new Vector3 (TempFloatX, TempFloatY, 0), Quaternion.identity) as GameObject;
				go.tag = "Obstacle";

				foreach (GameObject ea_GO in ArrayGO) {
					if (ea_GO != null) {
						if (ea_GO != ArrayGO [i]) {
							while (ArrayGO [i].GetComponent<Renderer> ().bounds.Intersects (ea_GO.GetComponent<Renderer> ().bounds)) {
								TempFloatY = Random.Range (obstacle_world_size.y * 0.5f, BoundforObstaclesSpawn_center.y - obstacle_world_size.y * 0.5f);
								ArrayGO [i].transform.position = new Vector3 (ArrayGO [i].transform.position.x, TempFloatY, ArrayGO [i].transform.position.z);
							}
						}
					}
				}
			}
		}
	}
}

