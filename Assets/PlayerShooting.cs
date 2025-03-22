using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // 子弹预制体
    public Transform firePoint; // 子弹发射点（可以直接使用玩家位置）
    public float bulletSpeed = 10f; // 子弹速度

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 鼠标左键点击
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 生成子弹
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        // 设置子弹方向
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * bulletSpeed; // 子弹沿着玩家朝向方向移动
    }
}
