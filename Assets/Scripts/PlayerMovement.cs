using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PhysicsObject
{
    public float maxSpeed = 7;
    public float JumpSpeed = 10;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && base.isGrounded)
        {
            base.velocity.y = this.JumpSpeed;
        }
        // Allow the player to cancel the jump by letting go of the jump button
        else if (Input.GetButtonUp("Jump"))
        {
            if (base.velocity.y > 0)
            {
                base.velocity.y *= 0.5f;
            }
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0) : (move.x < 0));
        if (flipSprite)
        {
            this.spriteRenderer.flipX = !this.spriteRenderer.flipX;
        }

        // Set the animator properties for the animator controller state machine attached to the player
        this.animator.SetBool("isGrounded", base.isGrounded);
        this.animator.SetFloat("velocityX", Mathf.Abs(base.velocity.x) / maxSpeed);

        base.targetVelocity = move * this.maxSpeed;
    }
}
