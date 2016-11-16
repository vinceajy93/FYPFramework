using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Arcade_Control : MonoBehaviour
{
	public bool bPause = false;
	private GameObject GO_PauseMenu;

	public bool bQuit = false;
	private GameObject GO_QuitMenu;

	private GameObject GO_Player;

	public int _Speed;
	public GameObject GO_Bullet;

	public int _Health;
	private GameObject GO_Health;

	public int _Damage;

	void Start ()
	{
		GO_QuitMenu = GameObject.FindGameObjectWithTag ("Menu_Quit");
		GO_QuitMenu.SetActive (false);

		GO_PauseMenu = GameObject.FindGameObjectWithTag ("Menu_Pause");
		GO_PauseMenu.SetActive (false);

		_Speed = PlayerPrefs.GetInt ("S_Speed", 10);
		_Health = PlayerPrefs.GetInt ("S_Health", 10);
		_Damage = PlayerPrefs.GetInt ("S_Damage", 10);

		GO_Player = Instantiate (Resources.Load ("Turret/" + PlayerPrefs.GetString ("S_T", "Turret 1"))) as GameObject;
		GO_Player.transform.position = GameObject.FindGameObjectWithTag ("Player").transform.position;
		GO_Player.transform.localScale = new Vector3 (1, 1, 1);
		GO_Player.transform.SetParent (GameObject.FindGameObjectWithTag ("Player").transform);
		GO_Player.GetComponent<Player_Control> ().enabled = false;
		GO_Player.GetComponent<Animator> ().enabled = false;
		GO_Player.AddComponent <Arcade_Player> ();

		GO_Bullet = Resources.Load ("Bullet/" + PlayerPrefs.GetString ("S_B", "Bullet 1")) as GameObject;
	}

	void Update ()
	{
	
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
}

