using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Patrol : MonoBehaviour {

    public float movementSpeed = 1;
    public float xStart = 0, xFinish = 0;
    public float yStart = 0, yFinish = 0;


    SpriteRenderer sr;
    Rigidbody2D rb;

    Vector2 startPos, endPos;
    enum Direction
    {
        LEFT = -1,
        RIGHT = 1,
    }
    Direction directionLR;
    Direction directionUD;

    private void OnDrawGizmosSelected()
    {
            Gizmos.DrawIcon(new Vector3(xStart + transform.position.x, yStart + transform.position.y, transform.position.z), "PatrolL.png");
            Gizmos.DrawIcon(new Vector3(xFinish + transform.position.x, yFinish + transform.position.y, transform.position.z), "PatrolR.png");
    }

    // Use this for initialization
    void Start ()
    {
        startPos = transform.position;
        endPos = transform.position;
        startPos.x += xStart;
        startPos.y += yStart;
        endPos.x += xFinish;
        endPos.y += yFinish;
        directionLR = Direction.LEFT;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.x - startPos.x < 0.1 && startPos.y - transform.position.y < 0.1)
            directionLR = Direction.RIGHT;
        if (endPos.x - transform.position.x < 0.1 && transform.position.y - endPos.y < 0.1)
            directionLR = Direction.LEFT;

        sr.flipX = (directionLR == Direction.LEFT) ? false : true;

        Vector2 bearing = (endPos - (Vector2)transform.position).normalized; 

        rb.MovePosition(transform.position + (Vector3)(bearing * ((float)directionLR * Time.deltaTime * movementSpeed)));
    }
}
