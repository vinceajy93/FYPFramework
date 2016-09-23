using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//[RequireComponent(typeof(CircleCollider2D))]

public class CharacterControl : MonoBehaviour {
	private bool onPointerDownL = false;
	private bool onPointerDownR = false;
	private bool isInCanvas = false;

	public Canvas canvas;
	//Start is called at the start of the script
	void Start(){
	}

	// Update is called once per frame
	void Update () {
		// Get Camera's width and height (only in orthographic mode)
		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;

		// Get Player_1 Size in worldspace
		GameObject player_sprite = GameObject.Find("Player1");
		Vector2 sprite_size = player_sprite.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / player_sprite.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 player_world_size = local_sprite_size;
		player_world_size.x *= player_sprite.transform.lossyScale.x;
		player_world_size.y *= player_sprite.transform.lossyScale.y;

		if(PlayerPrefs.HasKey("Control")){
			// Move turret
			if (PlayerPrefs.GetInt ("Control") == 0) {
				
			} else { // Move bullet
				int total = PlayerPrefs.GetInt ("NoBullet");
				int counter = 1;
				counter = 1;
				foreach (GameObject player_1_bullet in GameObject.FindGameObjectsWithTag("Bullet_1")) {
					if (total == counter) {
						if (onPointerDownL) {
							if (player_1_bullet.transform.position.x >= 10f) {
								player_1_bullet.transform.position += Vector3.left * Time.deltaTime * 200;
							}
						}

						if (onPointerDownR) {
							if (player_1_bullet.transform.position.x <= width - 10f) {
								player_1_bullet.transform.position += Vector3.right * Time.deltaTime * 200;
							}
						}
						break;
					} else
						counter++;
				}
			}
		}
	}

	public void onButtonDownLeft(){
		onPointerDownL = true;
	}

	public void onButtonDownRight(){
		onPointerDownR = true;
	}

	public void onButtonUpLeft(){
		onPointerDownL = false;
	}

	public void onButtonUpRight(){
		onPointerDownR = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("working");
		if (other.gameObject.tag == "Canvas") {
			isInCanvas = true;
		} else {
			isInCanvas = false;
		}
	}
}
