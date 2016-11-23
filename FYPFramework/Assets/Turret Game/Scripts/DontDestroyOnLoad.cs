using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour {

	public static DontDestroyOnLoad instance;

	Scene _Scene;

	//Awake
	void Awake(){
		if (instance != null) {
			if (instance != this) {
				Destroy (this.gameObject); //change to remove component if not other scripts will be destroyed as well
			}
		} else {
			instance = this;
			DontDestroyOnLoad (this);
		}
	}
	void Update(){
		_Scene = SceneManager.GetActiveScene();

		if (_Scene.name == "Loadout Select")
			this.gameObject.transform.localScale = Vector3.one;
		else
			this.gameObject.transform.localScale = Vector3.zero;
	}
}
