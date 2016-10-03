using UnityEngine;
using System.Collections;

/*
 *  Camera_Control : Switch between each Bullet's Camera and Base Camera (Turret)
*/
public class Camera_Control : MonoBehaviour {
	private GameObject P1Cam;
	private GameObject P2Cam;

	private Vector3 Single_OriginalPos; // Single Player Original Camera Position
	private Vector3 P1Cam_OriginalPos;
	private Vector3 P2Cam_OriginalPos;

	private int camera_switch_no = 0; // 0 = base, 1++ = bullet
	private GameObject current_gameobject; // Focus camera on this gameobject

	private GameObject Bg; // To get the size of the background in worldspace
	private int num_bg; // Number of bg place together
	Vector3 bg_world_size; // Get Background size in world size

	// Move current bullet
	private Mode_Control mcontrol;
	private bool Move_Left = false;
	private bool Move_Right = false;

	// Use this for initialization
	void Start () {
		// Get Background and the number of background using tag
		Bg = GameObject.FindGameObjectWithTag ("Background");
		num_bg = GameObject.FindGameObjectsWithTag ("Background").Length;

		Vector2 sprite_size = Bg.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / Bg.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		bg_world_size = local_sprite_size;
		bg_world_size.x *= Bg.transform.lossyScale.x;
		bg_world_size.y *= Bg.transform.lossyScale.y;

		mcontrol = GameObject.Find ("Scripts").GetComponent<Mode_Control> ();
		// Get Camera depending on the Game_Play Mode
		if (mcontrol.game_mode_Single) {
			Single_OriginalPos = Camera.main.transform.position;
		} else {
			P2Cam = GameObject.Find ("Top Camera"); // Get gameobject as a whole such that variable is still updated and able to change
			P1Cam = GameObject.Find ("Bottom Camera");

			P2Cam_OriginalPos = GameObject.Find ("Top Camera").transform.position; // Get the value and won't change according to the Gameobject
			P1Cam_OriginalPos = GameObject.Find ("Bottom Camera").transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!mcontrol.game_mode_Single) { // Multiplayer
			if (current_gameobject != null && current_gameobject.tag != "Bullet_Rest") {
				float height = 0f;
				if(Move_Left)
				{
					current_gameobject.transform.position += Vector3.left * 0.1f;
				}

				if(Move_Right)
				{
					current_gameobject.transform.position += Vector3.right * 0.1f;
				}

				if (this.tag == "Player1") {
					height = 2f * P1Cam.GetComponent<Camera>().orthographicSize;

					Vector3 new_pos = current_gameobject.transform.position + new Vector3 (0, 2, -10);
					// Limit the camera within the background zone
					if ((new_pos.y < bg_world_size.y * num_bg - height - height / 2) && (new_pos.y > -height / 2)) {	
						P1Cam.transform.position = new Vector3 (P1Cam.transform.position.x, new_pos.y, P1Cam.transform.position.z);
					}
				}
				if (this.tag == "Player2") {
					height = 2f * P2Cam.GetComponent<Camera> ().orthographicSize;

					Vector3 new_pos = current_gameobject.transform.position + new Vector3 (0, -2, -10);
					// Limit the camera within the background zone
					if ((new_pos.y > -height / 2) && (new_pos.y < bg_world_size.y * num_bg - height - height / 2)) {	
						P2Cam.transform.position = new Vector3 (P2Cam.transform.position.x, new_pos.y, P2Cam.transform.position.z);;
					}
				}
			} else { // Single
				camera_switch_no = 0;
				mcontrol.move_player = true;
				if (this.tag == "Player1") {
					P1Cam.transform.position = P1Cam_OriginalPos;
				}

				if (this.tag == "Player2") {
					P2Cam.transform.position = P2Cam_OriginalPos;
				}
			}
		} else {
			if (current_gameobject != null && current_gameobject.tag != "Bullet_Rest") {
				Camera cam = Camera.main;
				float height = 2f * cam.orthographicSize;

				if(Move_Left)
				{
					current_gameobject.transform.position += Vector3.left * 0.1f;
				}

				if(Move_Right)
				{
					current_gameobject.transform.position += Vector3.right * 0.1f;
				}

				Vector3 new_pos = current_gameobject.transform.position + new Vector3 (0, 2, -10);
				// Limit the camera within the background zone
				if ((new_pos.y < bg_world_size.y * num_bg - height) && (new_pos.y > 0)) {	
					Camera.main.transform.position = new Vector3 (cam.transform.position.x, new_pos.y, cam.transform.position.z);;
				}
			} else {
				camera_switch_no = 0;
				mcontrol.move_player = true;
				Camera.main.transform.position = Single_OriginalPos;
			}
		}
	}

	public void switch_up() {
		camera_switch_no--;

		GameObject[] all_bullets = null;
		int nbBullet = 0;

		if (this.tag == "Player1") {
			// Find all gameobject with tag : bullet_1
			all_bullets = GameObject.FindGameObjectsWithTag ("Bullet_1");
			nbBullet = all_bullets.Length;
		}

		if (this.tag == "Player2") {
			// Find all gameobject with tag : bullet_2
			all_bullets = GameObject.FindGameObjectsWithTag ("Bullet_2");
			nbBullet = all_bullets.Length;
		}

		// If value is -1 || Check if camera_no is more than bullet on screen
		if (camera_switch_no < 0 || camera_switch_no > nbBullet) {
			camera_switch_no = nbBullet;
		}

		if (camera_switch_no > 0) {
			current_gameobject = all_bullets [camera_switch_no - 1];
			mcontrol.move_player = false;
		} else {
			current_gameobject = null;
			mcontrol.move_player = true;
		}
	}

	public void switch_down() {
		camera_switch_no++;

		GameObject[] all_bullets = null;
		int nbBullet = 0;

		if (this.tag == "Player1") {
			// Find all gameobject with tag : bullet_1
			all_bullets = GameObject.FindGameObjectsWithTag ("Bullet_1");
			nbBullet = all_bullets.Length;
		}

		if (this.tag == "Player2") {
			// Find all gameobject with tag : bullet_2
			all_bullets = GameObject.FindGameObjectsWithTag ("Bullet_2");
			nbBullet = all_bullets.Length;
		}

		if (camera_switch_no > nbBullet) {
			camera_switch_no = 0;
			current_gameobject = null;
			mcontrol.move_player = true;
		} else {
			current_gameobject = all_bullets [camera_switch_no - 1];
			mcontrol.move_player = false;
		}
	}

	public void BulletMove_PressDown_Left()
	{
		Move_Left = true;
	}

	public void BulletMove_PressUp_Left()
	{
		Move_Left = false;
	}

	public void BulletMove_PressDown_Right()
	{
		Move_Right = true;
	}

	public void BulletMove_PressUp_Right()
	{
		Move_Right = false;
	}
}
