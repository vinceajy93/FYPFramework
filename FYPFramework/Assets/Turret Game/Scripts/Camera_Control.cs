using UnityEngine;
using System.Collections;

/*
 *  Camera_Control : Switch between each Bullet's Camera and Base Camera (Turret)
*/
public class Camera_Control : MonoBehaviour
{
    // Getting some camera varibles
    private GameObject P1Cam;
    private GameObject P2Cam;

    private float PCam_Height;
    private float PCam_Width;

	private Camera Camera_P1Cam;
	private Camera Camera_P2Cam;

	private Camera MainCam;
	private float MainCam_Height;
	private float MainCam_Width;

	private int touch_point = -1;
	private PauseScript _pauseScript;
	private Overlay_Control _panelScript;

    private Vector3 Single_OriginalPos; // Single Player Original Camera Position
    private Vector3 P1Cam_OriginalPos;
    private Vector3 P2Cam_OriginalPos;

    private int camera_switch_no = 0; // 0 = base, 1++ = bullet
    private GameObject current_gameobject; // Focus camera on this gameobject
	private Vector3 current_GO_Size;

    private GameObject Bg; // To get the size of the background in worldspace
    private int num_bg; // Number of bg place together
    Vector3 bg_world_size; // Get Background size in world size

    // Move current bullet
    private Mode_Control mcontrol;
    private bool Move_Left = false;
    private bool Move_Right = false;
    private Animator animator;

	// Check mouse and player's turret position
	private bool overSprite = false;
	private Vector3 final_pos;

	private GameObject Boundary_P1;
	private GameObject Boundary_P2;

	private RectTransform P1_Screen;
	private RectTransform P2_Screen;

    // Use this for initialization
    void Start()
    {
        // Get Background and the number of background using tag
        Bg = GameObject.FindGameObjectWithTag("Background");
        num_bg = GameObject.FindGameObjectsWithTag("Background").Length;
		_pauseScript = GameObject.Find("Scripts").GetComponent<PauseScript>();
		_panelScript = GameObject.Find ("Scripts").GetComponent<Overlay_Control> ();
		mcontrol = GameObject.Find("Scripts").GetComponent<Mode_Control>();

		Vector2 sprite_size = Bg.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 local_sprite_size = sprite_size / Bg.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        bg_world_size = local_sprite_size;
        bg_world_size.x *= Bg.transform.lossyScale.x;
        bg_world_size.y *= Bg.transform.lossyScale.y;

		Boundary_P1 = GameObject.FindGameObjectWithTag ("Boundary_P1");
		Boundary_P2 = GameObject.FindGameObjectWithTag ("Boundary_P2");

        // Get Camera depending on the Game_Play Mode
        if (mcontrol.game_mode_Single)
        {
			// Get Camera's width and height (only in orthographic mode)
			MainCam = Camera.main;
			MainCam_Height = 2f * MainCam.orthographicSize;
			MainCam_Width = MainCam_Height * MainCam.aspect;
			
            Single_OriginalPos = Camera.main.transform.position;
        }
        else
        {
            P2Cam = GameObject.Find("Top Camera"); // Get gameobject as a whole such that variable is still updated and able to change
            P1Cam = GameObject.Find("Bottom Camera");
			Camera_P1Cam = P1Cam.GetComponent<Camera> ();
			Camera_P2Cam = P2Cam.GetComponent<Camera> ();

            P2Cam_OriginalPos = GameObject.Find("Top Camera").transform.position; // Get the value and won't change according to the Gameobject
            P1Cam_OriginalPos = GameObject.Find("Bottom Camera").transform.position;

			PCam_Height = 2f * Camera_P1Cam.orthographicSize;
			PCam_Width = PCam_Height * Camera_P1Cam.aspect;

			P1_Screen = GameObject.FindGameObjectWithTag ("Screen_P1").GetComponent<RectTransform> ();
			P2_Screen = GameObject.FindGameObjectWithTag ("Screen_P2").GetComponent<RectTransform> ();
        }
    }

