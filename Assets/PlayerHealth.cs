using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 9;
    public int currentHealth;
    public PlayerHealthBar healthBar; // 关联血条

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
{
    currentHealth -= damage;
    healthBar.SetHealth(currentHealth);

    if (currentHealth <= 0)
    {
        Debug.Log("玩家死亡 — 重新加载场景");
        // 重新加载当前活动场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthBar.SetHealth(currentHealth);
    }
}
