using Assets.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float power;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Vector2 directionVector = (collision.transform.position - transform.position);
            collision.GetComponent<PlayerBehaviour>().Knockback(directionVector * power);
        }
    }
}
