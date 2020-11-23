using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PhysicsObject : MonoBehaviour
    {
        public float gravityModifier = 1f;
        public float minGroundNormalY = 0.65f;

        protected Rigidbody2D rb2d;
        protected Vector2 velocity;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
        protected bool isGrounded;
        protected Vector2 groundNormal;
        protected Vector2 targetVelocity;

        protected const float _minMoveDistance = 0.001f;
        protected const float _shellRadius = 0.01f;

        protected virtual void ComputeVelocity()
        {

        }

        public void OnEnable()
        {
            this.rb2d = GetComponent<Rigidbody2D>();
        }

        public void Start()
        {
            // This sets the contact filter to ignore triggers from collider objects, and 
            // instead it will check the project settings for 2D Physics Objects.
            this.contactFilter.useTriggers = false;
            this.contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            contactFilter.useLayerMask = true;
        }

        public void Update()
        {
            this.targetVelocity = Vector2.zero;
            ComputeVelocity();
        }

        public void FixedUpdate()
        {
            this.velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            this.velocity.x = targetVelocity.x;

            this.isGrounded = false;

            Vector2 deltaPosition = velocity * Time.deltaTime;

            // Gets a vector perpendicular to the two ground normals so that we can store the vector that goes along the ground
            Vector2 moveAlongGround = new Vector2(this.groundNormal.y, -this.groundNormal.x);

            Vector2 move = moveAlongGround * deltaPosition.x;

            Movement(move, false);

            move = Vector2.up * deltaPosition.y;

            Movement(move, true);
        }

        private void Movement(Vector2 move, bool yMovement)
        {
            float distance = move.magnitude;

            if (distance > _minMoveDistance)
            {
                int count = this.rb2d.Cast(move, this.contactFilter, this.hitBuffer, distance + _shellRadius);
                this.hitBufferList.Clear();

                for (int i = 0; i < count; i++)
                {
                    this.hitBufferList.Add(hitBuffer[i]);
                }

                for (int i = 0; i < hitBufferList.Count; i++)
                {
                    Vector2 currentNormal = this.hitBufferList[i].normal;

                    // Can only set player to a 'grounded' state when the ground below is at a specific angle (not a wall)
                    // This will cause slopes that aren't very steep to have the player not 'slip'
                    // However, if the slope is steep enough the player may still 'slip'
                    if (currentNormal.y > this.minGroundNormalY)
                    {
                        this.isGrounded = true;
                        if (yMovement)
                        {
                            this.groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }

                    float projection = Vector2.Dot(this.velocity, currentNormal);

                    if (projection < 0)
                    {
                        this.velocity -= projection * currentNormal;
                    }

                    float modifiedDistance = this.hitBufferList[i].distance - _shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }

            this.rb2d.position += move.normalized * distance;
        }
    }
}
