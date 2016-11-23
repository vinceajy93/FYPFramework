using UnityEngine;
using System.Collections;

public class Arcade_Enemy : MonoBehaviour {
	public enum enemy_state {
		Follow = 0,
		Dash,
		None,
		Total,
		Burst,
		Mini_Boss,
		Boss
	};
	public enemy_state curret_state = enemy_state.None;

	private Arcade_Control _scriptController;
	private GameObject GO_Player;

	public int Health { get; set;}
	public SpriteRenderer _sprite;

	public bool hit = false;
	public float hit_timeleft = 0.5f;
	public float speed = 1f;

	private float camera_height;
	private float camera_width;
	private bool dash = true;
	private float dash_timeleft = 5f;
	private Vector3 size;

	private float delTime = 0f;
	private float FireRate;
	private float nextFire;

	private bool move_left = true;
	public bool Barrier_Activation = true;
	private bool Orb_Respawn = true;
	private float roam_time = 5f;
	public GameObject Boss_Barrier;
	public int _damage = 1;

	private float waveRate = 3f;
	private float nextWave = 7f;

	private bool Boss_Dash = false;
	private bool Boss_Bullet = false;
	private float Fire_TimeLeft = 10f;

	void Start () {
		_scriptController = GameObject.Find ("Script").GetComponent<Arcade_Control> ();
		GO_Player = GameObject.FindGameObjectWithTag ("Player1").transform.GetChild (0).gameObject;
		_sprite = this.GetComponent<SpriteRenderer> ();

		camera_height = 2f * Camera.main.orthographicSize;
		camera_width = camera_height * Camera.main.aspect;

		// Get Player_1 Size in worldspace
		Vector2 sprite_size = this.GetComponent<SpriteRenderer>().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		size = local_sprite_size;
		size.x *= this.transform.lossyScale.x;
		size.y *= this.transform.lossyScale.y;
	}

	public void Initialisation () {
		// Get Player_1 Size in worldspace
		Vector2 sprite_size = this.GetComponent<SpriteRenderer>().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		size = local_sprite_size;
		size.x *= this.transform.lossyScale.x;
		size.y *= this.transform.lossyScale.y;

		if (curret_state == enemy_state.Burst) {
			FireRate = 7f;
			nextFire = 5f;
		} else if (curret_state == enemy_state.Mini_Boss) {
			FireRate = 0f;
			nextFire = 1f;
		} else if (curret_state == enemy_state.Boss) {
			FireRate = 0f;
			nextFire = 0.7f;
		}
	}

	void Update () {
		if (!_scriptController.bPause && !_scriptController.bEnd) {
			if (hit) {
				hit_timeleft -= Time.deltaTime;

				if (hit_timeleft < 0) {
					hit_timeleft = 0.5f;
					hit = false;
					_sprite.color = new Color (1, 1, 1);
				}
			}

			switch (curret_state) {
			case enemy_state.Follow:
				Follow ();
				break;

			case enemy_state.Dash:
				Dash ();
				break;

			case enemy_state.None:
				Straight ();
				break;

			case enemy_state.Burst:
				Burst ();
				break;

			case enemy_state.Mini_Boss:
				Mini ();
				break;

			case enemy_state.Boss:
				Boss ();
				break;

			default:
				break;
			}
		}
	}

	void Follow () {
		Vector3 _direction = (GO_Player.transform.position - this.transform.position);
		float angle = Mathf.Atan2 (_direction.y, _direction.x) * Mathf.Rad2Deg - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, q, Time.deltaTime * 1f);

