using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Whoosh : MonoBehaviour {

    public float whooshTime = 0.5f;
    public float stayTime = 2.0f;

    float whooshStart;
    Text whooshText;
    bool whooshing = false;

    void Start()
    {
        whooshText = GetComponent<Text>();
    }

	// Update is called once per frame
	void Update ()
    {
		if(whooshing)
        {
            if (Time.time - whooshStart < whooshTime)
            {

            }
            else if (Time.time - whooshStart > whooshTime && Time.time - whooshStart < whooshTime + stayTime)
            {

            }
            else if (Time.time - whooshStart > whooshTime + stayTime && Time.time - whooshStart > whooshTime + stayTime + whooshTime)
            {

            }
            else
            {
                whooshing = false;
                whooshText.gameObject.transform.scale = Vector3.zero;
            }

        }
	}

    public void Whoosh(string text)
    {
        whooshText.text = text;
        whooshStart = Time.time;
        whooshing = true;
    }
}
