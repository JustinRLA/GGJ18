using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RandomFX_Scream : MonoBehaviour {

	public AudioClip[] clips;
	public AudioMixerGroup output;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    float TIMER;
	// Need to call this funciton to randomize
	public void PlayFootstepStone(){
	if(Time.time < TIMER) { return; }
		// Randomize
		int randcomClip = Random.Range (0, clips.Length);

		// Create an AudioSource
		AudioSource source = gameObject.AddComponent<AudioSource>();

		// Load a Clip into the AudioSource
		source.clip = clips[randcomClip];

		// Set tje output for AudioSource
		source.outputAudioMixerGroup = output;

		// Play the clip
		source.Play();

		// Destroy the AudioSource when done playing clip
		Destroy(source, clips[randcomClip].length);

        TIMER = Time.time + Random.Range(0.5f, 1f);
    }







}
