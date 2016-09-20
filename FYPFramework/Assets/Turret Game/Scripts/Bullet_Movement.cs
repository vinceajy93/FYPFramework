using UnityEngine;
using System.Collections;

public class Bullet_Movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//dt += Time.deltaTime;
		transform.Rotate (0, 0, -Time.deltaTime * 500);

		//transform.localPosition += Vector3.up * Time.deltaTime * 10;
		if (gameObject.tag == "Player1")
			transform.localPosition += Vector3.up * Time.deltaTime * 50;
		if(gameObject.tag == "Player2")
			this.transform.localPosition += Vector3.down * Time.deltaTime * 50;

		//check if bullet is out of gameplay screen
		//Debug.Log(transform.position);
		if (transform.position.y > -549.0f || transform.position.y < 557.2f) {
			//Destroy (gameObject);

		}
	}
	void OnCollisionEnter2D( Collision2D other){
		Debug.Log ("works");
		if(other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2"){
			Destroy (gameObject);
			Destroy (other.gameObject);
		}
	}
}
