using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadOut_Control : MonoBehaviour {
	enum Tab {
		Turret_Canvas,
		Bullet_Canvas,
		Base_Canvas,
		Card_Canvas
	};

	private Tab Active_Tab = Tab.Turret_Canvas;

	private GameObject[] All_Canvas;
	private GameObject[] All_Tab;
	private GameObject[] All_Background;
	private GameObject[] All_Panel;
	public GameObject Stat;

	private Color Color_hidden = new Color (1, 1, 1, 0);
	private Color Color_show = new Color (1, 1, 1, 1);

	private Text Credit;
	private Text Crystal;

	private Image Turret;
	private Image Bullet;
	private Image Hover;

	private GameObject Card_Left;
	private GameObject Card_Middle;
	private GameObject Card_Right;

	// Use this for initialization
	void Start () {
		All_Canvas = GameObject.FindGameObjectsWithTag ("Canvas");
		All_Tab = GameObject.FindGameObjectsWithTag ("Loadout_Tab");
		All_Background = GameObject.FindGameObjectsWithTag ("Background");
		All_Panel = GameObject.FindGameObjectsWithTag ("Loadout_Panel");
		Stat = GameObject.FindGameObjectWithTag ("Loadout_Stats").transform.GetChild(0).gameObject;

		Switch_Tab ( Active_Tab.ToString() );

		Credit = GameObject.FindGameObjectWithTag ("Credit").GetComponentInChildren<Text> ();
		Crystal = GameObject.FindGameObjectWithTag ("Crystal").GetComponentInChildren<Text> ();

		Credit.text = PlayerPrefs.GetInt ("Credit", 99999).ToString();
		Crystal.text = PlayerPrefs.GetInt ("Crystal", 99999).ToString();

		GameObject Turret_Parent = GameObject.FindGameObjectWithTag ("Loadout_Turret");
		Turret = Turret_Parent.transform.GetChild (0).GetComponentInChildren<Image> ();

		GameObject Bullet_Parent = GameObject.FindGameObjectWithTag ("Loadout_Bullet");
		Bullet = Bullet_Parent.transform.GetChild (0).GetComponentInChildren<Image> ();

		GameObject Hover_Parent = GameObject.FindGameObjectWithTag ("Loadout_Base");
		Hover = Hover_Parent.transform.GetChild (0).GetComponentInChildren<Image> ();

		if (PlayerPrefs.HasKey ("S_T")) {
			Turret.sprite = Resources.Load<Sprite> ("Turret/Sprite/" + PlayerPrefs.GetString ("S_T"));
		}

		if (PlayerPrefs.HasKey ("S_B")) {
			Bullet.sprite = Resources.Load<Sprite> ("Bullet/Sprite/" + PlayerPrefs.GetString ("S_B"));
		}

		if (PlayerPrefs.HasKey ("S_H")) {
			Hover.sprite = Resources.Load<Sprite> ("Base/Sprite/" + PlayerPrefs.GetString ("S_H"));
		}

		GameObject Card_Button_Parent = GameObject.FindGameObjectWithTag ("Card_P1");
		Card_Left = Card_Button_Parent.transform.GetChild (3).gameObject;
		Card_Middle = Card_Button_Parent.transform.GetChild (4).gameObject;
		Card_Right = Card_Button_Parent.transform.GetChild (5).gameObject;

		if (PlayerPrefs.HasKey ("Card_Left") && PlayerPrefs.GetString ("Card_Left") != "") {
			Card_Left.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Card/Sprite/" + PlayerPrefs.GetString ("Card_Left"));
			Card_Left.GetComponent<Image> ().color = Color_show;
		} else {
			Card_Left.GetComponent<Image> ().sprite = null;
			Card_Left.GetComponent<Image> ().color = Color_hidden;
		}

		if (PlayerPrefs.HasKey ("Card_Middle") && PlayerPrefs.GetString ("Card_Middle") != "") {
			Card_Middle.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Card/Sprite/" + PlayerPrefs.GetString ("Card_Middle"));
			Card_Middle.GetComponent<Image> ().color = Color_show;
		} else {
			Card_Middle.GetComponent<Image> ().sprite = null;
			Card_Middle.GetComponent<Image> ().color = Color_hidden;
		}

		if (PlayerPrefs.HasKey ("Card_Right") && PlayerPrefs.GetString ("Card_Right") != "") {
			Card_Right.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Card/Sprite/" + PlayerPrefs.GetString ("Card_Right"));
			Card_Right.GetComponent<Image> ().color = Color_show;
		} else {
			Card_Right.GetComponent<Image> ().sprite = null;
			Card_Right.GetComponent<Image> ().color = Color_hidden;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Switch_Tab (Button button) {
		// If Button Pressed is not Active_Tab (Current Tab), switch to that tab
		if (LayerMask.LayerToName (button.gameObject.layer) == "Turret_Canvas") {
			if (Active_Tab != Tab.Turret_Canvas) {
				Active_Tab = Tab.Turret_Canvas;
				Switch_Tab ( Active_Tab.ToString() );
			}
		} else if (LayerMask.LayerToName (button.gameObject.layer) == "Bullet_Canvas") {
			if (Active_Tab != Tab.Bullet_Canvas) {
				Active_Tab = Tab.Bullet_Canvas;
				Switch_Tab ( Active_Tab.ToString() );
			}
		} else if (LayerMask.LayerToName (button.gameObject.layer) == "Base_Canvas") {
			if (Active_Tab != Tab.Base_Canvas) {
				Active_Tab = Tab.Base_Canvas;
				Switch_Tab ( Active_Tab.ToString() );
			}
		} else if (LayerMask.LayerToName (button.gameObject.layer) == "Card_Canvas") {
			if(Active_Tab != Tab.Card_Canvas) {
				Active_Tab = Tab.Card_Canvas;
				Switch_Tab ( Active_Tab.ToString() );
			}
		}
	}

	void Switch_Tab (string name) {
		// Active Tab : Turret Tab, Others Hide
		foreach (GameObject ea_canvas in All_Canvas) {
			if (LayerMask.LayerToName (ea_canvas.layer) == name) {
				ea_canvas.GetComponent<Canvas> ().sortingOrder = 10;
			} else {
				ea_canvas.GetComponent<Canvas> ().sortingOrder = 1;
			}
		}

		// ^^^^ Same Process
		foreach (GameObject ea_Bg in All_Background) {
			if (LayerMask.LayerToName (ea_Bg.layer) == name) {
				ea_Bg.SetActive (true);
			} else {
				ea_Bg.SetActive (false);
			}
		}

		foreach (GameObject ea_panel in All_Panel) {
			ea_panel.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1;
		}

		if (name == "Card_Canvas") {
			Stat.SetActive (false);
		} else {
			Stat.SetActive (true);
		}
	}

	public void UnEquip (Button button) {
		button.gameObject.GetComponent<Image> ().sprite = null;
		button.gameObject.GetComponent<Image> ().color = Color_hidden;
	}
}