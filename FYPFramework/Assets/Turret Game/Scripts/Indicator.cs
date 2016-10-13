using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Indicator : Mode_Control {
	// Needed varible to Show/Hide Indicator on Canvas
	private Overlay_Control m_Overlay_Control;

	// Canvas Varibles (Only usable in canvas)
	private GameObject MainCanvas;
	private float MainCanvas_Height;

	// Camera
	private Camera MainCam;
	private float MainCam_Height;
	private float MainCam_Width;

	private Camera P1Cam;
	private Camera P2Cam;
	private float PCam_Height;

	// Cavas Indicator Setting
	private GameObject canvas_indicator;
	private float canvas_indicator_height;
	private bool On_X = false; // Check if bullet is on Camera Width X axis
	private bool On_Y = false; // Check if bullet is not within the Camera Height Y axis
	private string indicator_name = "Indicator";
	public Sprite red;
	public Sprite yellow;
	public Sprite green;
	public Sprite original;

	// Size of Hp Bar
	private float Hp;

	// Size of Map
	private GameObject Map;
	private int num_Map;
	private Vector2 Map_world_size;
	private float Map_TotalHeight;

	// Use this for initialization
	void Start () {
		// Check if Single player
		if (GameObject.Find("Scripts").GetComponent<Mode_Control>().game_mode_Single) {
			game_mode_Single = true;
		} else {
			P2Cam = GameObject.Find ("Top Camera").GetComponent<Camera> ();
			P1Cam = GameObject.Find ("Bottom Camera").GetComponent<Camera> ();

			PCam_Height = 2f * P1Cam.orthographicSize;
		}

		// Get Canvas as Gameobject
		MainCanvas = GameObject.Find("Canvas");
		MainCanvas_Height = MainCanvas.GetComponent<RectTransform> ().rect.height;

		// Some Camera Varibles
		MainCam = Camera.main;
		MainCam_Height = 2f * MainCam.orthographicSize;
		MainCam_Width = MainCam_Height * MainCam.aspect;

		// Clone a indicator on Canvas and Set to false since bullet is not within game screen view
		canvas_indicator = Instantiate (Resources.Load (indicator_name), Vector3.zero, Quaternion.identity) as GameObject;
		canvas_indicator.transform.SetParent (MainCanvas.transform, false);
		canvas_indicator_height = canvas_indicator.GetComponent<RectTransform> ().rect.height;
		canvas_indicator.SetActive (false);

		Map = GameObject.FindGameObjectWithTag ("Background");
		num_Map = GameObject.FindGameObjectsWithTag ("Background").Length;

		Vector2 Map_sprite_size = Map.GetComponent<SpriteRenderer> ().sprite.rect.size;
		Vector2 Map_local_sprite_size = Map_sprite_size / Map.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
		Map_world_size = Map_local_sprite_size;
		Map_world_size.x *= Map.transform.lossyScale.x;
		Map_world_size.y *= Map.transform.lossyScale.y;

		Map_TotalHeight = Map_world_size.y * num_Map;

		Hp = GameObject.Find ("Canvas/HealthBar P1").GetComponent<RectTransform>().rect.height;
	}
	
	// Update is called once per frame
	void Update () {
		// For All
		// Check if bullet is in the camera view, set true, else, set false
		if (this.transform.position.x < MainCam_Width / 2 && this.transform.position.x > -MainCam_Width / 2) {
			On_X = true;
		} else {
			On_X = false;
			canvas_indicator.GetComponent<Image> ().sprite = original;
		}

		// If Bullet is in Camera View, Check for 
		if (On_X) {
			// Multiple PLayer
			if (!game_mode_Single) {
				Camera temp_cam = null;

				if (this.CompareTag ("Bullet_1")) { // If Bullet 1, check in camera 2
					temp_cam = P2Cam;
				} else if(this.CompareTag("Bullet_2")){ // If Bullet 2, check in camera 1
					temp_cam = P1Cam;
				}

				if (this.transform.position.y > (-PCam_Height / 2) + temp_cam.transform.position.y && this.transform.position.y < (PCam_Height / 2) + temp_cam.transform.position.y) {
					On_Y = false;
				} else {
					On_Y = true;
				}
			}
		}

		// If bullet is in Camera X and outside Camera Y, active = true
		if (On_X && On_Y) {
			canvas_indicator.SetActive (true);
			canvas_indicator.transform.position = new Vector3 (MainCam.WorldToScreenPoint (this.transform.position).x, 0, 0);

			if (this.CompareTag ("Bullet_1")) { // If Bullet 1, check in camera 2
				if (this.transform.position.y > (PCam_Height / 2) + P2Cam.transform.position.y) {
					canvas_indicator.GetComponent<RectTransform> ().localEulerAngles = new Vector3 (0, 0, 180);
					canvas_indicator.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (canvas_indicator.GetComponent<RectTransform> ().anchoredPosition.x, MainCanvas_Height / 2 - canvas_indicator_height / 2);
				} else {
					canvas_indicator.GetComponent<RectTransform> ().localEulerAngles = new Vector3 (0, 0, 0);
					canvas_indicator.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (canvas_indicator.GetComponent<RectTransform> ().anchoredPosition.x, canvas_indicator_height / 2 + Hp);
				}

				// Check sprite depending on the position of the bullet
				if (this.transform.position.y > ((Map_TotalHeight / 3) * 2) - (PCam_Height / 2)) {
					canvas_indicator.GetComponent<Image> ().sprite = red;
				} else if (this.transform.position.y > (Map_TotalHeight / 3) - (PCam_Height / 2)) {
					canvas_indicator.GetComponent<Image> ().sprite = yellow;
				} else {
					canvas_indicator.GetComponent<Image> ().sprite = green;
				}
			} else if(this.CompareTag("Bullet_2")) { // If Bullet 2, check in camera 1
				if (this.transform.position.y > (PCam_Height / 2) + P1Cam.transform.position.y) {
					canvas_indicator.GetComponent<RectTransform> ().localEulerAngles = new Vector3 (0, 0, 180);
					canvas_indicator.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (canvas_indicator.GetComponent<RectTransform> ().anchoredPosition.x, -canvas_indicator_height / 2 - Hp);
				} else {
					canvas_indicator.GetComponent<RectTransform> ().localEulerAngles = new Vector3 (0, 0, 0);
					canvas_indicator.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (canvas_indicator.GetComponent<RectTransform> ().anchoredPosition.x, -MainCanvas_Height / 2 + canvas_indicator_height / 2);
				}

				if (this.transform.position.y > ((Map_TotalHeight / 3) * 2) - (PCam_Height / 2)) {
					canvas_indicator.GetComponent<Image> ().sprite = green;
				} else if (this.transform.position.y > (Map_TotalHeight / 3) - (PCam_Height / 2)) {
					canvas_indicator.GetComponent<Image> ().sprite = yellow;
				} else {
					canvas_indicator.GetComponent<Image> ().sprite = red;
				}
			}
		} else {
			canvas_indicator.SetActive (false);
		}
	}

	//Vector2 WorldObject_ScreenPosition()
	//{
		// Method 1
		//RectTransform CanvasRect = MainCanvas.GetComponent<RectTransform> ();
		//Vector2 ViewportPosition = MainCam.WorldToViewportPoint (this.transform.position);
		//Vector2 WorldObject_ScreenPosition = new Vector2( (ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f), (ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f) ); // *0.5f to move the (0,0) from world space @ lower left corner to (0,0) from canvas @ centre
		////canvas_indicator.GetComponent<RectTransform> ().anchoredPosition = WorldObject_ScreenPosition;
		//return WorldObject_ScreenPosition;

		// Method 2 *Claim to cause lag (Unknown)*
		//Vector2 pos = this.transform.position;
		//Vector2 ViewportPoint = Camera.main.WorldToViewportPoint (pos);
		////canvas_indicator.GetComponent<RectTransform> ().anchorMin = ViewportPoint;
		////canvas_indicator.GetComponent<RectTransform> ().anchorMax = ViewportPoint;
		//return ViewportPoint;

		// How to Call the Function
		// Method 1
		//canvas_indicator.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (WorldObject_ScreenPosition ().x, 0);

		// Method 2
		//canvas_indicator.GetComponent<RectTransform> ().anchorMin = WorldObject_ScreenPosition();
		//canvas_indicator.GetComponent<RectTransform> ().anchorMax = WorldObject_ScreenPosition();
	//}
}
