using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Triangle : MonoBehaviour
{
    // 初始颜色（红色，不透明）
    public Color color = new Color(1f, 0f, 0f, 1f);
    public bool isPlayer = false;
    public Vector2 pointing;
    // 归属对象，例如环绕中心（直接使用 GameObject 类型）
    public GameObject ownerObject;
    public Camera currentCam;
    // 期望的方向（相对于 ownerObject 的方向向量）
    public Vector2 pointingXYs;
    
    // 环绕中心的距离
    public float orbitRadius = 5f;
    
    // 当前在环绕轨道上的角度位置（单位：弧度）
    public float cyclePosition = 0f;
    
    // 最大旋转速度（弧度/秒）
    public float speed = 1f;
    
    // 旋转加速度（弧度/秒²），用于初始加速
    public float rotationalAcceleration = 0.5f;

    // 协程引用，保证只启动一次
    private Coroutine pointCoroutine;

    private void Start()
    {
        CreateTriangleMesh();
        SetupMaterial();

        // 如果指定了 ownerObject，则先设置初始位置，并启动旋转协程
        if (ownerObject != null)
        {
            UpdatePositionAndRotation();
            pointCoroutine = StartCoroutine(Point());
        }
    }

    /// <summary>
    /// 生成三角形 Mesh，并赋给 MeshFilter
    /// </summary>
    private void CreateTriangleMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "TriangleMesh";
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0.5f, 0),    // 上顶点（默认指向）
            new Vector3(-1, -1, 0),  // 左下
            new Vector3(1, -1, 0)    // 右下
        };
        int[] triangles = new int[] { 0, 1, 2 };
        Color[] colors = new Color[] { color, color, color };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        GetComponent<MeshFilter>().mesh = mesh;
    }
    // public void change_camera(string tag_){


    //     currentCam=GameObject.FindGameObjectWithTag(tag_).GetComponent<Camera>();
    // }
    /// <summary>
    /// 设置材质（使用支持透明度的 Sprites/Default Shader）
    /// </summary>
    private void SetupMaterial()
    {
        Material material = new Material(Shader.Find("Sprites/Default"));
        material.color = color;
        GetComponent<MeshRenderer>().material = material;
    }

    /// <summary>
    /// 根据当前 cyclePosition 更新对象位置和朝向
    /// </summary>
    private void UpdatePositionAndRotation()
    {
        Vector3 ownerPos = ownerObject.transform.position;
        Vector3 newPos = ownerPos + new Vector3(Mathf.Cos(cyclePosition), Mathf.Sin(cyclePosition), 0) * orbitRadius;
        transform.position = newPos;
        transform.up = (newPos - ownerPos).normalized;
    }

    /// <summary>
    /// 动态修改三角形的颜色（包括 Mesh 顶点颜色和材质颜色）
    /// </summary>
    public void SetColor(Color newColor)
    {
        color = newColor;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Color[] colors = new Color[mesh.vertexCount];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = newColor;
        }
        mesh.colors = colors;
        GetComponent<MeshRenderer>().material.color = newColor;
    }

    /// <summary>
    /// 协程：根据 pointingXYs 计算目标角度，
    /// 使用初始加速模式将对象旋转至目标方向，
    /// 当误差足够小时直接设置目标角度并结束协程，
    /// 这样就不会在目标附近反复震荡。
    /// </summary>
    private IEnumerator Point()
{
    if (ownerObject == null)
        yield break;

    // 初始加速旋转
    float currentRotationSpeed = 0f;
    // 定义终止误差（弧度），例如 0.05 弧度约 3°左右
    float threshold = 0.01f;
    
    while (true)
    {
        // 计算目标角度：pointingXYs 为相对于 ownerObject 的方向向量
        float targetAngle = Mathf.Atan2(pointingXYs.y, pointingXYs.x);
        targetAngle = Mathf.Repeat(targetAngle, 2 * Mathf.PI);

        // 归一化当前角度
        cyclePosition = Mathf.Repeat(cyclePosition, 2 * Mathf.PI);

        // 计算当前角度与目标角度之间的差值（单位：弧度）
        float deltaAngle = Mathf.DeltaAngle(cyclePosition * Mathf.Rad2Deg, targetAngle * Mathf.Rad2Deg) * Mathf.Deg2Rad;

        // 如果误差足够小，认为已稳定，直接设置目标角度并重置旋转速度
        if (Mathf.Abs(deltaAngle) < threshold)
        {
            cyclePosition = targetAngle;
            currentRotationSpeed = 0f;
        }
        else
        {
            // 初始加速模式：逐渐加速，且不超过最大旋转速度
            currentRotationSpeed += rotationalAcceleration * Time.deltaTime;
            currentRotationSpeed = Mathf.Min(currentRotationSpeed, speed);
            float step = currentRotationSpeed * Time.deltaTime;
            cyclePosition += Mathf.Sign(deltaAngle) * step;
        }

        // 更新对象位置和朝向
        UpdatePositionAndRotation();

        yield return null;
    }
}



    private void Update()
    {
        if (isPlayer && ownerObject != null)
{
    //change_camera("camera2");
    //Debug.LogError(1);
    // 获取鼠标屏幕坐标
    Vector3 screenPos = Input.mousePosition;
    
    // 计算摄像机与 ownerObject 所在平面之间的距离
    //float distance = Mathf.Abs(currentCam.transform.position.z - ownerObject.transform.position.z);
    //screenPos.z = distance;
    
    // 将屏幕坐标转换为世界坐标
    screenPos.z=0;
    Vector3 mouseWorldPos = currentCam.ScreenToWorldPoint(screenPos);
    
    // 强制把鼠标世界坐标的 z 设为 ownerObject 的 z（确保在同一平面上计算）
    //mouseWorldPos.z = ownerObject.transform.position.z;
    
    // 计算从 ownerObject 指向鼠标位置的差值（只考虑 X 和 Y）
    Vector2 origin = ownerObject.transform.position;
    //Debug.LogError(currentCam.transform.position.x);
    Vector2 target = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
    pointingXYs = target - origin;
    
    if (pointingXYs.sqrMagnitude > 0f)
    {
        pointing = pointingXYs.normalized;
    }
}




    }
}
