using UnityEngine;
using System.Collections;

public class Bullet_Movement : MonoBehaviour {
	private GameObject Bg;
	private int num_bg;

	private Camera P1Cam;
	private Camera P2Cam;
	private bool mode = false; // True - Single, False - Multiplayer

	private GameObject indicator;
	// Use this for initialization
	void Start () {
		Bg = GameObject.FindGameObjectWithTag ("Background");
		num_bg = GameObject.FindGameObjectsWithTag ("Background").Length;

		if (GameObject.Find ("Top Camera") == null && GameObject.Find ("Bottom Camera") == null) {
			mode = true;
		} else {
			P2Cam = GameObject.Find ("Top Camera").GetComponent<Camera> ();
			P1Cam = GameObject.Find ("Bottom Camera").GetComponent<Camera> ();
		}

		foreach (Transform child in transform) {
			if (child.gameObject.tag == "Indicator") {
				indicator = child.gameObject;
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;

		Vector2 sprite_size = Bg.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / Bg.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 world_size = local_sprite_size;
		world_size.x *= Bg.transform.lossyScale.x;
		world_size.y *= Bg.transform.lossyScale.y;

		//Bullet movement, Delete if outside of background (Y axis)
		if (gameObject.tag == "Bullet_1") {
			gameObject.transform.localPosition += Vector3.up * Time.deltaTime * 10;

			if(gameObject.transform.position.y > (world_size.y * num_bg) - (height / 2))
				Destroy (gameObject);	
		} else if (gameObject.tag == "Bullet_2") {
			gameObject.transform.localPosition += Vector3.down * Time.deltaTime * 10;

			if (gameObject.transform.position.y < (-height / 2))
				Destroy (gameObject);	
		}

		if (mode) {
			if (this.transform.position.y > (height / 2) + cam.transform.position.y || this.transform.position.y < (-height / 2) + cam.transform.position.y) {
				indicator.SetActive (true);
			} else
				indicator.SetActive (false);
		} else {
			// Player1's bullet to be shown Player2's screen
			if (this.tag == "Bullet_1") {
				Camera Player_Cam = P2Cam;
				float Player_Cam_height = 2f * Player_Cam.orthographicSize;

				if (this.transform.position.y > (Player_Cam_height / 2) + Player_Cam.transform.position.y || this.transform.position.y < (-Player_Cam_height / 2) + Player_Cam.transform.position.y) {
					indicator.SetActive (true);
				} else
					indicator.SetActive (false);
			}

			if (this.tag == "Bullet_2") {
				Camera Player_Cam = P1Cam;
				float Player_Cam_height = 2f * Player_Cam.orthographicSize;

				if (this.transform.position.y > (Player_Cam_height / 2) + Player_Cam.transform.position.y || this.transform.position.y < (-Player_Cam_height / 2) + Player_Cam.transform.position.y) {
					indicator.SetActive (true);
				} else
					indicator.SetActive (false);
			}
		}
	}
}
