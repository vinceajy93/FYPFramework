using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class CharacterControl : MonoBehaviour {

	public Button LButton;
	public Button RButton;

	private Vector3 screenPoint;
	private Vector3 offset;

	private bool onPointerDownL = false;
	private bool onPointerDownR = false;

	void start(){
		
	}

	// Update is called once per frame
	void Update () {

	
	}
		
	void FixedUpdate(){

		//Debug.Log ("y: " + this.transform.position.y);
		//restrict character from moving out of view
		if (this.gameObject.tag == "Player1") {
			
			if (this.transform.position.x >= -2.5f && onPointerDownL == true) {
				this.transform.Translate (new Vector3 (-0.1f, 0, 0));
			}
			else if (this.transform.position.x <= 2.5f && onPointerDownR == true) {
				this.transform.Translate (new Vector3 (0.1f, 0, 0));
			}
		}

		if(this.gameObject.tag == "Player2"){
			
			if (this.transform.position.x <= 2.5f && onPointerDownL == true) {
				this.transform.Translate (new Vector3 (-0.1f, 0, 0));
			}
			else if (this.transform.position.x >= -2.5f && onPointerDownR == true) {
				this.transform.Translate (new Vector3 (0.1f, 0, 0));
			}
		}

		//To move the player when the button pressed
		/*if (onPointerDownL == true) {
			this.transform.Translate(new Vector3 (-0.1f, 0, 0));
		}
		if (onPointerDownR == true) {
			this.transform.Translate (new Vector3(0.1f, 0, 0));
		}*/



	}

	public void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y));
	}

	public void OnMouseDrag(){
		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y);
		//Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, -3.5f);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
		this.transform.position = curPosition;
	}

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
		
}
