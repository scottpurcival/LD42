using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Vector2 input = new Vector2();
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
    GameManager gm;
    public GameObject landingBlast;
    public AudioSource footsteps;
    public AudioSource jump;
    public AudioSource fileWhoosh;
    public float playerSpeed = 10.0f;
    public float jumpHeight = 40.0f;
    public float jumpControl = 0.5f;
    public Vector2 terminalVelocity;
    public float footstepSpeedModifier = 1.0f;
    public float maxLandingVelocity = 1.0f;
    public float decelRate = 1.0f;
    public float flipVelocity = 0.1f;
    public float distanceToGround = 0;
    public float groundedDistance = 1.0f;
    public float onGroundDist = 0.7f;
    public float blastVelocity = 3.5f;
    public string[] groundLayers;

    bool controlsEnabled = true;
    bool isAlive = true;
    bool isGrounded = true;
    bool isFalling = false;
    bool tryingToStop = false;
    bool dancing = false;

    float lastJump = 0;
    public float minTimeBetweenJumps = 0.1f;


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
        // enable landing blast if required
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Landing") && isFalling && isGrounded && rb.velocity.y < blastVelocity)
            Instantiate(landingBlast, transform.position, Quaternion.identity);
        isFalling = !isGrounded && (rb.velocity.y < -flipVelocity);
        if (isFalling)
            Debug.Log(RayCastDistToGround() + " : " + rb.velocity.y);

        // set animator
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        sprite.flipX = (rb.velocity.x < flipVelocity) ? true : false;
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("Falling", isFalling);

        if (controlsEnabled)
        {
            // get input
            input = Vector2.zero;
            input.x = Input.GetAxis("Horizontal") * (playerSpeed) * (isGrounded ? 1.0f : jumpControl);
            if (Input.GetButton("Jump") && isGrounded && (Time.time - lastJump) > minTimeBetweenJumps)
            {
                lastJump = Time.time;
                input.y = jumpHeight;
                jump.pitch = Random.Range(0.8f, 1.2f);
                jump.Play();
            }

            if(Input.GetAxis("Vertical") < 0 && !dancing)
            {
                // DANCE MOFO!
                anim.SetTrigger("Dance");
                dancing = true;
            }
            
            if(Input.GetAxis("Vertical") >= 0 && dancing)
            {
                    // STOP!
                    anim.SetTrigger("StopDance");
                    dancing = false;
            }

        }

        // check if player is trying to decel
        if (rb.velocity.x > 0 && input.x < 0 || rb.velocity.x < 0 && input.x > 0)
        {
            // we are trying to slow down!!!
            tryingToStop = true;
        }

        if (input.x == 0 && isGrounded)
        {
            // decel
            rb.AddForce(-rb.velocity * decelRate);
        }

        if (isGrounded)
            footsteps.pitch = Mathf.Abs(rb.velocity.x) / terminalVelocity.x * footstepSpeedModifier;
        else
            footsteps.pitch = 0;

        // add force
        rb.AddForce(new Vector2(input.x,0),ForceMode2D.Impulse);
        rb.AddForce(new Vector2(0, input.y), ForceMode2D.Impulse);

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

        if(!isAlive && isGrounded)
            rb.simulated = false;   // disable rigidbody on death.

        if (!controlsEnabled && isGrounded)
            rb.simulated = false;  // stop player on win!

        anim.SetFloat("DistanceToGround", RayCastDistToGround());
    }

    float RayCastDistToGround()
    {
        RaycastHit2D groundTest = Physics2D.Raycast(transform.position, Vector2.down, 10.0f, LayerMask.GetMask(groundLayers));
        if (groundTest)
            return groundTest.distance;
        else
            return 100.0f;

    }

    bool CheckIsGrounded()
    {
        return (RayCastDistToGround() < onGroundDist) || (RayCastDistToGround() < groundedDistance && Mathf.Abs(rb.velocity.y) < flipVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FileToClear")
        {
            Destroy(collision.gameObject);
            gm.CollectFile();
            fileWhoosh.Play();
        }

        if (collision.gameObject.tag == "GarbageCan")
        {
            if (gm.AreAllFilesCollected())
            {
                anim.SetTrigger("Dance");
                gm.PlayerExit();
                controlsEnabled = false;
                //rb.AddForce(-rb.velocity, ForceMode2D.Impulse);
            }
        }

        if (collision.gameObject.tag == "AudioTrigger")
            collision.GetComponent<PlaySoundTrigger>().PlaySound();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (controlsEnabled)    // check to prevent us dying after we already died/won
            {
                // we are hit!! disable control
                controlsEnabled = false;
                isAlive = false;
                // play animation
                anim.SetTrigger("Died");
                // die
                gm.Die();
            }
        }
    }

}
