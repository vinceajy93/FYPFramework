using UnityEngine;
using System.Collections;

public class Arcade_Bullet : MonoBehaviour {
	private Arcade_Control _scriptController;

	private float camera_height;
	private float camera_width;
	private Vector3 bullet_size;

	public float bullet_speed = 0.25f;
	public int _damage = 1;

	public bool burst = false;
	private float burst_time = 1.5f;
	public bool burst_bullet = false;

	void Start () {
		_scriptController = GameObject.Find ("Script").GetComponent<Arcade_Control> ();

		camera_height = 2f * Camera.main.orthographicSize;
		camera_width = camera_height * Camera.main.aspect;

		Vector2 sprite_size = this.GetComponent<SpriteRenderer>().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		bullet_size = local_sprite_size;
		bullet_size.x *= this.transform.lossyScale.x;
		bullet_size.y *= this.transform.lossyScale.y;
	}

	void Update () {
		if (!_scriptController.bPause && !_scriptController.bEnd) {
			if (!burst) {
				if (!burst_bullet) {
					gameObject.transform.localPosition += this.transform.up * bullet_speed;
				} else {
					gameObject.transform.localPosition += this.transform.up * 0.1f;
				}
				if (gameObject.transform.position.y > ((camera_height * 0.5f) + (bullet_size.y)) || gameObject.transform.position.y < ((-camera_height * 0.5f) - (bullet_size.y)) || gameObject.transform.position.x < ((-camera_width * 0.5f) - (bullet_size.y)) || gameObject.transform.position.x > ((camera_width * 0.5f) + (bullet_size.y))) {
					Destroy (this.gameObject);
				}
			} else {
				gameObject.transform.localPosition += this.transform.up * 0.05f;

				burst_time -= Time.deltaTime;
				if (burst_time < 0) {
					Burst ();
					GameObject Effect = Instantiate (Resources.Load ("Bullet/Bullet_Destroy")) as GameObject;
					Effect.transform.position = this.transform.position;
					Effect.transform.rotation = this.transform.rotation;
					Effect.GetComponent<Bullet_Destroy> ().isAllowedToTrigger = true;
					Effect.GetComponent<Animator> ().SetBool ("Destroy", true);
					Destroy (this.gameObject);
				}

				if (gameObject.transform.position.y > ((camera_height * 0.5f) - (bullet_size.y * 0.5f)) || gameObject.transform.position.y < ((-camera_height * 0.5f) + (bullet_size.y * 0.5f)) || gameObject.transform.position.x < -camera_width * 0.5f || gameObject.transform.position.x > camera_width * 0.5f) {
					Burst ();
					GameObject Effect = Instantiate (Resources.Load ("Bullet/Bullet_Destroy")) as GameObject;
					Effect.transform.position = this.transform.position;
					Effect.transform.rotation = this.transform.rotation;
					Effect.GetComponent<Bullet_Destroy> ().isAllowedToTrigger = true;
					Effect.GetComponent<Animator> ().SetBool ("Destroy", true);
					Destroy (this.gameObject);
				}
			}
		}
	}

	void Burst () {
		int total_bullet = 5;
		float rand = Random.Range (0, 90);
		for (int i = 0; i < total_bullet; i++) {
			GameObject bullet_clone = Instantiate (Resources.Load ("Bullet/EBullet 4")) as GameObject;
			bullet_clone.transform.position = this.transform.position;
			bullet_clone.transform.localScale = new Vector3 (0.7f, 0.7f, 1f);
			bullet_clone.tag = "Bullet_E";
			bullet_clone.GetComponent<Bullet_Movement> ().isAllowedToTrigger = false;
			bullet_clone.GetComponent<Bullet_Movement> ().enabled = false;
			bullet_clone.GetComponent<Indicator> ().enabled = false;
			bullet_clone.AddComponent<Arcade_Bullet> ();
			bullet_clone.GetComponent<Arcade_Bullet> ().burst_bullet = true;
			bullet_clone.transform.Rotate (new Vector3 (0f, 0f, rand + i * (360 / total_bullet)));
		}
	}

