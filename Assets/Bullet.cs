using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f; // 子弹存活时间
    public int damage = 1; // 伤害值

    void Start()
    {
        Destroy(gameObject, lifeTime); // 超时销毁子弹
    }

    void Update()
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            transform.right = GetComponent<Rigidbody2D>().velocity.normalized; // 让子弹朝向飞行方向
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("ShieldWall")) // 碰到墙壁或盾墙
        {
            if (collision.CompareTag("ShieldWall"))
            {
                ShieldWall shield = collision.GetComponent<ShieldWall>();
                if (shield != null)
                {
                    shield.TakeDamage(); // 盾墙受到攻击
                }
            }
            Destroy(gameObject); // 子弹消失
        }
        else if (collision.CompareTag("Enemy")) // 碰到敌人
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            if (enemy != null) 
            {
                enemy.TakeDamage(damage); // 造成伤害
            }
            Destroy(gameObject); // 子弹消失
        }
    }
}
