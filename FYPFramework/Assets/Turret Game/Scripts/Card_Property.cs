using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Card_Property : MonoBehaviour {
	private Mode_Control mcontrol;
	private HealthManager _HealthManager;

	private Color Useable = new Color(1f, 1f, 1f, 1f);
	private Color32 Useless = new Color32(106, 106, 106, 150);

	private float bullet_speed = 0.3f;
	private float new_fireRate = 0.5f;

	public bool b_Health;
	public bool b_Invinciblity;
	public bool b_Ghost_BurstShot;
	public bool b_Barrier;
	public bool b_FireRate;
	public bool b_BulletSpeed;

	// Use this for initialization
	void Start () {
		mcontrol = GameObject.Find ("Scripts").GetComponent<Mode_Control> ();
		_HealthManager = GameObject.Find("Scripts").GetComponent<HealthManager>();

		if (b_Health) {
			Image color = this.GetComponent<Image> ();
			color.color = Useless;
		}

		if (b_BulletSpeed) {
			Image color = this.GetComponent<Image> ();
			color.color = Useless;
		}

		if (b_Ghost_BurstShot) {
			Image color = this.GetComponent<Image> ();
			color.color = Useless;
		}

		if (b_Barrier) {
			if (b_Ghost_BurstShot) {
				Image color = this.GetComponent<Image> ();
				color.color = Useable;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (mcontrol.game_mode_Single) { // Single Player
			if (b_Health) {
				if (_HealthManager.P1Health.CurrentVal == _HealthManager.P1Health.MaxVal) {
					Image color = this.GetComponent<Image> ();
					color.color = Useless;
				} else {
					Image color = this.GetComponent<Image> ();
					color.color = Useable;
				}
			}

			if (b_BulletSpeed) {
				if (mcontrol.move_player_P1) { // If not moveing player = moving player
					Image color = this.GetComponent<Image> ();
					color.color = Useless;
				} else {
					Image color = this.GetComponent<Image> ();
					color.color = Useable;
				}
			}

			if (b_Ghost_BurstShot) {
				GameObject[] all_bullet = GameObject.FindGameObjectsWithTag("Bullet_1");

				foreach (GameObject bullet in all_bullet) {
					if (bullet.GetComponent<Bullet_Movement> ().bullet_follow) { // If bullet is being followed, 
						if (!bullet.GetComponent<Bullet_Movement> ().bullet_burstshot) {
							Image color = this.GetComponent<Image> ();
							color.color = Useable;
						} else {
							Image color = this.GetComponent<Image> ();
							color.color = Useless;
						}
						break;
					}
				}

				if (mcontrol.move_player_P1) {
					Image color = this.GetComponent<Image> ();
					color.color = Useless;
				}
			}

			if (b_Barrier) {
				if (!mcontrol.move_player_P1) {
					Image color = this.GetComponent<Image> ();
					color.color = Useless;
				} else {
					Image color = this.GetComponent<Image> ();
					color.color = Useable;
				}
			}
		}
		else {
			if (b_Health) {
				if (this.CompareTag ("Card_P1") && _HealthManager.P1Health.CurrentVal == _HealthManager.P1Health.MaxVal) {
					Image color = this.GetComponent<Image> ();
					color.color = Useless;
				} else if (this.CompareTag ("Card_P2") && _HealthManager.P2Health.CurrentVal == _HealthManager.P2Health.MaxVal) {
					Image color = this.GetComponent<Image> ();
					color.color = Useless;
				} else {
					Image color = this.GetComponent<Image> ();
					color.color = Useable;
				}
			}

			if (b_BulletSpeed) {
				if (this.CompareTag ("Card_P1") && mcontrol.move_player_P1) { // If not moveing player = moving player
					Image color = this.GetComponent<Image> ();
					color.color = Useless;
				} else if (this.CompareTag ("Card_P2") && mcontrol.move_player_P2) { // If not moveing player = moving player
					Image color = this.GetComponent<Image> ();
					color.color = Useless;
				} else {
					Image color = this.GetComponent<Image> ();
					color.color = Useable;
				}
			}

			if (b_Ghost_BurstShot) {
				if (this.CompareTag ("Card_P1")) {
					GameObject[] all_bullet = GameObject.FindGameObjectsWithTag ("Bullet_1");

					foreach (GameObject bullet in all_bullet) {
						if (bullet.GetComponent<Bullet_Movement> ().bullet_follow) { // If bullet is being followed, 
							if (!bullet.GetComponent<Bullet_Movement> ().bullet_burstshot) {
								Image color = this.GetComponent<Image> ();
								color.color = Useable;
							} else {
								Image color = this.GetComponent<Image> ();
								color.color = Useless;
							}
							break;
						}
					}

					if (mcontrol.move_player_P1) {
						Image color = this.GetComponent<Image> ();
						color.color = Useless;
					}
				} else {
					GameObject[] all_bullet = GameObject.FindGameObjectsWithTag("Bullet_2");

					foreach (GameObject bullet in all_bullet) {
						if (bullet.GetComponent<Bullet_Movement> ().bullet_follow) { // If bullet is being followed, 
							if (!bullet.GetComponent<Bullet_Movement> ().bullet_burstshot) {
								Image color = this.GetComponent<Image> ();
								color.color = Useable;
							} else {
								Image color = this.GetComponent<Image> ();
								color.color = Useless;
							}
							break;
						}
					}

					if (mcontrol.move_player_P2) {
						Image color = this.GetComponent<Image> ();
						color.color = Useless;
					}
				}
			}

			if (b_Barrier) {
				if (this.CompareTag ("Card_P1") && !mcontrol.move_player_P1) { // If not moveing player = moving player
					Image color = this.GetComponent<Image> ();
					color.color = Useless;
				} else if (this.CompareTag ("Card_P2") && !mcontrol.move_player_P2) { // If not moveing player = moving player
					Image color = this.GetComponent<Image> ();
					color.color = Useless;
				} else {
					Image color = this.GetComponent<Image> ();
					color.color = Useable;
				}
			}
		}
	}

	public void Button_Click (Button button) {
		if (b_Barrier) {
			Barrier (button);
		}  

		if (b_BulletSpeed) {
			BulletSpeed (button);
		}

		if (b_FireRate) {
			FireRate (button);
		} 

		if (b_Ghost_BurstShot) {
			Ghost_BurstShot (button);
		} 

		if (b_Health) {
			Health (button);
		} 

		if (b_Invinciblity) {
			Invinciblity (button);
		}
	}

	private void Invinciblity (Button button) {
		if (button.CompareTag ("Card_P1")) {

		} else {

		}
	}

	private void Ghost_BurstShot (Button button) {
		if (button.CompareTag ("Card_P1")) {
			if (!mcontrol.move_player_P1) { // If not moveing player = moving player
				GameObject[] all_bullet = GameObject.FindGameObjectsWithTag("Bullet_1");

				foreach (GameObject bullet in all_bullet) {
					if (bullet.GetComponent<Bullet_Movement> ().bullet_follow && !bullet.GetComponent<Bullet_Movement> ().bullet_burstshot) { // If bullet is being followed, and burst shot has not been done be4
						bullet.SendMessage ("SetBurstShot", true);
						button.gameObject.SetActive (false);

						GameObject all_Card = GameObject.FindGameObjectWithTag ("Card_P1");
						all_Card.SendMessage ("DisableMenu", true);
						break;
					}
				}
			}
		} else {
			if (!mcontrol.move_player_P2) {
				GameObject[] all_bullet = GameObject.FindGameObjectsWithTag("Bullet_2");

				foreach (GameObject bullet in all_bullet) {
					if (bullet.GetComponent<Bullet_Movement> ().bullet_follow && !bullet.GetComponent<Bullet_Movement> ().bullet_burstshot) {
						bullet.SendMessage ("SetBurstShot", true);
						button.gameObject.SetActive (false);

						GameObject all_Card = GameObject.FindGameObjectWithTag ("Card_P2");
						all_Card.SendMessage ("DisableMenu", false);
						break;
					}
				}
			}
		}
	}

	private void Health (Button button) {
		if (button.CompareTag ("Card_P1")) {
			if (_HealthManager.P1Health.CurrentVal != _HealthManager.P1Health.MaxVal) {
				_HealthManager.objHealth = HealthManager.ObjectsHealth.player1;
				_HealthManager.SendMessage ("AddHealth", 1); //damage done
				button.gameObject.SetActive (false);

				GameObject all_Card = GameObject.FindGameObjectWithTag ("Card_P1");
				all_Card.SendMessage ("DisableMenu", true);
			}
		} else {
			if (_HealthManager.P2Health.CurrentVal != _HealthManager.P2Health.MaxVal) {
				_HealthManager.objHealth = HealthManager.ObjectsHealth.player2;
				_HealthManager.SendMessage ("AddHealth", 1); //damage done
				button.gameObject.SetActive (false);

				GameObject all_Card = GameObject.FindGameObjectWithTag ("Card_P2");
				all_Card.SendMessage ("DisableMenu", false);
			}
		}
	}

	private void Barrier (Button button) {
		if (button.CompareTag ("Card_P1")) {
			if (mcontrol.move_player_P1) {
				GameObject barrier = GameObject.FindGameObjectWithTag ("Barrier_P1");
				barrier.SendMessage ("SetBarrier", true);
				button.gameObject.SetActive (false);

				GameObject all_Card = GameObject.FindGameObjectWithTag ("Card_P1");
				all_Card.SendMessage ("DisableMenu", true);
			}
		} else {
			if (mcontrol.move_player_P1) {
				GameObject barrier = GameObject.FindGameObjectWithTag ("Barrier_P2");
				barrier.SendMessage ("SetBarrier", true);
				button.gameObject.SetActive (false);

				GameObject all_Card = GameObject.FindGameObjectWithTag ("Card_P2");
				all_Card.SendMessage ("DisableMenu", false);
			}
		}
	}

	private void FireRate (Button button) {
		if (button.CompareTag ("Card_P1")) {
			GameObject Player = GameObject.FindGameObjectWithTag ("Player1");
			Player.SendMessage ("SetFireRate", new_fireRate);
			button.gameObject.SetActive (false);

			GameObject all_Card = GameObject.FindGameObjectWithTag ("Card_P1");
			all_Card.SendMessage ("DisableMenu", true);
		} else {
			GameObject Player = GameObject.FindGameObjectWithTag ("Player2");
			Player.SendMessage ("SetFireRate", new_fireRate);
			button.gameObject.SetActive (false);

			GameObject all_Card = GameObject.FindGameObjectWithTag ("Card_P2");
			all_Card.SendMessage ("DisableMenu", false);
		}
	}

	private void BulletSpeed (Button button) {
		if (button.CompareTag ("Card_P1")) {
			if (!mcontrol.move_player_P1) { // If not moveing player = moving player
				GameObject[] all_bullet = GameObject.FindGameObjectsWithTag("Bullet_1");

				foreach (GameObject bullet in all_bullet) {
					if (bullet.GetComponent<Bullet_Movement> ().bullet_follow) { // If bullet is being followed, 
						bullet.SendMessage ("SetBulletSpeed", bullet_speed);
						button.gameObject.SetActive (false);

						GameObject all_Card = GameObject.FindGameObjectWithTag ("Card_P1");
						all_Card.SendMessage ("DisableMenu", true);
						break;
					}
				}
			}
		} else {
			if (!mcontrol.move_player_P2) {
				GameObject[] all_bullet = GameObject.FindGameObjectsWithTag("Bullet_2");

				foreach (GameObject bullet in all_bullet) {
					if (bullet.GetComponent<Bullet_Movement> ().bullet_follow) {
						bullet.SendMessage ("SetBulletSpeed", bullet_speed);
						button.gameObject.SetActive (false);

						GameObject all_Card = GameObject.FindGameObjectWithTag ("Card_P2");
						all_Card.SendMessage ("DisableMenu", false);
						break;
					}
				}
			}
		}
	}
}
