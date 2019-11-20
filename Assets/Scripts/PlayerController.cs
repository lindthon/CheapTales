using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5;
    public float jump = 10f;

    private Animator anim;

    private enum State {idle, running, jumping, falling, crouching};
    private State state = State.idle;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float HDirection = Input.GetAxis("Horizontal");
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        if(HDirection<0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        else if (HDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            state = State.jumping;
        }

        StateSwitch();
        anim.SetInteger("state",(int)state);
    }

    private void StateSwitch()
    {
        if (state == State.jumping)
        {
            //Jumping
            if(rb.velocity.y < 0.1f)
            {
                state = State.falling;

            }
        }else if(state == State.falling)
        {
            //Falling
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if(Mathf.Abs(rb.velocity.x)> Mathf.Epsilon)
        {
            //Moving
            state = State.running;

        }else if(Input.GetKey(KeyCode.DownArrow)){
            state = State.crouching;
        }
        else
        {
            //Standing
            state = State.idle;
        }
    }
}
