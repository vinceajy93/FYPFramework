using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModeAnimation : MonoBehaviour
{

	[SerializeField]
	private Button mode_button1, mode_button2;

	private float upperTreshHold, lowerTreshhold, offset;
	private float currentTimePassed;

	private bool isTop = false;
	[SerializeField]
	private Animator Anim;

	// Use this for initialization
	void Start ()
	{
		//variables for the moving of the buttons
		offset = 0.2f;
		currentTimePassed = 0.0f;
		upperTreshHold = 0.5f;
		lowerTreshhold = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (currentTimePassed <= upperTreshHold && isTop == false) {
			currentTimePassed += Time.deltaTime;
			mode_button1.transform.Translate (0, offset, 0);
			mode_button2.transform.Translate (0, offset, 0);
			if (currentTimePassed >= upperTreshHold)
				isTop = true;
		}

		if (currentTimePassed >= lowerTreshhold && isTop == true) {
			currentTimePassed -= Time.deltaTime;
			mode_button1.transform.Translate (0, -offset, 0);
			mode_button2.transform.Translate (0, -offset, 0);
			if (currentTimePassed <= lowerTreshhold)
				isTop = false;
				
		}
	}
}
