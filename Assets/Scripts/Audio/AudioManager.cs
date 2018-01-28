using UnityEngine.Audio;
using UnityEngine;
using System;


public class AudioManager : MonoBehaviour {

	public Sound [] sounds;

	public static AudioManager instance;

	// Use this for initialization
	void Awake () {

		//Destroy the copy of AudioManger if their more than on in a same scene
		if (instance == null)
			instance = this;
		else {
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);

		foreach (Sound s in sounds) 
		{
			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}

	}
		

	//void Start ()
	//{
		//Play ("Theme");
	//}

	//Search for the sound name and play it
	public void Play (string name)
	{
		Sound s = Array.Find (sounds, sound => sound.name == name);
		if (s == null)
			//If there a spelling mistake the debug log will tell you witch one is
			Debug.LogWarning ("Sound: " + name + " not found");
			return;
		s.source.Play();

		//To call sound function
		//FindObjectOfType<AudioManager>().Play("NameOfTheClip");
	}
}
