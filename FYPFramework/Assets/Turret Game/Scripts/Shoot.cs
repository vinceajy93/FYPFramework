using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	public GameObject Shooting_Position;
	public GameObject Bullet;

	public GameObject Bg;
	private bool shoot = false;
	private GameObject mBull;

	//private float nextfire = 0.3f;
	//private float firerate = 0.1f;
	//private float dt;
	// Use this for initialization
	void Start () {
		//dt = 0;
		PlayerPrefs.SetInt ("NoBullet", 0);
	}
	
	// Update is called once per frame
	void Update () {
		/*dt += Time.deltaTime;
		if (dt > nextfire) {
			nextfire += firerate;
			Instantiate (Bullet, new Vector3(Shooting_Position.transform.position.x, Shooting_Position.transform.position.y, Shooting_Position.transform.position.z + 10.0f), Shooting_Position.transform.rotation);
		}*/

		// Get Camera Size in worldspace
		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;

		// Get Background Size in worldspace
		Vector2 sprite_size = Bg.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / Bg.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 world_size = local_sprite_size;
		world_size.x *= Bg.transform.lossyScale.x;
		world_size.y *= Bg.transform.lossyScale.y;

		/*if (Camera.main.transform.position.y < world_size.y - (height / 2)) {
			Camera.main.transform.position += Vector3.up * Time.deltaTime * 100;
		}*/

		if (shoot == true) {
			shoot = false;
			Camera.main.transform.position = new Vector3 (width / 2, height / 2, -10);
		}

		if (mBull != null) {
			if (Camera.main.transform.position.y < world_size.y - (height / 2) && mBull.transform.position.y > (height / 2)) {
				Camera.main.transform.position = new Vector3 (width / 2, mBull.transform.position.y, -10);
			}
			PlayerPrefs.SetInt ("Control", 1);
		} else {
			//Camera.main.transform.position = new Vector3 (width / 2, height / 2, -10);
			PlayerPrefs.SetInt ("Control", 0);
		}
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
