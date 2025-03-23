using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBullet : MonoBehaviour
{
    public int healAmount = 1; // 加血值
    public float lifeTime = 5f; // 子弹存活时间

    void Start()
    {
        Destroy(gameObject, lifeTime); // 一定时间后销毁
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // 碰撞到敌人
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                Debug.Log("HealBullet 碰撞到敌人，尝试加血！");
                enemy.Heal(1); // 加 1 点血
            }
            Destroy(gameObject); // 碰撞后子弹销毁
        }
        else if (collision.gameObject.CompareTag("ShieldWall")) // 碰到盾墙
        {
            ShieldWall shield = collision.gameObject.GetComponent<ShieldWall>();
            if (shield != null)
            {
                shield.Heal(1); // 盾墙受到回血攻击
            }
            Destroy(gameObject); // 子弹消失
        }
    }
}
