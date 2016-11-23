using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Arcade_Control : MonoBehaviour
{
	public bool bPause = false;
	private GameObject GO_PauseMenu;

	public bool bQuit = false;
	private GameObject GO_QuitMenu;

	public bool bEnd = false;
	private GameObject GO_EndMenu;

	private GameObject GO_Player;

	public int _Speed;
	public GameObject GO_Bullet;

	public GameObject GO_Base;

	public int _Health;
	private GameObject GO_Health;
	public Stat PlayerHealth;
	public Stat PlayerReload;

	public int _Damage;
	public float bullet_speed = 0.25f;
	public bool Card_BulletSpeed = false;
	private float Card_TimeLeft = 10f;

	private float camera_height;
	private float camera_width;

	public float delTime;
	public bool boss_spawn = false;

	private float waveRate = 5f;
	private float nextWave = 10f;
	private int current_wave = 0;

	private float nextBurst = 5f;

	private int total_enemy;
	public int add_health;
	private int addition_health = 0; // Add only after completing each boss
	public int addition_damage = 0;

	private GameObject Card_Menu;
	private GameObject Card_Left;
	private GameObject Card_Middle;
	private GameObject Card_Right;

	public bool Card_Barrier = false;
	private float death_time = 1.5f;

	private int Wave_Boss = 10;
	private int Wave_Mini = 5;

	public int Credits = 0;
	public int Crystal = 0;

	void Start ()
	{
		camera_height = 2f * Camera.main.orthographicSize;
		camera_width = camera_height * Camera.main.aspect;

		GO_QuitMenu = GameObject.FindGameObjectWithTag ("Menu_Quit");
		GO_QuitMenu.SetActive (false);

		GO_PauseMenu = GameObject.FindGameObjectWithTag ("Menu_Pause");
		GO_PauseMenu.SetActive (false);

		GO_EndMenu = GameObject.FindGameObjectWithTag ("Menu_End");
		GO_EndMenu.SetActive (false);

		_Speed = PlayerPrefs.GetInt ("S_Speed", 10);
		_Health = PlayerPrefs.GetInt ("S_Health", 5);
		_Damage = PlayerPrefs.GetInt ("S_Damage", 1);

		PlayerHealth.MaxVal = _Health;
		PlayerHealth.CurrentVal = _Health;

		GO_Player = Instantiate (Resources.Load ("Turret/" + PlayerPrefs.GetString ("S_T", "Turret 1"))) as GameObject;
		GO_Player.transform.position = GameObject.FindGameObjectWithTag ("Player").transform.position;
		GO_Player.transform.localScale = new Vector3 (1, 1, 1);
		GO_Player.transform.SetParent (GameObject.FindGameObjectWithTag ("Player").transform);
		GO_Player.tag = "Player1";
		GO_Player.GetComponent<Player_Control> ().enabled = false;
		GO_Player.AddComponent <Arcade_Player> ();

		GO_Base = Instantiate (Resources.Load ("Base/" + PlayerPrefs.GetString ("S_B", "Hovercraft 1"))) as GameObject;
		GO_Base.transform.position = GO_Player.transform.position + new Vector3 (0f, 0.25f, 0f);
		GO_Base.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
		GO_Base.transform.SetParent (GO_Player.transform);

		GO_Bullet = Resources.Load ("Bullet/" + PlayerPrefs.GetString ("S_B", "Bullet 1")) as GameObject;

		nextBurst = Random.Range (15f, 25f);

		Card_Menu = GameObject.FindGameObjectWithTag ("Card_P1");

		if (PlayerPrefs.HasKey ("Card_Left") && PlayerPrefs.GetString ("Card_Left") != "") {
			Card_Left = Instantiate (Resources.Load ("Card/Arcade/Icon_" + PlayerPrefs.GetString ("Card_Left"))) as GameObject;
			Card_Left.transform.SetParent (Card_Menu.transform.GetChild (0).transform);
			Card_Left.transform.position = Card_Menu.transform.GetChild (0).position;
			Card_Left.transform.localScale = Card_Menu.transform.GetChild (0).localScale;
			Card_Left.tag = "Card_Left";
		}

		if (PlayerPrefs.HasKey ("Card_Middle") && PlayerPrefs.GetString ("Card_Middle") != "") {
			Card_Middle = Instantiate (Resources.Load ("Card/Arcade/Icon_" + PlayerPrefs.GetString ("Card_Middle"))) as GameObject;
			Card_Middle.transform.SetParent (Card_Menu.transform.GetChild (1).transform);
			Card_Middle.transform.position = Card_Menu.transform.GetChild (1).position;
			Card_Middle.transform.localScale = Card_Menu.transform.GetChild (1).localScale;
			Card_Left.tag = "Card_Middle";
		}

		if (PlayerPrefs.HasKey ("Card_Right") && PlayerPrefs.GetString ("Card_Right") != "") {
			Card_Right = Instantiate (Resources.Load ("Card/Arcade/Icon_" + PlayerPrefs.GetString ("Card_Right"))) as GameObject;
			Card_Right.transform.SetParent (Card_Menu.transform.GetChild (2).transform);
			Card_Right.transform.position = Card_Menu.transform.GetChild (2).position;
			Card_Right.transform.localScale = Card_Menu.transform.GetChild (2).localScale;
			Card_Left.tag = "Card_Right";
		}
	}

	void Update ()
	{
		if (!bPause && !bEnd) {
			PlayerHealth.CurrentVal = _Health;

			if (_Health <= 0) {
				bEnd = true;
			}

			if (Card_BulletSpeed) {
				Card_TimeLeft -= Time.deltaTime;
				bullet_speed = 0.5f;
				if (Card_TimeLeft < 0f) {
					Card_BulletSpeed = false;
					Card_TimeLeft = 10f;

					bullet_speed = 0.25f;
				}
			}

			// Prevent Spawning of other mobs when boss is spawn
			if (!boss_spawn) {
				delTime += Time.deltaTime;
				if (delTime > waveRate) {
					current_wave += 1;
					waveRate = delTime + nextWave;

					add_health = (int)(current_wave * 0.33f) + addition_health;
					total_enemy = 2 + (int)(delTime * 0.05f); 

					if (current_wave == Wave_Boss) {
						Wave_Boss += 10;
						boss_spawn = true;
						Enemy_Boss ();
						addition_health += 5;
					} else if (current_wave == Wave_Mini) {
						Wave_Mini += 10;
						boss_spawn = true;
						Enemy_MiniBoss ();
						addition_health += 3;
					} else {
						Enemy_Spawn ();
					}
				}

				nextBurst -= Time.deltaTime;
				if (nextBurst < 0 && !boss_spawn) {
					nextBurst = Random.Range (10f, 30f);
					Enemy_Burst ();
				}
			}
		} 

		if (bEnd) {
			death_time -= Time.deltaTime;
			if (death_time < 0) {
				GO_EndMenu.SetActive (true);

				Text crd = GameObject.FindGameObjectWithTag ("Credit").transform.GetChild(0).GetComponent<Text> ();
				crd.text = Credits.ToString();

				Text cry = GameObject.FindGameObjectWithTag ("Crystal").transform.GetChild(0).GetComponentInChildren<Text> ();
				cry.text = Crystal.ToString();
			}
		}
	}

	public void Button_Pause () {
		if (bPause) {
			bPause = false;
			GO_PauseMenu.SetActive (false);
		} else {
			bPause = true;
			GO_PauseMenu.SetActive (true);
		}
	}

	public void Button_Quit () {
		if (bQuit) {
			bQuit = false;
			GO_QuitMenu.SetActive (false);
		} else {
			bQuit = true;
			GO_QuitMenu.SetActive (true);
		}
	}

	public void Button_End () {
		if (PlayerPrefs.HasKey ("Credit")) {
			PlayerPrefs.SetInt ("Credit", PlayerPrefs.GetInt ("Credit") + Credits);
		} else {
			PlayerPrefs.SetInt ("Credit", Credits + 500);
		}

		if (PlayerPrefs.HasKey ("Crystal")) {
			PlayerPrefs.SetInt ("Crystal", PlayerPrefs.GetInt ("Crystal") + Crystal);
		} else {
			PlayerPrefs.SetInt ("Crystal", Crystal + 5);
		}
	}

	public void Button_Resume () {
		PlayerHealth.CurrentVal = PlayerHealth.MaxVal;
		GO_EndMenu.SetActive (false);
	}

	void Enemy_Spawn () {
		int total_state = (int) Arcade_Enemy.enemy_state.Total;
		int ran = Random.Range (0, total_state - 1);
		switch (ran) {
		case (int) Arcade_Enemy.enemy_state.Follow:
			Enemy_Dash ();
			break;

		case (int) Arcade_Enemy.enemy_state.Dash:
			Enemy_Dash ();
			break;

		case (int) Arcade_Enemy.enemy_state.None:
			Enemy_Dash ();
			break;

		default:
			Enemy_Dash ();
			break;
		}
	}

	public void Enemy_Follow () {
		for (int i = 0; i < total_enemy; i++) {
			GameObject GO_Enemy = Instantiate (Resources.Load ("Turret/Enemy 4")) as GameObject;
			GO_Enemy.transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
			GO_Enemy.tag = "Enemy";
			GO_Enemy.GetComponent <Enemy> ().enabled = false;
			GO_Enemy.AddComponent <Arcade_Enemy> ();

			Arcade_Enemy _script = GO_Enemy.GetComponent<Arcade_Enemy> ();
			_script.curret_state = Arcade_Enemy.enemy_state.Follow;
			_script.Health = 1 + add_health;
			_script.speed += delTime * 0.01f;
			_script._damage = 1 + addition_damage;

			// Get Player_1 Size in worldspace
			Vector2 sprite_size = GO_Enemy.GetComponent<SpriteRenderer>().sprite.rect.size;
			Vector2 local_sprite_size = sprite_size / GO_Enemy.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
			Vector3 enemy_size = local_sprite_size;
			enemy_size.x *= GO_Enemy.transform.lossyScale.x;
			enemy_size.y *= GO_Enemy.transform.lossyScale.y;

			float temp_x;
			if (i % 2 == 0) {
				temp_x = (camera_width * 0.5f) + (enemy_size.x * 0.5f);
			} else {
				temp_x = (-camera_width * 0.5f) - (enemy_size.x * 0.5f);
			}
			float temp_y = Random.Range(-camera_height * 0.5f, camera_height * 0.5f);
			GO_Enemy.transform.position = new Vector3 (temp_x, temp_y, GO_Enemy.transform.position.z);

			Vector3 _direction = (GO_Player.transform.position - GO_Enemy.transform.position);
			float angle = Mathf.Atan2 (_direction.y, _direction.x) * Mathf.Rad2Deg - 90;
			GO_Enemy.transform.Rotate(new Vector3(0, 0, angle));
		}
	}

	void Enemy_Dash () {
		for (int i = 1; i <= 4; i++) {
			GameObject GO_Enemy = Instantiate (Resources.Load ("Turret/Enemy 2")) as GameObject;
			GO_Enemy.transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
			GO_Enemy.tag = "Enemy";
			GO_Enemy.GetComponent <Enemy> ().enabled = false;
			GO_Enemy.AddComponent <Arcade_Enemy> ();

			Arcade_Enemy _script = GO_Enemy.GetComponent<Arcade_Enemy> ();
			_script.curret_state = Arcade_Enemy.enemy_state.Dash;
			_script.Health = 2 + add_health;
			_script.speed += delTime * 0.01f;
			_script.Initialisation ();
			_script._damage = 2 + addition_damage;

			// Get Player_1 Size in worldspace
			Vector2 sprite_size = GO_Enemy.GetComponent<SpriteRenderer>().sprite.rect.size;
			Vector2 local_sprite_size = sprite_size / GO_Enemy.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
			Vector3 enemy_size = local_sprite_size;
			enemy_size.x *= GO_Enemy.transform.lossyScale.x;
			enemy_size.y *= GO_Enemy.transform.lossyScale.y;

			float ea_width = (camera_width - (enemy_size.x * 4f)) * 0.2f;

			float temp_x = -(camera_width * 0.5f) + (ea_width * i) + (enemy_size.x * (i - 1)) + (enemy_size.x * 0.5f);
			float temp_y = (camera_height * 0.5f) + (enemy_size.y * 0.5f);

			GO_Enemy.transform.position = new Vector3 (temp_x, temp_y, GO_Enemy.transform.position.z);
			GO_Enemy.transform.Rotate (new Vector3 (0f, 0f, 180f));
		}
	}

	void Enemy_Line () {
		for (int i = 1; i <= 4; i++) {
			for (int k = 1; k <= 3; k++) {
				GameObject GO_Enemy = Instantiate (Resources.Load ("Turret/Enemy 5")) as GameObject;
				GO_Enemy.transform.localScale = new Vector3 (1f, 1f, 1f);
				GO_Enemy.tag = "Enemy";
				GO_Enemy.GetComponent <Enemy> ().enabled = false;
				GO_Enemy.AddComponent <Arcade_Enemy> ();

				Arcade_Enemy _script = GO_Enemy.GetComponent<Arcade_Enemy> ();
				_script.curret_state = Arcade_Enemy.enemy_state.None;
				_script.Health = 1 + (int) (add_health * 0.5);
				_script.speed += delTime * 0.01f;
				_script._damage = 1 + addition_damage;

				// Get Player_1 Size in worldspace
				Vector2 sprite_size = GO_Enemy.GetComponent<SpriteRenderer> ().sprite.rect.size;
				Vector2 local_sprite_size = sprite_size / GO_Enemy.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
				Vector3 enemy_size = local_sprite_size;
				enemy_size.x *= GO_Enemy.transform.lossyScale.x;
				enemy_size.y *= GO_Enemy.transform.lossyScale.y;

				float ea_width = (camera_width - (enemy_size.x * 4f)) * 0.2f;

				float temp_x = -(camera_width * 0.5f) + (ea_width * i) + (enemy_size.x * (i - 1)) + (enemy_size.x * 0.5f);
				float temp_y = (camera_height * 0.5f) + (enemy_size.y * ((k - 1) + i));

				GO_Enemy.transform.position = new Vector3 (temp_x, temp_y, GO_Enemy.transform.position.z);
				GO_Enemy.transform.Rotate (new Vector3 (0f, 0f, 180f));
			}
		}
	}

	void Enemy_Burst () {
		GameObject GO_Enemy = Instantiate (Resources.Load ("Turret/Enemy 6")) as GameObject;
		GO_Enemy.transform.localScale = new Vector3 (1f, 1f, 1f);
		GO_Enemy.tag = "Enemy";
		GO_Enemy.GetComponent <Enemy> ().enabled = false;
		GO_Enemy.AddComponent <Arcade_Enemy> ();

		Arcade_Enemy _script = GO_Enemy.GetComponent<Arcade_Enemy> ();
		_script.curret_state = Arcade_Enemy.enemy_state.Burst;
		_script.Health = 5 + add_health;
		_script.speed += delTime * 0.01f;
		_script.Initialisation ();
		_script._damage = 1 + addition_damage;

		// Get Player_1 Size in worldspace
		Vector2 sprite_size = GO_Enemy.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / GO_Enemy.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 enemy_size = local_sprite_size;
		enemy_size.x *= GO_Enemy.transform.lossyScale.x;
		enemy_size.y *= GO_Enemy.transform.lossyScale.y;

		float temp_x = Random.Range (-camera_width * 0.5f, camera_width * 0.5f);
		float temp_y = (camera_height * 0.5f) + (enemy_size.y * 0.5f);
		GO_Enemy.transform.position = new Vector3 (temp_x, temp_y, GO_Enemy.transform.position.z);
		GO_Enemy.transform.Rotate (new Vector3 (0f, 0f, 180f));
	}

	void Enemy_MiniBoss () {
		GameObject GO_Enemy = Instantiate (Resources.Load ("Turret/Enemy 1")) as GameObject;
		GO_Enemy.transform.localScale = new Vector3 (2.5f, 2.5f, 1f);
		GO_Enemy.tag = "Enemy";
		GO_Enemy.GetComponent <Enemy> ().enabled = false;
		GO_Enemy.AddComponent <Arcade_Enemy> ();

		Arcade_Enemy _script = GO_Enemy.GetComponent<Arcade_Enemy> ();
		_script.curret_state = Arcade_Enemy.enemy_state.Mini_Boss;
		_script.Health = 10 + add_health;
		_script.speed += delTime * 0.01f;
		_script.Initialisation ();
		_script._damage = 2 + addition_damage;

		// Get Player_1 Size in worldspace
		Vector2 sprite_size = GO_Enemy.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / GO_Enemy.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 enemy_size = local_sprite_size;
		enemy_size.x *= GO_Enemy.transform.lossyScale.x;
		enemy_size.y *= GO_Enemy.transform.lossyScale.y;

		float temp_y = (camera_height * 0.5f) + (enemy_size.y * 0.5f);
		GO_Enemy.transform.position = new Vector3 (0, temp_y, GO_Enemy.transform.position.z);
		GO_Enemy.transform.Rotate (new Vector3 (0f, 0f, 180f));
	}

	void Enemy_Boss () {
		GameObject GO_Enemy = Instantiate (Resources.Load ("Turret/Enemy 3")) as GameObject;
		GO_Enemy.transform.localScale = new Vector3 (3f, 3f, 1f);
		GO_Enemy.tag = "Enemy";
		GO_Enemy.GetComponent <Enemy> ().enabled = false;
		GO_Enemy.AddComponent <Arcade_Enemy> ();

		Arcade_Enemy _script = GO_Enemy.GetComponent<Arcade_Enemy> ();
		_script.curret_state = Arcade_Enemy.enemy_state.Boss;
		_script.Health = 15 + add_health;
		_script.speed += delTime * 0.01f;
		_script.Initialisation ();
		_script._damage = 3 + addition_damage;

		// Get Player_1 Size in worldspace
		Vector2 sprite_size = GO_Enemy.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / GO_Enemy.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 enemy_size = local_sprite_size;
		enemy_size.x *= GO_Enemy.transform.lossyScale.x;
		enemy_size.y *= GO_Enemy.transform.lossyScale.y;

		float temp_y = (camera_height * 0.5f) + (enemy_size.y * 0.5f);
		GO_Enemy.transform.position = new Vector3 (0, temp_y, GO_Enemy.transform.position.z);
		GO_Enemy.transform.Rotate (new Vector3 (0f, 0f, 180f));

		GameObject GO_Barrier = Instantiate (Resources.Load ("Turret/E_Barrier")) as GameObject;
		GO_Barrier.transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
		GO_Barrier.transform.position = GO_Enemy.transform.position - new Vector3 (0f, 1f, 0f);
		GO_Barrier.transform.Rotate (new Vector3 (0f, 0f, 180f));
		GO_Barrier.transform.SetParent (GO_Enemy.transform);

		GO_Enemy.GetComponent<Arcade_Enemy> ().Boss_Barrier = GO_Barrier;
	}
}

