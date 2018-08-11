using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text fileCounter;
    public int filesInScene = 0;
    public int filesCollected = 0;


	// Use this for initialization
	void Start ()
    {
        GetAllFiles();
        UpdateFileCount();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
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
