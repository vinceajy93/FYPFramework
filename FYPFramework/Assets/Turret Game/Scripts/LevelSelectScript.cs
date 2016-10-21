using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class LevelSelectScript : MonoBehaviour {


	private string GO_Name;
	private Image GO_Scale;


	static int max_Obstacle = 10;
	public int time, rounds, no_obstacles;

	//time buttons
	[SerializeField]
	private Button button_30s, button_60s, button_90s;
	//round buttons
	[SerializeField]
	private Button button_round1, button_round2, button_round3;
	//show settings chosen by user at the main screen
	[SerializeField]
	private Image Displaytime_Image;
	[SerializeField]
	private Sprite spr_30s, spr_60s, spr_90s;
	//number of obstacles
	[SerializeField]
	private Text obstacles_Text, Displayrounds_Text, Displayobstacles_Text;

	[SerializeField]
	private GameObject Settings_panel, Stage_panel;

	//color for selected buttons (changable)
	private Color color = new Color(0.9f, 0.9f, 0.5f);

	//Color block to set the color of UI buttons (highlighted)
	ColorBlock colorBlockSelected = ColorBlock.defaultColorBlock;
	ColorBlock colorBlockDefault = ColorBlock.defaultColorBlock;

	// Use this for initialization
	void Start () {
		//Set panel to default active state
		Settings_panel.SetActive(false);
		Stage_panel.SetActive (true);

		//change the color of the selected button to highlighted color (yellow)
		colorBlockSelected.normalColor = color;
		colorBlockSelected.highlightedColor = color;
		colorBlockSelected.pressedColor = color;

		//Reset the color of other buttons to default color (white)
		colorBlockDefault.normalColor = Color.white;

		//default timer starts at 60s
		time = 60;

		//default round starts at 2
		rounds = 2;

		//default obstacles in round
		no_obstacles = 4;


		//Set current selected button color to selected color
		button_60s.colors = colorBlockSelected;
		button_round2.colors = colorBlockSelected;

		//Send default settings to playerprefs
		PlayerPrefs.SetInt("no_obstacles", no_obstacles);
		PlayerPrefs.SetInt ("time", time);
		PlayerPrefs.SetInt("rounds", rounds);
	}
	
	// Update is called once per frame
	void Update () {
		//Set the text of the obstacles_text to the no_obstacles
		obstacles_Text.text = (no_obstacles.ToString());

		//Set the image of display timer to match the shown one in the setting
		switch (time) {
		case 30:
			Displaytime_Image.sprite = spr_30s;
			break;
		case 60:
			Displaytime_Image.sprite = spr_60s;
			break;
		case 90:
			Displaytime_Image.sprite = spr_90s;
			break;
		}

		//Set the number of rounds to match with the one shown in the setting
		Displayrounds_Text.text = rounds.ToString ();

		//Set the numer of Obstacles to match the one shown in the setting
		Displayobstacles_Text.text = no_obstacles.ToString();
	}

	public void SetTime(){
		GO_Name = EventSystem.current.currentSelectedGameObject.name;

		switch (GO_Name) {
		case "Button_30s":
			//set the time of the game per round
			time = 30;

			//Set current selected button color to selected color
			button_30s.colors = colorBlockSelected;

			//reset the color of other buttons back to white
			button_60s.colors = colorBlockDefault;
			button_90s.colors = colorBlockDefault;
			break;

		case "Button_60s":
			//set the time of the game per round
			time = 60;

			//Set current selected button color to selected color
			button_60s.colors = colorBlockSelected;

			//reset the color of other buttons back to white
			button_30s.colors = colorBlockDefault;
			button_90s.colors = colorBlockDefault;
			break;
		case "Button_90s":
			//set the time of the game per round
			time = 90;

			//Set current selected button color to selected color
			button_90s.colors = colorBlockSelected;

			//reset the color of other buttons back to white
			button_30s.colors = colorBlockDefault;
			button_60s.colors = colorBlockDefault;
			break;
		}
	}

	public void SetRounds(){
		GO_Name = EventSystem.current.currentSelectedGameObject.name;

		switch (GO_Name) {
		case "Button_round1":
			//set the number of rounds per game
			rounds = 1;

			//Set current selected button color to selected color
			button_round1.colors = colorBlockSelected;

			//reset the color of other buttons back to white
			button_round2.colors = colorBlockDefault;
			button_round3.colors = colorBlockDefault;
			break;

		case "Button_round2":
			//set the number of rounds per game
			rounds = 2;

			//Set current selected button color to selected color
			button_round2.colors = colorBlockSelected;

			//reset the color of other buttons back to white
			button_round1.colors = colorBlockDefault;
			button_round3.colors = colorBlockDefault;
			break;
		case "Button_round3":
			//set the number of rounds per game
			rounds = 3;

			//Set current selected button color to selected color
			button_round3.colors = colorBlockSelected;

			//reset the color of other buttons back to white
			button_round1.colors = colorBlockDefault;
			button_round2.colors = colorBlockDefault;
			break;
		}
	}

	//visual feedback on button press
	public void OnButtonDown(){
		GO_Name = EventSystem.current.currentSelectedGameObject.name;
		GO_Scale = EventSystem.current.currentSelectedGameObject.GetComponent<Image> ();

		switch (GO_Name) {
		case "Button_30s":
			GO_Scale.transform.localScale = new Vector2 (1.4f, 1.4f);
			break;
		case "Button_60s":
			GO_Scale.transform.localScale = new Vector2 (1.4f, 1.4f);
			break;
		case "Button_90s":
			GO_Scale.transform.localScale = new Vector2 (1.4f, 1.4f);
			break;
		case "Button_round1":
			GO_Scale.transform.localScale = new Vector2 (0.7f, 0.7f);
			break;
		case "Button_round2":
			GO_Scale.transform.localScale = new Vector2 (0.7f, 0.7f);
			break;
		case "Button_round3":
			GO_Scale.transform.localScale = new Vector2 (0.7f, 0.7f);
			break;
		case "plusObstacles":
			GO_Scale.transform.localScale = new Vector2 (2.0f, 2.0f);
			break;
		case "minusObstacles":
			GO_Scale.transform.localScale = new Vector2 (2.0f, 2.0f);
			break;
		case "Back_Button":
			GO_Scale.transform.localScale = new Vector2 (0.45f, 0.45f);
			break;
		case "Settings_Button":
			GO_Scale.transform.localScale = new Vector2 (0.45f, 0.45f);
			break;
		}
	}

	public void OnButtonUp(){
		GO_Name = EventSystem.current.currentSelectedGameObject.name;
		GO_Scale = EventSystem.current.currentSelectedGameObject.GetComponent<Image> ();

		switch (GO_Name) {
		case "Button_30s":
			GO_Scale.transform.localScale = new Vector2 (1.7f, 1.7f);
			break;
		case "Button_60s":
			GO_Scale.transform.localScale = new Vector2 (1.7f, 1.7f);
			break;
		case "Button_90s":
			GO_Scale.transform.localScale = new Vector2 (1.7f, 1.7f);
			break;
		case "Button_round1":
			GO_Scale.transform.localScale = new Vector2 (1.0f, 1.0f);
			break;
		case "Button_round2":
			GO_Scale.transform.localScale = new Vector2 (1.0f, 1.0f);
			break;
		case "Button_round3":
			GO_Scale.transform.localScale = new Vector2 (1.0f, 1.0f);
			break;
		case "plusObstacles":
			GO_Scale.transform.localScale = new Vector2 (3.0f, 3.0f);
			break;
		case "minusObstacles":
			GO_Scale.transform.localScale = new Vector2 (3.0f, 3.0f);
			break;
		case "Back_Button":
			GO_Scale.transform.localScale = new Vector2 (0.5f, 0.5f);
			break;
		case "Settings_Button":
			GO_Scale.transform.localScale = new Vector2 (0.5f, 0.5f);
			break;
		}
	}

	public void Increment_Obstacles(){
		if (no_obstacles < max_Obstacle) {
			no_obstacles += 2;
		}
	}

	public void Decrement_Obstacles(){
		if (no_obstacles > 0) {
			no_obstacles -= 2;
		}
	}

	public void EnterExitSettingsFunction(){
		if (Settings_panel.activeSelf == true) {
			Settings_panel.SetActive (false);
			Stage_panel.SetActive (true);

			//Send settings to playerprefs
			PlayerPrefs.SetInt("no_obstacles", no_obstacles);
			PlayerPrefs.SetInt ("time", time);
			PlayerPrefs.SetInt("rounds", rounds);
		}
			
		else{
			Settings_panel.SetActive (true);
			Stage_panel.SetActive (false);
		}
			

	}
}
