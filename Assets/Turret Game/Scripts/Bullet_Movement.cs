using UnityEngine;
using System.Collections;

public class Bullet_Movement : MonoBehaviour {
	private GameObject Bg;
	private int num_bg;
	// Use this for initialization
	void Start () {
		Bg = GameObject.FindGameObjectWithTag ("Background");
		num_bg = GameObject.FindGameObjectsWithTag ("Background").Length;
	}
	
	// Update is called once per frame
	void Update () {
		//Bullet movement
		if (gameObject.tag == "Bullet_1")
			gameObject.transform.localPosition += Vector3.up * Time.deltaTime * 10;
		else if(gameObject.tag == "Bullet_2")
			gameObject.transform.localPosition += Vector3.down * Time.deltaTime * 10;

		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;

		Vector2 sprite_size = Bg.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / Bg.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 world_size = local_sprite_size;
		world_size.x *= Bg.transform.lossyScale.x;
		world_size.y *= Bg.transform.lossyScale.y;

		if (gameObject.transform.position.y > ((world_size.y * num_bg) - (height / 2)) && gameObject.tag == "Bullet_1") {
			Destroy (gameObject);
		}
	}
}
