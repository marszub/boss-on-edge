using Assets.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargulBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectilePosition;
    [SerializeField] private float projectileVelocity;
    [SerializeField] private float timeBetweenBlinks;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float knockbackDelta;

    [SerializeField] private AudioClip blinkSound;
    [SerializeField] private AudioClip projectileSound;
    [SerializeField] private AudioClip meleSound;

    private Animator animator;
    private AudioSource bossSound;
    private bool duringAttack;
    private bool playerDead;

    private float lastBlinkTime;
    private float lastAttackTime;

    public delegate void Event();
    public static Event Win;

    private void OnEnable()
    {
        PlayerBehaviour.Die += PlayerDie;
        Win += PlayerDie;
    }

    private void OnDisable()
    {
        PlayerBehaviour.Die -= PlayerDie;
        Win -= PlayerDie;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        bossSound = GetComponent<AudioSource>();
        lastBlinkTime = Time.time - timeBetweenBlinks/2;
        playerDead = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
            Win?.Invoke();
    }

    private void Update()
    {
        if (playerDead)
            return;
        if (lastBlinkTime + timeBetweenBlinks < Time.time)
            Blink();

        if (lastAttackTime + timeBetweenAttacks < Time.time)
            ProjectileAttack();
    }

    public void ProjectileAttack()
    {
        animator.SetTrigger("Projectile");
        lastAttackTime = Time.time;
        duringAttack = true;
    }

    public void FireProjectile()
    {
        bossSound.clip = projectileSound;
        bossSound.Play();

        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = projectilePosition.position;
        Vector2 lookVector = GameObject.FindGameObjectWithTag("Player").transform.position - projectile.transform.position;
        projectile.transform.Rotate(new Vector3(0, 0, Vector2.SignedAngle(Vector2.left, lookVector)));

        projectile.GetComponent<Rigidbody2D>().velocity = lookVector.normalized * projectileVelocity;
    }

    public void MeleAttack()
    {
        animator.SetTrigger("Mele");
        duringAttack = true;
    }

    public void MeleHit()
    {
        bossSound.clip = meleSound;
        bossSound.Play();
    }

    public void EndAttack()
    {
        duringAttack = false;
    }

    private void Blink()
    {
        animator.SetTrigger("Blink");
        bossSound.clip = blinkSound;
        bossSound.Play();
        lastBlinkTime = Time.time;
    }

    public void Knockback(float power)
    {
        transform.position = transform.position + Vector3.right * knockbackDelta * power;
    }

    private void PlayerDie()
    {
        playerDead = true;
    }
}
