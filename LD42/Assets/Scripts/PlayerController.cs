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
    public float maxLandingVelocity = 1.0f;
    public float decelRate = 1.0f;
    public float groundedVelocity = 0.1f;
    public float distanceToGround = 0;
    public string[] groundLayers;

    bool isGrounded = true;
    bool isFalling = false;
    bool tryingToStop = false;

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
        // find state
        isGrounded = CheckIsGrounded();
        if (isGrounded && isFalling)
            tryingToStop = true;
        isFalling = (rb.velocity.y < -groundedVelocity);

        // set animator
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        sprite.flipX = (rb.velocity.x < groundedVelocity) ? true : false;
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("Falling", isFalling);

        // get input
        input = Vector2.zero;

        // check if player is trying to decel
        if (rb.velocity.x > 0 && input.x < 0 || rb.velocity.x < 0 && input.x > 0)
        {
            // we are trying to slow down!!!
            tryingToStop = true;
        }

        input.x = Input.GetAxis("Horizontal") * (playerSpeed) * (isGrounded ? 1.0f : jumpControl);

        if (input.x == 0 && isGrounded)
        {
            // decel
            rb.AddForce(-rb.velocity * decelRate);
        }

        if (Input.GetButton("Jump") && isGrounded)
        {
            input.y = jumpHeight;
        }

        // add force
        rb.AddForce(input);

   }

    private void FixedUpdate()
    {
        if (tryingToStop)
        {
            // clamp velocity
            Vector2 newRB = rb.velocity;
            newRB.x = Mathf.Clamp(newRB.x, -maxLandingVelocity, maxLandingVelocity);
            rb.velocity = newRB;
            tryingToStop = false;
        }
        else
        {
            // clamp velocity
            Vector2 newRB = rb.velocity;
            newRB.x = Mathf.Clamp(newRB.x, -terminalVelocity.x, terminalVelocity.x);
            newRB.y = Mathf.Clamp(newRB.y, -terminalVelocity.y, terminalVelocity.y);
            rb.velocity = newRB;
        }

            RaycastHit2D groundTest = Physics2D.Raycast(transform.position, Vector2.down, 10.0f, LayerMask.GetMask(groundLayers));
            if (groundTest)
                distanceToGround = groundTest.distance;
            else
                distanceToGround = 100;

            anim.SetFloat("DistanceToGround", distanceToGround);
    }

    bool CheckIsGrounded()
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

        if (collision.gameObject.tag == "AudioTrigger")
            collision.GetComponent<PlaySoundTrigger>().PlaySound();
    }

}
