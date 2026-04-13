using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗场景管理器：用于控制玩家和敌人角色的等级与皮肤显示
/// </summary>
public class BattleSkinManager : MonoBehaviour
{
    private DataManager _dataManager;

    [Header("配置参数")]
    // 玩家相关
    public List<ImageReplacement> playerSkinReplacements = new List<ImageReplacement>();
    public List<ImageReplacement> playerLevelReplacements = new List<ImageReplacement>();

    // 敌人相关
    public List<ImageReplacement> enemySkinReplacements = new List<ImageReplacement>();
    public List<ImageReplacement> enemyLevelReplacements = new List<ImageReplacement>();

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
            _frameCounter = 0;
            Refresh();
        }
    }

    /// <summary>
    /// 刷新战斗场景中的角色状态（等级与皮肤）
    /// </summary>
    public void Refresh()
    {
        if (_dataManager == null) return;

        // ==========================
        // 1. 玩家逻辑 (Player Logic)
        // ==========================
        // 玩家等级解锁：topUpT >= 1
        if (_dataManager.topUpT >= 1)
        {
            ReplaceImages(playerLevelReplacements);
        }

        // 玩家皮肤解锁：topUpT >= 2
        if (_dataManager.topUpT >= 2)
        {
            ReplaceImages(playerSkinReplacements);
        }

        // ==========================
        // 2. 敌人逻辑 (Enemy Logic)
        // ==========================
        // 敌人等级解锁逻辑：
        // (gameT >= 2 && topUpT == 0) || (topUpT >= 1)
        bool enemyLevelCondition = (_dataManager.gameT >= 2 && _dataManager.topUpT == 0) || 
                                   (_dataManager.topUpT >= 1);
        if (enemyLevelCondition)
        {
            ReplaceImages(enemyLevelReplacements);
        }

        // 敌人皮肤解锁逻辑：
        // (gameT >= 2 && topUpT == 1) || (topUpT >= 2)
        bool enemySkinCondition = (_dataManager.gameT >= 2 && _dataManager.topUpT == 1) || 
                                  (_dataManager.topUpT >= 2);
        if (enemySkinCondition)
        {
            ReplaceImages(enemySkinReplacements);
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
public class BattleImageReplacement
{
    [Tooltip("需要替换图片的 GameObject")]
    public GameObject targetObject;

    [Tooltip("替换后的新 Sprite")]
    public Sprite newSprite;
}