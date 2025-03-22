using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;  // 子弹速度
    public int damage = 1;    // 子弹伤害

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("子弹缺少 Rigidbody2D 组件！");
            return;
        }

        rb.velocity = transform.right * speed; // **让子弹向前飞行**
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player")) // **检查碰到的对象是否是玩家**
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // **玩家扣血**
                Debug.Log("子弹命中玩家，玩家扣血：" + damage);
            }

            Destroy(gameObject); // **销毁子弹**
        }
    }
}
