using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundTrigger : MonoBehaviour {

    AudioSource audioSource;
    public AudioClip[] sounds;
    bool hasPlayed = false;
    public bool playOnceOnly = false;
    public bool autoFire = false;
    public float autoFireRate = 5.0f;
    public bool playSequentially = false;
    int soundclip = 0;

	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        if (autoFire)
            StartCoroutine(AutoFireSounds(autoFireRate));
    }

    public void PlaySound()
    {
        if (!(hasPlayed && playOnceOnly))
        {
            if(!playSequentially)
                audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length - 1)]);
            else
            {
                audioSource.PlayOneShot(sounds[soundclip]);
                soundclip++;
                if (soundclip >= sounds.Length)
                    soundclip = 0;
            }
        }
        hasPlayed = true;
    }

    public bool isPlaying()
    {
        return audioSource.isPlaying;
    }

    IEnumerator AutoFireSounds(float fireRate)
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            PlaySound();
        }
    }
}