    // Update is called once per frame
    void Update()
    {
		if (current_gameobject != null && !current_gameobject.CompareTag ("Bullet_Rest") && !current_gameobject.CompareTag ("Bullet_Rest_2")) {
			// If Camera following the bullet but it wasn't suppose to, Set Camera Back to follow player, 
			if (!current_gameobject.GetComponent<Bullet_Movement> ().bullet_follow) {
				camera_switch_no = 0;
				current_gameobject = null;
				current_GO_Size = Vector3.zero;

				if (!mcontrol.game_mode_Single) { // Multiplayer
					if (this.CompareTag ("Player1")) {
						mcontrol.move_player_P1 = true;
						P1Cam.transform.position = P1Cam_OriginalPos;
					}

					if (this.CompareTag ("Player2")) {
						mcontrol.move_player_P2 = true;
						P2Cam.transform.position = P2Cam_OriginalPos;
					}
				} else { // Single Player
					mcontrol.move_player_P1 = true;
					Camera.main.transform.position = Single_OriginalPos;
				}
			} else { // Camera following the bullet
				if ((this.CompareTag ("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag ("Player2") && !mcontrol.card_menu_P2)) {
					// Inside the boundary, move player left and right + Animation
					if ((current_gameobject.transform.position.y < Boundary_P2.transform.position.y) && (current_gameobject.transform.position.y > Boundary_P1.transform.position.y)) {
						if (!_panelScript.PanelisActive && !_pauseScript.Paused) {
							if (!mcontrol.game_mode_Single) {
								//Button_Press (PCam_Width);
								Dragging_Control (PCam_Width);
							} else {
								//Button_Press (MainCam_Width);
								Dragging_Control (MainCam_Width);
							}
						}
					} else { // Outside the boundary, Unable to move left and right + Animation false
						if (animator != null) {
							animator.SetBool ("Left", false);
							animator.SetBool ("Right", false);
						}
					}
				}

				if (!mcontrol.game_mode_Single) {
					if (this.CompareTag ("Player1")) {
						Vector3 new_pos = current_gameobject.transform.position + new Vector3 (0, PCam_Height / 4, 0);
						// Limit the camera within the background zone
						if ((new_pos.y > P1Cam_OriginalPos.y) && (new_pos.y < P2Cam_OriginalPos.y)) {
							P1Cam.transform.position = new Vector3 (P1Cam.transform.position.x, new_pos.y, P1Cam.transform.position.z);
						}
					}
					if (this.CompareTag ("Player2")) {
						Vector3 new_pos = current_gameobject.transform.position + new Vector3 (0, -(PCam_Height / 4), 0);
						// Limit the camera within the background zone
						if ((new_pos.y > P1Cam_OriginalPos.y) && (new_pos.y < P2Cam_OriginalPos.y)) {
							P2Cam.transform.position = new Vector3 (P2Cam.transform.position.x, new_pos.y, P2Cam.transform.position.z);
						}
					}
				} else {
					Vector3 new_pos = current_gameobject.transform.position + new Vector3 (0, MainCam_Height / 4, -10);
					// Limit the camera within the background zone
					if ((new_pos.y > Single_OriginalPos.y) && (new_pos.y < bg_world_size.y * num_bg - MainCam_Height)) {
						Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, new_pos.y, Camera.main.transform.position.z);
					}
				}
			}
		} else {
			camera_switch_no = 0;
			current_gameobject = null;
			current_GO_Size = Vector3.zero;

			if (!mcontrol.game_mode_Single) { // Multiplayer
				if (this.CompareTag ("Player1")) {
					mcontrol.move_player_P1 = true;
					P1Cam.transform.position = P1Cam_OriginalPos;
				}

				if (this.CompareTag ("Player2")) {
					mcontrol.move_player_P2 = true;
					P2Cam.transform.position = P2Cam_OriginalPos;
				}
			} else { // Single Player
				mcontrol.move_player_P1 = true;
				Camera.main.transform.position = Single_OriginalPos;
			}
		}
    }

	void Button_Press (float Camera_Width) {
		if (Move_Left) {
			if (animator != null) {
				if (current_gameobject.CompareTag ("Bullet_1"))
					animator.SetBool ("Left", true);

				if (current_gameobject.CompareTag ("Bullet_2"))
					animator.SetBool ("Right", true);
			}

			Vector3 new_pos = current_gameobject.transform.position + (Vector3.left * 0.1f);
			if (new_pos.x > -Camera_Width / 2 + current_GO_Size.x / 2) {
				current_gameobject.transform.position = new_pos;
			}
		}

		if (Move_Right) {
			if (animator != null) {
				if (current_gameobject.CompareTag ("Bullet_1"))
					animator.SetBool ("Right", true);

				if (current_gameobject.CompareTag ("Bullet_2"))
					animator.SetBool ("Left", true);
			}

			Vector3 new_pos = current_gameobject.transform.position + (Vector3.right * 0.1f);
			if (new_pos.x < Camera_Width / 2 - current_GO_Size.x / 2) {
				current_gameobject.transform.position = new_pos;
			}
		}
	}

	void Dragging_Control (float Camera_Width) {
		if (!mcontrol.game_mode_Single) {
			//multiplayer
			int nbTouches = Input.touchCount;

			if (nbTouches > 0) {
				for (int i = 0; i < nbTouches; i++) {
					Touch touch = Input.GetTouch (i);

					TouchPhase phase = touch.phase;

					if (this.CompareTag("Player1")) {
						if (mcontrol.card_menu_P1 || mcontrol.move_player_P1 || current_gameobject.CompareTag("Bullet_2") || !RectTransformUtility.RectangleContainsScreenPoint (P1_Screen, touch.position))
							phase = TouchPhase.Ended;
					}

					if (this.CompareTag("Player2")) {
						if (mcontrol.card_menu_P2 || mcontrol.move_player_P2 || current_gameobject.CompareTag("Bullet_1") || !RectTransformUtility.RectangleContainsScreenPoint (P2_Screen, touch.position))
							phase = TouchPhase.Ended;
					}

					switch (phase) {
					case TouchPhase.Began:
						if (this.CompareTag ("Player1") && RectTransformUtility.RectangleContainsScreenPoint (P1_Screen, touch.position)) {
							RaycastHit2D hit = Physics2D.Raycast (Camera_P1Cam.ScreenToWorldPoint (touch.position), current_gameobject.transform.position);
							if (hit.collider != null && hit.transform.gameObject == current_gameobject) {
								overSprite = true;
							}
						}

						if (this.CompareTag ("Player2") && RectTransformUtility.RectangleContainsScreenPoint (P2_Screen, touch.position)) {
							RaycastHit2D hit = Physics2D.Raycast (Camera_P2Cam.ScreenToWorldPoint (touch.position), current_gameobject.transform.position);
							if (hit.collider != null && hit.transform.gameObject == current_gameobject) {
								overSprite = true;
							}
						}

						if (overSprite) {
							touch_point = i;
						}
						break;

					case TouchPhase.Moved: //Dragging
						if (touch_point == i) {
							if (this.CompareTag ("Player1")) {
								Vector2 touchPosition = Camera_P1Cam.ScreenToWorldPoint (touch.position);
								Dragging (PCam_Width, current_GO_Size, touchPosition);
							}

							if (this.CompareTag ("Player2")) {
								Vector2 touchPosition = Camera_P2Cam.ScreenToWorldPoint (touch.position);
								Dragging (PCam_Width, current_GO_Size, touchPosition);
							}
						}
						break;

					case TouchPhase.Ended: // Release touch from screen
						if (touch_point == i) {
							overSprite = false;
							touch_point = -1;

							if (animator != null) {
								animator.SetBool ("Left", false);
								animator.SetBool ("Right", false);
							}
						}
						break;
					}
				}
			}


			if (Input.GetMouseButtonDown (0)) {
				if (!mcontrol.card_menu_P1 && !mcontrol.move_player_P1 && this.CompareTag ("Player1") && current_gameobject.CompareTag("Bullet_1")) {
					if (RectTransformUtility.RectangleContainsScreenPoint (P1_Screen, Input.mousePosition)) {
						RaycastHit2D hit = Physics2D.Raycast(Camera_P1Cam.ScreenToWorldPoint(Input.mousePosition), current_gameobject.transform.position);
						if (hit.collider != null && hit.transform.gameObject == current_gameobject) {
							overSprite = true;
						}
					}
				}

				if (!mcontrol.card_menu_P2 && !mcontrol.move_player_P2 && this.CompareTag ("Player2") && current_gameobject.CompareTag("Bullet_2")) {
					if (RectTransformUtility.RectangleContainsScreenPoint (P2_Screen, Input.mousePosition)) {
						RaycastHit2D hit = Physics2D.Raycast(Camera_P2Cam.ScreenToWorldPoint(Input.mousePosition), current_gameobject.transform.position);
						if (hit.collider != null && hit.transform.gameObject == current_gameobject) {
							overSprite = true;
						}
					}
				}
			}

			if (Input.GetMouseButton (0) && overSprite) {
				if (this.CompareTag ("Player1")) {
					Vector2 mousePosition = Camera_P1Cam.ScreenToWorldPoint (Input.mousePosition);
					Dragging (PCam_Width, current_GO_Size, mousePosition);
				}

				if (this.CompareTag ("Player2")) {
					Vector2 mousePosition = Camera_P2Cam.ScreenToWorldPoint (Input.mousePosition);
					Dragging (PCam_Width, current_GO_Size, mousePosition);
				}
			} else {
				overSprite = false;

				if (animator != null) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", false);
				}
			}
		} else { //single player
			if (Input.GetMouseButtonDown (0) && !mcontrol.card_menu_P1) {
				if (!mcontrol.card_menu_P1 && !mcontrol.move_player_P1 && this.CompareTag ("Player1") && current_gameobject.CompareTag("Bullet_1")) {
					RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), current_gameobject.transform.position);
					if (hit.collider != null && hit.transform.gameObject == current_gameobject) {
						overSprite = true;
					}
				}
			}

