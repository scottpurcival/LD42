using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Vector2 input = new Vector2();
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
    GameManager gm;
    public float playerSpeed = 10.0f;
    public float jumpHeight = 40.0f;
    public float jumpControl = 0.5f;
    public Vector2 terminalVelocity;
    public float decelRate = 1.0f;
    public float groundedVelocity = 0.1f;

    public GameObject holding;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // set animator
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        sprite.flipX = (rb.velocity.x < groundedVelocity) ? true : false;
        anim.SetBool("isGrounded", isGrounded());
        anim.SetBool("Falling", rb.velocity.y < 0.1 ? true : false);

        // clamp velocity
        Vector2 newRB = rb.velocity;
        newRB.x = Mathf.Clamp(newRB.x, -terminalVelocity.x, terminalVelocity.x);
        newRB.y = Mathf.Clamp(newRB.y, -terminalVelocity.y, terminalVelocity.y);
        rb.velocity = newRB;


        // get input
        input = Vector2.zero;
        input.x = Input.GetAxis("Horizontal") * playerSpeed * (isGrounded() ? 1.0f : jumpControl);

        if (input.x == 0 && isGrounded())
        {
            // decel
            rb.AddForce(-rb.velocity * decelRate);
        }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit: " + collision.gameObject.tag + " and Holding: " + holding);

        if (collision.gameObject.tag == "FileToClear")
        {
            Destroy(collision.gameObject);
            gm.CollectFile();
        }

        if (collision.gameObject.tag == "GarbageCan")
            gm.PlayerExit();
    }

}
