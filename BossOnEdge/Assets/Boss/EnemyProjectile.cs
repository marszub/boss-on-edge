using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private float dieTime;

    private void Start()
    {
        dieTime = Time.time + lifeTime;
    }

    private void Update()
    {
        if (Time.time > dieTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyProjectile>(out EnemyProjectile _))
            return;

        else
            Destroy(gameObject);
    }
}
