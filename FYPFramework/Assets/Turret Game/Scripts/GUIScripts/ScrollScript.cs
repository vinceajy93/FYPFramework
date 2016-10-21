using UnityEngine;
using System.Collections;

public class ScrollScript : MonoBehaviour {

	//Speed of the scrolling
	public float scrollSpeed = 0.1f;
	Vector2 offset;
	public enum ScrollDir{
		ScrollLeft,
		ScrollRight,
		ScrollUp,
		ScrollDown
	};

	//Enum scroll direction
	public ScrollDir _ScrollDir;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//switch case to check and set the direction of scroll
		switch (_ScrollDir) {
		case ScrollDir.ScrollLeft:
			offset = new Vector2 (Mathf.Repeat( Time.time * scrollSpeed, 1), 0);
			break;
		case ScrollDir.ScrollRight:
			offset = - new Vector2 (Mathf.Repeat( Time.time * scrollSpeed, 1), 0);
			break;
		case ScrollDir.ScrollUp:
			offset = - new Vector2 (0, Mathf.Repeat( Time.time * scrollSpeed, 1));
			break;
		case ScrollDir.ScrollDown:
			offset = new Vector2 (0, Mathf.Repeat( Time.time * scrollSpeed, 1));
			break;
		}



		GetComponent<Renderer> ().material.mainTextureOffset = offset;
	}
}
