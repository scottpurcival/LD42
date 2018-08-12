using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour{

    AudioSource audio;

    public void GoToNextSceneAfterAudio()
    {
        audio = GetComponent<AudioSource>();

        if (audio)
            StartCoroutine(WaitForSound());
        else
            GoNextScene();

    }

    public void RestartSceneAfterAudio()
    {
        audio = GetComponent<AudioSource>();

        if (audio)
            StartCoroutine(WaitForSoundRestart());
        else
            RestartLevel();

    }

    public void GoToMenuAfterAudio()
    {
        audio = GetComponent<AudioSource>();

        if (audio)
            StartCoroutine(WaitForSoundMenu());
        else
            GoToMenu();

    }

    IEnumerator WaitForSound()
    {
        while(audio.isPlaying)
        {
            yield return null;
        }

        GoNextScene();
    }

    IEnumerator WaitForSoundRestart()
    {
        while (audio.isPlaying)
        {
            yield return null;
        }

        RestartLevel();
    }

    IEnumerator WaitForSoundMenu()
    {
        while (audio.isPlaying)
        {
            yield return null;
        }

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().TogglePause();
        GoToMenu();
    }

    public void GoNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene >= SceneManager.sceneCountInBuildSettings)
            nextScene = 0;

        SceneManager.LoadSceneAsync(nextScene);
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void TogglePause()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().TogglePause();
    }
}
