using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text fileCounter;
    public Text timer;
    public int filesInScene = 0;
    public int filesCollected = 0;
    public Vector3 parTime;
    float playTime;
    GameObject gcanFlare;


	// Use this for initialization
	void Start ()
    {
        GetAllFiles();
        UpdateFileCount();
        gcanFlare = GameObject.FindGameObjectWithTag("GarbageCanFlare");
        gcanFlare.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        playTime += Time.deltaTime;
        UpdateTime();
	}

    void UpdateTime()
    {
        int minutes = (int)Mathf.Abs(playTime / 60.0f);
        int seconds = (int)Mathf.Abs((playTime - (minutes * 60)));
        int millisecs = (int)Mathf.Abs((playTime - (minutes * 60) - (seconds) * 100));
        Int32.TryParse(millisecs.ToString().PadLeft(2,'0').Substring(0, 2), out millisecs);
        timer.text = minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0') + ":" + millisecs.ToString().PadLeft(2, '0');
    }

    void GetAllFiles()
    {
        GameObject[] files = GameObject.FindGameObjectsWithTag("FileToClear");
        filesInScene = files.Length;

    }

    public void CollectFile()
    {
        filesCollected++;
        UpdateFileCount();
    }

    void UpdateFileCount()
    {
        fileCounter.text = filesCollected.ToString().PadLeft(2, '0') + " / " + filesInScene.ToString().PadLeft(2, '0');
        if (filesCollected >= filesInScene)
        {
            GameObject.FindGameObjectWithTag("GarbageCan").GetComponentInChildren<Wiggle>().enabled = true;
            gcanFlare.SetActive(true);
        }
    }

    public void PlayerExit()
    {
        // player has entered the exit, can they leave?
        if(filesCollected >= filesInScene)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("You Win!");
    }
}
