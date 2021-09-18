using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargulBehaviour : MonoBehaviour
{
    private Animator animator;
    private bool duringAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        duringAttack = false;
    }

    public void ProjectileAttack()
    {
        animator.SetTrigger("Projectile");
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
}
