// #if UNITY_EDITOR
// using UnityEditor;
// #endif
// using UnityEngine;

// [RequireComponent(typeof(MeshFilter))]
// [RequireComponent(typeof(MeshRenderer))]
// public class SquareMeshGenerator : MonoBehaviour
// {
//     // Inspector 中可设置正方形的颜色和尺寸
//     public Color squareColor = Color.white;
//     public float size = 1f;

//     // 生成后的 Mesh 对象
//     public Mesh generatedMesh;

//     void Start()
//     {
//         GenerateMesh();
//     }
//     void Update(){
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
            
//             SaveMeshAsset();
//         }
//     }

//     // 生成正方形 Mesh
//     public void GenerateMesh()
//     {
//         Mesh mesh = new Mesh();
//         mesh.name = "ProceduralSquare";

//         float halfSize = size * 0.5f;
//         // 定义正方形顶点（中心在原点）
//         Vector3[] vertices = new Vector3[4]
//         {
//             new Vector3(-halfSize, -halfSize, 0), // 左下
//             new Vector3(halfSize, -halfSize, 0),  // 右下
//             new Vector3(halfSize, halfSize, 0),   // 右上
//             new Vector3(-halfSize, halfSize, 0)   // 左上
//         };
//         mesh.vertices = vertices;

//         // 定义两个三角形构成正方形（按照逆时针顺序）
//         int[] triangles = new int[6]
//         {
//             0, 1, 2,
//             0, 2, 3
//         };
//         mesh.triangles = triangles;

//         // 设置 UV 坐标
//         Vector2[] uv = new Vector2[4]
//         {
//             new Vector2(0, 0),
//             new Vector2(1, 0),
//             new Vector2(1, 1),
//             new Vector2(0, 1)
//         };
//         mesh.uv = uv;

//         // 设置顶点颜色，每个顶点使用相同颜色
//         Color[] colors = new Color[4];
//         for (int i = 0; i < colors.Length; i++)
//         {
//             colors[i] = squareColor;
//         }
//         mesh.colors = colors;

//         // 计算法线，保证光照正确
//         mesh.RecalculateNormals();

//         generatedMesh = mesh;
//         // 将生成的 Mesh 赋值给 MeshFilter 组件
//         GetComponent<MeshFilter>().mesh = generatedMesh;
//     }

//     // 使用上下文菜单命令在编辑器中保存生成的 Mesh 为资产
//     #if UNITY_EDITOR
//     [ContextMenu("保存 Mesh 资产")]
//     public void SaveMeshAsset()
//     {
//         if (generatedMesh == null)
//         {
//             Debug.LogError("Mesh 未生成，请先生成 Mesh。");
//             return;
//         }
//         // 指定保存路径，例如 Assets 目录下
//         string assetPath = "Assets/ProceduralSquare.asset";
//         AssetDatabase.CreateAsset(generatedMesh, assetPath);
//         AssetDatabase.SaveAssets();
//         Debug.Log("Mesh 资产已保存到 " + assetPath);
//     }
//     #endif
// }
