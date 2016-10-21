using UnityEngine;
using System.Collections;

public class OS_Checker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
		Debug.Log("Android code here");
		#endif

		#if UNITY_IOS
		Debug.Log("IOS code here");
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
