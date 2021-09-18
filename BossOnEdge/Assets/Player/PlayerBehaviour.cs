using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera vCamera;
        [SerializeField] private float speed;
        [SerializeField] private float acceleration;
        [SerializeField] private float jumpForce;
        [SerializeField] private float smallVelocity;

        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform projectilePosition;

        private new Rigidbody2D rigidbody;
        private Animator animator;

        private float lastGroundTime;
        private List<GameObject> collidingGround;
        private bool duringJump;
        private bool facingRight;

        private bool duringAttack;

        public delegate void Event();
        public static Event Die;

        private void OnEnable()
        {
            Die += PlayerBehaviour_Die;
        }

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            collidingGround = new List<GameObject>();
            duringJump = true;
            duringAttack = false;
            facingRight = true;
        }

        private void FixedUpdate()
        {
            
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
                PrepareJump();

            if (Input.GetButtonDown("Fire1"))
                SwordAttack();

            if (Input.GetButtonDown("Fire2"))
                ProjectileAttack();

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

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "Bottom")
                Die?.Invoke();

            if(collider.gameObject.tag == "BossAttack")
            {
                Vector2 forceVector = (transform.position - collider.transform.position);
                rigidbody.AddForce(forceVector.normalized * 10000);
            }
        }

        private void OnDisable()
        {
            Die -= PlayerBehaviour_Die;
        }

        private void ProjectileAttack()
        {
            if (duringAttack)
                return;

            duringAttack = true;
            animator.SetTrigger("Projectile");
        }

        public void FireProjectile()
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = projectilePosition.position;
            projectile.GetComponent<ProjectileBehaviour>().rotation = projectilePosition.rotation.eulerAngles.y;
        }
        
        private void SwordAttack()
        {
            if (duringAttack)
                return;

            duringAttack = true;
            animator.SetTrigger("Mele");
        }

        public void FinishAttack()
        {
            duringAttack = false;
        }

        private void Move(float direction)
        {
            if(rigidbody.velocity.x < speed)
            {
                rigidbody.AddForce(Vector2.up * acceleration);
            }

            float V = speed * direction;

            rigidbody.velocity = new Vector2(V, rigidbody.velocity.y);

            if (V < -smallVelocity && facingRight)
            {
                facingRight = false;
                transform.Rotate(new Vector3(0, 180, 0));
            }

            if (V > smallVelocity && !facingRight)
            {
                facingRight = true;
                transform.Rotate(new Vector3(0, -180, 0));
            }

            if (V > smallVelocity || V < -smallVelocity)
                animator.SetBool("Moving", true);
            else
                animator.SetBool("Moving", false);
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
            rigidbody.AddForce(new Vector2(0, jumpForce));
        }

        private void PlayerBehaviour_Die()
        {
            vCamera.Follow = null;
        }
    }
}
