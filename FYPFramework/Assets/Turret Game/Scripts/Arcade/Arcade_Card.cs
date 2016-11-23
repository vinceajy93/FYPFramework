using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Arcade_Card : MonoBehaviour {
	private Arcade_Control _control;

	public bool b_Health;
	public bool b_Barrier;
	public bool b_FireRate;
	public bool b_BulletSpeed;

	private Color Useable = new Color(1f, 1f, 1f, 1f);
	private Color32 Useless = new Color32(106, 106, 106, 150);

	void Start () {
		_control = GameObject.Find ("Script").GetComponent<Arcade_Control> ();

		if (b_Health) {
			Image color = this.GetComponent<Image> ();
			color.color = Useless;
		}

		if (b_BulletSpeed) {
			Image color = this.GetComponent<Image> ();
			color.color = Useable;
		}

		if (b_FireRate) {
			Image color = this.GetComponent<Image> ();
			color.color = Useable;
		}

		if (b_Barrier) {
			Image color = this.GetComponent<Image> ();
			color.color = Useable;
		}
	}

	void Update () {
		if (b_Health) {
			if (_control.PlayerHealth.CurrentVal == _control.PlayerHealth.MaxVal) {
				Image color = this.GetComponent<Image> ();
				color.color = Useless;
			} else {
				Image color = this.GetComponent<Image> ();
				color.color = Useable;
			}
		}

		if (b_Barrier) {
			if (_control.Card_Barrier) {
				Image color = this.GetComponent<Image> ();
				color.color = Useless;
			} else {
				Image color = this.GetComponent<Image> ();
				color.color = Useable;
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

		if (b_Health) {
			Health (button);
		} 
	}

	private void Health (Button button) {
		if (_control.PlayerHealth.CurrentVal != _control.PlayerHealth.MaxVal) {
			_control._Health += 1;

			PlayerPrefs.SetInt ("Card_Health", PlayerPrefs.GetInt ("Card_Health") - 1);
			PlayerPrefs.DeleteKey (button.gameObject.tag);
			button.gameObject.SetActive (false);
		}
	}

	private void Barrier (Button button) {
		_control.Card_Barrier = true;

		GameObject barrier = Instantiate (Resources.Load ("Turret/P_Barrier")) as GameObject;
		barrier.transform.SetParent (GameObject.FindGameObjectWithTag ("Player1").transform);
		barrier.transform.position = GameObject.FindGameObjectWithTag ("Player1").transform.position + new Vector3 (0f, 0.35f, 0f);

		PlayerPrefs.SetInt ("Card_Barrier", PlayerPrefs.GetInt ("Card_Barrier") - 1);
		PlayerPrefs.DeleteKey (button.gameObject.tag);
		button.gameObject.SetActive (false);
	}

	private void FireRate (Button button) {
		GameObject Player = GameObject.FindGameObjectWithTag ("Player1");
		Player.GetComponent<Arcade_Player> ().Card_FireRate = true;

		PlayerPrefs.SetInt ("Card_FireRate", PlayerPrefs.GetInt ("Card_FireRate") - 1);
		PlayerPrefs.DeleteKey (button.gameObject.tag);
		button.gameObject.SetActive (false);
	}

	private void BulletSpeed (Button button) {
		_control.Card_BulletSpeed = true;

		PlayerPrefs.SetInt ("Card_BulletSpeed", PlayerPrefs.GetInt ("Card_BulletSpeed") - 1);
		PlayerPrefs.DeleteKey (button.gameObject.tag);
		button.gameObject.SetActive (false);
	}
}
