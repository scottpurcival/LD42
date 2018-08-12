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
    public float parTime = 120;
    float playTime;
    public GameObject gcanFlare;
    public Whoosh whooshText;
    public PlaySoundTrigger allFilesCollectedTrigger;
    public PlaySoundTrigger oneFileCollectedTrigger;
    public PlaySoundTrigger deathTrigger;
    ShutdownPanel shutDown;
    GameObject popupContainer;
    public GameObject popUp;
    bool popUpsFired = false;

	// Use this for initialization
	void Start ()
    {
        GetAllFiles();
        UpdateFileCount();
        gcanFlare.SetActive(false);
        whooshText.WhooshNow("DESTROY ALL FILES!!!!");
        shutDown = GameObject.FindGameObjectWithTag("ShutDown").GetComponent<ShutdownPanel>();
        playTime = parTime;
        popupContainer = GameObject.FindGameObjectWithTag("PopUpContainer");
    }
	
	// Update is called once per frame
	void Update ()
    {
        playTime -= Time.deltaTime;
        UpdateTime();

        if (Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }
	}

    public void TogglePause()
    {
        shutDown.TogglePause();
    }

    void UpdateTime()
    {
        int minutes = (int)(Mathf.Abs(playTime) / 60.0f);
        int seconds = (int)(Mathf.Abs(playTime) - (minutes * 60));
        int millisecs = (int)((Mathf.Abs(playTime) - (minutes * 60) - (seconds)) * 100.0f);
        Int32.TryParse(millisecs.ToString().PadLeft(2,'0').Substring(0, 2), out millisecs);
        timer.text = ((playTime < 0) ? "-" : "") + minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0') + ":" + millisecs.ToString().PadLeft(2, '0');
        if (playTime < 0)
            timer.color = Color.red;

        if (playTime < 0 && !popUpsFired)
            StartCoroutine(PopUps());

    }

    IEnumerator PopUps()
    {
        popUpsFired = true;
        while(true)
        {
            // create popup
            GameObject temp = Instantiate(popUp, popupContainer.transform, false);
            Vector2 screensize = new Vector2(Screen.width, Screen.height);
            Vector2 spawnPos = new Vector2(UnityEngine.Random.Range(screensize.x*0.15f, screensize.x * 0.85f), UnityEngine.Random.Range(screensize.y * 0.15f, screensize.y * 0.85f));
            temp.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(10.0f);
        }
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
        if(filesCollected < filesInScene)
            oneFileCollectedTrigger.PlaySound();
    }

    public bool AreAllFilesCollected()
    {
        return (filesCollected >= filesInScene);
    }

    void UpdateFileCount()
    {
        fileCounter.text = filesCollected.ToString().PadLeft(2, '0') + " / " + filesInScene.ToString().PadLeft(2, '0');

        if (filesCollected >= filesInScene)
        {
            gcanFlare.GetComponent<Wiggle>().enabled = true;
            gcanFlare.SetActive(true);
            whooshText.WhooshNow("CHUCK EM IN THE TRAAAAASH!!!");
            allFilesCollectedTrigger.PlaySound();
        }
    }

    public void PlayerExit()
    {
        // player has entered the exit, can they leave?
        if(filesCollected >= filesInScene)
        {
            whooshText.WhooshNow("HECK YEA BOI!!!!!\n\nYOU WIN!");
            StartCoroutine(EndGame(true));
        }
    }

    public void Die()
    {
        whooshText.WhooshNow("YOU DIED!!");
        deathTrigger.PlaySound();
        StartCoroutine(EndGame(false));
    }

    IEnumerator EndGame(bool didWin)
    {
        while (allFilesCollectedTrigger.isPlaying())
        { yield return null; } // wait 

        yield return new WaitForSeconds(4.0f);

        if (didWin)
        {
            GetComponent<NextScene>().GoNextScene();
            Debug.Log("You Win!");
        }
        else
        {
            GetComponent<NextScene>().RestartLevel();
            Debug.Log("You LOSE!");
        }
        yield break;
    }

}
