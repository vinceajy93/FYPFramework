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
	private Sprite Turret_Image;

	// The current Equiped Turret
	private Image Screen_Turret;

	public bool Upgrade_Mode = false;
	private GameObject Equip_Button;
	private Text Equip_Text;
	private GameObject Upgrade_Button;
	private Text Upgrade_Text;
	private GameObject Buy_Button;
	private Text Buy_Text;

	public int Upgrade_Level;

	public int Original_Stat;
	private Text Original_Stat_Text;
	public GameObject Upgrade_GO;
	public int Upgrade_Stat;
	private Text Upgrade_Stat_Text;

	// Use this for initialization
	void Start () {
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
			
		Screen_Turret = GameObject.FindGameObjectWithTag ("Loadout_Turret").GetComponentInChildren<Image> ();
		Turret_Image = Resources.Load<Sprite> ("Turret/Sprite/" + this.name);

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
			Upgrade_Mode = true;
		}

		Upgrade_Level = PlayerPrefs.GetInt(this.name + "_L", Upgrade_Level);
		if (Upgrade_Level >= 5) {
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
			if (int.Parse (Credit.text) < Price) {
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

		if (Screen_Turret.sprite == Turret_Image) {
			Equip_Text.text = "Equiped";
			Equip_Text.transform.localScale = new Vector3 (0.25f, 0.3f, 1f);
			Equip_Button.GetComponent<Image> ().sprite = Equiped;
			Equip_Button.GetComponent<Button> ().interactable = false;
		} else {
			Equip_Text.text = "Equip";
			Equip_Text.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			Equip_Button.GetComponent<Image> ().sprite = Original;
			Equip_Button.GetComponent<Button> ().interactable = true;
		}
	}

	public void Equip () {
		Screen_Turret.sprite = Turret_Image;
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
		if (Upgrade_Level >= 5) {
			Upgrade_Text.text = "MAX";
			Upgrade_Text.transform.localScale = new Vector3 (0.4f, 0.4f, 1f);
			Upgrade_Button.GetComponent<Image> ().sprite = Full;
			Upgrade_Button.GetComponent<Button> ().interactable = false;

			Price_Image.SetActive (false);
			Price_Text.text = "";

			Upgrade_GO.SetActive (false);
			Upgrade_Stat_Text.text = "";
		}
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
	}
}