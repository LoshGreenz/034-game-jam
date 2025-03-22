using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // 角色移动速度
    private Rigidbody2D rb;
    private Vector2 movement;
    private float currentVelocity; // 旋转速度
    public float rotationSmoothTime = 0.1f; // 控制旋转的平滑度

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // 获取 Rigidbody2D 组件
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // 让移动更流畅
        rb.freezeRotation = true; // 防止物理系统影响旋转
    }

    void Update()
    {
        // 获取 WASD 按键输入
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D 或 ←/→ 控制 x 轴
        movement.y = Input.GetAxisRaw("Vertical");   // W/S 或 ↑/↓ 控制 y 轴
        movement = movement.normalized; // 归一化，防止斜方向速度过快
    }

    void FixedUpdate()
    {
        // 使用 Rigidbody2D 移动
        rb.velocity = movement * moveSpeed;
        
        // 让玩家朝向鼠标
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 

        Vector3 direction = (mousePos - transform.position).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // 使用 SmoothDampAngle 进行平滑旋转
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref currentVelocity, rotationSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
        }
    }
}
