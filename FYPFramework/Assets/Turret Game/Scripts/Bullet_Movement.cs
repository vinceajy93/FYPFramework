using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Bullet_Movement : MonoBehaviour {

	//list of bullets and its dmg
	/*struct ProjectileDmg{
		int bulletDmg = 2; 
	}*/

	private HealthManager _HealthManager;

	//public Text debugtext;
	private GameObject Bg;
	private int num_bg;

	private GameObject indicator;
	private Vector3 bg_world_size;
	private float object_max_height;

	private Camera cam;
	private float cam_height; // Size of camera in y

	private GameObject Bullet_Rest;

	private PauseScript _pauseScript;

	// Use this for initialization
	void Start () {
		//pass by reference from pauseScript
		_pauseScript = GameObject.Find("Scripts").GetComponent<PauseScript>();
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
		if (_pauseScript.Paused == false) {

			//Bullet movement, Delete if outside of background (Y axis)
			if (gameObject.tag == "Bullet_1") {
				gameObject.transform.localPosition += Vector3.up * 0.1f;

				// "Destroy" by placing them back to bullet_rest gameobject
				if (gameObject.transform.position.y > object_max_height) {
					gameObject.tag = "Bullet_Rest";
					gameObject.transform.position = Bullet_Rest.transform.position;
					gameObject.transform.SetParent (Bullet_Rest.transform);
				}
			} else if (gameObject.tag == "Bullet_2") {
				gameObject.transform.localPosition += Vector3.down * 0.1f;

				// "Destroy" by placing them back to bullet_rest gameobject
				if (gameObject.transform.position.y < (-cam_height / 2)) {
					gameObject.tag = "Bullet_Rest";
					gameObject.transform.position = Bullet_Rest.transform.position;
					gameObject.transform.SetParent (Bullet_Rest.transform);
				}
			} else if (gameObject.tag == "Bullet_E") {
				gameObject.transform.localPosition += Vector3.down * 0.1f;

				// "Destroy" by placing them back to bullet_rest gameobject
				if (gameObject.transform.position.y < (-cam_height / 2)) {
					gameObject.tag = "Bullet_Rest";
					gameObject.transform.position = Bullet_Rest.transform.position;
					gameObject.transform.SetParent (Bullet_Rest.transform);
				}
			}
		}
			
	}

	// "Destroy" by placing them back to bullet_rest gameobject
	void OnCollisionEnter2D(Collision2D coll) {
		//bullet 1
		if (coll.gameObject.CompareTag("Bullet_1")) {
			GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_1.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_1.transform.position = gameObject.transform.position;
			Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
			Bullet_Destroy_1.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (gameObject.CompareTag ("Bullet_E")) {
				gameObject.tag = "Bullet_Rest_E";
			} else if (gameObject.CompareTag ("Bullet_2")) {
				gameObject.tag = "Bullet_Rest_2";
			} else {
				gameObject.tag = "Bullet_Rest";
			}
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.SetParent(Bullet_Rest.transform);

			GameObject Bullet_Destroy_2 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_2.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_2.transform.position = coll.gameObject.transform.position;
			Bullet_Destroy_2.transform.rotation = coll.gameObject.transform.rotation;
			Bullet_Destroy_2.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (coll.gameObject.CompareTag ("Bullet_E")) {
				coll.gameObject.tag = "Bullet_Rest_E";
			} else if (coll.gameObject.CompareTag ("Bullet_2")) {
				coll.gameObject.tag = "Bullet_Rest_2";
			} else {
				coll.gameObject.tag = "Bullet_Rest";
			}
			coll.gameObject.transform.position = Bullet_Rest.transform.position;
			coll.gameObject.transform.SetParent(Bullet_Rest.transform);
		}
		//bullet 2
		if (coll.gameObject.CompareTag("Bullet_2")) {
			GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_1.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_1.transform.position = gameObject.transform.position;
			Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
			Bullet_Destroy_1.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (gameObject.CompareTag ("Bullet_E")) {
				gameObject.tag = "Bullet_Rest_E";
			} else if (gameObject.CompareTag ("Bullet_2")) {
				gameObject.tag = "Bullet_Rest_2";
			} else {
				gameObject.tag = "Bullet_Rest";
			}
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.SetParent(Bullet_Rest.transform);

			GameObject Bullet_Destroy_2 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_2.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_2.transform.position = coll.gameObject.transform.position;
			Bullet_Destroy_2.transform.rotation = coll.gameObject.transform.rotation;
			Bullet_Destroy_2.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (coll.gameObject.CompareTag ("Bullet_E")) {
				coll.gameObject.tag = "Bullet_Rest_E";
			} else if (coll.gameObject.CompareTag ("Bullet_2")) {
				coll.gameObject.tag = "Bullet_Rest_2";
			} else {
				coll.gameObject.tag = "Bullet_Rest";
			}
			coll.gameObject.transform.position = Bullet_Rest.transform.position;
			coll.gameObject.transform.SetParent(Bullet_Rest.transform);
		}
		//enemy's bullet
		if (coll.gameObject.CompareTag ("Bullet_E")) {
			GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_1.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_1.transform.position = gameObject.transform.position;
			Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
			Bullet_Destroy_1.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (gameObject.CompareTag ("Bullet_E")) {
				gameObject.tag = "Bullet_Rest_E";
			} else if (gameObject.CompareTag ("Bullet_2")) {
				gameObject.tag = "Bullet_Rest_2";
			} else {
				gameObject.tag = "Bullet_Rest";
			}
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.SetParent(Bullet_Rest.transform);

			GameObject Bullet_Destroy_2 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_2.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_2.transform.position = coll.gameObject.transform.position;
			Bullet_Destroy_2.transform.rotation = coll.gameObject.transform.rotation;
			Bullet_Destroy_2.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (coll.gameObject.CompareTag ("Bullet_E")) {
				coll.gameObject.tag = "Bullet_Rest_E";
			} else if (coll.gameObject.CompareTag ("Bullet_2")) {
				coll.gameObject.tag = "Bullet_Rest_2";
			} else {
				coll.gameObject.tag = "Bullet_Rest";
			}
			coll.gameObject.transform.position = Bullet_Rest.transform.position;
			coll.gameObject.transform.SetParent(Bullet_Rest.transform);
		}
		//wall 1
		if(coll.gameObject.CompareTag("player1_wall")){
			_HealthManager.objHealth = HealthManager.ObjectsHealth.wall1;
			_HealthManager.SendMessage("ApplyDamage", 1); //damage done

			GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_1.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_1.transform.position = gameObject.transform.position;
			Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
			Bullet_Destroy_1.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (gameObject.CompareTag ("Bullet_E")) {
				gameObject.tag = "Bullet_Rest_E";
			} else if (gameObject.CompareTag ("Bullet_2")) {
				gameObject.tag = "Bullet_Rest_2";
			} else {
				gameObject.tag = "Bullet_Rest";
			}
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.SetParent(Bullet_Rest.transform);
		}
		//wall 2
		if(coll.gameObject.CompareTag("player2_wall")){
			_HealthManager.objHealth = HealthManager.ObjectsHealth.wall2;
			_HealthManager.SendMessage("ApplyDamage", 1); //damage done

			GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_1.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_1.transform.position = gameObject.transform.position;
			Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
			Bullet_Destroy_1.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (gameObject.CompareTag ("Bullet_E")) {
				gameObject.tag = "Bullet_Rest_E";
			} else if (gameObject.CompareTag ("Bullet_2")) {
				gameObject.tag = "Bullet_Rest_2";
			} else {
				gameObject.tag = "Bullet_Rest";
			}
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.SetParent(Bullet_Rest.transform);
		}
		//wall enemy
		if(coll.gameObject.CompareTag("enemy_wall")){
			//_HealthManager.objHealth = HealthManager.ObjectsHealth.wall2;
			//_HealthManager.SendMessage("ApplyDamage", 1); //damage done

			GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_1.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_1.transform.position = gameObject.transform.position;
			Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
			Bullet_Destroy_1.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (gameObject.CompareTag ("Bullet_E")) {
				gameObject.tag = "Bullet_Rest_E";
			} else if (gameObject.CompareTag ("Bullet_2")) {
				gameObject.tag = "Bullet_Rest_2";
			} else {
				gameObject.tag = "Bullet_Rest";
			}
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.SetParent(Bullet_Rest.transform);
		}
		//player 1
		if(coll.gameObject.CompareTag("Player1") && this.tag != "Bullet_1"){
			_HealthManager.objHealth = HealthManager.ObjectsHealth.player1;
			_HealthManager.SendMessage("ApplyDamage", 2); //damage done

			GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_1.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_1.transform.position = gameObject.transform.position;
			Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
			Bullet_Destroy_1.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (gameObject.CompareTag ("Bullet_E")) {
				gameObject.tag = "Bullet_Rest_E";
			} else if (gameObject.CompareTag ("Bullet_2")) {
				gameObject.tag = "Bullet_Rest_2";
			} else {
				gameObject.tag = "Bullet_Rest";
			}
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.SetParent(Bullet_Rest.transform);

		}
		//player 2
		if(coll.gameObject.CompareTag("Player2") && this.tag != "Bullet_2"){
			_HealthManager.objHealth = HealthManager.ObjectsHealth.player2;
			_HealthManager.SendMessage("ApplyDamage", 2); //damage done

			GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_1.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_1.transform.position = gameObject.transform.position;
			Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
			Bullet_Destroy_1.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (gameObject.CompareTag ("Bullet_E")) {
				gameObject.tag = "Bullet_Rest_E";
			} else if (gameObject.CompareTag ("Bullet_2")) {
				gameObject.tag = "Bullet_Rest_2";
			} else {
				gameObject.tag = "Bullet_Rest";
			}
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.SetParent(Bullet_Rest.transform);
		}
		//Enemy
		if(coll.gameObject.CompareTag("Enemy") && this.tag != "Bullet_E"){
			//_HealthManager.objHealth = HealthManager.ObjectsHealth.player2;
			//_HealthManager.SendMessage("ApplyDamage", 2); //damage done
			GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_1.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_1.transform.position = gameObject.transform.position;
			Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
			Bullet_Destroy_1.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (gameObject.CompareTag ("Bullet_E")) {
				gameObject.tag = "Bullet_Rest_E";
			} else if (gameObject.CompareTag ("Bullet_2")) {
				gameObject.tag = "Bullet_Rest_2";
			} else {
				gameObject.tag = "Bullet_Rest";
			}
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.SetParent(Bullet_Rest.transform);
		}
		//obstacle 1
		if (coll.gameObject.CompareTag ("Obstacle")) {

			if (coll.gameObject.name == "OBSTY TEST") {
				_HealthManager.objHealth = HealthManager.ObjectsHealth.obstacle1;

				_HealthManager.SendMessage("ApplyDamage", 1); //damage done

				if (_HealthManager.obstacle1_health <= 0)
					Destroy (coll.gameObject);
				
			} else if (coll.gameObject.name == "OBSTY TEST 2") {
				_HealthManager.objHealth = HealthManager.ObjectsHealth.obstacle2;

				_HealthManager.SendMessage("ApplyDamage", 1); //damage done

				if (_HealthManager.obstacle2_health <= 0)
					Destroy (coll.gameObject);
				
			} else if (coll.gameObject.name == "OBSTY TEST 3") {
				_HealthManager.objHealth = HealthManager.ObjectsHealth.obstacle3;

				_HealthManager.SendMessage("ApplyDamage", 1); //damage done

				if (_HealthManager.obstacle3_health <= 0)
					Destroy (coll.gameObject);
				
			} else if (coll.gameObject.name == "OBSTY TEST 4") {
				_HealthManager.objHealth = HealthManager.ObjectsHealth.obstacle4;

				_HealthManager.SendMessage("ApplyDamage", 1); //damage done

				if(_HealthManager.obstacle4_health <= 0)
					Destroy (coll.gameObject);
			}

			GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag ("Bullet_Effect_Stop");
			Bullet_Destroy_1.tag = "Bullet_Effect_Play";
			Bullet_Destroy_1.transform.SetParent (null);
			Bullet_Destroy_1.transform.position = gameObject.transform.position;
			Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
			Bullet_Destroy_1.GetComponent<Animator> ().SetBool ("Destroy", true);

			if (gameObject.CompareTag ("Bullet_E")) {
				gameObject.tag = "Bullet_Rest_E";
			} else if (gameObject.CompareTag ("Bullet_2")) {
				gameObject.tag = "Bullet_Rest_2";
			} else {
				gameObject.tag = "Bullet_Rest";
			}
			gameObject.transform.position = Bullet_Rest.transform.position;
			gameObject.transform.SetParent (Bullet_Rest.transform);
		}
	}
}
