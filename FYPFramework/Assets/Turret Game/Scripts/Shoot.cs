using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	public GameObject Shooting_Position;
	public GameObject Bullet;

	private bool shoot = false;
	private GameObject mBull;
	private GameObject Bg;
	private int num_bg;

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("NoBullet", 0);
		Bg = GameObject.FindGameObjectWithTag ("Background");
		foreach (GameObject all_background in GameObject.FindGameObjectsWithTag("Background")) {
			num_bg += 1;
		}
		//Debug.Log (num_bg);
	}
	
	// Update is called once per frame
	void Update () {
		// Get Camera Size in worldspace
		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;

		// Get Background Size in worldspace
		Vector2 sprite_size = Bg.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / Bg.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 world_size = local_sprite_size;
		world_size.x *= Bg.transform.lossyScale.x;
		world_size.y *= Bg.transform.lossyScale.y * num_bg;

		if (shoot == true) {
			shoot = false;
			Camera.main.transform.position = new Vector3 (0, 0, -10);
		}

		if (mBull != null) {
			Camera.main.transform.position = new Vector3 (width / 2, mBull.transform.position.y, -10);
			PlayerPrefs.SetInt ("Control", 1);
		} else {
			//Camera.main.transform.position = new Vector3 (0, 0, -10);
			PlayerPrefs.SetInt ("Control", 0);
		}

		// TO DO: Change Camera to Bullet
		//if (Camera.main.transform.position.y < world_size.y - height)
			//Camera.main.transform.position += Vector3.up * Time.deltaTime * 5;
	}

	public void onClick(){
		float x = Shooting_Position.transform.position.x;
		float y = Shooting_Position.transform.position.y;

		GameObject smt = Instantiate (Bullet, new Vector3 (x, y, Shooting_Position.transform.position.z), Shooting_Position.transform.rotation) as GameObject;
		if (gameObject.tag == "Player1")
			smt.tag = "Bullet_1";
		else
			smt.tag = "Bullet_2";
		
		mBull = smt;
		shoot = true;

		int total = PlayerPrefs.GetInt ("NoBullet");
		PlayerPrefs.SetInt ("NoBullet", total + 1);
	}
}
