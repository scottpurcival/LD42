using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutdownPanel : MonoBehaviour {

    GameManager gm;
    GameObject panel;
    AudioSource clickSound;

	// Use this for initialization
	void Start ()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        clickSound = GetComponent<AudioSource>();
        panel = gameObject.transform.GetChild(0).gameObject;
        panel.SetActive(false);
	}
	
    public void TogglePause()
    {
        if (panel.activeSelf)
        {
            Time.timeScale = 1;
            panel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            panel.SetActive(true);
        }
    }

    public void PlaySound()
    {
        clickSound.Play();
    }
}
