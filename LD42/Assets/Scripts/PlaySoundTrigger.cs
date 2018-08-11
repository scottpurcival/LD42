using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundTrigger : MonoBehaviour {

    AudioSource audioSource;
    public AudioClip[] sounds;
    bool hasPlayed = false;
    public bool playOnceOnly = false;

	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if(!(hasPlayed && playOnceOnly))
            audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length-1)]);

        hasPlayed = true;
    }
}
