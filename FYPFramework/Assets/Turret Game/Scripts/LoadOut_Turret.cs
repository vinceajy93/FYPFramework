using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadOut_Turret : MonoBehaviour {
	public enum Price_Property {
		Credit,
		Crystal
	};
	public Price_Property Price_Payment;
	public int Price;
	private int Price_Original;
	public int Price_Upgrade;

	private Text Credit;
	private Text Crystal;
	private Text Price_Text;
	private GameObject Price_Image;

	private Sprite Original;
	public Sprite Full;
	public Sprite Equiped;
	private Sprite Option_Image;

	// The current Equiped Image
	private Image Screen_Image;

	public bool Upgrade_Mode = false;
	private GameObject Equip_Button;
	private Text Equip_Text;
	private GameObject Upgrade_Button;
	private Text Upgrade_Text;
	private GameObject Buy_Button;
	private Text Buy_Text;

	public int Upgrade_Level;
	private int Max_Level = 5;

	public int Original_Stat;
	private Text Original_Stat_Text;
	private GameObject Upgrade_GO;
	public int Upgrade_Stat;
	private Text Upgrade_Stat_Text;

	private GameObject Stats;
	private Slider Stats_Slider;

	// Use this for initialization
	void Start () {
		Stats = GameObject.Find ("Script").GetComponent<LoadOut_Control> ().Stat;

		// Get Player's Credit & Crytstal
		Credit = GameObject.FindGameObjectWithTag ("Credit").GetComponentInChildren<Text> ();
		Crystal = GameObject.FindGameObjectWithTag ("Crystal").GetComponentInChildren<Text> ();

		// Check if Payment is Credit or Crystal
		if (Price_Payment == Price_Property.Credit) {
			this.transform.GetChild (3).GetChild (2).gameObject.SetActive (false);
			Price_Image = this.transform.GetChild (3).GetChild (1).gameObject;
		} else {
			this.transform.GetChild (3).GetChild (1).gameObject.SetActive (false);
			Price_Image = this.transform.GetChild (3).GetChild (2).gameObject;
		}

		if (LayerMask.LayerToName (this.gameObject.layer) == "Turret_Canvas") {
			Screen_Image = GameObject.FindGameObjectWithTag ("Loadout_Turret").GetComponentInChildren<Image> ();
			Option_Image = Resources.Load<Sprite> ("Turret/Sprite/" + this.name);
			Stats_Slider = Stats.transform.GetChild (0).GetComponentInChildren<Slider> ();
		} else if (LayerMask.LayerToName (this.gameObject.layer) == "Bullet_Canvas") {
			Screen_Image = GameObject.FindGameObjectWithTag ("Loadout_Bullet").GetComponentInChildren<Image> ();
			Option_Image = Resources.Load<Sprite> ("Bullet/Sprite/" + this.name);
			Stats_Slider = Stats.transform.GetChild (1).GetComponentInChildren<Slider> ();
		} else if (LayerMask.LayerToName (this.gameObject.layer) == "Base_Canvas") {
			Screen_Image = GameObject.FindGameObjectWithTag ("Loadout_Base").GetComponentInChildren<Image> ();
			Option_Image = Resources.Load<Sprite> ("Base/Sprite/" + this.name);
			Stats_Slider = Stats.transform.GetChild (2).GetComponentInChildren<Slider> ();
		}

		Equip_Button = this.transform.GetChild (2).GetChild (0).gameObject;
		Original = Equip_Button.GetComponent<Image> ().sprite;
		Equip_Text = Equip_Button.transform.GetComponentInChildren<Text> ();

		Upgrade_Button = this.transform.GetChild (2).GetChild (1).gameObject;
		Upgrade_Text = Upgrade_Button.transform.GetComponentInChildren<Text> ();

		Buy_Button = this.transform.GetChild (4).GetChild (0).gameObject;
		Buy_Text = Buy_Button.transform.GetComponentInChildren<Text> ();

		// Get the Price Tag and set to be shown on screen
		Price_Text = this.transform.GetChild (3).GetComponentInChildren<Text> ();

		if (PlayerPrefs.HasKey (this.name + "_L")) {
			Upgrade_Level = PlayerPrefs.GetInt(this.name + "_L");
			if (Upgrade_Level > 0)
				Upgrade_Mode = true;
		} else {
			PlayerPrefs.SetInt (this.name + "_L", Upgrade_Level);
		}

		if (Upgrade_Level >= Max_Level) {
			Upgrade_Text.text = "MAX";
			Upgrade_Text.transform.localScale = new Vector3 (0.4f, 0.4f, 1f);
			Upgrade_Button.GetComponent<Image> ().sprite = Full;
			Upgrade_Button.GetComponent<Button> ().interactable = false;

			Price_Image.SetActive (false);
			Price_Text.text = "";
		} else {

			Price_Original = Price;
			for (int i = 1; i <= Upgrade_Level; i++) {
				Price = Price_Original + Price_Upgrade;
				Price_Upgrade = Price_Upgrade + Price_Upgrade;
			}

			Price_Text.text = Price.ToString ();
		}

		Original_Stat_Text = this.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<Text>();

		Upgrade_GO = this.transform.GetChild (0).GetChild (2).gameObject;
		Upgrade_Stat_Text = Upgrade_GO.transform.GetChild(1).gameObject.GetComponent<Text>();

		if (Upgrade_Mode) {
			Buy_Button.SetActive (false);
	
			Original_Stat_Text.text = (Original_Stat + (Upgrade_Stat * (Upgrade_Level - 1))).ToString();
			Upgrade_Stat_Text.text = Upgrade_Stat.ToString();
		} else {
			Equip_Button.SetActive (false);
			Upgrade_Button.SetActive (false);

			Original_Stat_Text.text = Original_Stat.ToString();
			Upgrade_GO.SetActive (false);
			Upgrade_Stat_Text.text = "";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ( (Price_Payment == Price_Property.Credit && int.Parse (Credit.text) < Price) || (Price_Payment == Price_Property.Crystal && int.Parse (Crystal.text) < Price) ) {
			if (int.Parse (Credit.text) < Price && Upgrade_Level < Max_Level) {
				if (Upgrade_Mode) {
					Upgrade_Text.text = "POOR";
					Upgrade_Text.transform.localScale = new Vector3 (0.4f, 0.4f, 1f);
					Upgrade_Button.GetComponent<Image> ().sprite = Full;
					Upgrade_Button.GetComponent<Button> ().interactable = false;
				} else {
					Buy_Text.text = "POOR";
					Buy_Text.transform.localScale = new Vector3 (0.4f, 0.4f, 1f);
					Buy_Button.GetComponent<Image> ().sprite = Full;
					Buy_Button.GetComponent<Button> ().interactable = false;
				}
			}
		}

		if (Screen_Image.sprite == Option_Image) {
			Equip_Text.text = "Equiped";
			Equip_Text.transform.localScale = new Vector3 (0.25f, 0.3f, 1f);
			Equip_Button.GetComponent<Image> ().sprite = Equiped;
			Equip_Button.GetComponent<Button> ().interactable = false;

			int final_value = (int)Stats_Slider.maxValue - int.Parse (Original_Stat_Text.text);
			if (Stats_Slider.value != final_value) {
				if (Stats_Slider.value < final_value) {
					Stats_Slider.value += 1f;
				} else if (Stats_Slider.value > final_value) {
					Stats_Slider.value -= 1f;
				}
			}
		} else {
			Equip_Text.text = "Equip";
			Equip_Text.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			Equip_Button.GetComponent<Image> ().sprite = Original;
			Equip_Button.GetComponent<Button> ().interactable = true;
		}
	}

	public void Equip () {
		Screen_Image.sprite = Option_Image;

		if (LayerMask.LayerToName (this.gameObject.layer) == "Turret_Canvas") {
			PlayerPrefs.SetString ("S_T", this.name);
			PlayerPrefs.SetInt("S_Speed", int.Parse (Original_Stat_Text.text));
		} else if (LayerMask.LayerToName (this.gameObject.layer) == "Bullet_Canvas") {
			PlayerPrefs.SetString ("S_B", this.name);
			PlayerPrefs.SetInt("S_Damage", int.Parse (Original_Stat_Text.text));
		} else if (LayerMask.LayerToName (this.gameObject.layer) == "Base_Canvas") {
			PlayerPrefs.SetString ("S_H", this.name);
			PlayerPrefs.SetInt("S_Health", int.Parse (Original_Stat_Text.text));
		}
	}

	public void Upgrade () {
		// Money System
		if (Price_Payment == Price_Property.Credit) {
			Credit.text = (int.Parse (Credit.text) - Price).ToString ();
		} else {
			Crystal.text = (int.Parse (Crystal.text) - Price).ToString ();
		}

		Upgrade_Level++;
		Price = Price_Original + Price_Upgrade;
		Price_Upgrade = Price_Upgrade + Price_Upgrade;
		Price_Text.text = Price.ToString ();
		Original_Stat_Text.text = (Original_Stat + (Upgrade_Stat * (Upgrade_Level - 1))).ToString();

		// Check if Upgrade level is max out
		if (Upgrade_Level >= Max_Level) {
			Upgrade_Text.text = "MAX";
			Upgrade_Text.transform.localScale = new Vector3 (0.4f, 0.4f, 1f);
			Upgrade_Button.GetComponent<Image> ().sprite = Full;
			Upgrade_Button.GetComponent<Button> ().interactable = false;

			Price_Image.SetActive (false);
			Price_Text.text = "";

			Upgrade_GO.SetActive (false);
			Upgrade_Stat_Text.text = "";
		}
		PlayerPrefs.SetInt (this.name + "_L", Upgrade_Level);
		PlayerPrefs.SetInt ("Credit", int.Parse (Credit.text));
		PlayerPrefs.SetInt ("Crystal", int.Parse (Crystal.text));
	}

	public void Buy () {
		// Money System
		if (Price_Payment == Price_Property.Credit) {
			Credit.text = (int.Parse (Credit.text) - Price).ToString ();
		} else {
			Crystal.text = (int.Parse (Crystal.text) - Price).ToString ();
		}

		Upgrade_Mode = true;
		Buy_Button.SetActive (false);
		Equip_Button.SetActive (true);
		Upgrade_Button.SetActive (true);
		Upgrade_GO.SetActive (true);

		Upgrade_Level++;
		Price = Price_Original + Price_Upgrade;
		Price_Upgrade = Price_Upgrade + Price_Upgrade;
		Price_Text.text = Price.ToString ();
		Upgrade_Stat_Text.text = Upgrade_Stat.ToString();
		PlayerPrefs.SetInt (this.name + "_L", Upgrade_Level);
		PlayerPrefs.SetInt ("Credit", int.Parse (Credit.text));
		PlayerPrefs.SetInt ("Crystal", int.Parse (Crystal.text));
	}
}