using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class BossController : MonoBehaviour
{
    [Header("Triangle Settings")]
    public GameObject triangleObj1;
    public GameObject triangleObj2;
    public GameObject triangleObj3;

    [Header("Orbit & Rotation")]
    public float orbitRadius = 5f;
    public float baseSpeed = 45f;  // 基准旋转速度

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float burstInterval = 0.1f;

    [Header("Wandering & Health")]
    public float roamRadius = 3f;
    public float roamSpeed = 2f;
    public float health = 60f;
    public GameObject intention;

    [Header("Boss State")]
    // state == -1 表示 Boss 正在初始化（缓动动画），state == 1 表示正常状态
    public int state = -1;

    private Transform[] triangleTransforms;
    private Vector3 roamCenter;
    private Vector3 roamTarget;
    private float rotationSpeed;
    private float currentAngle;
    public Transform player;
    private bool initStarted=false;

    private void Awake()
    {
        triangleTransforms = new Transform[3]
        {
            triangleObj1.transform,
            triangleObj2.transform,
            triangleObj3.transform
        };

        rotationSpeed = baseSpeed;
        roamCenter = transform.position;
        PickNewRoamTarget();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("player")?.transform;
        // 启动随机速度与发射弹幕的协程（仅在正常状态下有效）
        StartCoroutine(RandomizeSpeedAndFire());

        // 如果状态为 -1，则执行初始化动画，让三角形缓动到目标位置和旋转
        if (state == -1)
        {
            StartCoroutine(InitializeTriangles());
        }
    }

    private void Update()
    {
        // 只有状态为 1 时，才执行旋转和游走逻辑
        if (state == 1)
        {
            Debug.Log("2");
            if (health<= 0)
            {
                Destroy(gameObject);
            }
            if (player != null)
            {
                Triangle tri = intention.GetComponent<Triangle>();
                tri.pointingXYs=player.position-transform.position;
            }

            RotateTriangles();
            Wander();
        }
         if (state == -1 && !initStarted)
    {
        initStarted = true;
        StartCoroutine(InitializeTriangles());
    }
        
    }

    // 正常状态下，三角形以 currentAngle 和 rotationSpeed 控制绕 Boss 旋转
    private void RotateTriangles()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        for (int i = 0; i < triangleTransforms.Length; i++)
        {
            float angle = currentAngle + i * 120f;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * orbitRadius;
            // 设置三角形在世界坐标中的位置：Boss 位置 + offset
            triangleTransforms[i].position = transform.position + offset;
            // 设置旋转，使三角形尖角指向 Boss 中心（加 180° 翻转）
            triangleTransforms[i].rotation = Quaternion.Euler(0f, 0f, angle + 180f);
        }
    }

    // 随机调整旋转速度并发射弹幕（仅在 state == 1 时发射）
    private IEnumerator RandomizeSpeedAndFire()
{
    while (true)
    {
        float waitTime = Random.Range(1f, 4f);
        if (health < 10f)
            waitTime *= 0.5f;
        yield return new WaitForSeconds(waitTime);

        if (state != 1)
            continue;

        // 随机调整旋转速度
        rotationSpeed = baseSpeed * Random.Range(0.5f, 2f);
        if (health < 10f)
            rotationSpeed *= 2f;

        // 计算发射数量（基础 2~6 发，血量低时翻倍）
        float t = (rotationSpeed / baseSpeed - 0.5f) / (2f - 0.5f);
        int bulletCount = Mathf.Clamp(Mathf.RoundToInt(Mathf.Lerp(2, 6, t)), 2, 6);
        if (health < 10f)
            bulletCount *= 2;

        // 计算单发子弹间隔（低血量时减半）
        float shotInterval = burstInterval;
        if (health < 10f)
            shotInterval *= 0.5f;

        foreach (var tri in triangleTransforms)
            StartCoroutine(FireBurst(tri, bulletCount, shotInterval));
    }
}

private IEnumerator FireBurst(Transform tri, int count, float interval)
{
    for (int i = 0; i < count; i++)
    {
        GameObject bullet = Instantiate(bulletPrefab, tri.position, Quaternion.identity);
        if (bullet.TryGetComponent<Rigidbody2D>(out var rb))
            rb.velocity = tri.up * bulletSpeed;

        yield return new WaitForSeconds(interval);
    }
}
    // Boss 游走逻辑：在以 roamCenter 为中心的圆形区域内随机移动
    private void Wander()
    {
        transform.position = Vector3.MoveTowards(transform.position, roamTarget, roamSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, roamTarget) < 0.1f)
            PickNewRoamTarget();
    }

    private void PickNewRoamTarget()
    {
        Vector2 offset = Random.insideUnitCircle * roamRadius;
        roamTarget = roamCenter + new Vector3(offset.x, offset.y, 0f);
    }

    // 初始化动画：在 1 秒内缓动将三个三角形移到它们应在 Boss 周围的位置和旋转，
    // 完成后将 Boss 状态切换为 1
    private IEnumerator InitializeTriangles()
{
    float moveDuration = 1f;
    float rotateDuration = 0.5f;
    float elapsed = 0f;

    Vector3[] startPos = new Vector3[3];
    Quaternion[] startRot = new Quaternion[3];
    for (int i = 0; i < 3; i++)
    {
        startPos[i] = triangleTransforms[i].position;
        startRot[i] = triangleTransforms[i].rotation;
    }

    Vector3[] targetPos = new Vector3[3];
    Quaternion[] targetRot = new Quaternion[3];
    for (int i = 0; i < 3; i++)
    {
        float angle = i * 120f;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * orbitRadius;
        targetPos[i] = transform.position + offset;
        targetRot[i] = Quaternion.Euler(0f, 0f, angle + 180f);
    }

    // Phase 1: 1 秒 Ease‑In‑Out 移动
    elapsed = 0f;
    while (elapsed < moveDuration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / moveDuration);
        float eased = Mathf.SmoothStep(0f, 1f, t);

        for (int i = 0; i < 3; i++)
        {
            triangleTransforms[i].position = Vector3.Lerp(startPos[i], targetPos[i], eased);
            triangleTransforms[i].rotation = Quaternion.Slerp(startRot[i], targetRot[i], eased);
        }
        yield return null;
    }

    // Phase 2: 0.5 秒 缓慢旋转加速
    elapsed = 0f;
    float initialAngle = currentAngle;
    float targetSpeed = baseSpeed;
    float startSpeed = 0f;

    while (elapsed < rotateDuration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / rotateDuration);
        rotationSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);

        currentAngle += rotationSpeed * Time.deltaTime;
        for (int i = 0; i < 3; i++)
        {
            float angle = currentAngle + i * 120f;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f) * orbitRadius;
            triangleTransforms[i].position = transform.position + offset;
            triangleTransforms[i].rotation = Quaternion.Euler(0f, 0f, angle + 180f);
        }
        yield return null;
    }

    // Ensure final alignment
    for (int i = 0; i < 3; i++)
    {
        triangleTransforms[i].position = targetPos[i];
        triangleTransforms[i].rotation = targetRot[i];
    }

    state = 1;
}
}
