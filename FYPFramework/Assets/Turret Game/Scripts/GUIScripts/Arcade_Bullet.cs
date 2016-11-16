using UnityEngine;
using System.Collections;

public class Arcade_Bullet : MonoBehaviour {
	private Arcade_Control _scriptController;

	private float camera_height;
	private Vector3 bullet_size;

	private float bullet_speed = 0.25f;
	void Start () {
		_scriptController = GameObject.Find ("Script").GetComponent<Arcade_Control> ();

		camera_height = 2f * Camera.main.orthographicSize;

		Vector2 sprite_size = this.GetComponent<SpriteRenderer>().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		bullet_size = local_sprite_size;
		bullet_size.x *= this.transform.lossyScale.x;
		bullet_size.y *= this.transform.lossyScale.y;
	}

	void Update () {
		if (!_scriptController.bPause) {
			gameObject.transform.localPosition += Vector3.up * bullet_speed;

			// "Destroy" by placing them back to bullet_rest gameobject
			if (gameObject.transform.position.y > ((camera_height * 0.5f) + (bullet_size.y)))
			{
				Destroy (this.gameObject);
			}
		}
	}
}
