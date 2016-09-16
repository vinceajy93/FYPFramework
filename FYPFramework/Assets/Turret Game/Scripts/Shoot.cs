using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	public GameObject Shooting_Position;
	public GameObject Bullet;

	//private float nextfire = 0.3f;
	//private float firerate = 0.1f;
	//private float dt;
	// Use this for initialization
	void Start () {
		//dt = 0;
	}
	
	// Update is called once per frame
	void Update () {
		/*dt += Time.deltaTime;
		if (dt > nextfire) {
			nextfire += firerate;
			Instantiate (Bullet, new Vector3(Shooting_Position.transform.position.x, Shooting_Position.transform.position.y, Shooting_Position.transform.position.z + 10.0f), Shooting_Position.transform.rotation);
		}*/
	}

	public void onClick(){
		GameObject smt = Instantiate (Bullet, new Vector3 (Shooting_Position.transform.position.x, Shooting_Position.transform.position.y, Shooting_Position.transform.position.z + 10.0f), Shooting_Position.transform.rotation) as GameObject;
		smt.tag = gameObject.tag;
	}
}
