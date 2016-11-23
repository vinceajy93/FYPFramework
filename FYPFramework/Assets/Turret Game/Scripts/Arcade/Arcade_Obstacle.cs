using UnityEngine;
using System.Collections;

public class Arcade_Obstacle : MonoBehaviour {
	private Arcade_Control _scriptController;
	private float delTime = 10f;
	public bool timer = true;
	public int health = 5;

	public bool hit = false;
	public float hit_timeleft = 0.5f;
	public float speed = 1f;
	private SpriteRenderer _sprite;

	void Start () {
		_scriptController = GameObject.Find ("Script").GetComponent<Arcade_Control> ();
		_sprite = this.GetComponent<SpriteRenderer> ();
	}

	void Update () {
		if (!_scriptController.bPause && !_scriptController.bEnd) {
			if (timer) {
				delTime -= Time.deltaTime;
				if (delTime < 0f) {
					_scriptController.Card_Barrier = false;
					Destroy (this.gameObject);
				}
			} else {
				if (hit) {
					hit_timeleft -= Time.deltaTime;

					if (hit_timeleft < 0) {
						hit_timeleft = 0.5f;
						hit = false;
						_sprite.color = new Color (1, 1, 1);
					}
				}
			}
		}
	}
}
