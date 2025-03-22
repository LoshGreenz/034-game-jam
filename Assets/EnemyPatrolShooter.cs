using System.Collections;
using UnityEngine;

public class EnemyPatrolShooter : MonoBehaviour
{
    public Transform[] waypoints; // 巡逻路径点
    public float speed = 2f; // 移动速度
    private int currentWaypointIndex = 0;

    public GameObject bulletPrefab; // 子弹预制体
    public Transform firePoint; // 发射位置
    public float shootInterval = 2f; // 每隔2秒发射一次

    void Start()
    {
        StartCoroutine(ShootRoutine()); // 开启射击协程
    }

    void Update()
    {
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
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }
}
