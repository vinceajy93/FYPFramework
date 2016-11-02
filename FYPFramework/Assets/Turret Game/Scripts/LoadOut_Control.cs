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

	private Tab Active_Tab = Tab.Card_Canvas;

	private GameObject[] All_Canvas;
	private GameObject[] All_Tab;
	private GameObject[] All_Background;
	private Color32 Color_hidden = new Color32 (255, 255, 255, 100);
	private Color Color_show = new Color (1f, 1f, 1f, 1f);

	// Use this for initialization
	void Start () {
		All_Canvas = GameObject.FindGameObjectsWithTag ("Canvas");
		All_Tab = GameObject.FindGameObjectsWithTag ("Loadout_Tab");
		All_Background = GameObject.FindGameObjectsWithTag ("Background");

		Switch_Tab ( Active_Tab.ToString() );
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
		foreach (GameObject ea_Tab in All_Tab) {
			if (LayerMask.LayerToName (ea_Tab.layer) == name) {
				ea_Tab.GetComponent<Image> ().color = Color_show;
			} else {
				ea_Tab.GetComponent<Image> ().color = Color_hidden;
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
	}
}