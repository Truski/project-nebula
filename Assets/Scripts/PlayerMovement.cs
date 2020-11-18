using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D prb { get; set; }
    private SpriteRenderer spriteRenderer { get; set; }
    private bool isGrounded = false;

    public float PlayerSpeed = 5;
    public float JumpForce = 5;

    // Start is called before the first frame update
    public void Start()
    {
        this.prb = GetComponent<Rigidbody2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called every frame
    public void Update() 
    {
        Move();
        Jump();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            this.isGrounded = true;
        }
    }

 
    private void Move() 
    {
        float direction = Input.GetAxisRaw("Horizontal");
        FlipSprite(direction);
        float movementValue = direction * this.PlayerSpeed;
        this.prb.velocity = new Vector2(movementValue, this.prb.velocity.y);
    }

    // TODO: Rewrite jump code so that it takes longer to get to the top of the jump
    // than it does to come back down
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.isGrounded) 
        {
            this.prb.AddForce(new Vector2(0.0f, this.JumpForce), ForceMode2D.Impulse);
            this.isGrounded = false;
        }
    }

    private void FlipSprite(float direction)
    {
        if (direction != 0)
            this.spriteRenderer.flipX = direction >= 0;
    
    }
}
