using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public float lifeTime = 5f; // 子弹存活时间
    public float bounceForce = 1f; // 反弹力度
    public int damage = 1; // 伤害值
    private Rigidbody2D rb;
    public GameObject hitEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime); // 超时销毁子弹
    }

    void Update()
    {
        transform.right = rb.velocity.normalized; // 让子弹朝向飞行方向
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.gameObject.CompareTag("Wall")) // 碰到墙壁时反弹
        // {
        //     Vector2 reflectDirection = Vector2.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
        //     rb.velocity = reflectDirection * rb.velocity.magnitude * bounceForce;
        // }
        
        if (collision.gameObject.CompareTag("player")) // 碰到敌人时
        {
            

             PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("玩家受到了 " + damage + " 点伤害！");
            }

        }
        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