	void OnTriggerEnter2D(Collider2D GO_Collide) {
		if (!this.CompareTag ("Bullet_E")) {
			if (GO_Collide.CompareTag ("Enemy")) {
				Arcade_Enemy _script = GO_Collide.GetComponent<Arcade_Enemy> ();
				_script.Health -= _damage;

				if (_script.Health <= 0) {
					GameObject Effect = Instantiate (Resources.Load ("Bullet/Bullet_Destroy")) as GameObject;
					Effect.transform.position = GO_Collide.gameObject.transform.position;
					Effect.transform.rotation = GO_Collide.gameObject.transform.rotation;
					Effect.GetComponent<Bullet_Destroy> ().isAllowedToTrigger = true;
					Effect.GetComponent<Animator> ().SetBool ("Destroy", true);

					if (GO_Collide.GetComponent<Arcade_Enemy> ().curret_state == Arcade_Enemy.enemy_state.Mini_Boss || GO_Collide.GetComponent<Arcade_Enemy> ().curret_state == Arcade_Enemy.enemy_state.Boss) {
						_scriptController.boss_spawn = false;
						_scriptController.addition_damage++;
						if (GO_Collide.GetComponent<Arcade_Enemy> ().curret_state == Arcade_Enemy.enemy_state.Boss) {
							_scriptController.Crystal++;
							_scriptController.Credits += GO_Collide.GetComponent<Arcade_Enemy> ()._damage * 50;
						} else {
							_scriptController.Credits += GO_Collide.GetComponent<Arcade_Enemy> ()._damage * 10;
						}
					} else {
						_scriptController.Credits += GO_Collide.GetComponent<Arcade_Enemy> ()._damage * 5;
					}
					Destroy (GO_Collide.gameObject);
				} else {
					GO_Collide.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0);
					GO_Collide.GetComponent<Arcade_Enemy> ().hit = true;
					GO_Collide.GetComponent<Arcade_Enemy> ().hit_timeleft = 0.5f;
				}

				Destroy (this.gameObject);
			}

			if (GO_Collide.CompareTag ("Barrier_E")) {
				Destroy (this.gameObject);
			}

			if (GO_Collide.CompareTag ("Barrier Orb")) {
				Arcade_Obstacle _script = GO_Collide.GetComponent<Arcade_Obstacle> ();
				_script.health -= _damage;
				if (_script.health <= 0) {
					Destroy (GO_Collide.gameObject);
				}
				Destroy (this.gameObject);
			}
		}

		if (GO_Collide.CompareTag ("Player1") && !this.CompareTag ("Bullet_1") && !this.burst) {
			Arcade_Player _script = GO_Collide.GetComponent<Arcade_Player> ();
			if (!_script.hit) {
				_scriptController._Health -= _damage;

				if (_scriptController._Health <= 0) {
					GameObject Effect = Instantiate (Resources.Load ("Bullet/Bullet_Destroy")) as GameObject;
					Effect.transform.position = this.gameObject.transform.position;
					Effect.transform.rotation = this.gameObject.transform.rotation;
					Effect.GetComponent<Bullet_Destroy> ().isAllowedToTrigger = true;
					Effect.GetComponent<Animator> ().SetBool ("Destroy", true);
				} else {
					GO_Collide.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0);
					_script.hit = true;
					_script.hit_timeleft = 1f;
				}
			}
				
			GameObject Effect2 = Instantiate (Resources.Load ("Bullet/Bullet_Destroy")) as GameObject;
			Effect2.transform.position = GO_Collide.gameObject.transform.position;
			Effect2.transform.rotation = GO_Collide.gameObject.transform.rotation;
			Effect2.GetComponent<Bullet_Destroy> ().isAllowedToTrigger = true;
			Effect2.GetComponent<Animator> ().SetBool ("Destroy", true);

			Destroy (this.gameObject);
		}
	}
}
