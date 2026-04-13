using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于管理充值轮次中 UI 图片显示、隐藏及替换的管理器
/// 版本：简化父物体控制版
/// </summary>
public class UIImageManager : MonoBehaviour
{
    private DataManager _dataManager;

    [Header("配置参数")]
    public List<ImageReplacement> skinReplacements = new List<ImageReplacement>();
    public List<ImageReplacement> levelReplacements = new List<ImageReplacement>();

    [Header("UI 物体引用 (直接控制父物体)")]
    public GameObject mingwenParent; // 铭文父物体
    public GameObject diamondParent; // 宝石父物体
    public GameObject petObject;     // 宠物物体

    [Header("性能优化设置")]
    [Tooltip("每隔多少帧刷新一次 UI (例如: 10帧 ≈ 0.3秒 @ 30FPS)")]
    public int refreshIntervalFrames = 10; 

    private int _frameCounter = 0;

    private void Awake()
    {
        _dataManager = DataManager.Instance;
    }

    private void Update()
    {
        if (_dataManager == null) return;

        // 帧计数器逻辑
        _frameCounter++;
        if (_frameCounter >= refreshIntervalFrames)
        {
            _frameCounter = 0; // 重置计数器
            Refresh();         // 执行刷新
        }
    }

    /// <summary>
    /// 刷新 UI 状态
    /// </summary>
    public void Refresh()
    {
        if (_dataManager.topUpT >= 1)
        {
            ReplaceImages(levelReplacements);
        }

        if (_dataManager.topUpT >= 2)
        {
            ReplaceImages(skinReplacements);
        }
        // 铭文显示控制 (gameT >=1 && topUpT == 2)
        // 直接控制父物体显隐
        if (mingwenParent != null)
        {
            mingwenParent.SetActive(_dataManager.topUpT >= 3);
        }

        // 宝石显示控制 (gameT >=1 && topUpT == 3)
        // 直接控制父物体显隐
        if (diamondParent != null)
        {
            diamondParent.SetActive(_dataManager.topUpT >= 4);
        }

        // 宠物显示控制 (gameT >=1 && topUpT == 3)
        if (petObject != null)
        {
            petObject.SetActive(_dataManager.topUpT >= 5);
        }

    }

    /// <summary>
    /// 通用方法：替换图片素材
    /// </summary>
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