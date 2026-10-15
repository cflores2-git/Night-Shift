using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpPower = 10f;

    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;

    public Transform lowattackpoint;
    public float attackRadius = 0.6f;
    public Transform highattackpoint;

    private bool jumpRequested;
    private bool isJumping;
    private bool hasLaunched;
    private float horizontalMovement;
    private Animator animation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animation.SetBool("IsWalking", Mathf.Abs(horizontalMovement) > 0.1f); // Absolute value to horizontal movement for true statement, turning off exit time will let walk return to idle without finish walking, loop time allows for animation to repeat
        if(isJumping && hasLaunched && rb.linearVelocity.y <= 0f && IsGrounded())//we check and active jump while still crouching on the floor, reset and update the animator
        {
            isJumping = false;
            hasLaunched = false;
        }
        animation.SetBool("IsJumping", isJumping);
    }
    
    void OnMove(InputValue value){
        horizontalMovement = value.Get<float>(); //Horizontal movement gives integers -1(A) and 1(D) and 0
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalMovement*moveSpeed,rb.linearVelocity.y); //Player movement, with no vertical speed to mess with gravity or jumping
        if (jumpRequested) //if jump is requested
        {
            if (IsGrounded()) //player still grounded after the crouch
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,jumpPower);
            hasLaunched = true;
        }
        else
        {
            isJumping = false; // cancel sequence
        }
        }
        jumpRequested = false; // one time action, request could keep being processed
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed && IsGrounded() && !isJumping) //Checks key is pressed, while on the ground and no jumping animation sequencing
        {
            isJumping = true;
            hasLaunched = false;
            animation.SetBool("IsJumping", true);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer) != null;
    }

    void OnHighAttack()
    {
        if(isJumping) return;
        animation.ResetTrigger("LowAttack");
        animation.SetTrigger("HighAttack");
    }
    void OnLowAttack()
    {
        if(isJumping) return;
        animation.ResetTrigger("HighAttack");
        animation.SetTrigger("LowAttack");
    }
    public void LaunchJump()
    {
        if (isJumping && !hasLaunched) 
        {
            jumpRequested = true; //no active jump and no takeoff, request takeoff
        }
    }

    public void HitLow()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            lowattackpoint.position,
            attackRadius
        );
        foreach (Collider2D hit in hits)
        {
            Enemies enemy = hit.GetComponent<Enemies>();
            if(enemy!=null)
            {
                enemy.match(false);
            }
        }
    }

    public void HitHigh()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            highattackpoint.position,
            attackRadius
        );
        foreach (Collider2D hit in hits)
        {
            Enemies enemy = hit.GetComponent<Enemies>();
            if(enemy!=null)
            {
                enemy.match(true);
            }
        }
    }
}
