using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LevelSelectScript : MonoBehaviour {


	private string GO_Name;
	private Image GO_Scale;
	private GameObject GO;
	private static int scrollSpeed = 1000;

	static int max_Obstacle = 10;
	public int time, rounds, no_obstacles;
	private bool isPanelMoving = false;
	private bool isToSettingsPanel = true;

	private string[] Description_String = new string[4];

	//time buttons
	[SerializeField]
	private Button button_30s, button_60s, button_90s;
	//round buttons
	[SerializeField]
	private Button button_round1, button_round2, button_round3;
	//show settings chosen by user at the main screen
	[SerializeField]
	private Image Displaytime_Image, DisplayEnlargeImage;
	[SerializeField]
	private Sprite spr_30s, spr_60s, spr_90s;
	//number of obstacles
	[SerializeField]
	private Text obstacles_Text, Displayrounds_Text, Displayobstacles_Text, Description_Text;

	//color for selected buttons (changable)
	private Color color = new Color(0.5f, 0.83f, 0.88f);

	//Color block to set the color of UI buttons (highlighted)
	ColorBlock colorBlockSelected = ColorBlock.defaultColorBlock;
	ColorBlock colorBlockDefault = ColorBlock.defaultColorBlock;

	[SerializeField]
	private GameObject Panel;

	private GameObject[] buttons;

	// Use this for initialization
	void Start () {

		//Reset the roundsPassed back to 0 (starting a new game)
		PlayerPrefs.SetInt("roundsPassed", 1);

		//Set the text of the string array
		Description_String[0] = "Stage: Ice Studio \n\nconditions are harsh.\nbeware of lifeforms floating about. Give sweets if needed";
		Description_String [1] = "Stage: Hakuna Marteena \n\n Hail the queen of marteena!";
		Description_String[2] = "Stage: Arki Island \n\nYour turret give me cancer";
		Description_String [3] = "Stage: Random \n\nVenture into warp vortex and appear on a random map";

		//Set the description text to show the first stage description
		Description_Text.text = Description_String[0];

		//Set the color of the default selected stage to highlighted color
		Component[] tempComponent;

		tempComponent = GameObject.Find("stage_1").GetComponentsInChildren<Image> ();
		foreach (Image ImageCol in tempComponent) {
			ImageCol.color = color;
		}
		//change the color of the selected button to highlighted color
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

		//reset the number of rounds won by each player to 0
		if(PlayerPrefs.HasKey("roundWon_P1"))
			PlayerPrefs.SetInt("roundWon_P1", 0);

		if(PlayerPrefs.HasKey("roundWon_P2"))
			PlayerPrefs.SetInt("roundWon_P2", 0);
		
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

		//Move the panel if ispanelmoving = true 
		if (isPanelMoving) {
			DisableAllButtons (); //disable the interactability of all buttons

			// translate the panel if Settings button is pressed (going <<)
			if (Panel.transform.localPosition.x >= 0 && Panel.transform.localPosition.x < 793 && isToSettingsPanel) {
				Panel.transform.Translate (Time.deltaTime * scrollSpeed, 0, 0);

				if (Panel.transform.localPosition.x >= 793) {
					EnableAllButtons (); //enable the interactablility of all buttons
					isToSettingsPanel = false;
					isPanelMoving = false;
					Panel.transform.localPosition = new Vector3 (793, 0, 0);
				}

			}
			// translate the panel if Settings button is pressed (going >>)
			else if (Panel.transform.localPosition.x > 0 && Panel.transform.localPosition.x <= 793 && !isToSettingsPanel) {
				Panel.transform.Translate (-Time.deltaTime * scrollSpeed, 0, 0);

				//if panel is out of place, put it back in place
				if (Panel.transform.localPosition.x <= 0) {
					EnableAllButtons(); //enable the interactability of all buttons
					isToSettingsPanel = true;
					isPanelMoving = false;
					Panel.transform.localPosition = new Vector3 (0, 0, 0);
				}

			}
		}
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

	//visual feedback on button press /mouse down
	public void OnButtonDown(){
		GO_Name = EventSystem.current.currentSelectedGameObject.name;
		GO_Scale = EventSystem.current.currentSelectedGameObject.GetComponent<Image> ();

		switch (GO_Name) {
		case "Button_30s":
			GO_Scale.transform.localScale = new Vector2 (2.5f, 2.5f);
			break;
		case "Button_60s":
			GO_Scale.transform.localScale = new Vector2 (2.5f, 2.5f);
			break;
		case "Button_90s":
			GO_Scale.transform.localScale = new Vector2 (2.5f, 2.5f);
			break;
		case "Button_round1":
			GO_Scale.transform.localScale = new Vector2 (1.5f, 1.5f);
			break;
		case "Button_round2":
			GO_Scale.transform.localScale = new Vector2 (1.5f, 1.5f);
			break;
		case "Button_round3":
			GO_Scale.transform.localScale = new Vector2 (1.5f, 1.5f);
			break;
		case "plusObstacles":
			GO_Scale.transform.localScale = new Vector2 (4.5f, 4.5f);
			break;
		case "minusObstacles":
			GO_Scale.transform.localScale = new Vector2 (4.5f, 4.5f);
			break;
		}
	}

	//on button release /mouse up
	public void OnButtonUp(){
		GO_Name = EventSystem.current.currentSelectedGameObject.name;
		GO_Scale = EventSystem.current.currentSelectedGameObject.GetComponent<Image> ();

		switch (GO_Name) {
		case "Button_30s":
			GO_Scale.transform.localScale = new Vector2 (3.0f, 3.0f);
			break;
		case "Button_60s":
			GO_Scale.transform.localScale = new Vector2 (3.0f, 3.0f);
			break;
		case "Button_90s":
			GO_Scale.transform.localScale = new Vector2 (3.0f, 3.0f);
			break;
		case "Button_round1":
			GO_Scale.transform.localScale = new Vector2 (1.75f, 1.75f);
			break;
		case "Button_round2":
			GO_Scale.transform.localScale = new Vector2 (1.75f, 1.75f);
			break;
		case "Button_round3":
			GO_Scale.transform.localScale = new Vector2 (1.75f, 1.75f);
			break;
		case "plusObstacles":
			GO_Scale.transform.localScale = new Vector2 (5.0f, 5.0f);
			break;
		case "minusObstacles":
			GO_Scale.transform.localScale = new Vector2 (5.0f, 5.0f);
			break;
		}
	}

	public void SelectStageFunction(){
		GO_Name = EventSystem.current.currentSelectedGameObject.name;
		GO = EventSystem.current.currentSelectedGameObject;

		switch(GO_Name){
		case "stage_1":
			//set the description text 
			Description_Text.text = Description_String [0];

			//change the color of the selected rest to white, selected image to blue
			Component[] tempCol;

			tempCol = GameObject.Find ("Grid").GetComponentsInChildren<Image> ();
			foreach (Image ImageCol in tempCol) {
				ImageCol.color = Color.white;
			}
			tempCol = GO.GetComponentsInChildren<Image> ();
			foreach (Image ImageCol in tempCol) {
				ImageCol.color = color;
			}

			//Set the enlarge image to be the select image one
			DisplayEnlargeImage.sprite = GO.transform.Find("Image").GetComponent<Image>().sprite;
			break;
		case "stage_2":
			//set the description text 
			Description_Text.text = Description_String [1];
			//change the color of the selected rest to white, selected image to blue
			tempCol = GameObject.Find ("Grid").GetComponentsInChildren<Image> ();
			foreach (Image ImageCol in tempCol) {
				ImageCol.color = Color.white;
			}
			tempCol = GO.GetComponentsInChildren<Image> ();
			foreach (Image ImageCol in tempCol) {
				ImageCol.color = color;
			}

			//Set the enlarge image to be the select image one
			DisplayEnlargeImage.sprite = GO.transform.Find("Image").GetComponent<Image>().sprite;
			break;
		case "stage_3":
			//set the description text 
			Description_Text.text = Description_String [2];
			//change the color of the selected rest to white, selected image to blue
			tempCol = GameObject.Find ("Grid").GetComponentsInChildren<Image> ();
			foreach (Image ImageCol in tempCol) {
				ImageCol.color = Color.white;
			}
			tempCol = GO.GetComponentsInChildren<Image> ();
			foreach (Image ImageCol in tempCol) {
				ImageCol.color = color;
			}

			//Set the enlarge image to be the select image one
			DisplayEnlargeImage.sprite = GO.transform.Find("Image").GetComponent<Image>().sprite;
			break;
		case "stage_4":
			//set the description text 
			Description_Text.text = Description_String [3];
			//change the color of the selected rest to white, selected image to blue
			tempCol = GameObject.Find ("Grid").GetComponentsInChildren<Image> ();
			foreach (Image ImageCol in tempCol) {
				ImageCol.color = Color.white;
			}
			tempCol = GO.GetComponentsInChildren<Image> ();
			foreach (Image ImageCol in tempCol) {
				ImageCol.color = color;
			}

			//Set the enlarge image to be the select image one
			DisplayEnlargeImage.sprite = GO.transform.Find("Image").GetComponent<Image>().sprite;
			break;

		}
	}

	//Increase the amount of obstacles in the game
	public void Increment_Obstacles(){
		if (no_obstacles < max_Obstacle) {
			no_obstacles += 2;
		}
	}

	//Decrease the amount of obstacles in the game
	public void Decrement_Obstacles(){
		if (no_obstacles > 0) {
			no_obstacles -= 2;
		}
	}

	//function used to switch between panels
	public void EnterExitSettingsFunction ()
	{
		isPanelMoving = true;

		//Send settings to playerprefs
		PlayerPrefs.SetInt ("no_obstacles", no_obstacles);
		PlayerPrefs.SetInt ("time", time);
		PlayerPrefs.SetInt ("rounds", rounds);


	}

	public void DisableAllButtons(){
		buttons = GameObject.FindGameObjectsWithTag("Buttons");

		foreach (GameObject button in buttons) {
			button.GetComponent<Button> ().interactable = false;
		}
	}

	public void EnableAllButtons(){
		buttons = GameObject.FindGameObjectsWithTag("Buttons");

		foreach (GameObject button in buttons) {
			button.GetComponent<Button> ().interactable = true;
		}
	}
}
