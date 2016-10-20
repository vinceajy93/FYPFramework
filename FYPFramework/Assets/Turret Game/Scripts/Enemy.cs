using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	private Camera Main_Cam;
	private float Main_Cam_Height;
	private float Main_Cam_Width;

	private PauseScript _pauseScript;
	private Animator Turret_anim;
	private Animator Bullet_anim;

	private Vector3 enemy_world_size;
	private Vector3 bullet_world_size;
	private GameObject bg;
	private Vector3 bg_world_size;
	private int numbg;

	public enum EnemyState
	{
		Idle, // 0
		Turret_Move,
		Turret_Shoot,
		Bullet_Move,
		Dead,
		Total_State
	}
	public EnemyState currentEnemyState;

	private GameObject Enemy_Turret;
	private GameObject current_bullet;

	// Probability
	private int Pro_Shoot = 70;
	private int Pro_Move = 700;

	// Idle
	private bool setState = true;
	private float stateRate = 0.0f;
	private float minRate = 0.3f;
	private float maxRate = 1.0f;
	private float nextState = 0.0f;

	// Moving Turret
	private bool pos_set = false;
	private Vector3 next_pos;
	private float fixed_diff = 0f;

	// Shooting
	private string bullet_name = "Bullet/Bullet";
	private GameObject shoot_location;
	private string effect_name = "Bullet/Bullet_Destroy";
	private float fireRate  = 1.0f;
	private float nextFire = 0.0f;
	private float delTime;

	// Move Bullet
	private bool ranSet = false;
	private int Rand_Move = 80;
	private int Rand_Bullet = 10;
	private GameObject nearestGO;

	// Use this for initialization
	void Start () {
		// Main Camera Size
		Main_Cam = Camera.main;
		Main_Cam_Height = 2f * Main_Cam.orthographicSize;
		Main_Cam_Width = Main_Cam_Height * Main_Cam.aspect;

		//pass by reference from pauseScript
		_pauseScript = GameObject.Find("Scripts").GetComponent<PauseScript>();

		Turret_anim = this.GetComponent<Animator> ();

		currentEnemyState = EnemyState.Turret_Move;
		Enemy_Turret = GameObject.FindGameObjectWithTag ("Enemy");

		// Get Enemy Turret Size in worldspace
		Vector2 sprite_size = Enemy_Turret.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / Enemy_Turret.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		enemy_world_size = local_sprite_size;
		enemy_world_size.x *= Enemy_Turret.transform.lossyScale.x;
		enemy_world_size.y *= Enemy_Turret.transform.lossyScale.y;

		bg = GameObject.FindGameObjectWithTag ("Background");
		sprite_size = bg.GetComponent<SpriteRenderer> ().sprite.rect.size;
		local_sprite_size = sprite_size / bg.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		bg_world_size = local_sprite_size;
		bg_world_size.x *= bg.transform.lossyScale.x;
		bg_world_size.y *= bg.transform.lossyScale.y;

		numbg = GameObject.FindGameObjectsWithTag ("Background").Length;

		shoot_location = Enemy_Turret.transform.FindChild("bullet_pos").gameObject;
	}

	void Awake () {
		for (int i = 0; i < 10; i++) {
			GameObject new_clone = Instantiate (Resources.Load (bullet_name), GameObject.Find ("Bullet_Rest").transform.position, Quaternion.identity) as GameObject;
			new_clone.tag = "Bullet_Rest_E";
			new_clone.transform.SetParent (GameObject.Find ("Bullet_Rest").transform);

			GameObject new_effect = Instantiate (Resources.Load (effect_name), GameObject.Find ("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
			new_effect.tag = "Bullet_Effect_Stop";
			new_effect.transform.SetParent (GameObject.Find ("Bullet_Effect").transform);
		}

		// Get Enemy Bullet Size in worldspace
		GameObject enemy_bullet = GameObject.FindGameObjectWithTag("Bullet_Rest_E");
		Vector2 sprite_size = enemy_bullet.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / enemy_bullet.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		bullet_world_size = local_sprite_size;
		bullet_world_size.x *= enemy_bullet.transform.lossyScale.x;
		bullet_world_size.y *= enemy_bullet.transform.lossyScale.y;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (currentEnemyState);
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			//currentEnemyState = (EnemyState)Random.Range (0, (int)EnemyState.Total_State);
			currentEnemyState = EnemyState.Idle;
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentEnemyState = EnemyState.Turret_Move;
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			currentEnemyState = EnemyState.Turret_Shoot;
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			currentEnemyState = EnemyState.Bullet_Move;
		}

		if (_pauseScript.Paused == false) {
			delTime += Time.deltaTime;
			FSM ();
		}
	}

	void FSM () {
		switch (currentEnemyState) {
		case EnemyState.Idle:
			Idle ();
			break;

		case EnemyState.Turret_Move:
			// If position is not set, randomise a position
			if (!pos_set) {
				pos_set = true;
				next_pos = new Vector3 (Random.Range ((-Main_Cam_Width / 2) + (enemy_world_size.x / 2), (Main_Cam_Width / 2) - (enemy_world_size.x / 2)), 0f, 0f);
			} else {
				Move_Turret ();
			}
			break;

		case EnemyState.Turret_Shoot:
			GameObject[] enemy_bullet = GameObject.FindGameObjectsWithTag ("Bullet_E");
			int Rand_Shoot = Random.Range (0, 100);

			if ((delTime > nextFire && enemy_bullet.Length == 0) || delTime > nextFire && Rand_Shoot < Pro_Shoot) {
				nextFire = delTime + fireRate;
				Shoot ();
			}
			currentEnemyState = EnemyState.Idle;
			break;

		case EnemyState.Bullet_Move:
			Move_Bullet ();
			break;

		case EnemyState.Dead:
			currentEnemyState = EnemyState.Idle;
			break;
		}
	}

	void Idle () {
		if (!setState) {
			setState = true;
			stateRate = Random.Range (minRate, maxRate);
			nextState = stateRate + delTime;
		} else {
			if (delTime > nextState) {
				setState = false;
				currentEnemyState = (EnemyState)Random.Range (1, (int)EnemyState.Total_State);
			}
		}
	}

	void Move_Turret () {
		if (Enemy_Turret.transform.position.x > next_pos.x)
			Enemy_Turret.transform.position += Vector3.left * 0.05f;

		if (Enemy_Turret.transform.position.x < next_pos.x)
			Enemy_Turret.transform.position += Vector3.right * 0.05f;

		float diff = Enemy_Turret.transform.position.x - next_pos.x;
		if (diff == fixed_diff) {
			Enemy_Turret.transform.position = new Vector3 (next_pos.x, Enemy_Turret.transform.position.y, Enemy_Turret.transform.position.z);

			pos_set = false;
			currentEnemyState = EnemyState.Turret_Shoot;
		} else {
			fixed_diff = diff;
		}
	}

	void Shoot () {
		Turret_anim.SetInteger ("turretState", 1);
		Vector3 shoot_position = shoot_location.transform.position;
		GameObject new_bullet = GameObject.FindGameObjectWithTag ("Bullet_Rest_E");
		if (new_bullet != null) {
			new_bullet.transform.SetParent (null);
			new_bullet.transform.position = shoot_position;
			new_bullet.transform.rotation = shoot_location.transform.rotation;
			if (new_bullet != null) {
				new_bullet.tag = "Bullet_E";
			}
		}
	}

	void Move_Bullet () {
		if (current_bullet == null || current_bullet.tag == "Bullet_Rest_E") {
			GameObject[] enemy_bullet = GameObject.FindGameObjectsWithTag ("Bullet_E");

			if (enemy_bullet.Length > 0) {
				int Rand_Selective = Random.Range (0, 100);
				if (Rand_Selective < Rand_Bullet) {
					Move_Bullet_Random_Bullet (enemy_bullet);
				} else {
					Move_Bullet_Selective_Bullet (enemy_bullet);
				}
			} else {
				currentEnemyState = EnemyState.Idle;
				current_bullet = null;
				Bullet_anim = null;
				setState = true;
			}
		} else {
			if (!ranSet) {
				Rand_Move = Random.Range (0, 100);
				ranSet = true;
			}
			else {
				if (Rand_Move < Pro_Move || ranSet) {
					// Aim bullet @ obstacle || bullet
					Move_Bullet_Aim();
				} else {
					// Move Bullet to avoid obstacle or to hit bullet
					Move_Bullet_Ran ();
				}
			}
		}
	}

	void Move_Bullet_Random_Bullet (GameObject[] enemy_bullet) {
		int Rand_Bullet = 0;
		Rand_Bullet = Random.Range (0, enemy_bullet.Length - 1);

		if (enemy_bullet [Rand_Bullet] != null) {
			if (enemy_bullet [Rand_Bullet].transform.position.y < 0 || enemy_bullet [Rand_Bullet].transform.position.y > bg_world_size.y * (numbg - 1)) {
				currentEnemyState = EnemyState.Idle;
				current_bullet = null;
				Bullet_anim = null;
				setState = true;
			} else {
				current_bullet = enemy_bullet [Rand_Bullet];
				Bullet_anim = current_bullet.GetComponent<Animator> ();
			}
		} else {
			currentEnemyState = EnemyState.Idle;
			current_bullet = null;
			Bullet_anim = null;
			setState = true;
		}
	}

	void Move_Bullet_Selective_Bullet (GameObject[] enemy_bullet) { // Select which bullet is closer to obstacle / bullet
		GameObject[] obstacle = GameObject.FindGameObjectsWithTag ("Obstacle");
		GameObject[] player_bullet = GameObject.FindGameObjectsWithTag ("Bullet_1");

		float shortest_distance = 100f;
		float temp_diff = 0;

		foreach (GameObject AI_Bullet in enemy_bullet) { // Check all AI bullet with player's bullet and obstacle and see which is closes to danger
			if (obstacle.Length > 0) { //Check if there is obstacle first then loop though the whole list
				foreach (GameObject Obstacle_No in obstacle) {
					Vector2 sprite_size = Obstacle_No.GetComponent<SpriteRenderer> ().sprite.rect.size;
					Vector2 local_sprite_size = sprite_size / Obstacle_No.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
					Vector2 obstacle_world_size = local_sprite_size;
					obstacle_world_size.x *= Obstacle_No.transform.lossyScale.x;
					obstacle_world_size.y *= Obstacle_No.transform.lossyScale.y;

					temp_diff = AI_Bullet.transform.position.y - (Obstacle_No.transform.position.y - (obstacle_world_size.y / 2));
					if (temp_diff > 0 && temp_diff < shortest_distance) {
						current_bullet = AI_Bullet;
						Bullet_anim = current_bullet.GetComponent<Animator> ();
						shortest_distance = temp_diff;
						ranSet = true;
					}
				}
			}

			if (player_bullet.Length > 0) { //Check if there is bullet first then loop though the whole list
				foreach (GameObject Player_Bullet_No in player_bullet) {
					temp_diff = AI_Bullet.transform.position.y - Player_Bullet_No.transform.position.y;
					if (temp_diff > 0 && temp_diff < shortest_distance) {
						current_bullet = AI_Bullet;
						Bullet_anim = current_bullet.GetComponent<Animator> ();
						shortest_distance = temp_diff;
						ranSet = true;
					}
				}
			}
		}

		if (current_bullet == null) {
			Move_Bullet_Random_Bullet (enemy_bullet);
			Bullet_anim = null;
		}
	}
		
	void Move_Bullet_Aim () { // Aim at obstacle / bullet depending on which is closer, else aim at player's turret
		if (!pos_set) {
			GameObject[] obstacle = GameObject.FindGameObjectsWithTag ("Obstacle");
			GameObject[] player_bullet = GameObject.FindGameObjectsWithTag ("Bullet_1");
			float shortest_distance = 100f;
			float temp_diff = 0;
			if (obstacle.Length > 0) {
				foreach (GameObject obst_no in obstacle) {
					Vector2 sprite_size = obst_no.GetComponent<SpriteRenderer> ().sprite.rect.size;
					Vector2 local_sprite_size = sprite_size / obst_no.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
					Vector2 obstacle_world_size = local_sprite_size;
					obstacle_world_size.x *= obst_no.transform.lossyScale.x;
					obstacle_world_size.y *= obst_no.transform.lossyScale.y;

					temp_diff = current_bullet.transform.position.y - (obst_no.transform.position.y - (obstacle_world_size.y / 2));
					if (temp_diff > 0 && temp_diff < shortest_distance) {
						nearestGO = obst_no;
						shortest_distance = temp_diff;
					}
				}
			}

			if (player_bullet.Length > 0) {
				foreach (GameObject bullet_no in player_bullet) {
					temp_diff = current_bullet.transform.position.y - bullet_no.transform.position.y;
					if (temp_diff > 0 && temp_diff < shortest_distance) {
						nearestGO = bullet_no;
						shortest_distance = temp_diff;
					}
				}
			}

			pos_set = true;
			if (nearestGO != null && nearestGO.CompareTag ("Obstacle")) {
				Vector2 sprite_size = nearestGO.GetComponent<SpriteRenderer> ().sprite.rect.size;
				Vector2 local_sprite_size = sprite_size / nearestGO.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
				Vector2 obstacle_world_size = local_sprite_size;
				obstacle_world_size.x *= nearestGO.transform.lossyScale.x;
				obstacle_world_size.y *= nearestGO.transform.lossyScale.y;

				next_pos = new Vector3 (Random.Range ((-Main_Cam_Width / 2) + (bullet_world_size.x / 2), (Main_Cam_Width / 2) - (bullet_world_size.x / 2)), 0f, 0f);
				Vector3 distance = next_pos - nearestGO.transform.position;
				float distanceX = Mathf.Abs (distance.x);
				while (distanceX <= bullet_world_size.x / 2 + obstacle_world_size.x / 2) {
					next_pos = new Vector3 (Random.Range ((-Main_Cam_Width / 2) + (bullet_world_size.x / 2), (Main_Cam_Width / 2) - (bullet_world_size.x / 2)), 0f, 0f);
					distance = next_pos - nearestGO.transform.position;
					distanceX = Mathf.Abs (distance.x);
				}
				nearestGO = null;
			} else if (nearestGO != null && nearestGO.CompareTag ("Bullet_1")) {
				next_pos = new Vector3 (nearestGO.transform.position.x, current_bullet.transform.position.y, current_bullet.transform.position.z);
				nearestGO = null;
			} else {
				next_pos = new Vector3 (GameObject.FindGameObjectWithTag("Player1").transform.position.x, current_bullet.transform.position.y, current_bullet.transform.position.z);
				nearestGO = null;
			}
		} else {
			if (current_bullet.transform.position.y < 0 || current_bullet.transform.position.y > bg_world_size.y * (numbg - 1)) {
				if (Bullet_anim != null) {
					Bullet_anim.SetBool ("Left", false);
					Bullet_anim.SetBool ("Right", false);
				}

				pos_set = false;
				currentEnemyState = EnemyState.Idle;
				ranSet = false;
				current_bullet = null;
				Bullet_anim = null;
			} else {
				if (current_bullet.transform.position.x > next_pos.x) {
					if (Bullet_anim != null) {
						Bullet_anim.SetBool ("Left", true);
						Bullet_anim.SetBool ("Right", false);
					}
					current_bullet.transform.position += Vector3.left * 0.05f;
				}

				if (current_bullet.transform.position.x < next_pos.x) {
					if (Bullet_anim != null) {
						Bullet_anim.SetBool ("Left", false);
						Bullet_anim.SetBool ("Right", true);
					}
					current_bullet.transform.position += Vector3.right * 0.05f;
				}

				float diff = current_bullet.transform.position.x - next_pos.x;
				if (diff == fixed_diff) {
					current_bullet.transform.position = new Vector3 (next_pos.x, current_bullet.transform.position.y, current_bullet.transform.position.z);

					if (Bullet_anim != null) {
						Bullet_anim.SetBool ("Left", false);
						Bullet_anim.SetBool ("Right", false);
					}

					pos_set = false;
					currentEnemyState = EnemyState.Idle;
					current_bullet = null;
					Bullet_anim = null;
					ranSet = false;
				} else {
					fixed_diff = diff;
				}
			}
		}
	}

	void Move_Bullet_Ran () {
		if (!pos_set) {
			pos_set = true;
			next_pos = new Vector3 (Random.Range ((-Main_Cam_Width / 2) + (bullet_world_size.x / 2), (Main_Cam_Width / 2) - (bullet_world_size.x / 2)), 0f, 0f);
		} else {
			if (current_bullet.transform.position.y < 0 || current_bullet.transform.position.y > bg_world_size.y * (numbg - 1)) {
				if (Bullet_anim != null) {
					Bullet_anim.SetBool ("Left", false);
					Bullet_anim.SetBool ("Right", false);
				}

				pos_set = false;
				currentEnemyState = EnemyState.Idle;
				ranSet = false;
				current_bullet = null;
				Bullet_anim = null;
			} else {
				if (current_bullet.transform.position.x > next_pos.x) {
					if (Bullet_anim != null) {
						Bullet_anim.SetBool ("Left", true);
						Bullet_anim.SetBool ("Right", false);
					}
					current_bullet.transform.position += Vector3.left * 0.05f;
				}

				if (current_bullet.transform.position.x < next_pos.x) {
					if (Bullet_anim != null) {
						Bullet_anim.SetBool ("Left", false);
						Bullet_anim.SetBool ("Right", true);
					}
					current_bullet.transform.position += Vector3.right * 0.05f;
				}

				float diff = current_bullet.transform.position.x - next_pos.x;
				if (diff == fixed_diff) {
					current_bullet.transform.position = new Vector3 (next_pos.x, current_bullet.transform.position.y, current_bullet.transform.position.z);

					if (Bullet_anim != null) {
						Bullet_anim.SetBool ("Left", false);
						Bullet_anim.SetBool ("Right", false);
					}

					pos_set = false;
					currentEnemyState = EnemyState.Idle;
					current_bullet = null;
					Bullet_anim = null;
					ranSet = false;
				} else {
					fixed_diff = diff;
				}
			}
		}
	}

	public void PlayAnimationIdle ()
	{
		//reset the animation to idle
		//to be called in the animation_fire
		Turret_anim.SetInteger ("turretState", 0);
	}
}