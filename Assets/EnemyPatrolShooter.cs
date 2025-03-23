using System.Collections;
using UnityEngine;

public class EnemyPatrolShooter : MonoBehaviour
{
    public Transform[] waypoints; // 巡逻路径点
    public float speed = 2f; // 移动速度
    private int currentWaypointIndex = 0;
    public GameObject intention;
    public GameObject bulletPrefab; // 子弹预制体
    public Transform firePoint; // 发射位置
    public float shootInterval = 2f; // 每隔 shootInterval 秒发射一次
    public float detectionRange = 5f; // 侦测范围

    private Transform player; // 玩家引用

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("player")?.transform; // 获取玩家
        StartCoroutine(ShootRoutine()); // 开启射击协程
    }

    void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                Triangle tri = intention.GetComponent<Triangle>();
                tri.pointingXYs=player.position-transform.position;
            }
        Patrol();
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        // 移动到当前目标点
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        // 如果到达目标点，切换下一个目标
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                //Triangle tri = intention.GetComponent<Triangle>();
                //tri.pointingXYs=player.position-transform.position;
                Shoot();
            }
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // **计算朝向玩家的角度**
        //Vector2 direction = (player.position - firePoint.position).normalized;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // **生成子弹，并旋转朝向玩家**
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);



    

        // 设置子弹方向
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * 3f; // 子弹沿着玩家朝向方向移动
        // **子弹 3 秒后自动销毁**
        Destroy(bullet, 3f);
    }
}