		this.transform.position += this.transform.up * (Time.deltaTime * speed);
	}

	void Dash () {
		if (dash) {
			Vector3 next_pos = this.transform.position;
			next_pos = this.transform.position + Vector3.down * (Time.deltaTime * speed * 5f);

			if (next_pos.y > camera_height * 0.25f) {
				this.transform.position = next_pos;
			} else {
				this.transform.position = new Vector3 (this.transform.position.x, camera_height * 0.25f, this.transform.position.z);
				dash = false;
			}
		} else {
			dash_timeleft -= (Time.deltaTime * speed);

			if (dash_timeleft < 0) {
				this.transform.position += Vector3.down * (Time.deltaTime * speed * 10f);
			}
		}

		if (this.transform.position.y < -(camera_height * 0.5f) - size.y) {
			Destroy (this.gameObject);
		}
	}

	void Straight () {
		this.transform.position += Vector3.down * (Time.deltaTime * speed * 3f);

		if (this.transform.position.y < -(camera_height * 0.5f) - size.y) {
			Destroy (this.gameObject);
		}
	}

	void Burst () {
		this.transform.position += Vector3.down * (Time.deltaTime * speed);

		Vector3 _direction = (GO_Player.transform.position - this.transform.position);
		float angle = Mathf.Atan2 (_direction.y, _direction.x) * Mathf.Rad2Deg - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, q, Time.deltaTime * 1f);

		delTime += Time.deltaTime;
		if (delTime > nextFire) {
			nextFire += FireRate;

			GameObject bullet_clone = Instantiate (_scriptController.GO_Bullet) as GameObject;
			bullet_clone.transform.position = this.transform.GetChild(0).position;
			bullet_clone.transform.rotation = this.transform.rotation;
			bullet_clone.transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
			bullet_clone.tag = "Bullet_E";
			bullet_clone.GetComponent<Bullet_Movement> ().isAllowedToTrigger = false;
			bullet_clone.GetComponent<Bullet_Movement> ().enabled = false;
			bullet_clone.GetComponent<Indicator> ().enabled = false;
			bullet_clone.GetComponent<Animator> ().enabled = false;
			bullet_clone.AddComponent<Arcade_Bullet> ();
			bullet_clone.GetComponent<Arcade_Bullet> ().burst = true;
			bullet_clone.GetComponent<Arcade_Bullet> ()._damage = _damage;
		}

		if (this.transform.position.y < -(camera_height * 0.5f) - size.y) {
			Destroy (this.gameObject);
		}
	}

	void Mini () {
		if (dash) {
			Vector3 next_pos = this.transform.position;
			next_pos = this.transform.position + Vector3.down * (Time.deltaTime * speed * 5f);

			if (next_pos.y > (camera_height * 0.5f) - (size.y * 0.5f)) {
				this.transform.position = next_pos;
			} else {
				this.transform.position = new Vector3 (this.transform.position.x, (camera_height * 0.5f) - (size.y * 0.5f), this.transform.position.z);
				dash = false;
			}
		} else {
			if (move_left) {
				Vector3 next_pos = this.transform.position;
				next_pos = this.transform.position + Vector3.left * (Time.deltaTime * speed * 5f);

				if (next_pos.x > (-camera_width * 0.5f) + (size.x * 0.5f)) {
					this.transform.position = next_pos;
				} else {
					this.transform.position = new Vector3 ((-camera_width * 0.5f) + (size.x * 0.5f), this.transform.position.y, this.transform.position.z);
					move_left = false;
				}
			} else {
				Vector3 next_pos = this.transform.position;
				next_pos = this.transform.position + Vector3.right * (Time.deltaTime * speed * 5f);

				if (next_pos.x < (camera_width * 0.5f) - (size.x * 0.5f)) {
					this.transform.position = next_pos;
				} else {
					this.transform.position = new Vector3 ((camera_width * 0.5f) - (size.x * 0.5f), this.transform.position.y, this.transform.position.z);
					move_left = true;
				}
			}

			delTime += Time.deltaTime;
			if (delTime > FireRate) {
				FireRate = delTime + nextFire;
				Shoot (true);
			}
		}
	}

	void Boss () {
		if (dash) {
			Vector3 next_pos = this.transform.position;
			next_pos = this.transform.position + Vector3.down * (Time.deltaTime * speed * 5f);

			if (next_pos.y > (camera_height * 0.5f) - (size.y * 0.5f)) {
				this.transform.position = next_pos;
			} else {
				this.transform.position = new Vector3 (this.transform.position.x, (camera_height * 0.5f) - (size.y * 0.5f), this.transform.position.z);
				dash = false;
			}
		} else {
			if (move_left) {
				Vector3 next_pos = this.transform.position;
				next_pos = this.transform.position + Vector3.left * (Time.deltaTime * speed * 5f);

				if (next_pos.x > (-camera_width * 0.5f) + (size.x * 0.25f)) {
					this.transform.position = next_pos;
				} else {
					this.transform.position = new Vector3 ((-camera_width * 0.5f) + (size.x * 0.25f), this.transform.position.y, this.transform.position.z);
					move_left = false;
				}
			} else {
				Vector3 next_pos = this.transform.position;
				next_pos = this.transform.position + Vector3.right * (Time.deltaTime * speed * 5f);

				if (next_pos.x < (camera_width * 0.5f) - (size.x * 0.25f)) {
					this.transform.position = next_pos;
				} else {
					this.transform.position = new Vector3 ((camera_width * 0.5f) - (size.x * 0.25f), this.transform.position.y, this.transform.position.z);
					move_left = true;
				}
			}

			delTime += Time.deltaTime;
			if (Boss_Bullet) {
				Fire_TimeLeft -= Time.deltaTime;
				if (Fire_TimeLeft < 0) {
					Boss_Bullet = false;
					waveRate = delTime + nextWave;
					Fire_TimeLeft = 10f;
				} else {
					if (delTime > FireRate) {
						FireRate = delTime + nextFire;
						Shoot (true);
					}
				}
			}

			if (!Boss_Dash && !Boss_Bullet) {
				if (delTime > waveRate) {
					waveRate = delTime + nextWave;

					int ran = Random.Range (1, 3);
					switch (ran) {
					case 1:
						_scriptController.Enemy_Follow ();
						break;

					case 2:
						//Boss_Dash = true;
						Boss_Bullet = true;
						FireRate = delTime;
						break;

					case 3:
						Boss_Bullet = true;
						FireRate = delTime;
						break;

					default:
						_scriptController.Enemy_Follow ();
						break;
					}
				}
			}
		}

		// Barrier
		if (Orb_Respawn) {
			Orb_Respawn = false;
			for (int i = 0; i < 2; i++) {
				GameObject orb = Instantiate (Resources.Load ("Turret/Barrier Orb")) as GameObject;

				Vector2 sprite_size = orb.GetComponent<SpriteRenderer> ().sprite.rect.size;
				Vector2 local_sprite_size = sprite_size / orb.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
				Vector3 orb_size = local_sprite_size;
				orb_size.x *= orb.transform.lossyScale.x;
				orb_size.y *= orb.transform.lossyScale.y;

				float temp_x = Random.Range ((-camera_width * 0.25f) + (orb_size.x * 0.5f), (camera_width * 0.5f) - (orb_size.x * 0.5f));
				float temp_y = Random.Range ((-camera_height * 0.5f) + (orb_size.y * 1.5f), 0);

				orb.transform.position = new Vector3 (temp_x, temp_y, 0f);
				foreach (GameObject ea_orb in GameObject.FindGameObjectsWithTag("Barrier Orb")) {
					if (ea_orb != orb) {
						while (orb.GetComponent<SpriteRenderer> ().bounds.Intersects (ea_orb.GetComponent<SpriteRenderer> ().bounds)) {
							temp_x = Random.Range ((-camera_width * 0.25f) + (orb_size.x * 0.5f), (camera_width * 0.5f) - (orb_size.x * 0.5f));
							temp_y = Random.Range ((-camera_height * 0.5f) + (orb_size.y * 1.5f), 0);
							orb.transform.position = new Vector3 (temp_x, temp_y, 0f);
						}
					}
				}
			}
		} else {
			if (GameObject.FindGameObjectWithTag ("Barrier Orb") == null) {
				Barrier_Activation = false;
				Boss_Barrier.SetActive(false);
			}

			if (!Barrier_Activation) {
				roam_time -= Time.deltaTime;
				if (roam_time < 0) {
					roam_time = 5f;

					Orb_Respawn = true;
					Barrier_Activation = true;
					Boss_Barrier.SetActive(true);
				}
			}
		}
	}

	void Shoot (bool Main_Boss) {
		GameObject bullet_clone = null;
		if (Main_Boss) {
			bullet_clone = Instantiate (Resources.Load ("Bullet/EBullet 3")) as GameObject;
		} else {
			bullet_clone = Instantiate (Resources.Load ("Bullet/EBullet 1")) as GameObject;
		}
		bullet_clone.transform.position = this.transform.GetChild(0).position;
		bullet_clone.transform.rotation = this.transform.rotation;
		bullet_clone.transform.localScale = new Vector3 (1f, 1f, 1f);
		bullet_clone.tag = "Bullet_E";
		bullet_clone.GetComponent<Bullet_Movement> ().isAllowedToTrigger = false;
		bullet_clone.GetComponent<Bullet_Movement> ().enabled = false;
		bullet_clone.GetComponent<Indicator> ().enabled = false;
		bullet_clone.GetComponent<Animator> ().enabled = false;
		bullet_clone.AddComponent<Arcade_Bullet> ();
		bullet_clone.GetComponent<Arcade_Bullet> ()._damage = _damage;
	}

	void OnTriggerEnter2D(Collider2D GO_Collide) {
		if (GO_Collide.CompareTag ("Player1")) {
			Arcade_Player _script = GO_Collide.GetComponent<Arcade_Player> ();
			if (!_script.hit) {
				_scriptController._Health -= _damage;

				if (_scriptController._Health <= 0) {
					GameObject Effect = Instantiate (Resources.Load ("Bullet/Bullet_Destroy")) as GameObject;
					Effect.transform.position = GO_Collide.gameObject.transform.position;
					Effect.transform.rotation = GO_Collide.gameObject.transform.rotation;
					Effect.GetComponent<Bullet_Destroy> ().isAllowedToTrigger = true;
					Effect.GetComponent<Animator> ().SetBool ("Destroy", true);
				} else {
					GO_Collide.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0);
					_script.hit = true;
					_script.hit_timeleft = 1f;
				}
			}

			if (curret_state == enemy_state.Follow || curret_state == enemy_state.Dash || curret_state == enemy_state.None) {
				GameObject Effect = Instantiate (Resources.Load ("Bullet/Bullet_Destroy")) as GameObject;
				Effect.transform.position = this.gameObject.transform.position;
				Effect.transform.rotation = this.gameObject.transform.rotation;
				Effect.GetComponent<Bullet_Destroy> ().isAllowedToTrigger = true;
				Effect.GetComponent<Animator> ().SetBool ("Destroy", true);

				Destroy (this.gameObject);
			}
		}
	}
}
