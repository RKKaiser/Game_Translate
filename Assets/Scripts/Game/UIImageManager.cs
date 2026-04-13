using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于管理充值轮次中 UI 图片显示、隐藏及替换的管理器
/// </summary>
public class UIImageManager: MonoBehaviour
{
    private DataManager _dataManager;

    [Header("配置参数")]
    public List<ImageReplacement> skinReplacements = new List<ImageReplacement>();
    public List<ImageReplacement> levelReplacements = new List<ImageReplacement>();

    [Header("UI 物体引用 (自动查找子物体)")]
    public GameObject mingwenParent; // 指向 Mingwen_1 物体
    public GameObject diamondParent; // 指向 Diamond_1 物体
    public GameObject petObject;     // 指向 Pet_Image 物体

    private void Awake()
    {
        _dataManager = DataManager.Instance;
    }

    /// <summary>
    /// 刷新 UI 状态
    /// 对应你要求的 Open 函数逻辑
    /// </summary>
    public void Refresh()
    {
        if (_dataManager == null) return;

        // 1. 铭文显示控制 (gameT >=1 && topUpT == 2)
        // 假设 mingwenParent 是 Mingwen_1，我们需要控制它和它的兄弟节点
        HandleGroupDisplay(mingwenParent, _dataManager.gameT >= 1 && _dataManager.topUpT == 2);

        // 2. 宝石显示控制 (gameT >=1 && topUpT == 3)
        HandleGroupDisplay(diamondParent, _dataManager.gameT >= 1 && _dataManager.topUpT == 3);

        // 3. 宠物显示控制 (gameT >=1 && topUpT == 3)
        if (petObject != null)
        {
            petObject.SetActive(_dataManager.gameT >= 1 && _dataManager.topUpT == 4);
        }

        // 4. 皮肤图片替换 (gameT >=1 && topUpT == 1)
        if (_dataManager.gameT >= 1 && _dataManager.topUpT == 1)
        {
            ReplaceImages(skinReplacements);
        }
        // 5. 等级图片替换 (gameT >=1 && topUpT == 0)
        else if (_dataManager.gameT >= 1 && _dataManager.topUpT == 0)
        {
            ReplaceImages(levelReplacements);
        }
        // 6. 默认情况：如果条件不满足，可以恢复默认或隐藏（根据需求，这里选择不处理，保持原样或由其他逻辑控制）
        // 如果需要恢复默认图片，可以在这里添加逻辑
    }

    /// <summary>
    /// 通用方法：处理一组带有 _1, _2, _3 后缀的物体的显示/隐藏
    /// </summary>
    /// <param name="sampleObject">传入其中一个物体（如 _1），用于查找同级的其他物体</param>
    /// <param name="active">是否激活</param>
    private void HandleGroupDisplay(GameObject sampleObject, bool active)
    {
        if (sampleObject == null) return;

        Transform parent = sampleObject.transform.parent;
        if (parent == null) return;

        // 查找并控制 _1, _2, _3
        for (int i = 1; i <= 3; i++)
        {
            string objName = sampleObject.name.Replace("_1", "") + "_" + i;
            Transform child = parent.Find(objName);
            if (child != null)
            {
                child.gameObject.SetActive(active);
            }
        }
    }

    /// <summary>
    /// 通用方法：替换图片素材
    /// </summary>
    /// <param name="replacements">替换列表</param>
    private void ReplaceImages(List<ImageReplacement> replacements)
    {
        foreach (var item in replacements)
        {
            if (item.targetObject != null && item.newSprite != null)
            {
                Image img = item.targetObject.GetComponent<Image>();
                if (img != null)
                {
                    img.sprite = item.newSprite;
                }
                else
                {
                    Debug.LogWarning($"物体 {item.targetObject.name} 上没有 Image 组件");
                }
            }
        }
    }
}

/// <summary>
/// 序列化类：用于在 Inspector 中配置 物体 -> 图片 的替换关系
/// </summary>
[System.Serializable]
public class ImageReplacement
{
    [Tooltip("需要替换图片的 GameObject")]
    public GameObject targetObject;
    
    [Tooltip("替换后的新 Sprite")]
    public Sprite newSprite;
}