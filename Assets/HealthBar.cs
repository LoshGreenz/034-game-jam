using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject circlePrefab; // 圆形（3 滴血）
    public GameObject squarePrefab; // 方形（2 滴血）
    public GameObject trianglePrefab; // 三角形（1 滴血）

    private List<GameObject> healthIcons = new List<GameObject>(); // 存放血条图形

    public void SetHealth(int health)
    {
        ClearHealthBar(); // 清空旧的血条

        int remainingHealth = health;

        // 先放圆形
        while (remainingHealth >= 3)
        {
            AddHealthIcon(circlePrefab);
            remainingHealth -= 3;
        }

        // 再放方形
        while (remainingHealth >= 2)
        {
            AddHealthIcon(squarePrefab);
            remainingHealth -= 2;
        }

        // 最后放三角形
        while (remainingHealth >= 1)
        {
            AddHealthIcon(trianglePrefab);
            remainingHealth -= 1;
        }
    }

    private void AddHealthIcon(GameObject prefab)
    {
        GameObject icon = Instantiate(prefab, transform);
        icon.transform.localPosition = new Vector3(healthIcons.Count * 0.5f, 0, 0); // 横向排列
        healthIcons.Add(icon);
    }

    private void ClearHealthBar()
    {
        foreach (GameObject icon in healthIcons)
        {
            Destroy(icon);
        }
        healthIcons.Clear();
    }
}
