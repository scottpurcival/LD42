using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour {

    public GameObject[] content;
    public float timeToStay = 5.0f;
    
	// Use this for initialization
	void Start ()
    {
        content[Random.Range(0, content.Length - 1)].SetActive(true);

        StartCoroutine(WaitAndDestroy());
    }
	
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(timeToStay);

        Destroy(this.gameObject);
    }
}
