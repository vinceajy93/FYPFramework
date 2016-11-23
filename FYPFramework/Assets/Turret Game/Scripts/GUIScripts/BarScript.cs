using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarScript : MonoBehaviour {

	private float fillAmount;

	[SerializeField]
	private float lerpSpeed;
	[SerializeField]
	private Image content;
    [SerializeField]
    private Text valueText;
	public float MaxValue { get; set; }

	private Stat Stat;

	public float Value {
		set {
            //store the string before colon into a temp string array
            //string[] tmp = valueText.text.Split(':');
			if(valueText != null)
				valueText.text = value.ToString();//tmp[0] + ": " +  value;
			fillAmount = Map (value, 0, MaxValue);
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (fillAmount != content.fillAmount) {
			if (lerpSpeed == 0f)
				content.fillAmount = fillAmount;
			else
				HandleBar ();
		}
	}

	void HandleBar(){
		content.fillAmount = Mathf.Lerp (content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
	}

	float Map(float value, float inMin, float inMax){
		//currentHP / MaxHP
		return value / inMax;

	}
}
