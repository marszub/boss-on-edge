using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargulBehaviour : MonoBehaviour
{
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
        Debug.LogWarning("Not implemented");
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
}
