﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Player_Control : Check if single or multiplayer mode, 
 * 					different dragging code depending on the mode
 * 
*/
public class Player_Control : Overlay_Control {
	//Access overlay control's scripts variable
	private Overlay_Control m_Overlay_Control;
	// Check mouse and player's turret position
	private bool overSprite = false;
	private Vector3 offset;

	// Check interver between each mouse down
	private const float set_cooldown = 0.5f; //half a sec
	private float button_cooldown = 0f;
	private int button_count = 0;

	private Camera P1Cam;
	private Camera P2Cam;
	public bool game_mode_Single = false; // True - Single, False - Multiplayer
	private int touch_point = -1;

	// Use this for initialization
	void Start () {

		m_Overlay_Control = GameObject.Find ("Scripts").GetComponent<Overlay_Control> ();
		if (GameObject.Find ("Top Camera") == null && GameObject.Find ("Bottom Camera") == null) {
			game_mode_Single = true;
		} else {
			P2Cam = GameObject.Find ("Top Camera").GetComponent<Camera> ();
			P1Cam = GameObject.Find ("Bottom Camera").GetComponent<Camera> ();
		}
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

		//Debug.Log ("panel isActive " + PanelisActive);
		//Allow players to play only when overlay panel is gone
		if(!m_Overlay_Control.PanelisActive){
			Control (width, height, player_world_size);	
		}

	}
		
	void Control(float width, float height, Vector3 player_world_size)
	{
		if (!game_mode_Single) {
			int nbTouches = Input.touchCount;

			if (nbTouches > 0) {
				for (int i = 0; i < nbTouches; i++) {
					Touch touch = Input.GetTouch (i);

					TouchPhase phase = touch.phase;

					switch (phase) {
					case TouchPhase.Began:
						if (this.tag == "Player1") {
							Vector2 touchPosition = P1Cam.ScreenToWorldPoint (touch.position);
							overSprite = this.GetComponent<SpriteRenderer> ().bounds.Contains (touchPosition);
							offset = new Vector3 (this.transform.position.x, 0, 0) - new Vector3 (touchPosition.x, 0, 0);
						}

						if (this.tag == "Player2") {
							Vector2 touchPosition = P2Cam.ScreenToWorldPoint (touch.position);
							overSprite = this.GetComponent<SpriteRenderer> ().bounds.Contains (touchPosition);
							offset = new Vector3 (this.transform.position.x, 0, 0) - new Vector3 (touchPosition.x, 0, 0);
						}


						// if player tap on the player 2 twice, shoot. Else, increase tap_count(button_count)++
						if (overSprite) {
							touch_point = i;

							button_count += 1;
							button_cooldown = set_cooldown;
							if (button_count > 1) {
								Shoot ();
								button_count = 0;
							}
						}
						break;

					case TouchPhase.Moved: //Dragging
						if (touch_point == i) {
							if (this.tag == "Player1") {
								Vector2 touchPosition = P1Cam.ScreenToWorldPoint (touch.position);
								Dragging_touch (width, height, player_world_size, touchPosition);
							}
							if (this.tag == "Player2") {
								Vector2 touchPosition = P2Cam.ScreenToWorldPoint (touch.position);
								Dragging_touch (width, height, player_world_size, touchPosition);
							}
						}
						break;

					case TouchPhase.Stationary:
						break;

					case TouchPhase.Ended: // Release touch from screen
						if (touch_point == i) {
							overSprite = false;
							touch_point = -1;
						}
						break;

					case TouchPhase.Canceled:
						break;
					}
				}
			}
		} else {
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
		}

		//reset bullet count when over time
		if (button_cooldown > 0) {
			button_cooldown -= 1 * Time.deltaTime;
		} else {
			button_count = 0;
		}
	}

	//Single PLayer Dragging
	void Dragging(float width, float height, Vector3 player_world_size)
	{
		// Dragging of Player turret
		Vector3 prev_pos = this.transform.position;
		Vector3 new_pos = new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, prev_pos.y, prev_pos.z) + offset;

		// Check if turret is still within camera
		if (new_pos.x >= -width / 2 + player_world_size.x / 2 &&
		    new_pos.x <= width / 2 - player_world_size.x / 2) {
			this.transform.position = new_pos;
		} else { 
			// if turret exceed the left border, snap back to the left border within the camera space
			if (new_pos.x < -width / 2 + player_world_size.x / 2) {
				new_pos = new Vector3 (-width / 2 + player_world_size.x / 2, prev_pos.y, prev_pos.z);
				this.transform.position = new_pos;
			}

			// if turret exceed the right border, snap back to the right border within the camera space
			else if (new_pos.x > width / 2 - player_world_size.x / 2) { 
				new_pos = new Vector3 (width / 2 - player_world_size.x / 2, prev_pos.y, prev_pos.z);
				this.transform.position = new_pos;
			}
		} 
	}

	//Multiplayer Dragging : Added Touch_position to deduce repeated calculation
	void Dragging_touch(float width, float height, Vector3 player_world_size, Vector2 touch_position)
	{
		Vector3 prev_pos = this.transform.position;
		Vector3 new_pos = new Vector3(touch_position.x, prev_pos.y, prev_pos.z);

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
		GameObject new_bullet = Instantiate (Resources.Load ("Bullet/Bullet 1"), new Vector3 (shoot_position.x, shoot_position.y, 0f), shoot_location.transform.rotation) as GameObject; 
		if (this.tag == "Player1") {
			new_bullet.layer = LayerMask.NameToLayer ("Player 1");
			new_bullet.tag = "Bullet_1";
		} else {
			new_bullet.layer = LayerMask.NameToLayer ("Player 2");
			new_bullet.tag = "Bullet_2";
		}
	}
}
