using UnityEngine;
using System.Collections;

public class Bullet_Movement : Shoot {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//dt += Time.deltaTime;
		transform.Rotate (0, 0, -Time.deltaTime * 500);

		//transform.localPosition += Vector3.up * Time.deltaTime * 10;
		if (gameObject.tag == "Player1")
			transform.localPosition += Vector3.up * Time.deltaTime * 10;
		if(gameObject.tag == "Player2")
			this.transform.position += Vector3.down * Time.deltaTime * 10;
		
		if (transform.localPosition.y > 6 || transform.localPosition.y < -6) {
			Destroy (gameObject);

		}
	}
}
