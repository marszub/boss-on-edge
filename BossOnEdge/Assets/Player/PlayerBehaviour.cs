using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float smallVelocity;

        private new Rigidbody2D rigidbody;
        private Animator animator;
        private SpriteRenderer sprite;

        private float lastGroundTime;
        private List<GameObject> collidingGround;
        private bool duringJump;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();
            collidingGround = new List<GameObject>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
                PrepareJump();

            if (rigidbody.velocity.y < -smallVelocity)
                animator.SetBool("Up", false);

            Move(Input.GetAxis("Horizontal"));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "Ground")
            {
                collidingGround.Add(collision.gameObject);
                duringJump = false;
                animator.SetBool("MidAir", false);
                animator.SetBool("Up", false);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                collidingGround.Remove(collision.gameObject);
                if (collidingGround.Count == 0)
                {
                    duringJump = true;
                    animator.SetBool("MidAir", true);
                }
            }
        }

        private void ProjectileAttack()
        {
            throw new NotImplementedException();
        }
        
        private void SwordAttack()
        {
            throw new NotImplementedException();
        }

        private void Move(float direction)
        {
            float V = speed * direction;
            rigidbody.velocity = new Vector2(V, rigidbody.velocity.y);

            if (V < -smallVelocity)
                sprite.flipX = true;

            if (V > smallVelocity)
                sprite.flipX = false;
        }

        private void PrepareJump()
        {
            if (!duringJump)
            {
                duringJump = true;
                animator.SetBool("Up", true);
                animator.SetBool("MidAir", true);
            }
        }

        public void Jump()
        {
            lastGroundTime = Time.time;
            rigidbody.AddForce(new Vector2(0, jumpForce));
        }
    }
}
