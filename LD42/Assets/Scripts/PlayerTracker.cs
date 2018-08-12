using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour {

    GameObject player;
    public float upperLimit = 0;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector3 movePos;

    void Start()
    {
        upperLimit = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

	// Update is called once per frame
	void Update ()
    {
        movePos = transform.position;
        movePos.x = player.transform.position.x;
        rb.MovePosition(movePos);

        if (player.transform.position.y > upperLimit)
            sr.enabled = true;
        else
            sr.enabled = false;
	}
}
