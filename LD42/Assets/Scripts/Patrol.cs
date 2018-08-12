using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Patrol : MonoBehaviour {

    public float movementSpeed = 1;
    public float xStart = 0, xFinish = 0;

    SpriteRenderer sr;
    Rigidbody2D rb;

    Vector2 startPos, endPos;
    enum Direction
    {
        LEFT = -1,
        RIGHT = 1
    }
    Direction direction;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawIcon(new Vector3(xStart + transform.position.x, transform.position.y, transform.position.z), "PatrolL.png");
        Gizmos.DrawIcon(new Vector3(xFinish + transform.position.x, transform.position.y, transform.position.z), "PatrolR.png");
    }

    // Use this for initialization
    void Start ()
    {
        startPos = transform.position;
        endPos = transform.position;
        startPos.x += xStart;
        endPos.x += xFinish;
        direction = Direction.LEFT;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.x - startPos.x < 0.1)
            direction = Direction.RIGHT;
        if (endPos.x - transform.position.x < 0.1 )
            direction = Direction.LEFT;

        sr.flipX = (direction == Direction.LEFT) ? false : true;

        float newX = transform.position.x + ((float)direction * Time.deltaTime * movementSpeed);
        rb.MovePosition(new Vector2(newX, transform.position.y));
    }
}
