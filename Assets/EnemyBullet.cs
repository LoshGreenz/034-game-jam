using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;  // 子弹速度
    public int damage = 1;    // 伤害值
    public float lifeTime = 3f; // **子弹存活时间**

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("子弹缺少 Rigidbody2D 组件！");
            return;
        }

        rb.velocity = transform.right * speed; // 🚀 让子弹朝向当前方向飞行

        // **在 `lifeTime` 秒后自动销毁子弹**
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("玩家受到了 " + damage + " 点伤害！");
            }

            Destroy(gameObject); // **击中玩家后销毁**
        }
        else if (collision.CompareTag("Wall") )
        {
            // **如果子弹碰到墙壁或者地面，也销毁**
            Destroy(gameObject);
        }
    }
}
