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

	//Start is called at the start of the script
	void Start(){

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.W)) {
			Debug.Log ("transform pos" + this.transform.localPosition.x);
			Debug.Log ("Screen width" + Screen.width);
		}
	}
		
	void FixedUpdate(){
		//restrict character from moving out of view
			if (this.transform.localPosition.x >= -345.0f && onPointerDownL == true) {
				this.transform.Translate (new Vector3 (-2.0f, 0, 0));
			}
			else if (this.transform.localPosition.x <= 345.0f && onPointerDownR == true) {
				this.transform.Translate (new Vector3 (2.0f, 0, 0));
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
		//this.transform.Translate(new Vector3 (0, 0.1f, 0));
	}

	public void onButtonDownRight(){
		onPointerDownR = true;
		//this.transform.Translate (new Vector3(0, -0.1f, 0));
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
