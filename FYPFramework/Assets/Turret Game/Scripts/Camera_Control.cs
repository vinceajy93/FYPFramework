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

    private Vector3 Single_OriginalPos; // Single Player Original Camera Position
    private Vector3 P1Cam_OriginalPos;
    private Vector3 P2Cam_OriginalPos;

    private int camera_switch_no = 0; // 0 = base, 1++ = bullet
    private GameObject current_gameobject; // Focus camera on this gameobject

    private GameObject Bg; // To get the size of the background in worldspace
    private int num_bg; // Number of bg place together
    Vector3 bg_world_size; // Get Background size in world size
    float total_bg_height;

    // Move current bullet
    private Mode_Control mcontrol;
    private bool Move_Left = false;
    private bool Move_Right = false;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        // Get Background and the number of background using tag
        Bg = GameObject.FindGameObjectWithTag("Background");
        num_bg = GameObject.FindGameObjectsWithTag("Background").Length;

        Vector2 sprite_size = Bg.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 local_sprite_size = sprite_size / Bg.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        bg_world_size = local_sprite_size;
        bg_world_size.x *= Bg.transform.lossyScale.x;
        bg_world_size.y *= Bg.transform.lossyScale.y;
        total_bg_height = bg_world_size.y * num_bg;

        mcontrol = GameObject.Find("Scripts").GetComponent<Mode_Control>();
        // Get Camera depending on the Game_Play Mode
        if (mcontrol.game_mode_Single)
        {
            Single_OriginalPos = Camera.main.transform.position;
        }
        else
        {
            P2Cam = GameObject.Find("Top Camera"); // Get gameobject as a whole such that variable is still updated and able to change
            P1Cam = GameObject.Find("Bottom Camera");

            P2Cam_OriginalPos = GameObject.Find("Top Camera").transform.position; // Get the value and won't change according to the Gameobject
            P1Cam_OriginalPos = GameObject.Find("Bottom Camera").transform.position;

            PCam_Height = 2f * P1Cam.GetComponent<Camera>().orthographicSize;
            PCam_Width = PCam_Height * P1Cam.GetComponent<Camera>().aspect;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mcontrol.game_mode_Single)
        { // Multiplayer
            if (current_gameobject != null && !current_gameobject.CompareTag("Bullet_Rest") && !current_gameobject.CompareTag("Bullet_Rest_2"))
            {
				if (!current_gameobject.GetComponent<Bullet_Movement> ().bullet_follow) {
					camera_switch_no = 0;
					current_gameobject = null;
					if (this.CompareTag ("Player1")) {
						mcontrol.move_player_P1 = true;
						P1Cam.transform.position = P1Cam_OriginalPos;
					}

					if (this.CompareTag ("Player2")) {
						mcontrol.move_player_P2 = true;
						P2Cam.transform.position = P2Cam_OriginalPos;
					}
				} else {

					if (((this.CompareTag ("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag ("Player2") && !mcontrol.card_menu_P2)) && (current_gameobject.transform.position.y < total_bg_height - (PCam_Height * 2) && current_gameobject.transform.position.y > 0)) {
						if (Move_Left) {
							if (current_gameobject.CompareTag ("Bullet_1"))
								animator.SetBool ("Right", true);

							if (current_gameobject.CompareTag ("Bullet_2"))
								animator.SetBool ("Left", true);
							Vector2 sprite_size = current_gameobject.GetComponent<SpriteRenderer> ().sprite.rect.size;
							Vector2 local_sprite_size = sprite_size / current_gameobject.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
							Vector3 current_world_size = local_sprite_size;
							current_world_size.x *= Bg.transform.lossyScale.x;

							Vector3 new_pos = current_gameobject.transform.position + (Vector3.left * 0.1f);
							if (new_pos.x > -PCam_Width / 2 + current_world_size.x / 2) {
								current_gameobject.transform.position = new_pos;
								//current_gameobject.transform.position += Vector3.left * 0.1f;
							}
						}

						if (Move_Right) {
							if (current_gameobject.CompareTag ("Bullet_1"))
								animator.SetBool ("Left", true);

							if (current_gameobject.CompareTag ("Bullet_2"))
								animator.SetBool ("Right", true);
							Vector2 sprite_size = current_gameobject.GetComponent<SpriteRenderer> ().sprite.rect.size;
							Vector2 local_sprite_size = sprite_size / current_gameobject.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
							Vector3 current_world_size = local_sprite_size;
							current_world_size.x *= Bg.transform.lossyScale.x;

							Vector3 new_pos = current_gameobject.transform.position + (Vector3.right * 0.1f);
							if (new_pos.x < PCam_Width / 2 - current_world_size.x / 2) {
								current_gameobject.transform.position = new_pos;
								//current_gameobject.transform.position += Vector3.right * 0.1f;
							}
						}
					} else {
						animator.SetBool ("Left", false);
						animator.SetBool ("Right", false);
					}

					if (this.CompareTag ("Player1")) {
						Vector3 new_pos = current_gameobject.transform.position + new Vector3 (0, 2, -10);
						// Limit the camera within the background zone
						if ((new_pos.y < bg_world_size.y * num_bg - PCam_Height - PCam_Height / 2) && (new_pos.y > -PCam_Height / 2)) {
							P1Cam.transform.position = new Vector3 (P1Cam.transform.position.x, new_pos.y, P1Cam.transform.position.z);
						}
					}
					if (this.CompareTag ("Player2")) {
						Vector3 new_pos = current_gameobject.transform.position + new Vector3 (0, -2, -10);
						// Limit the camera within the background zone
						if ((new_pos.y > -PCam_Height / 2) && (new_pos.y < bg_world_size.y * num_bg - PCam_Height - PCam_Height / 2)) {
							P2Cam.transform.position = new Vector3 (P2Cam.transform.position.x, new_pos.y, P2Cam.transform.position.z);
							;
						}
					}
				}
            }
            else
            {
                camera_switch_no = 0;
                if (this.CompareTag("Player1"))
                {
                    mcontrol.move_player_P1 = true;
                    P1Cam.transform.position = P1Cam_OriginalPos;
                }

                if (this.CompareTag("Player2"))
                {
                    mcontrol.move_player_P2 = true;
                    P2Cam.transform.position = P2Cam_OriginalPos;
                }
            }
        }
        else
        { // Single
            if (current_gameobject != null && !current_gameobject.CompareTag("Bullet_Rest"))
            {
                Camera cam = Camera.main;
                float height = 2f * cam.orthographicSize;
                float width = height * Camera.main.GetComponent<Camera>().aspect;

				if (!current_gameobject.GetComponent<Bullet_Movement> ().bullet_follow) {
					current_gameobject = null;

					camera_switch_no = 0;
					mcontrol.move_player_P1 = true;
					Camera.main.transform.position = Single_OriginalPos;
				} else {
					if (!mcontrol.card_menu_P1 && (current_gameobject.transform.position.y < (total_bg_height - height) && current_gameobject.transform.position.y > 0)) {
						if (Move_Left) {
							if (current_gameobject.CompareTag ("Bullet_1"))
								animator.SetBool ("Right", true);

							if (current_gameobject.CompareTag ("Bullet_2"))
								animator.SetBool ("Left", true);
							Vector2 sprite_size = current_gameobject.GetComponent<SpriteRenderer> ().sprite.rect.size;
							Vector2 local_sprite_size = sprite_size / current_gameobject.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
							Vector3 current_world_size = local_sprite_size;
							current_world_size.x *= Bg.transform.lossyScale.x;

							Vector3 new_pos = current_gameobject.transform.position + (Vector3.left * 0.1f);
							if (new_pos.x > -width / 2 + current_world_size.x / 2) {
								current_gameobject.transform.position = new_pos;
							}
						}

						if (Move_Right) {
							if (current_gameobject.CompareTag ("Bullet_1"))
								animator.SetBool ("Left", true);

							if (current_gameobject.CompareTag ("Bullet_2"))
								animator.SetBool ("Right", true);
							Vector2 sprite_size = current_gameobject.GetComponent<SpriteRenderer> ().sprite.rect.size;
							Vector2 local_sprite_size = sprite_size / current_gameobject.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
							Vector3 current_world_size = local_sprite_size;
							current_world_size.x *= Bg.transform.lossyScale.x;

							Vector3 new_pos = current_gameobject.transform.position + (Vector3.right * 0.1f);
							if (new_pos.x < width / 2 - current_world_size.x / 2) {
								current_gameobject.transform.position = new_pos;
							}
						}
					} else {
						animator.SetBool ("Left", false);
						animator.SetBool ("Right", false);
					}

					Vector3 new_cam_pos = current_gameobject.transform.position + new Vector3 (0, 2, -10);
					// Limit the camera within the background zone
					if ((new_cam_pos.y > 0) && (new_cam_pos.y < bg_world_size.y * num_bg - height)) {
						Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, new_cam_pos.y, Camera.main.transform.position.z);
						;
					}
				}
            }
            else
            {
                camera_switch_no = 0;
                mcontrol.move_player_P1 = true;
                Camera.main.transform.position = Single_OriginalPos;
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
                animator = current_gameobject.GetComponent<Animator>();
                if (this.CompareTag("Player1"))
                    mcontrol.move_player_P1 = false;
                else
                    mcontrol.move_player_P2 = false;
            }
            else
            {
                current_gameobject = null;
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
                if (current_gameobject.transform.position.y < total_bg_height - (PCam_Height * 2) && current_gameobject.transform.position.y > 0)
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

    public void BulletMove_PressUp_Left()
    {
        if ((this.CompareTag("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag("Player2") && !mcontrol.card_menu_P2))
        {
            Move_Left = false;

			if (animator != null && current_gameobject != null)
            {
                if (current_gameobject.transform.position.y < total_bg_height - (PCam_Height * 2) && current_gameobject.transform.position.y > 0)
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

    public void BulletMove_PressDown_Right()
    {
        if ((this.CompareTag("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag("Player2") && !mcontrol.card_menu_P2))
        {
            Move_Right = true;

			if (animator != null && current_gameobject != null)
            {
                if (current_gameobject.transform.position.y < total_bg_height - (PCam_Height * 2) && current_gameobject.transform.position.y > 0)
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

    public void BulletMove_PressUp_Right()
    {
        if ((this.CompareTag("Player1") && !mcontrol.card_menu_P1) || (this.CompareTag("Player2") && !mcontrol.card_menu_P2))
        {
            Move_Right = false;

			if (animator != null && current_gameobject != null)
            {
                if (current_gameobject.transform.position.y < total_bg_height - (PCam_Height * 2) && current_gameobject.transform.position.y > 0)
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
}