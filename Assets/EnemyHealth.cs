using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 6; // 最大血量
    public int health; // 当前血量（等于初始血量）

    public GameObject healthBarPrefab; // 关联血条预制体
    private HealthBar healthBar;

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();

        if (healthBar == null)
        {
            Debug.LogError("HealthBar 组件未找到！请确保 HealthBar 作为 Enemy 的子对象存在！");
            return;
        }

        healthBar.SetHealth(health); // 显示初始血量
    }

    public void Heal(int amount)
    {
        health += amount;
        Debug.Log("加血成功，当前血量：" + health);

        if (health > maxHealth) // 超出最大血量就死亡
        {
            Debug.Log("敌人血量溢出，死亡！");
            Destroy(gameObject);
        }

        healthBar.SetHealth(health);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("敌人受到伤害，当前血量：" + health);

        if (health <= 0)
        {
            Debug.Log("敌人死亡！");
            Destroy(gameObject);
        }

        healthBar.SetHealth(health);
    }
}
