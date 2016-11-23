﻿using UnityEngine;

[System.Serializable]
public class Sound {
	public string name;
	public AudioClip clip;
	public bool loop = false;

	[Range(0f, 1f)]
	public float volume = 1f;
	[Range(0f, 1f)]
	public float pitch = 1f;

	[Range(0f, 0.5f)]
	public float randomVolume = 0.1f;
	[Range(0f, 0.5f)]
	public float randomPitch = 0.1f;

	private AudioSource source;

	public void SetSource(AudioSource _source){
		source = _source;
		source.clip = clip;
		source.loop = loop;
	}

	public void Play(){
		source.volume = volume * (1 + Random.Range (-randomVolume / 2f, randomVolume / 2f));
		source.pitch = pitch * (1 + Random.Range (-randomPitch / 2f, randomPitch / 2f));
		source.Play ();

	}

	public void Stop(){
		source.Stop ();
	}
		
}

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	[SerializeField]
	Sound[] sounds;

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

	void Start(){
		for(int i = 0; i < sounds.Length; i ++){
			GameObject _go = new GameObject ("Sound_" + i + "_" + sounds[i].name);
			_go.transform.SetParent (this.transform);
			sounds[i].SetSource (_go.AddComponent<AudioSource> ());

		}
	}

	public void PlaySound(string _name){
		for(int i = 0; i < sounds.Length; i++){
			if (sounds [i].name == _name) {
				sounds [i].Play ();
				return;
			}
		}

		//return warning if no sounds with _name
		Debug.LogWarning("SoundManager: Sound not found in list: " + _name);
	}

	public void StopSound(string _name){
		for(int i = 0; i < sounds.Length; i++){
			if (sounds [i].name == _name) {
				sounds [i].Stop ();
				return;
			}
		}

		//return warning if no sounds with _name
		Debug.LogWarning("SoundManager: Sound not found in list: " + _name);
	}



	//for debugging, remove when done
	void Update(){

		if (Time.time > 5.0f) {
			StopSound ("test");
		}
		if(Input.GetKeyDown(KeyCode.W)){
			PlaySound ("test");
		}

	}
		
}
