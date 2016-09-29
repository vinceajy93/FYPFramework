using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Bullet_Movement : MonoBehaviour {

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
	// Use this for initialization
	void Start () {

		//debugtext
		debugtext = GameObject.Find("debugtext").GetComponent<Text>();
		debugtext.text = ("bullet works");
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
		if (coll.gameObject.tag == "Bullet_1") {
			Debug.Log ("bullet collided");
		}
			
			//coll.gameObject.SendMessage("ApplyDamage", 10);

	}
}
