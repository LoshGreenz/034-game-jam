using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject circlePrefab;  // 代表 3 滴血
    public GameObject squarePrefab;  // 代表 2 滴血
    public GameObject trianglePrefab; // 代表 1 滴血

    public float iconSize = 1.5f;  // 图标大小，可调整
    public float spacing = 30f;    // 图标间距，可调整
    public Vector2 healthBarPosition = new Vector2(-300, 150); // **血条位置（可调）**

    private List<GameObject> healthIcons = new List<GameObject>(); // 存放血条图形

    void Start()
    {
        // 让血条固定在 Canvas 内，并调整到指定位置
        transform.SetParent(GameObject.Find("Canvas").transform, false);
        transform.localPosition = healthBarPosition; // **血条位置可调**
    }

    public void SetHealth(int health)
    {
        ClearHealthBar(); // 先清空血条

        int remainingHealth = health;

        while (remainingHealth >= 3) { AddHealthIcon(circlePrefab); remainingHealth -= 3; }
        while (remainingHealth >= 2) { AddHealthIcon(squarePrefab); remainingHealth -= 2; }
        while (remainingHealth >= 1) { AddHealthIcon(trianglePrefab); remainingHealth -= 1; }

        float totalWidth = (healthIcons.Count - 1) * spacing;

        for (int i = 0; i < healthIcons.Count; i++)
        {
            healthIcons[i].transform.localPosition = new Vector3(i * spacing - totalWidth / 2, 0, 0);
        }
    }

    private void AddHealthIcon(GameObject prefab)
    {
        GameObject icon = Instantiate(prefab, transform);
        icon.transform.localScale = Vector3.one * iconSize; // **调整图标大小**
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
