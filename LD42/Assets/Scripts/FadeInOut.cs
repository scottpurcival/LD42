using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    SpriteRenderer sr;
    public GameObject goToDisable;

    public float fadeTime = 0.5f;
    public float waitBeforeFade = 0.2f;
    float initialAlpha;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        initialAlpha = sr.color.a;
        StartCoroutine(FadeInThenOut(initialAlpha, fadeTime, waitBeforeFade));

    }

    IEnumerator FadeInThenOut(float to, float duration, float waitBefore)
    {
        Color newColour = sr.color;
        newColour.a = 0;
        sr.color = newColour;

        yield return new WaitForSeconds(waitBefore);

        float startTime = Time.time;

        while (Time.time - startTime < duration / 2.0f)
        {
            newColour.a = (((Time.time - startTime) / duration) * (to));
            sr.color = newColour;
            yield return null;
        }

        while (Time.time - startTime < duration)
        {
            newColour.a = to - (((Time.time - startTime) / duration) * (to));
            sr.color = newColour;
            yield return null;
        }

        if (goToDisable)
            Destroy(goToDisable);
    }


}
