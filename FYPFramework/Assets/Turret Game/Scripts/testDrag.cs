using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

public class testDrag: MonoBehaviour {

	public Camera P1;

	private Vector3 screenPoint;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		offset = gameObject.transform.position - P1.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
		
	void onMouseDrag(){
		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = P1.ScreenToWorldPoint (curScreenPoint) + offset;
		transform.position = curPosition;
	}
}