			if (Input.GetMouseButton (0) && overSprite) {
				Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Dragging (MainCam_Width, current_GO_Size, mousePosition);
			} else {
				overSprite = false;

				if (animator != null) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", false);
				}
			}
		}
	}
		
	void Dragging(float width, Vector3 current_world_size, Vector2 Dragging_Position)
	{
		// Dragging of Player turret
		Vector3 prev_pos = current_gameobject.transform.position;
		Vector3 new_pos = new Vector3(Dragging_Position.x, prev_pos.y, prev_pos.z);

		if (prev_pos.x > Dragging_Position.x) {
			new_pos = current_gameobject.transform.position + (Vector3.left * 0.1f);

			if (animator != null) {
				if (current_gameobject.CompareTag ("Bullet_1")) {
					animator.SetBool ("Left", true);
					animator.SetBool ("Right", false);
				}

				if (current_gameobject.CompareTag ("Bullet_2")) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", true);
				}
			}

			if (new_pos.x < Dragging_Position.x) {
				new_pos = new Vector3 (Dragging_Position.x, prev_pos.y, prev_pos.z);

				if (animator != null) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", false);
				}
			}
		} else if (prev_pos.x < Dragging_Position.x) {
			new_pos = current_gameobject.transform.position + (Vector3.right * 0.1f);

			if (animator != null) {
				if (current_gameobject.CompareTag ("Bullet_1")) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", true);
				}

				if (current_gameobject.CompareTag ("Bullet_2")) {
					animator.SetBool ("Left", true);
					animator.SetBool ("Right", false);
				}
			}

			if (new_pos.x > Dragging_Position.x) {
				new_pos = new Vector3 (Dragging_Position.x, prev_pos.y, prev_pos.z);

				if (animator != null) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", false);
				}
			}
		} else {
			if (animator != null) {
				animator.SetBool ("Left", false);
				animator.SetBool ("Right", false);
			}
		}

		// Check if turret is still within camera
		if (new_pos.x >= -width / 2 + current_world_size.x / 2 &&
			new_pos.x <= width / 2 - current_world_size.x / 2)
		{
			current_gameobject.transform.position = new_pos;
		}
		else
		{
			// if turret exceed the left border, snap back to the left border within the camera space
			if (new_pos.x < -width / 2 + current_world_size.x / 2)
			{
				new_pos = new Vector3(-width / 2 + current_world_size.x / 2, prev_pos.y, prev_pos.z);
				current_gameobject.transform.position = new_pos;

				if (animator != null) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", false);
				}
			}

			// if turret exceed the right border, snap back to the right border within the camera space
			else if (new_pos.x > width / 2 - current_world_size.x / 2)
			{
				new_pos = new Vector3(width / 2 - current_world_size.x / 2, prev_pos.y, prev_pos.z);
				current_gameobject.transform.position = new_pos;

				if (animator != null) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", false);
				}
			}
		}
	}

    public void switch_up()
    {
        if ((this.CompareTag("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag("Player2") && !mcontrol.card_menu_P2))
        {
            camera_switch_no--;

            GameObject[] all_bullets = null;
            int nbBullet = 0;
            if (this.CompareTag("Player1"))
			{
                // Find all gameobject with tag : bullet_1
                all_bullets = GameObject.FindGameObjectsWithTag("Bullet_1");
                nbBullet = all_bullets.Length;
            }

            if (this.CompareTag("Player2"))
			{
                // Find all gameobject with tag : bullet_2
                all_bullets = GameObject.FindGameObjectsWithTag("Bullet_2");
                nbBullet = all_bullets.Length;
            }

            // If value is -1 || Check if camera_no is more than bullet on screen
            if (camera_switch_no < 0 || camera_switch_no > nbBullet)
            {
                camera_switch_no = nbBullet;
            }

            if (camera_switch_no > 0 && current_gameobject == all_bullets[camera_switch_no - 1])
            {
                while (current_gameobject == all_bullets[camera_switch_no - 1])
                {
                    camera_switch_no--;
                    if (camera_switch_no <= 0)
                        break;
                }
            }

			if (animator != null && current_gameobject != null)
            {
                current_gameobject.SendMessage("SetFollow", false);
                animator = current_gameobject.GetComponent<Animator>();
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
            }

            if (camera_switch_no > 0)
            {
                current_gameobject = all_bullets[camera_switch_no - 1];
                current_gameobject.SendMessage("SetFollow", true);

				Vector2 sprite_size = current_gameobject.GetComponent<SpriteRenderer> ().sprite.rect.size;
				Vector2 local_sprite_size = sprite_size / current_gameobject.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
				current_GO_Size = local_sprite_size;
				current_GO_Size.x *= current_gameobject.transform.lossyScale.x;
				current_GO_Size.y *= current_gameobject.transform.lossyScale.y;

                animator = current_gameobject.GetComponent<Animator>();
                if (this.CompareTag("Player1"))
                    mcontrol.move_player_P1 = false;
                else
                    mcontrol.move_player_P2 = false;
            }
            else
            {
                current_gameobject = null;
				current_GO_Size = Vector3.zero;

                animator = null;
                if (this.CompareTag("Player1"))
                    mcontrol.move_player_P1 = true;
                else
                    mcontrol.move_player_P2 = true;
            }
        }
    }

    public void switch_down()
    {
        if ((this.CompareTag("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag("Player2") && !mcontrol.card_menu_P2))
        {
			camera_switch_no++;

            GameObject[] all_bullets = null;
            int nbBullet = 0;

            if (this.CompareTag("Player1"))
            {
                // Find all gameobject with tag : bullet_1
                all_bullets = GameObject.FindGameObjectsWithTag("Bullet_1");
                nbBullet = all_bullets.Length;
            }

            if (this.CompareTag("Player2"))
            {
                // Find all gameobject with tag : bullet_2
                all_bullets = GameObject.FindGameObjectsWithTag("Bullet_2");
                nbBullet = all_bullets.Length;
            }

            if (camera_switch_no <= nbBullet && current_gameobject == all_bullets[camera_switch_no - 1])
            {
                while (current_gameobject == all_bullets[camera_switch_no - 1])
                {
                    camera_switch_no++;
                    if (camera_switch_no > nbBullet)
                        break;
                }
            }

			if (animator != null && current_gameobject != null)
            {
                current_gameobject.SendMessage("SetFollow", false);
                animator = current_gameobject.GetComponent<Animator>();
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
            }

            if (camera_switch_no > nbBullet)
            {
                camera_switch_no = 0;
                current_gameobject = null;
				current_GO_Size = Vector3.zero;

                animator = null;
                if (this.CompareTag("Player1"))
                    mcontrol.move_player_P1 = true;
                else
                    mcontrol.move_player_P2 = true;
            }
            else
            {
                current_gameobject = all_bullets[camera_switch_no - 1];
                current_gameobject.SendMessage("SetFollow", true);

				Vector2 sprite_size = current_gameobject.GetComponent<SpriteRenderer> ().sprite.rect.size;
				Vector2 local_sprite_size = sprite_size / current_gameobject.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
				current_GO_Size = local_sprite_size;
				current_GO_Size.x *= current_gameobject.transform.lossyScale.x;
				current_GO_Size.y *= current_gameobject.transform.lossyScale.y;

                animator = current_gameobject.GetComponent<Animator>();
                if (this.CompareTag("Player1"))
                    mcontrol.move_player_P1 = false;
                else
                    mcontrol.move_player_P2 = false;
            }
        }
    }

    public void BulletMove_PressDown_Left()
    {
        if ((this.CompareTag("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag("Player2") && !mcontrol.card_menu_P2))
        {
            Move_Left = true;

			if (animator != null && current_gameobject != null)
            {
				if (current_gameobject.transform.position.y < Boundary_P2.transform.position.y && current_gameobject.transform.position.y > Boundary_P1.transform.position.y)
                {
                    if (current_gameobject.CompareTag("Bullet_1"))
                        animator.SetBool("Left", true);

                    if (current_gameobject.CompareTag("Bullet_2"))
                        animator.SetBool("Right", true);
                }
                else
                {
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                }
            }
        }
    }

    public void BulletMove_PressUp_Left()
    {
        if ((this.CompareTag("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag("Player2") && !mcontrol.card_menu_P2))
        {
            Move_Left = false;

			if (animator != null && current_gameobject != null)
            {
				if (current_gameobject.transform.position.y < Boundary_P2.transform.position.y && current_gameobject.transform.position.y > Boundary_P1.transform.position.y)
                {
                    if (current_gameobject.CompareTag("Bullet_1"))
                        animator.SetBool("Left", false);

                    if (current_gameobject.CompareTag("Bullet_2"))
                        animator.SetBool("Right", false);
                }
                else
                {
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                }
            }
        }
    }

    public void BulletMove_PressDown_Right()
    {
        if ((this.CompareTag("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag("Player2") && !mcontrol.card_menu_P2))
        {
            Move_Right = true;

			if (animator != null && current_gameobject != null)
            {
				if (current_gameobject.transform.position.y < Boundary_P2.transform.position.y && current_gameobject.transform.position.y > Boundary_P1.transform.position.y)
                {
                    if (current_gameobject.CompareTag("Bullet_1"))
                        animator.SetBool("Right", true);

                    if (current_gameobject.CompareTag("Bullet_2"))
                        animator.SetBool("Left", true);
                }
                else
                {
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                }
            }
        }
    }

    public void BulletMove_PressUp_Right()
    {
        if ((this.CompareTag("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag("Player2") && !mcontrol.card_menu_P2))
        {
            Move_Right = false;

			if (animator != null && current_gameobject != null)
            {
				if (current_gameobject.transform.position.y < Boundary_P2.transform.position.y && current_gameobject.transform.position.y > Boundary_P1.transform.position.y)
                {
                    if (current_gameobject.CompareTag("Bullet_1"))
                        animator.SetBool("Right", false);

                    if (current_gameobject.CompareTag("Bullet_2"))
                        animator.SetBool("Left", false);
                }
                else
                {
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                }
            }
        }
    }
}