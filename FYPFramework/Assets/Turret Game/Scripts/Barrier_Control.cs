using UnityEngine;
using System.Collections;

public class Barrier_Control : MonoBehaviour {
	private PauseScript _pauseScript;

	private GameObject child;

	private float duration = 2.0f;
	private float delTime = 0f;

	// Use this for initialization
	void Start () {
		_pauseScript = GameObject.Find ("Scripts").GetComponent<PauseScript> ();
		child = this.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (!_pauseScript.Paused && child.activeSelf) {
			delTime += Time.deltaTime;
			if (delTime > duration) {
				delTime = 0;
				child.SetActive (false);
			}
		}
	}

	public void SetBarrier (bool set)
	{
		child.SetActive(true);
	}
}