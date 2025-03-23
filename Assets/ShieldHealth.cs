using System.Collections;
using UnityEngine;

public class ShieldHealth : MonoBehaviour
{
    public int maxHealth = 6;      // 最大血量（可调节）
    public int health = 6;         // 初始血量（可调节）
    public float shakeDuration = 1.5f;  // 敌人溢出死亡前的抖动时间（可调节）

    public GameObject healthBarPrefab; // 关联血条预制体
    private HealthBar healthBar;

    void Start()
    {
        // 确保初始血量不会超过最大血量
        health = Mathf.Clamp(health, 1, maxHealth);

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

        if (health > maxHealth) // 超出最大血量就进入溢出死亡倒计时
        {
            Debug.Log("敌人血量溢出，进入死亡倒计时！");
            StartCoroutine(OverhealDeath());
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

    IEnumerator OverhealDeath()
    {
        float elapsedTime = 0f;
        Vector3 originalPos = transform.position;

        while (elapsedTime < shakeDuration)
        {
            float shakeAmount = 0.1f;
            transform.position = originalPos + (Vector3)Random.insideUnitCircle * shakeAmount;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos; // 复位
        Destroy(gameObject);
    }
}
