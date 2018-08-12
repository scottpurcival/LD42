using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour {

    Transform spriteTransform;
    public float wiggleAmount = 3.0f;
    public float wiggleSpeed = 360.0f;
    float currentInterval = 0;
    float pauseTimer;
    public float pauseBetweenWiggles = 1.0f;

	// Use this for initialization
	void Start ()
    {
        spriteTransform = this.transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (currentInterval == 360)
        {
            pauseTimer = pauseBetweenWiggles;
            currentInterval = 0;
        }

        if (pauseTimer < 0)
        {
            currentInterval += Time.deltaTime * wiggleSpeed;
            if (currentInterval > 360)
                currentInterval = 360;

            Vector3 newRotation = Vector3.zero;
            newRotation.z = Mathf.Sin((currentInterval / 360) * Mathf.PI * 2) * wiggleAmount;
            spriteTransform.localEulerAngles = newRotation;
        }
        else
            pauseTimer -= Time.deltaTime;
	}
}
