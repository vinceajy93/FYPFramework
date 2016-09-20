using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class debugging : MonoBehaviour {

	private float x, y = 0;
	public Text DebugText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		DebugText.text = "x: " + x  + " y: " + y;
	}

	void OnMouseDown () {

		Debug.Log ("works");
		x = Input.mousePosition.x;
		y = Input.mousePosition.y;

		DebugText.transform.position = new Vector3 (x, y + 20.0f, 0);
	}
}
