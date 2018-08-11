using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Vector2 input = new Vector2();
    Rigidbody2D rb;
    public float playerSpeed = 10.0f;
    public float jumpHeight = 40.0f;
    public Vector2 terminalVelocity;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // get input
        input = Vector2.zero;
        input.x = Input.GetAxis("Horizontal") * playerSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
            input.y = jumpHeight;

        // add force
        rb.AddForce(input);
	}
}
