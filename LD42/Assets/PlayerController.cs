using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Vector2 input = new Vector2();
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
    public float playerSpeed = 10.0f;
    public float jumpHeight = 40.0f;
    public Vector2 terminalVelocity;
    public float decelRate = 1.0f;
    public float groundedVelocity = 0.1f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // set animator
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        sprite.flipX = (rb.velocity.x < 0) ? true : false;


        // clamp velocity
        Vector2 newRB = rb.velocity;
        newRB.x = Mathf.Clamp(newRB.x, -terminalVelocity.x, terminalVelocity.x);
        newRB.y = Mathf.Clamp(newRB.y, -terminalVelocity.y, terminalVelocity.y);
        rb.velocity = newRB;


        // get input
        input = Vector2.zero;
        input.x = Input.GetAxis("Horizontal") * playerSpeed * (isGrounded() ? 1.0f : 0.5f);

        if (Input.GetButton("Jump") && isGrounded())
            input.y = jumpHeight;

        // add force
        rb.AddForce(input);
    }

    bool isGrounded()
    {
        if (Mathf.Abs(rb.velocity.y) < groundedVelocity)
            return true;
        else
            return false;
    }
}
