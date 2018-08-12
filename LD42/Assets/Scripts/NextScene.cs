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

    IEnumerator WaitForSoundMenu()
    {
        while (audio.isPlaying)
        {
            yield return null;
        }

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
}
