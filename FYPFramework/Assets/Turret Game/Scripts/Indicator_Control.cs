using UnityEngine;
using System.Collections;

public class Indicator_Control : MonoBehaviour {
	public Sprite red;
	public Sprite yellow;
	public Sprite green;

	private GameObject Bg;
	private int num_bg;

	private bool mode = false; // True - Single, False - Multiplayer
	private Camera P1Cam;
	private Camera P2Cam;

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
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 bg_sprite_size = Bg.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 bg_local_sprite_size = bg_sprite_size / Bg.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 bg_world_size = bg_local_sprite_size;
		bg_world_size.x *= Bg.transform.lossyScale.x;
		bg_world_size.y *= Bg.transform.lossyScale.y;

		// Get Indicator Size in worldspace
		Vector2 sprite_size = this.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 indicator_world_size = local_sprite_size;
		indicator_world_size.x *= this.transform.lossyScale.x;
		indicator_world_size.y *= this.transform.lossyScale.y;

		if (mode) {
			// Get Camera's width and height (only in orthographic mode)
			Camera cam = Camera.main;
			float height = 2f * cam.orthographicSize;

			if (this.transform.parent.position.y > (height / 2) + cam.transform.position.y) {
				this.transform.localEulerAngles = new Vector3 (0, 0, 180);
				this.transform.position = new Vector3 (this.transform.parent.position.x, (height / 2) - (indicator_world_size.y / 2) + cam.transform.position.y, -1);
			} else if (this.transform.parent.position.y < (-height / 2) + cam.transform.position.y) {
				this.transform.localEulerAngles = new Vector3 (0, 0, 0);
				this.transform.position = new Vector3 (this.transform.parent.position.x, (-height / 2) + (indicator_world_size.y / 2) + cam.transform.position.y, -1);
			}

			float total_length = bg_world_size.y * num_bg;
			if (this.transform.parent.position.y > ((total_length / 3) * 2) - (height / 2)) {
				this.GetComponent<SpriteRenderer> ().sprite = green;
			} else if (this.transform.parent.position.y > (total_length / 3) - (height / 2)) {
				this.GetComponent<SpriteRenderer> ().sprite = yellow;
			} else {
				this.GetComponent<SpriteRenderer> ().sprite = red;
			}
		} else {
			if (this.transform.parent.tag == "Bullet_1") {
				Camera cam = P2Cam;
				float height = 2f * cam.orthographicSize;

				if (this.transform.parent.position.y > (height / 2) + cam.transform.position.y) {
					this.transform.localEulerAngles = new Vector3 (0, 0, 180);
					this.transform.position = new Vector3 (this.transform.parent.position.x, (height / 2) - (indicator_world_size.y / 2) + cam.transform.position.y, -1);
				} else if (this.transform.parent.position.y < (-height / 2) + cam.transform.position.y) {
					this.transform.localEulerAngles = new Vector3 (0, 0, 0);
					this.transform.position = new Vector3 (this.transform.parent.position.x, (-height / 2) + (indicator_world_size.y / 2) + cam.transform.position.y, -1);
				}

				float total_length = bg_world_size.y * num_bg;
				if (this.transform.parent.position.y > ((total_length / 3) * 2) - (height / 2)) {
					this.GetComponent<SpriteRenderer> ().sprite = red;
				} else if (this.transform.parent.position.y > (total_length / 3) - (height / 2)) {
					this.GetComponent<SpriteRenderer> ().sprite = yellow;
				} else {
					this.GetComponent<SpriteRenderer> ().sprite = green;
				}
			}

			if (this.transform.parent.tag == "Bullet_2") {
				Camera cam = P1Cam;
				float height = 2f * cam.orthographicSize;

				if (this.transform.parent.position.y > (height / 2) + cam.transform.position.y) {
					this.transform.localEulerAngles = new Vector3 (0, 0, 0);
					this.transform.position = new Vector3 (this.transform.parent.position.x, (height / 2) - (indicator_world_size.y / 2) + cam.transform.position.y, -1);
				} else if (this.transform.parent.position.y < (-height / 2) + cam.transform.position.y) {
					this.transform.localEulerAngles = new Vector3 (0, 0, 180);
					this.transform.position = new Vector3 (this.transform.parent.position.x, (-height / 2) + (indicator_world_size.y / 2) + cam.transform.position.y, -1);
				}

				float total_length = bg_world_size.y * num_bg;
				if (this.transform.parent.position.y > ((total_length / 3) * 2) - (height / 2)) {
					this.GetComponent<SpriteRenderer> ().sprite = green;
				} else if (this.transform.parent.position.y > (total_length / 3) - (height / 2)) {
					this.GetComponent<SpriteRenderer> ().sprite = yellow;
				} else {
					this.GetComponent<SpriteRenderer> ().sprite = red;
				}
			}
		}
	}
}
