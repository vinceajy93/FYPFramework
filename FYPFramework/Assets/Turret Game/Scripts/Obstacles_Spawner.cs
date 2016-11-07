using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Obstacles_Spawner : MonoBehaviour {

	private LevelSelectScript _LevelSelectScript;

	//obstacle game object
	[SerializeField]
	private GameObject obstacles_Prefab;

	//debugging purpose
	private Text debugText;

	private int numberof_Obstacles;

	//list of obstacles
	//public List<GameObject> Obstacles_List = new List<GameObject>();
	public List<int> ObstaclesHealth_List = new List<int>();
	public List<GameObject> ObstaclesGO_List = new List<GameObject>();

	// Use this for initialization
	void Start () {
		//initialize the number of obstacles
		/*numberof_Obstacles = PlayerPrefs.GetInt ("no_obstacles");

		debugText = GameObject.Find ("noofobstacles").GetComponent<Text>();

		debugText.text = PlayerPrefs.GetInt ("no_obstacles").ToString();
		//Debug.Log ("no of obstacles" +PlayerPrefs.GetInt("no_obstacles"));
		/*
		for (int i = 0; i < numberof_Obstacles; i++) {
			Instantiate (obstacles_Prefab);
			ObstaclesGO_List.Add (obstacles_Prefab);
			ObstaclesHealth_List.Add (i);
		}*/
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.W)) {
			//ObstaclesHealth_List.Add(Random.Range(1, 100));
			//Debug.Log(ObstaclesHealth_List[5]);
		}


	}
}
