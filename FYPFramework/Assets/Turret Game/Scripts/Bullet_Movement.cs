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

	private Camera P1Cam;
	private Camera P2Cam;
	private bool mode = false; // True - Single, False - Multiplayer

	private GameObject indicator;
	private Vector3 bg_world_size;
	private float object_max_height;

	private Camera cam;
	private float cam_height; // Size of camera in y

	//ProjectileDmg projectileDmg;

	// Use this for initialization
	void Start () {

		//debugtext
		debugtext = GameObject.Find("debugtext").GetComponent<Text>();
		debugtext.text = ("bullet works");

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

		if (GameObject.Find ("Top Camera") == null && GameObject.Find ("Bottom Camera") == null) {
			mode = true;
		} else {
			P2Cam = GameObject.Find ("Top Camera").GetComponent<Camera> ();
			P1Cam = GameObject.Find ("Bottom Camera").GetComponent<Camera> ();
		}

		object_max_height = (bg_world_size.y * num_bg) - (cam_height / 2);

		foreach (Transform child in transform) {
			if (child.gameObject.tag == "Indicator") {
				indicator = child.gameObject;
				break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		//Bullet movement, Delete if outside of background (Y axis)
		if (gameObject.tag == "Bullet_1") {
			gameObject.transform.localPosition += Vector3.up * 0.1f;

			if(gameObject.transform.position.y > object_max_height)
				Destroy (gameObject);	
		} 
		else if (gameObject.tag == "Bullet_2") {
			gameObject.transform.localPosition += Vector3.down * 0.15f;

			if (gameObject.transform.position.y < (-cam_height / 2))
				Destroy (gameObject);	
		}

		// Indicator's code
		if (mode) {
			if (this.transform.position.y > (cam_height / 2) + cam.transform.position.y || this.transform.position.y < (-cam_height / 2) + cam.transform.position.y) {
				indicator.SetActive (true);
			} else
				indicator.SetActive (false);
		} else {
			// Player1's bullet to be shown Player2's screen
			Camera Player_Cam = null;
			float Player_Cam_height = 0;

			if (this.tag == "Bullet_1") {
				Player_Cam = P2Cam;
				Player_Cam_height = 2f * Player_Cam.orthographicSize;
			}

			if (this.tag == "Bullet_2") {
				Player_Cam = P1Cam;
				Player_Cam_height = 2f * Player_Cam.orthographicSize;

			}

			// If enemy's bullet is in the player's camera, set indiactor to false (indicator to be not shown)
			if (this.transform.position.y > (Player_Cam_height / 2) + Player_Cam.transform.position.y || this.transform.position.y < (-Player_Cam_height / 2) + Player_Cam.transform.position.y) {
				indicator.SetActive (true);
			} else
				indicator.SetActive (false);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//bullet 1
		if (coll.gameObject.CompareTag("Bullet_1")) {
			Destroy (coll.gameObject);
			Destroy (gameObject);
		}
		//bullet 2
		if (coll.gameObject.CompareTag("Bullet_2")) {
			Destroy (coll.gameObject);
			Destroy (gameObject);
		}
		//wall 1
		if(coll.gameObject.CompareTag("player1_wall")){
			Destroy (gameObject); //bullet
		}
		//wall 2
		if(coll.gameObject.CompareTag("player2_wall")){
			_HealthManager.objHealth = HealthManager.ObjectsHealth.wall2;
			_HealthManager.SendMessage("ApplyDamage", 1); //damage done
			Destroy (gameObject); //bullet
		}
		//player 1
		if(coll.gameObject.CompareTag("Player1")){
			_HealthManager.objHealth = HealthManager.ObjectsHealth.player1;
			_HealthManager.SendMessage("ApplyDamage", 2); //damage done
			Destroy (gameObject); //bullet

		}
		//player 2
		if(coll.gameObject.CompareTag("Player2")){
			_HealthManager.objHealth = HealthManager.ObjectsHealth.player2;
			_HealthManager.SendMessage("ApplyDamage", 2); //damage done
			Destroy (gameObject); //bullet
		}
	}
}
