using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//[RequireComponent(typeof(CircleCollider2D))]

public class CharacterControl : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;

	private bool onPointerDownL = false;
	private bool onPointerDownR = false;
	private bool isInCanvas = false;

	public GameObject Border;
	public Canvas canvas;
	//Start is called at the start of the script
	void Start(){
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.W)) {
			Debug.Log ("transform pos" + this.transform.localPosition.x);
			Debug.Log ("Screen width" + Screen.width);
		}

		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;

		if(PlayerPrefs.HasKey("Control")){
			// Move turret
			if (PlayerPrefs.GetInt ("Control") == 0) {
				GameObject player = GameObject.Find ("Player1");
				if (onPointerDownL) {
					if (player.transform.position.x >= 26f) {
						player.transform.position += Vector3.left * Time.deltaTime * 200;
					}
				}

				if (onPointerDownR) {
					if (player.transform.position.x <= width - 26f) {
						player.transform.position += Vector3.right * Time.deltaTime * 200;
					}
				}
			} else { // Move bullet
				int total = PlayerPrefs.GetInt ("NoBullet");
				int counter = 1;
				counter = 1;
				foreach (GameObject player_1 in GameObject.FindGameObjectsWithTag("Bullet_1")) {
					if (total == counter) {
						if (onPointerDownL) {
							if (player_1.transform.position.x >= 10f) {
								player_1.transform.position += Vector3.left * Time.deltaTime * 200;
							}
						}

						if (onPointerDownR) {
							if (player_1.transform.position.x <= width - 10f) {
								player_1.transform.position += Vector3.right * Time.deltaTime * 200;
							}
						}
						break;
					} else
						counter++;
				}
			}
		}
	}

	/*public void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y));
	}

	public void OnMouseDrag(){
		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y);
		//Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, -3.5f);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
		this.transform.position = curPosition;
	}*/

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
