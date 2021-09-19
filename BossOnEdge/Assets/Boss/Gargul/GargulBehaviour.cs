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

    private Animator animator;
    private bool duringAttack;

    private float lastBlinkTime;
    private float lastAttackTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        lastBlinkTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
            Win();
    }

    private void Update()
    {
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
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = projectilePosition.position;
        Vector2 lookVector = GameObject.FindGameObjectWithTag("Player").transform.position - projectile.transform.position;
        projectile.transform.Rotate(new Vector3(0, 0, Vector2.SignedAngle(Vector2.left, lookVector)));

        projectile.GetComponent<Rigidbody2D>().velocity = lookVector * projectileVelocity;
    }

    public void MeleAttack()
    {
        animator.SetTrigger("Mele");
        duringAttack = true;
    }

    public void EndAttack()
    {
        duringAttack = false;
    }

    private void Blink()
    {
        animator.SetTrigger("Blink");
        lastBlinkTime = Time.time;
    }

    public void Knockback(float power)
    {
        transform.position = transform.position + Vector3.right * knockbackDelta * power;
    }

    private void Win()
    {

    }
}
