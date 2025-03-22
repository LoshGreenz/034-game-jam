using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 6; // 最大血量
    private int currentHealth;
    public GameObject healthBarPrefab; // 关联血条预制体
    private HealthBar healthBar;

    void Start()
{
    currentHealth = maxHealth;

    // **找到 Enemy 物体的子对象中的 HealthBar 组件**
    healthBar = GetComponentInChildren<HealthBar>();

    if (healthBar != null)
    {
        healthBar.SetHealth(currentHealth);
    }
    else
    {
        Debug.LogError("HealthBar 组件未找到！请确保 HealthBar 作为 Enemy 的子对象存在！");
    }
}


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // 更新血条
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // 怪物死亡
        }
    }
}
