using UnityEngine;
using System.Collections;

public class Bullet_Movement : MonoBehaviour {
	public GameObject Bg;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//dt += Time.deltaTime;
		//transform.Rotate (0, 0, -Time.deltaTime * 500);

		//transform.localPosition += Vector3.up * Time.deltaTime * 10;
		if (gameObject.tag == "Bullet_1")
			transform.localPosition += Vector3.up * Time.deltaTime * 200;
		if(gameObject.tag == "Bullet_2")
			this.transform.localPosition += Vector3.down * Time.deltaTime * 70;

		//check if bullet is out of gameplay screen
		//Debug.Log(transform.position);
		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;

		Vector2 sprite_size = Bg.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / Bg.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Vector3 world_size = local_sprite_size;
		world_size.x *= Bg.transform.lossyScale.x;
		world_size.y *= Bg.transform.lossyScale.y;

		if (transform.position.y > world_size.y && gameObject.tag == "Bullet_1") {
			Destroy (gameObject);
			int total = PlayerPrefs.GetInt ("NoBullet");
			PlayerPrefs.SetInt ("NoBullet", total - 1);
		}
	}
	void OnCollisionEnter2D( Collision2D other){
		Debug.Log ("works");
		if(other.gameObject.tag == "Bullet_1"){
			Destroy (gameObject);
			Destroy (other.gameObject);
		}
	}
}
