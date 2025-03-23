using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // 普通子弹预制体（攻击）
    public GameObject healBulletPrefab; // 加血子弹预制体（治疗）
    public Transform firePoint; // 子弹发射点
    public float bulletSpeed = 10f; // 子弹速度

    public float shootCooldown = 0.5f;
public float healCooldown = 1f;

private float shootTimer = 0f;
private float healTimer = 0f;

void Update()
{
    // 每帧递减计时器
    shootTimer -= Time.deltaTime;
    healTimer -= Time.deltaTime;

    // 左键射击普通子弹（带冷却）
    if (Input.GetMouseButtonDown(0) && shootTimer <= 0f)
    {
        Shoot(bulletPrefab);
        shootTimer = shootCooldown;
    }

    // 右键射击加血子弹（带冷却）
    if (Input.GetMouseButtonDown(1) && healTimer <= 0f)
    {
        Shoot(healBulletPrefab);
        healTimer = healCooldown;
    }
}

    void Shoot(GameObject bulletType)
    {
        // 生成子弹
        GameObject bullet = Instantiate(bulletType, firePoint.position, firePoint.rotation);

        // 设置子弹方向
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * bulletSpeed; // 子弹沿着玩家朝向方向移动
    }
}
