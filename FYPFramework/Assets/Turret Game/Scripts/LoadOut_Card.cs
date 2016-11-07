using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadOut_Card : MonoBehaviour {

	public enum Card_Property {
		Card_GhostBullet,
		Card_Barrier,
		Card_Health,
		Card_FireRate,
		Card_BulletSpeed
	};
	public Card_Property card_ability;

	public enum Price_Property {
		Credit,
		Crystal
	};
	public Price_Property Price_Payment;
	public int Price;

	private GameObject Card_Left;
	private GameObject Card_Middle;
	private GameObject Card_Right;

	private Text Credit;
	private Text Crystal;
	private Text Price_Text;

	private Color shown = new Color(1, 1, 1, 1);

	private Sprite Original;
	public Sprite Full;
	private Sprite Card_Image;

	private Text Amount_Card;
	private GameObject Equip_Button;
	private Text Equip_Text;
	private GameObject Purchase_Button;
	private Text Purchase_Text;

	// Use this for initialization
	void Start () {
		GameObject Card_Button_Parent = GameObject.FindGameObjectWithTag ("Card_P1");
		Card_Left = Card_Button_Parent.transform.GetChild (3).gameObject;
		Card_Middle = Card_Button_Parent.transform.GetChild (4).gameObject;
		Card_Right = Card_Button_Parent.transform.GetChild (5).gameObject;

		Card_Image = Resources.Load<Sprite> ("Card/Sprite/" + card_ability.ToString ());

		// Get Player's Credit & Crytstal
		Credit = GameObject.FindGameObjectWithTag ("Credit").GetComponentInChildren<Text> ();
		Crystal = GameObject.FindGameObjectWithTag ("Crystal").GetComponentInChildren<Text> ();

		// Check if Payment is Credit or Crystal
		if (Price_Payment == Price_Property.Credit)
			this.transform.GetChild (3).GetChild (2).gameObject.SetActive (false);
		else
			this.transform.GetChild (3).GetChild (1).gameObject.SetActive (false);

		// Get the Price Tag and set to be shown on screen
		Price_Text = this.transform.GetChild (3).GetComponentInChildren<Text> ();
		Price_Text.text = Price.ToString ();

		// Get the amount of this card on the screen
		Amount_Card = this.transform.GetChild (1).GetComponentInChildren<Text> ();
		if (PlayerPrefs.HasKey (card_ability.ToString ())) {
			Amount_Card.text = PlayerPrefs.GetInt (card_ability.ToString ()).ToString ();

			if (int.Parse (Amount_Card.text) < 10) {
				Amount_Card.text = "0" + Amount_Card.text;
			}
		}
			
		Equip_Button = this.transform.GetChild (2).GetChild (0).gameObject;
		Equip_Text = Equip_Button.transform.GetComponentInChildren<Text> ();
		Original = Equip_Button.GetComponent<Image> ().sprite;
		if (Card_Left.GetComponent<Image> ().sprite != null && Card_Middle.GetComponent<Image> ().sprite != null && Card_Right.GetComponent<Image> ().sprite != null) {
			Equip_Text.text = "FULL";
			Equip_Button.GetComponent<Image> ().sprite = Full;
			Equip_Button.GetComponent<Button> ().interactable = false;
		}

		Purchase_Button = this.transform.GetChild (2).GetChild (1).gameObject;
		Purchase_Text = Purchase_Button.transform.GetComponentInChildren<Text> ();
		if (int.Parse (Amount_Card.text) >= 99) {
			Purchase_Text.text = "FULL";
			Purchase_Button.GetComponent<Image> ().sprite = Full;
			Purchase_Button.GetComponent<Button> ().interactable = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Compare in Credit System if Price is in Credit
		if ( (Price_Payment == Price_Property.Credit && int.Parse (Credit.text) < Price) || (Price_Payment == Price_Property.Crystal && int.Parse (Crystal.text) < Price) ) {
			if (int.Parse (Credit.text) < Price) {
				Purchase_Text.text = "POOR";
				Purchase_Button.GetComponent<Image> ().sprite = Full;
				Purchase_Button.GetComponent<Button> ().interactable = false;
			}
		}


		if (int.Parse (Amount_Card.text) == 0) {
			Equip_Text.text = "EMPTY";
			Equip_Button.GetComponent<Image> ().sprite = Full;
			Equip_Button.GetComponent<Button> ().interactable = false;
		}
		else if (Card_Left.GetComponent<Image> ().sprite != null && Card_Middle.GetComponent<Image> ().sprite != null && Card_Right.GetComponent<Image> ().sprite != null) {
			Equip_Text.text = "FULL";
			Equip_Button.GetComponent<Image> ().sprite = Full;
			Equip_Button.GetComponent<Button> ().interactable = false;
		} else if (int.Parse (Amount_Card.text) > 3) {
			Equip_Text.text = "EQUIP";
			Equip_Button.GetComponent<Image> ().sprite = Original;
			Equip_Button.GetComponent<Button> ().interactable = true;
		} else {
			int current_amount = int.Parse (Amount_Card.text);
			if (Card_Left.GetComponent<Image> ().sprite == Card_Image)
				current_amount--;

			if (Card_Middle.GetComponent<Image> ().sprite == Card_Image)
				current_amount--;

			if (Card_Right.GetComponent<Image>().sprite == Card_Image)
				current_amount--;

			if (current_amount <= 0) {
				Equip_Text.text = "EMPTY";
				Equip_Button.GetComponent<Image> ().sprite = Full;
				Equip_Button.GetComponent<Button> ().interactable = false;
			} else {
				Equip_Text.text = "EQUIP";
				Equip_Button.GetComponent<Image> ().sprite = Original;
				Equip_Button.GetComponent<Button> ().interactable = true;
			}
		}
	}

	public void Equip () {
		if (Card_Left.GetComponent<Image> ().sprite == null) {
			Card_Left.GetComponent<Image> ().sprite = Card_Image;
			Card_Left.GetComponent<Image> ().color = shown;
		} else if (Card_Middle.GetComponent<Image> ().sprite == null) {
			Card_Middle.GetComponent<Image> ().sprite = Card_Image;
			Card_Middle.GetComponent<Image> ().color = shown;
		} else if (Card_Right.GetComponent<Image> ().sprite == null) {
			Card_Right.GetComponent<Image> ().sprite = Card_Image;
			Card_Right.GetComponent<Image> ().color = shown;
		} else {
			Debug.Log ("Unable to Equip");
		}
	}

	public void Buy () {
		// Card System
		Amount_Card.text = (int.Parse (Amount_Card.text) + 1).ToString();

		if (int.Parse (Amount_Card.text) < 10) {
			Amount_Card.text = "0" + Amount_Card.text;
		}

		if (int.Parse (Amount_Card.text) >= 99) {
			Purchase_Text.text = "FULL";
			Purchase_Button.GetComponent<Image> ().sprite = Full;
			Purchase_Button.GetComponent<Button> ().interactable = false;
		}

		// Money System
		if (Price_Payment == Price_Property.Credit) {
			Credit.text = (int.Parse (Credit.text) - Price).ToString ();
		} else {
			Crystal.text = (int.Parse (Crystal.text) - Price).ToString ();
		}
	}
}