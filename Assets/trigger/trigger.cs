using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchCameraOnTrigger : MonoBehaviour
{
    private Canvas canvas;
    private GameObject canvasGO;
    // 当另一个 Collider 进入本对象的 Trigger 时被调用
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogWarning("1");
        // 只在碰撞到玩家或你想要的对象时切换（可选）
        if (!other.gameObject.CompareTag("player")) return; 
        //Debug.LogWarning("2");
        // 找到 Tag 为 "camera2" 的摄像机 GameObject
        GameObject camObj = GameObject.FindGameObjectWithTag("camera2");
        if (camObj == null)
        {
            Debug.LogWarning("未找到 Tag 为 camera2 的摄像机！");
            return;
        }

        Camera newCam = camObj.GetComponent<Camera>();
        
        if (newCam == null)
        {
            Debug.LogWarning("camera2 对象上没有 Camera 组件！");
            return;
        }
        GameObject playerIntent = GameObject.FindGameObjectWithTag("player_intention");
        Triangle tri = playerIntent.GetComponent<Triangle>();

        GameObject boss1 = GameObject.FindGameObjectWithTag("boss1_center");
if (boss1 == null)
{
    Debug.LogError("找不到 Tag 为 boss1_center 的对象！请检查 Tag 是否正确");
    newCam.enabled=false;
    Camera c1 =GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    c1.enabled=true;
    tri.currentCam=c1;
   canvasGO = GameObject.FindGameObjectWithTag("canvas");
    
if (canvasGO == null)
{
    Debug.LogWarning("未找到 Tag 为 'canvas' 的对象！");
    return;
}

// 获取 Canvas 组件
canvas = canvasGO.GetComponent<Canvas>();
if (canvas == null)
{
    Debug.LogWarning("Canvas 对象上没有 Canvas 组件！");
    return;
}

// 强制将 Canvas 渲染模式设为 Screen Space - Camera（如果尚未设置）
canvas.renderMode = RenderMode.ScreenSpaceCamera;

// 把 Canvas 的摄像机替换成 currentCam
canvas.worldCamera = c1;

    return;
}

BossController boss = boss1.GetComponent<BossController>();
if (boss == null)
{
    Debug.LogError("boss1_center 对象上没有 BossController 组件！");
    return;
}
if (boss.state!=0){
    return;
}
// 这里才安全地修改实例字段
boss.state = -1;
        
    tri.currentCam=GameObject.FindGameObjectWithTag("camera2").GetComponent<Camera>();
    //tri.change_camera("camera2");
    Debug.Log("changed");
        // 关闭场景中所有摄像机，然后启用新摄像机
        foreach (Camera cam in Camera.allCameras)
        {
            cam.enabled = false;
        }
        newCam.enabled = true;
        // 在任何需要切换 Canvas 摄像机的地方（例如子弹碰撞、状态变化后等）调用

// 找到场景中 Tag 为 "canvas" 的 GameObject
canvasGO = GameObject.FindGameObjectWithTag("canvas");
if (canvasGO == null)
{
    Debug.LogWarning("未找到 Tag 为 'canvas' 的对象！");
    return;
}

// 获取 Canvas 组件
canvas = canvasGO.GetComponent<Canvas>();
if (canvas == null)
{
    Debug.LogWarning("Canvas 对象上没有 Canvas 组件！");
    return;
}

// 强制将 Canvas 渲染模式设为 Screen Space - Camera（如果尚未设置）
canvas.renderMode = RenderMode.ScreenSpaceCamera;

// 把 Canvas 的摄像机替换成 currentCam
canvas.worldCamera = newCam;






    }
    




}