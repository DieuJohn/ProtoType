using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Player : MonoBehaviour
{
    //Declare variables for player script
    public float speed; 
    Rigidbody2D rb; 
    bool facingRight = true;

    bool isGrounded; 
    public Transform groundCheck; 
    public float checkRadius; 
    public LayerMask whatIsGround; 
    public float jumpForce;

    bool isTouchingFront; 
    public Transform frontCheck; 
    bool wallSliding; 
    public float wallSlidingSpeed;

    bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    public float moveInput; 

    public int maxHealth = 100;
	public int currentHealth;

	public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");

        moveInput = Input.GetAxis("Horizontal"); rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
   
        //Flip player when facing direction
        if (input> 0 && facingRight == false) {
            flip();
        } else if (input < 0 && facingRight == true) {
            flip();
        }

         isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //When jumping, check if player is grounded
        if (((Input.GetKeyDown(KeyCode.UpArrow)) || (Input.GetKeyDown(KeyCode.W))) && isGrounded == true) { 
            rb.velocity = Vector2.up * jumpForce;
        }
        
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);

        if (isTouchingFront == true && isGrounded == false){
            wallSliding = true;
        } else {
            wallSliding = false;
        }

        if (wallSliding) {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        //When jumping, check if player is wallsliding
        if (((Input.GetKeyDown(KeyCode.UpArrow)) || (Input.GetKeyDown(KeyCode.W)))  && wallSliding == true) {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (wallJumping == true) {
            rb.velocity = new Vector2(xWallForce * -input, yWallForce);
        }

        if (Input.GetKeyDown(KeyCode.Space))
		{
			TakeDamage(5);
		}
    }
    void flip() {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
    }

    void SetWallJumpingToFalse() {
        wallJumping = false;
    }

	void TakeDamage(int damage)
	{
		currentHealth -= damage;

		healthBar.SetHealth(currentHealth);
	}
}
