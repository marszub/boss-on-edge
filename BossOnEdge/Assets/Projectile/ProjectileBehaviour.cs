using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float rotation;

    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    private float deadTime;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.rotation = rotation;
        rb.velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * rotation), Mathf.Sin( Mathf.Deg2Rad * rotation)) * speed;
        deadTime = lifeTime + Time.time;
    }

    void Update()
    {
        if (Time.time > deadTime)
            Disappear();
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            Disappear();
    }
}
