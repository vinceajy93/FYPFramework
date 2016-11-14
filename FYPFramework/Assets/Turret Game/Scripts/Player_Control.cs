using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Player_Control : Check if single or multiplayer mode, 
 * 					different dragging code depending on the mode
 * 
*/
public class Player_Control : MonoBehaviour
{

    // Check mouse and player's turret position
	Vector3 player_world_size;
	private Animator animator;

    private bool overSprite = false;
    private Vector3 offset;

    // Check interver between each mouse down
    private const float set_cooldown = 0.5f;
    //half a sec
    private float button_cooldown = 0f;
    private int button_count = 0;

    private Camera P1Cam;
    private Camera P2Cam;
    private int touch_point = -1;

    private string bullet_name;
    private string bullet_name_2; // Player_2
    private GameObject shoot_location;

    private string effect_name = "Bullet/Bullet_Destroy";

    private Mode_Control mcontrol;
    private PauseScript _pauseScript;
    private Animator Turret_anim;

    //bullet cooldown
    private float fireRate = 1.0f;
    private float nextFire = 0.0f;

    private GameObject Reload;
    private Image Reload_Alpha;

	private RectTransform P1_Screen;
	private RectTransform P2_Screen;

    //deltaTime
    private float delTime;

    // Use this for initialization
    void Start()
    {
		mcontrol = GameObject.Find("Scripts").GetComponent<Mode_Control>();
        _pauseScript = GameObject.Find("Scripts").GetComponent<PauseScript>();

        Turret_anim = GetComponent<Animator>();
		shoot_location = this.transform.Find("bullet_pos").gameObject;
        //m_Overlay_Control = GameObject.Find ("Scripts").GetComponent<Overlay_Control> ();

		// Get Player_1 Size in worldspace
		Vector2 sprite_size = this.GetComponent<SpriteRenderer>().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		player_world_size = local_sprite_size;
		player_world_size.x *= this.transform.lossyScale.x;
		player_world_size.y *= this.transform.lossyScale.y;

		if(this.transform.FindChild("Effect") != null)
			animator = this.transform.Find("Effect").GetComponent<Animator>();

        if (mcontrol.game_mode_Single)
        {
            //game_mode_Single = true;
			if (GameObject.Find("Reload") != null)
            {
                Reload = GameObject.Find("Reload");
                Reload.GetComponent<Slider>().minValue = 0;
                Reload.GetComponent<Slider>().maxValue = -1;
                Reload_Alpha = Reload.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            }

			if (PlayerPrefs.HasKey ("S_P1_B"))
				bullet_name = "Bullet/Bullet " + PlayerPrefs.GetInt("LM_P1_B").ToString();
			else
				bullet_name = "Bullet/Bullet " + "6";
			
			for (int i = 0; i < 10; i++) {
				if (LayerMask.LayerToName (this.gameObject.layer) == "Player 1") {
					GameObject new_clone = Instantiate (Resources.Load (bullet_name), GameObject.Find ("Bullet_Rest").transform.position, Quaternion.identity) as GameObject;
					new_clone.tag = "Bullet_Rest";
					new_clone.transform.SetParent (GameObject.Find ("Bullet_Rest").transform);
				}

				GameObject new_effect = Instantiate (Resources.Load (effect_name), GameObject.Find ("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
				new_effect.tag = "Bullet_Effect_Stop";
				new_effect.transform.SetParent (GameObject.Find ("Bullet_Effect").transform);
			}
        }
        else
        {
            P2Cam = GameObject.Find("Top Camera").GetComponent<Camera>();
            P1Cam = GameObject.Find("Bottom Camera").GetComponent<Camera>();

			P1_Screen = GameObject.FindGameObjectWithTag ("Screen_P1").GetComponent<RectTransform> ();
			P2_Screen = GameObject.FindGameObjectWithTag ("Screen_P2").GetComponent<RectTransform> ();

			if (LayerMask.LayerToName(this.gameObject.layer) == "Player 1" && GameObject.Find("Reload P1") != null)
            {
                Reload = GameObject.Find("Reload P1");

                Reload.GetComponent<Slider>().minValue = 0;
                Reload.GetComponent<Slider>().maxValue = -1;
                Reload_Alpha = Reload.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            }

			if (LayerMask.LayerToName(this.gameObject.layer) == "Player 2" && GameObject.Find("Reload P2") != null)
            {
				Reload = GameObject.Find("Reload P2");

                Reload.GetComponent<Slider>().minValue = 0;
                Reload.GetComponent<Slider>().maxValue = -1;
                Reload_Alpha = Reload.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            }


			if (PlayerPrefs.HasKey ("LM_P1_B"))
				bullet_name = "Bullet/Bullet " + PlayerPrefs.GetInt("LM_P1_B").ToString();
			else
				bullet_name = "Bullet/Bullet " + "1";

			if (PlayerPrefs.HasKey ("LM_P2_B"))
				bullet_name_2 = "Bullet/Bullet " + PlayerPrefs.GetInt("LM_P2_B").ToString();
			else
				bullet_name_2 = "Bullet/Bullet " + "2";
			
			for (int i = 0; i < 10; i++) {
				if (LayerMask.LayerToName (this.gameObject.layer) == "Player 1") {
					GameObject new_clone = Instantiate (Resources.Load (bullet_name), GameObject.Find ("Bullet_Rest").transform.position, Quaternion.identity) as GameObject;
					new_clone.tag = "Bullet_Rest";
					new_clone.transform.SetParent (GameObject.Find ("Bullet_Rest").transform);
				} else if (LayerMask.LayerToName (this.gameObject.layer) == "Player 2") {
					GameObject new_clone = Instantiate (Resources.Load (bullet_name_2), GameObject.Find ("Bullet_Rest").transform.position, Quaternion.identity) as GameObject;
					new_clone.tag = "Bullet_Rest_2";
					new_clone.transform.SetParent (GameObject.Find ("Bullet_Rest").transform);
				}


				GameObject new_effect = Instantiate (Resources.Load (effect_name), GameObject.Find ("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
				new_effect.tag = "Bullet_Effect_Stop";
				new_effect.transform.SetParent (GameObject.Find ("Bullet_Effect").transform);
			}
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get Camera's width and height (only in orthographic mode)
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        //Allow players to play only when overlay panel is gone
        if (!GameObject.Find("Scripts").GetComponent<Overlay_Control>().PanelisActive && _pauseScript.Paused == false)
        {

            delTime += Time.deltaTime;
            Control(width, height, player_world_size);

            if (Reload.GetComponent<Slider>().value < Reload.GetComponent<Slider>().maxValue)
            {
                Reload.GetComponent<Slider>().value = delTime;
            }
        }
    }

    void Control(float width, float height, Vector3 player_world_size)
    {
        if (!mcontrol.game_mode_Single)
        { //multiplayer
            int nbTouches = Input.touchCount;

			if (nbTouches > 0) {
				for (int i = 0; i < nbTouches; i++) {
					Touch touch = Input.GetTouch (i);

					TouchPhase phase = touch.phase;

					if (this.CompareTag("Player1")) {
						if (mcontrol.card_menu_P1 || !mcontrol.move_player_P1 || !RectTransformUtility.RectangleContainsScreenPoint (P1_Screen, touch.position))
							phase = TouchPhase.Ended;
					}

					if (this.CompareTag("Player2")) {
						if (mcontrol.card_menu_P2 || !mcontrol.move_player_P2 || !RectTransformUtility.RectangleContainsScreenPoint (P2_Screen, touch.position))
							phase = TouchPhase.Ended;
					}

					switch (phase) {
					case TouchPhase.Began:
						if (this.CompareTag ("Player1") && RectTransformUtility.RectangleContainsScreenPoint (P1_Screen, touch.position)) {
							RaycastHit2D hit = Physics2D.Raycast (P1Cam.ScreenToWorldPoint (touch.position), this.transform.position);
							if (hit.collider != null && hit.transform.gameObject == this.gameObject) {
								overSprite = true;

								Vector2 touchPosition = P1Cam.ScreenToWorldPoint (touch.position);
								offset = new Vector3 (this.transform.position.x, 0, 0) - new Vector3 (touchPosition.x, 0, 0);
							}
						}

						if (this.CompareTag ("Player2") && RectTransformUtility.RectangleContainsScreenPoint (P2_Screen, touch.position)) {
							RaycastHit2D hit = Physics2D.Raycast (P2Cam.ScreenToWorldPoint (touch.position), this.transform.position);
							if (hit.collider != null && hit.transform.gameObject == this.gameObject) {
								overSprite = true;

								Vector2 touchPosition = P2Cam.ScreenToWorldPoint (touch.position);
								offset = new Vector3 (this.transform.position.x, 0, 0) - new Vector3 (touchPosition.x, 0, 0);
							}
						}

                            // if player tap on the player 2 twice, shoot. Else, increase tap_count(button_count)++
						if (overSprite) {
							touch_point = i;

							button_count += 1;
							button_cooldown = set_cooldown;
							if (button_count > 1) {

								if (delTime > nextFire) {
									nextFire = delTime + fireRate;
									Reload.GetComponent<Slider> ().minValue = delTime;
									Reload.GetComponent<Slider> ().maxValue = nextFire;
									Reload_Alpha.canvasRenderer.SetAlpha (0.0f);
									Reload_Alpha.CrossFadeAlpha (1f, fireRate, false);
									Shoot ();
								}
								button_count = 0;
							}
						}
						break;

					case TouchPhase.Moved: //Dragging
						if (touch_point == i) {
							if (this.CompareTag ("Player1")) {
								Vector2 touchPosition = P1Cam.ScreenToWorldPoint (touch.position);
								Dragging (width, player_world_size, touchPosition);
							}

							if (this.CompareTag ("Player2")) {
								Vector2 touchPosition = P2Cam.ScreenToWorldPoint (touch.position);
								Dragging (width, player_world_size, touchPosition);
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

			if (Input.GetMouseButtonDown(0))
			{
				if (this.CompareTag ("Player1") && RectTransformUtility.RectangleContainsScreenPoint (P1_Screen, Input.mousePosition)) {
					RaycastHit2D hit = Physics2D.Raycast (P1Cam.ScreenToWorldPoint (Input.mousePosition), this.transform.position);
					if (hit.collider != null && hit.transform.gameObject == this.gameObject) {
						overSprite = true;

						Vector2 mousePosition = P1Cam.ScreenToWorldPoint (Input.mousePosition);
						offset = new Vector3 (this.transform.position.x, 0, 0) - new Vector3 (mousePosition.x, 0, 0);
					}
				}

				if (this.CompareTag ("Player2") && RectTransformUtility.RectangleContainsScreenPoint (P2_Screen, Input.mousePosition)) {
					RaycastHit2D hit = Physics2D.Raycast (P2Cam.ScreenToWorldPoint (Input.mousePosition), this.transform.position);
					if (hit.collider != null && hit.transform.gameObject == this.gameObject) {
						overSprite = true;

						Vector2 mousePosition = P2Cam.ScreenToWorldPoint (Input.mousePosition);
						offset = new Vector3 (this.transform.position.x, 0, 0) - new Vector3 (mousePosition.x, 0, 0);
					}
				}

				if (overSprite) {
					button_count += 1;
					button_cooldown = set_cooldown;

					if (button_count > 1) {
						if (delTime > nextFire) {
							nextFire = delTime + fireRate;
							Reload.GetComponent<Slider> ().minValue = delTime;
							Reload.GetComponent<Slider> ().maxValue = nextFire;
							Reload_Alpha.canvasRenderer.SetAlpha (0.0f);
							Reload_Alpha.CrossFadeAlpha (1f, fireRate, false);
							Shoot ();
						}
						button_count = 0;
					}
				}
			}

			if (Input.GetMouseButton (0) && overSprite) {
				if (this.CompareTag ("Player1")) {
					Vector2 mousePosition = P1Cam.ScreenToWorldPoint (Input.mousePosition);
					Dragging (width, player_world_size, mousePosition);
				}

				if (this.CompareTag ("Player2")) {
					Vector2 mousePosition = P2Cam.ScreenToWorldPoint (Input.mousePosition);
					Dragging (width, player_world_size, mousePosition);
				}
			} else {
				overSprite = false;

				if (animator != null) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", false);
				}
			}
        }
        else
        { //single player
            if (Input.GetMouseButtonDown(0) && !mcontrol.card_menu_P1)
            {
                if (this.CompareTag ("Player1")) {
					RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), this.transform.position);
					if (hit.collider != null && hit.transform.gameObject == this.gameObject) {
						overSprite = true;

						Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						offset = new Vector3 (this.transform.position.x, 0, 0) - new Vector3 (mousePosition.x, 0, 0);
					}
				}

				if (overSprite) {
					button_count += 1;
					button_cooldown = set_cooldown;

					if (button_count > 1) {
						if (delTime > nextFire) {
							nextFire = delTime + fireRate;
							Reload.GetComponent<Slider> ().minValue = delTime;
							Reload.GetComponent<Slider> ().maxValue = nextFire;
							Reload_Alpha.canvasRenderer.SetAlpha (0.0f);
							Reload_Alpha.CrossFadeAlpha (1f, fireRate, false);
							Shoot ();
						}
						button_count = 0;
					}
				}
            }

			if (Input.GetMouseButton (0) && overSprite) {
				Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Dragging (width, player_world_size, mousePosition);
			}
            else
            {
                overSprite = false;

				if (animator != null) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", false);
				}
            }
        }

        //reset bullet count when over time
        if (button_cooldown > 0)
        {
            button_cooldown -= 1 * Time.deltaTime;
        }
        else
        {
            button_count = 0;
        }
    }

    //Single PLayer Dragging
	void Dragging(float width, Vector3 player_world_size, Vector2 Dragging_Position)
    {
        // Dragging of Player turret
        Vector3 prev_pos = this.transform.position;
		Vector3 new_pos = new Vector3 (Dragging_Position.x, prev_pos.y, prev_pos.z);// + offset;

		if (prev_pos.x > Dragging_Position.x) {
			new_pos = this.transform.position + (Vector3.left * 0.2f); //+ offset;
		
			if (animator != null) {
				if (this.CompareTag ("Player1")) {
					animator.SetBool ("Left", true);
					animator.SetBool ("Right", false);
				}

				if (this.CompareTag ("Player2")) {
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
			new_pos = this.transform.position + (Vector3.right * 0.2f); // + offset;
		
			if (animator != null) {
				if (this.CompareTag ("Player1")) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", true);
				}

				if (this.CompareTag ("Player2")) {
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
        if (new_pos.x >= -width / 2 + player_world_size.x / 2 &&
            new_pos.x <= width / 2 - player_world_size.x / 2)
        {
            this.transform.position = new_pos;
        }
        else
        {
            // if turret exceed the left border, snap back to the left border within the camera space
            if (new_pos.x < -width / 2 + player_world_size.x / 2)
            {
                new_pos = new Vector3(-width / 2 + player_world_size.x / 2, prev_pos.y, prev_pos.z);
                this.transform.position = new_pos;

				if (animator != null) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", false);
				}
            }

            // if turret exceed the right border, snap back to the right border within the camera space
            else if (new_pos.x > width / 2 - player_world_size.x / 2)
            {
                new_pos = new Vector3(width / 2 - player_world_size.x / 2, prev_pos.y, prev_pos.z);
                this.transform.position = new_pos;

				if (animator != null) {
					animator.SetBool ("Left", false);
					animator.SetBool ("Right", false);
				}
            }
        }
    }

    void Shoot()
    {
        Turret_anim.SetInteger("turretState", 1);
        Vector3 shoot_position = shoot_location.transform.position;

		if (LayerMask.LayerToName(this.gameObject.layer) == "Player 1")
        {
            GameObject new_bullet = GameObject.FindGameObjectWithTag("Bullet_Rest");
            if (new_bullet != null)
            {
                new_bullet.SendMessage("ResetVarible", new_bullet);

                new_bullet.transform.SetParent(null);
                new_bullet.transform.position = shoot_position;
                new_bullet.transform.rotation = shoot_location.transform.rotation;
                new_bullet.tag = "Bullet_1";
            }
            else
            {
                // Create new bullet & effect
                new_bullet = Instantiate(GameObject.FindGameObjectWithTag("Bullet_1"), Vector3.zero, Quaternion.identity) as GameObject;
                new_bullet.SendMessage("ResetVarible", new_bullet);
                new_bullet.transform.SetParent(null);
                new_bullet.transform.position = shoot_position;
                new_bullet.transform.rotation = shoot_location.transform.rotation;
                new_bullet.tag = "Bullet_1";

                GameObject new_effect = Instantiate(Resources.Load(effect_name), GameObject.Find("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
                new_effect.tag = "Bullet_Effect_Stop";
                new_effect.transform.SetParent(GameObject.Find("Bullet_Effect").transform);
            }
        }
		else if (LayerMask.LayerToName(this.gameObject.layer) == "Player 2")
        {
            GameObject new_bullet = GameObject.FindGameObjectWithTag("Bullet_Rest_2");
            if (new_bullet != null)
            {
                new_bullet.SendMessage("ResetVarible", new_bullet);

                new_bullet.transform.SetParent(null);
                new_bullet.transform.position = shoot_position;
                new_bullet.transform.rotation = shoot_location.transform.rotation;
                new_bullet.tag = "Bullet_2";
            }
            else
            {
                new_bullet = Instantiate(GameObject.FindGameObjectWithTag("Bullet_2"), Vector3.zero, Quaternion.identity) as GameObject;
                new_bullet.SendMessage("ResetVarible", new_bullet);
                new_bullet.transform.SetParent(null);
                new_bullet.transform.position = shoot_position;
                new_bullet.transform.rotation = shoot_location.transform.rotation;
                new_bullet.tag = "Bullet_2";

                GameObject new_effect = Instantiate(Resources.Load(effect_name), GameObject.Find("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
                new_effect.tag = "Bullet_Effect_Stop";
                new_effect.transform.SetParent(GameObject.Find("Bullet_Effect").transform);
            }
        }
    }

	public void Message ()
	{
		Debug.Log ("This works");
	}

    public void PlayAnimationIdle()
    {

        //reset the animation to idle
        //to be called in the animation_fire

        Turret_anim.SetInteger("turretState", 0);
    }

    public void SetFireRate(float new_fireRate)
    {
        fireRate = new_fireRate;
    }
}
