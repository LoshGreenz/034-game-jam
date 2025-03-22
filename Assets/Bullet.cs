using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f; // 子弹存活时间
    public float bounceForce = 1f; // 反弹力度
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime); // 防止子弹一直存在
    }
    void Update()
{
    transform.right = rb.velocity.normalized; // 让子弹朝向飞行方向
}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // 碰到墙壁时
        {
            Vector2 reflectDirection = Vector2.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
            rb.velocity = reflectDirection * rb.velocity.magnitude * bounceForce;
        }
    }
}
