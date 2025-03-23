using System.Collections;
using UnityEngine;

public class ShieldWall : MonoBehaviour
{
    public Color normalColor = Color.gray;  // 默认颜色
    public Color hitColor = Color.red;      // 受到普通攻击时的颜色
    public Color healStartColor = Color.blue;   // 血量充能起始颜色
    public Color healFullColor = Color.green;   // 充能满后颜色

    public float knockbackSpeed = 5f;  // **玩家反弹的固定速度**
    public int damageToPlayer = 1;      // 反弹时对玩家造成的伤害
    public float colorResetTime = 1f;   // 普通攻击后多久恢复颜色
    public float healAmountToDestroy = 5f; // 累计回血到多少后销毁
    public float healLerpSpeed = 0.5f; // 颜色变化速度

    private SpriteRenderer spriteRenderer;
    private float currentHealAmount = 0f; // 当前的回血累计量

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;  // 初始化颜色
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                // 计算反弹方向
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                playerMove.ApplyKnockback(knockbackDirection * knockbackSpeed);
            }

            // 让玩家掉血
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
        }
    }

    public void TakeDamage()
    {
        // **普通攻击使回血量清零**
        currentHealAmount = 0f;

        // **普通攻击无效，仅仅短暂变色**
        StopCoroutine(ResetColor()); // 确保不会重复执行
        spriteRenderer.color = hitColor;
        StartCoroutine(ResetColor());
    }

    // **✅ 新增 Heal() 方法，供 HealBullet 调用**
    public void Heal(float amount)
    {
        HealEffect(amount);
    }

    // 处理回血子弹的颜色变化和销毁逻辑
    public void HealEffect(float amount)
    {
        // 逐渐增加回血值
        currentHealAmount += amount;
        float lerpFactor = Mathf.Clamp01(currentHealAmount / healAmountToDestroy);
        
        // 颜色逐渐由 healStartColor 变为 healFullColor
        spriteRenderer.color = Color.Lerp(healStartColor, healFullColor, lerpFactor);

        if (currentHealAmount >= healAmountToDestroy)
        {
            Destroy(gameObject); // 充能满后，销毁墙
        }
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(colorResetTime);
        spriteRenderer.color = normalColor;
    }
}
