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
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float smallVelocity;

        [SerializeField] private AudioSource meleAttack;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform projectilePosition;

        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip painSound;
        [SerializeField] private AudioClip fireballSound;

        private new Rigidbody2D rigidbody;
        private Animator animator;
        private AudioSource audioSource;

        private List<GameObject> collidingGround;
        private bool duringJump;
        private bool facingRight;
        private bool immobilized;
        private bool won;

        private bool duringAttack;

        public delegate void Event();
        public static Event Die;

        private void OnEnable()
        {
            Die += PlayerBehaviour_Die;
            GargulBehaviour.Win += Win;
        }

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            collidingGround = new List<GameObject>();
            duringJump = true;
            duringAttack = false;
            facingRight = true;
            immobilized = false;
            won = false;
        }

        private void FixedUpdate()
        {
            if (immobilized && rigidbody.velocity.x <= smallVelocity && rigidbody.velocity.x >= -smallVelocity)
                EndKnockback();
        }

        private void Update()
        {
            if (immobilized || won)
                return;

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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Border")
                Die?.Invoke();
        }

        private void OnDisable()
        {
            GargulBehaviour.Win -= Win;
            Die -= PlayerBehaviour_Die;
        }

        private void Win()
        {
            won = true;
        }

        private void ProjectileAttack()
        {
            if (duringAttack)
                return;

            duringAttack = true;
            animator.SetTrigger("Projectile");
            audioSource.clip = fireballSound;
            audioSource.Play();
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

        private void SwordHit()
        {
            meleAttack.Play();
        }

        public void FinishAttack()
        {
            duringAttack = false;
        }

        private void Move(float direction)
        {
            float V = speed * direction;
            rigidbody.velocity = new Vector2(V, rigidbody.velocity.y);

            UpdateRotation(V);

            if (V > smallVelocity || V < -smallVelocity)
                animator.SetBool("Moving", true);
            else
                animator.SetBool("Moving", false);
        }

        private void UpdateRotation(float xVelocity)
        {
            if (xVelocity < -smallVelocity && facingRight)
            {
                facingRight = false;
                transform.Rotate(new Vector3(0, 180, 0));
            }

            if (xVelocity > smallVelocity && !facingRight)
            {
                facingRight = true;
                transform.Rotate(new Vector3(0, -180, 0));
            }
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
            audioSource.clip = jumpSound;
            audioSource.Play();
            rigidbody.AddForce(new Vector2(0, jumpForce));
        }

        public void Knockback(Vector2 velocity)
        {
            rigidbody.velocity = velocity;
            animator.SetBool("Knockback", true);
            immobilized = true;
            audioSource.clip = painSound;
            audioSource.Play();

            UpdateRotation(velocity.x);
        }

        private void EndKnockback()
        {
            animator.SetBool("Knockback", false);
            immobilized = false;
        }

        private void PlayerBehaviour_Die()
        {
            vCamera.Follow = null;
        }
    }
}
