using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Bullet_Movement : MonoBehaviour {

	//list of bullets and its dmg
	/*struct ProjectileDmg{
		int bulletDmg = 2; 
	}*/

	private HealthManager _HealthManager;

	public Text debugtext;
	private GameObject Bg;
	private int num_bg;

	private GameObject indicator;
	private Vector3 bg_world_size;
	private float object_max_height;

	private Camera cam;
	private float cam_height; // Size of camera in y

	private GameObject Bullet_Rest;

	// Use this for initialization
	void Start () {

		//pass by reference from health Manaager
		_HealthManager = GameObject.Find("Scripts").GetComponent<HealthManager>();

		Bg = GameObject.FindGameObjectWithTag ("Background");
		num_bg = GameObject.FindGameObjectsWithTag ("Background").Length;

		Vector2 sprite_size = Bg.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / Bg.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		bg_world_size = local_sprite_size;
		bg_world_size.x *= Bg.transform.lossyScale.x;
		bg_world_size.y *= Bg.transform.lossyScale.y;

		// Get Camera
		cam = Camera.main;
		cam_height = 2f * cam.orthographicSize;

		object_max_height = (bg_world_size.y * num_bg) - (cam_height / 2);

		Bullet_Rest = GameObject.Find ("Bullet_Rest");
	}

	// Update is called once per frame
	void Update () {
		//Bullet movement, Delete if outside of background (Y axis)
		if (gameObject.tag == "Bullet_1") {
			gameObject.transform.localPosition += Vector3.up * 0.1f;

			// "Destroy" by placing them back to bullet_rest gameobject
			if (gameObject.transform.position.y > object_max_height) {
				gameObject.tag = "Bullet_Rest";
				gameObject.transform.position = Bullet_Rest.transform.position;
				gameObject.transform.parent = Bullet_Rest.transform;
			}
		} 
		else if (gameObject.tag == "Bullet_2") {
			gameObject.transform.localPosition += Vector3.down * 0.1f;

			// "Destroy" by placing them back to bullet_rest gameobject
			if (gameObject.transform.position.y < (-cam_height / 2)) {
				gameObject.tag = "Bullet_Rest";
				gameObject.transform.position = Bullet_Rest.transform.position;
				gameObject.transform.parent = Bullet_Rest.transform;
			}
		}
	}

	// "Destroy" by placing them back to bullet_rest gameobject
	void OnCollisionEnter2D(Collision2D coll) {
		//bullet 1
		if (coll.gameObject.CompareTag("Bullet_1")) {
			gameObject.tag = "Bullet_Rest";
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.parent = Bullet_Rest.transform;

			coll.gameObject.tag = "Bullet_Rest";
			coll.gameObject.transform.position = Bullet_Rest.transform.position;
			coll.gameObject.transform.parent = Bullet_Rest.transform;
		}
		//bullet 2
		if (coll.gameObject.CompareTag("Bullet_2")) {
			gameObject.tag = "Bullet_Rest";
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.parent = Bullet_Rest.transform;

			coll.gameObject.tag = "Bullet_Rest";
			coll.gameObject.transform.position = Bullet_Rest.transform.position;
			coll.gameObject.transform.parent = Bullet_Rest.transform;
		}
		//wall 1
		if(coll.gameObject.CompareTag("player1_wall")){
			gameObject.tag = "Bullet_Rest";
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.parent = Bullet_Rest.transform;
		}
		//wall 2
		if(coll.gameObject.CompareTag("player2_wall")){
			_HealthManager.objHealth = HealthManager.ObjectsHealth.wall2;
			_HealthManager.SendMessage("ApplyDamage", 1); //damage done

			gameObject.tag = "Bullet_Rest";
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.parent = Bullet_Rest.transform;
		}
		//player 1
		if(coll.gameObject.CompareTag("Player1") && this.tag != "Bullet_1"){
			_HealthManager.objHealth = HealthManager.ObjectsHealth.player1;
			_HealthManager.SendMessage("ApplyDamage", 2); //damage done

			gameObject.tag = "Bullet_Rest";
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.parent = Bullet_Rest.transform;

		}
		//player 2
		if(coll.gameObject.CompareTag("Player2") && this.tag != "Bullet_2"){
			_HealthManager.objHealth = HealthManager.ObjectsHealth.player2;
			_HealthManager.SendMessage("ApplyDamage", 2); //damage done

			gameObject.tag = "Bullet_Rest";
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.parent = Bullet_Rest.transform;
		}
	}
}
