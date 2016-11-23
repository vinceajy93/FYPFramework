using UnityEngine;
using System.Collections;

public class Arcade_Background : MonoBehaviour {
	private Arcade_Control _scriptController;

	private Sprite[] all_Sprite;
	private int next_sprite;
	private int original_next_sprite;
	private SpriteRenderer _spriteR;
	private GameObject other_background;
	private float speed = 0.1f;

	private float camera_height;
	private Vector3 bg_size;
	private Vector3 bg_other_size;

	private bool change_alpha = false;
	private bool fade = false;

	void Start () {
		_scriptController = GameObject.Find ("Script").GetComponent<Arcade_Control> ();
		all_Sprite = Resources.LoadAll<Sprite> ("Background");
		_spriteR = this.GetComponent<SpriteRenderer> ();

		camera_height = 2f * Camera.main.orthographicSize;

		Vector2 sprite_size = this.GetComponent<SpriteRenderer>().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		bg_size = local_sprite_size;
		bg_size.x *= this.transform.lossyScale.x;
		bg_size.y *= this.transform.lossyScale.y;

		if (this.transform.position.y == 0) {
			if (_spriteR.color.a != 1f) {
				next_sprite = all_Sprite.Length - 4;
				fade = true;
			} else {
				next_sprite = 0;
			}
		} else {
			next_sprite = 1;
		}
			
		foreach (GameObject ea_bg in GameObject.FindGameObjectsWithTag("Background")) {
			if (this.transform.position.y != ea_bg.transform.position.y) {
				other_background = ea_bg;
			}
		}

		sprite_size = this.GetComponent<SpriteRenderer>().sprite.rect.size;
		local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		bg_other_size = local_sprite_size;
		bg_other_size.x *= this.transform.lossyScale.x;
		bg_other_size.y *= this.transform.lossyScale.y;

		if (this.transform.position.y != 0f) {
			float new_y = other_background.transform.position.y + bg_size.y;
			this.transform.position = new Vector3 (this.transform.position.x, new_y, this.transform.position.z);
		}
	}
	

	void Update () {
		if (!_scriptController.bPause && !_scriptController.bEnd) {
			speed += Time.deltaTime * 0.001f;
			if (this.transform.position.y <= ((-camera_height / 2) - (bg_size.y / 2))) {
				float new_y = other_background.transform.position.y + (bg_other_size.y / 2) + (bg_size.y / 2);
				this.transform.position = new Vector3 (this.transform.position.x, new_y, this.transform.position.z) + (Vector3.down * speed);

				next_sprite += 2;
				if (next_sprite > all_Sprite.Length - 1) {
					while (next_sprite - 2 >= 0) {
						next_sprite -= 2;
					}
				}

				_spriteR.sprite = all_Sprite [next_sprite];

				for (int i = next_sprite; i >= 0; i -= 4) {
					if (i == 0 && fade) {
						change_alpha = true;
						_spriteR.color = new Color (1f, 1f, 1f, 1f);
					}
				}
			} else {
				this.transform.position += Vector3.down * speed;

				if (change_alpha && this.transform.position.y < 0f) {
					if (_spriteR.color.a > 0) {
						float alpha = _spriteR.color.a;
						alpha -= speed * 0.4f;
						_spriteR.color = new Color (1f, 1f, 1f, alpha);
					} else {
						change_alpha = false;
					}
				}
			}
		}
	}
}
