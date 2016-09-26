using UnityEngine;
using System.Collections;

public class Player_Control : MonoBehaviour {
	// Check mouse and player's turret position
	private bool overSprite = false;
	private Vector3 offset;

	//Array of cameras
	int cameraCounts;
	//private Camera[] cameras;

	// Check interver between each mouse down
	private const float set_cooldown = 0.5f; //half a sec
	private float button_cooldown = 0f;
	private int button_count = 0;

	// Use this for initialization
	void Start () {
		//set the sze of the Camera array
		cameraCounts = Camera.allCamerasCount;
		Camera[] cameras = new Camera [cameraCounts];

		Debug.Log ("Camera Counts: " + cameraCounts);
		Debug.Log ("Camera array size: " + cameras.Length);
	}
	
	// Update is called once per frame
	void Update () {
		// Get Camera's width and height (only in orthographic mode)
		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;

		// Get Player_1 Size in worldspace
		Vector2 sprite_size = this.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 player_world_size = local_sprite_size;
		player_world_size.x *= this.transform.lossyScale.x;
		player_world_size.y *= this.transform.lossyScale.y;

		Control (width, height, player_world_size);	
	}
		
	void Control(float width, float height, Vector3 player_world_size)
	{
		if (Input.GetMouseButtonDown (0)) {
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			overSprite = this.GetComponent<SpriteRenderer> ().bounds.Contains (mousePosition);
			offset = new Vector3 (this.transform.position.x, 0, 0) - new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, 0, 0);

			if (overSprite) {
				button_count += 1;
				button_cooldown = set_cooldown;

				if (button_count > 1) {
					Shoot ();
					button_count = 0;
				}
			}
		}

		if (Input.GetMouseButton (0) && overSprite) {
			Dragging (width, height, player_world_size);
		} else {
			overSprite = false;
		}

		//reset bullet count when over time
		if (button_cooldown > 0) {
			button_cooldown -= 1 * Time.deltaTime;
		} else {
			button_count = 0;
		}
	}

	void Dragging(float width, float height, Vector3 player_world_size)
	{
		// Dragging of Player turret
		Vector3 prev_pos = this.transform.position;
		Vector3 new_pos = new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, prev_pos.y, prev_pos.z) + offset;

		if (new_pos.x >= -width / 2 + player_world_size.x / 2 &&
		    new_pos.x <= width / 2 - player_world_size.x / 2) {
			this.transform.position = new_pos;
		} else { 
			if (new_pos.x < -width / 2 + player_world_size.x / 2) {
				new_pos = new Vector3 (-width / 2 + player_world_size.x / 2, prev_pos.y, prev_pos.z);
				this.transform.position = new_pos;
			} else if (new_pos.x > width / 2 - player_world_size.x / 2) {
				new_pos = new Vector3 (width / 2 - player_world_size.x / 2, prev_pos.y, prev_pos.z);
				this.transform.position = new_pos;
			}
		} 
	}

	void Shoot()
	{
		GameObject shoot_location = this.transform.Find ("bullet_pos").gameObject;
		Vector3 shoot_position = shoot_location.transform.position;
		GameObject new_bullet = Instantiate (Resources.Load ("Bullet/bullet"), new Vector3 (shoot_position.x, shoot_position.y, 0f), shoot_location.transform.rotation) as GameObject; 
		if (this.tag == "Player1") {
			new_bullet.layer = LayerMask.NameToLayer ("Player 1");
			new_bullet.tag = "Bullet_1";
		} else {
			new_bullet.layer = LayerMask.NameToLayer ("Player 2");
			new_bullet.tag = "Bullet_2";
		}
	}
}
