using UnityEngine;
using System.Collections;

public class Arcade_Player : MonoBehaviour 
{
	private Arcade_Control _scriptController;
	private float camera_height;
	private float camera_width;

	private Vector3 player_size;

	private float fireRate = 1.0f;
	private float nextFire = 0.5f;
	private GameObject shooting_location;

	private bool overSprite = false;
	private int touch_point = -1;

	private float delTime;

	void Start () {
		_scriptController = GameObject.Find ("Script").GetComponent<Arcade_Control> ();

		camera_height = 2f * Camera.main.orthographicSize;
		camera_width = camera_height * Camera.main.aspect;

		fireRate = fireRate - (_scriptController._Speed / 100);
		shooting_location = this.transform.Find ("bullet_pos").gameObject;

		// Get Player_1 Size in worldspace
		Vector2 sprite_size = this.GetComponent<SpriteRenderer>().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		player_size = local_sprite_size;
		player_size.x *= this.transform.lossyScale.x;
		player_size.y *= this.transform.lossyScale.y;
	}

	void Update () {
		if (!_scriptController.bPause) {
			// Player movement
			Mouse_Input ();

			delTime += Time.deltaTime;
			if (delTime > nextFire) {
				nextFire = delTime + fireRate;
				Shoot ();
			}
		}
	}

	void Touch_Input () {
		int nbTouches = Input.touchCount;

		for (int i = 0; i < nbTouches; i++) {
			Touch touch = Input.GetTouch (i);

			TouchPhase phase = touch.phase;

			switch (phase) {
			case TouchPhase.Began:
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (touch.position), this.transform.position);
				if (hit.collider != null && hit.transform.gameObject == this.gameObject) {
					overSprite = true;
					touch_point = i;
				}
				break;

			case TouchPhase.Moved:
				if (touch_point == i && overSprite) {
					Vector2 touchPosition = Camera.main.ScreenToWorldPoint (touch.position);
					Dragging (touchPosition);
				}
				break;

			case TouchPhase.Ended:
				if (touch_point == i) {
					touch_point = -1;
					overSprite = false;
				}
				break;
			}
		}
	}

	void Mouse_Input () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), this.transform.position);
			if (hit.collider != null && hit.transform.gameObject == this.gameObject) {
				overSprite = true;
			}
		}

		if (Input.GetMouseButton (0) && overSprite) {
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Dragging (mousePosition);
		} else {
			overSprite = false;
		}
	}

	void Dragging (Vector2 Dragging_Position) {
		Vector3 prev_pos = this.transform.position;
		Vector3 next_pos = this.transform.position;

		if (prev_pos.x > Dragging_Position.x) {
			next_pos = this.transform.position + (Vector3.left * 0.1f);

			if (next_pos.x < Dragging_Position.x) {
				next_pos = new Vector3 (Dragging_Position.x, prev_pos.y, prev_pos.z);
			}
		} else if (prev_pos.x < Dragging_Position.x) {
			next_pos = this.transform.position + (Vector3.right * 0.1f);

			if (next_pos.x > Dragging_Position.x) {
				next_pos = new Vector3 (Dragging_Position.x, prev_pos.y, prev_pos.z);
			}
		}

		// Check if turret is still within camera
		if (next_pos.x >= -camera_width / 2 + player_size.x / 2 &&
			next_pos.x <= camera_width / 2 - player_size.x / 2) {
			this.transform.position = next_pos;
		} else {
			if (next_pos.x < -camera_width / 2 + player_size.x / 2) {
				next_pos = new Vector3(-camera_width / 2 + player_size.x / 2, prev_pos.y, prev_pos.z);
				this.transform.position = next_pos;
			} else if (next_pos.x > camera_width / 2 - player_size.x / 2) {
				next_pos = new Vector3(camera_width / 2 - player_size.x / 2, prev_pos.y, prev_pos.z);
				this.transform.position = next_pos;
			}
		}

		// Refresh the variable
		prev_pos = this.transform.position;
		next_pos = this.transform.position;

		if (prev_pos.y > Dragging_Position.y) {
			next_pos = this.transform.position + (Vector3.down * 0.1f);

			if (next_pos.y < Dragging_Position.y) {
				next_pos = new Vector3 (prev_pos.x, Dragging_Position.y, prev_pos.z);
			}
		} else if (prev_pos.y < Dragging_Position.y) {
			next_pos = this.transform.position + (Vector3.up * 0.1f);

			if (next_pos.y > Dragging_Position.y) {
				next_pos = new Vector3 (prev_pos.x, Dragging_Position.y, prev_pos.z);
			}
		}

		// Check if turret is still within camera
		if (next_pos.y >= -camera_height / 2 + player_size.y / 2 &&
		    next_pos.y <= camera_height / 2 - player_size.y / 2) {
			this.transform.position = next_pos;
		} else {
			if (next_pos.y < -camera_height / 2 + player_size.y / 2) {
				next_pos = new Vector3(prev_pos.x, -camera_height / 2 + player_size.y / 2, prev_pos.z);
				this.transform.position = next_pos;
			} else if (next_pos.y > camera_height / 2 - player_size.y / 2) {
				next_pos = new Vector3(prev_pos.x, camera_height / 2 - player_size.y / 2, prev_pos.z);
				this.transform.position = next_pos;
			}
		}
	}

	void Shoot () {
		GameObject bullet_clone = Instantiate (_scriptController.GO_Bullet) as GameObject;
		bullet_clone.transform.position = shooting_location.transform.position;
		bullet_clone.transform.rotation = shooting_location.transform.rotation;
		bullet_clone.transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
		bullet_clone.tag = "Bullet_1";
		bullet_clone.GetComponent<Bullet_Movement> ().enabled = false;
		bullet_clone.GetComponent<Indicator> ().enabled = false;
		bullet_clone.GetComponent<Animator> ().enabled = false;
		bullet_clone.AddComponent<Arcade_Bullet> ();
	}
}
