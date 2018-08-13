using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour {

    Transform spriteTransform;
    public float flareAmount = 3.0f;
    public float flareSpeed = 360.0f;
    float currentInterval = 0;
    float pauseTimer;
    public float pauseBetweenFlares = 0;
    public bool scaleX, scaleY;
    Vector2 initialScale;

	// Use this for initialization
	void Start ()
    {
        spriteTransform = this.transform;
        initialScale = spriteTransform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (currentInterval == 360)
        {
            pauseTimer = pauseBetweenFlares;
            currentInterval = 0;
        }

        if (pauseTimer < 0)
        {
            currentInterval += Time.deltaTime * flareSpeed;
            if (currentInterval > 360)
                currentInterval = 360;

            float newScale = 0;
            newScale = (Mathf.Sin((currentInterval / 360) * Mathf.PI * 2) * flareAmount);
            spriteTransform.localScale = new Vector2((scaleX ? newScale : 0) * initialScale.x, (scaleY ? newScale : 0) * initialScale.y) + initialScale;
        }
        else
            pauseTimer -= Time.deltaTime;
	}
}
