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

        private new Rigidbody2D rigidbody;

        private float lastGroundTime;
        private List<GameObject> collidingGround;
        private bool duringJump;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            collidingGround = new List<GameObject>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
                Jump();

            if (Input.GetButtonUp("Jump"))
                StopJump();

            Move(Input.GetAxis("Horizontal"));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "Ground")
            {
                collidingGround.Add(collision.gameObject);
                duringJump = false;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                collidingGround.Remove(collision.gameObject);
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
            if (duringJump && Time.time - lastGroundTime > jumpHeight / jumpSpeed)
                StopJump();

            rigidbody.velocity = new Vector2(speed * direction, rigidbody.velocity.y);
        }

        private void Jump()
        {
            if (collidingGround.Count > 0)
            {
                duringJump = true;
                lastGroundTime = Time.time;
                rigidbody.AddForce(new Vector2(0, jumpForce));
            }
        }

        private void StopJump()
        {
            Debug.Log(transform.position.y);
            duringJump = false;
        }
    }
}
