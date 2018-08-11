using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Whoosh : MonoBehaviour {

    public float whooshTime = 0.5f;
    public float stayTime = 2.0f;

    float whooshStart;
    Text whooshText;
    RectTransform whooshTransform;
    bool whooshing = false;

    void Start()
    {
        whooshText = GetComponent<Text>();
        whooshTransform = GetComponent<RectTransform>();
    }

	// Update is called once per frame
	void Update ()
    {
		if(whooshing)
        {
            float elapsedTime = Time.time - whooshStart;
            float newScale;
            if (elapsedTime < whooshTime)
            {
                newScale = elapsedTime / whooshTime;
                whooshTransform.localScale = new Vector3(newScale, newScale, newScale);
            }
            else if (elapsedTime > whooshTime && elapsedTime < whooshTime + stayTime)
            {
                whooshTransform.localScale = new Vector3(1, 1, 1);
            }
            else if (elapsedTime > whooshTime + stayTime && elapsedTime < whooshTime + stayTime + whooshTime)
            {
                newScale = 1 - ((elapsedTime - stayTime - whooshTime) / whooshTime);
                whooshTransform.localScale = new Vector3(newScale, newScale, newScale);
            }
            else
            {
                whooshing = false;
                whooshTransform.localScale = Vector3.zero;
            }
        }
	}

    public void WhooshNow(string text)
    {
        whooshText.text = text;
        whooshStart = Time.time;
        whooshing = true;
    }
}
