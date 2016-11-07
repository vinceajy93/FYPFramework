using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialSceneScript : MonoBehaviour {

	//for animation of the ship
	[SerializeField]
	private GameObject ship_anim;

	//Non-serialized fields
	private static float setTime = 2.0f; //time before the ship starts flying off the screen 
	private float tick;
	private bool shipAtOriginalPos = false;

	public RectTransform panel;
	public RectTransform center;
	public Button[] panelButtons;

	private float[] distance; //ALl buttons distance to the center
	private bool dragging = false; //true if is dragging
	private int bttnDistance; // hold the distance between the buttons;
	private int minButtonNum; // To hold the number of the button, with smallest distance to center

	// Use this for initialization
	void Start () {
		tick = 0;

		distance = new float[panelButtons.Length];

		//Get distance between buttons
		bttnDistance = (int)Mathf.Abs(panelButtons[1].GetComponent<RectTransform>().anchoredPosition.y - panelButtons[0].GetComponent<RectTransform>().anchoredPosition.y);
		Debug.Log (bttnDistance);

	}

	// Update is called once per frame
	void Update () {
		tick += Time.deltaTime;

		//do the animation for the plane_anim when the tick is higher than the set time
		if(tick > setTime){
			ship_anim.gameObject.transform.Translate (2.0f, 0, 0);
			if (ship_anim.transform.localPosition.x >= 510 && !shipAtOriginalPos) {
				ship_anim.transform.localPosition = new Vector2 (-600, ship_anim.transform.localPosition.y);
				shipAtOriginalPos = true;
			}
			//reset the tick back to 0 when the ship is back to its original position
			else if(ship_anim.transform.localPosition.x >= -235 && shipAtOriginalPos){
				tick = 0;
				ship_anim.transform.localPosition = new Vector2 (-235, ship_anim.transform.localPosition.y);
				shipAtOriginalPos = false;
			}
		}

		//for snapping to Panel scroll
		//saving each buttons distance to the distance array
		for(int i = 0; i < panelButtons.Length; i++){
			distance[i] = Mathf.Abs(center.transform.position.y - panelButtons[i].transform.position.y);
		}

		float minDistance = Mathf.Min (distance); //get the min distance

		for(int i = 0; i < panelButtons.Length; i++){
			if (minDistance == distance [i]) {
				minButtonNum = i;
			}
		}

		if (!dragging) {
			LerptoButton (minButtonNum * -bttnDistance);
		}
	}

	void LerptoButton(int position){
		float newY = Mathf.Lerp (position, panel.anchoredPosition.y, Time.deltaTime * 10f);
		Vector2 newPosition = new Vector2 (panel.anchoredPosition.x, newY); 

		panel.anchoredPosition = newPosition;
	}

	public void StartDrag(){
		dragging = true;
	}

	public void EndDrag(){
		dragging = false;
	}
}
