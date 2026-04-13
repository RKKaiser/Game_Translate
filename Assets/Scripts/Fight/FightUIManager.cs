using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗场景管理器：用于控制玩家和敌人角色的等级、皮肤、段位与VIP显示
/// </summary>
public class BattleSkinManager : MonoBehaviour
{
    private DataManager _dataManager;

    [Header("配置参数")]
    // 玩家相关配置
    public List<ImageReplacement> playerSkinReplacements = new List<ImageReplacement>();
    public List<ImageReplacement> playerLevelReplacements = new List<ImageReplacement>();
    
    // 敌人相关配置
    public List<ImageReplacement> enemySkinReplacements = new List<ImageReplacement>();
    public List<ImageReplacement> enemyLevelReplacements = new List<ImageReplacement>();

    [Header("战斗等级资产配置")]
    // --- 新增：玩家与敌人的资产配置 ---
    public List<RankAsset> playerRankAssets = new List<RankAsset>();
    public List<RankAsset> enemyRankAssets = new List<RankAsset>();
    
    public List<VIPAsset> playerVIPAssets = new List<VIPAsset>();
    public List<VIPAsset> enemyVIPAssets = new List<VIPAsset>();
    
    // --- UI引用：请在Inspector中拖入对应的Image物体 ---
    public GameObject playerRankTarget;   // 玩家段位显示物体
    public GameObject playerVIPTarget;    // 玩家VIP显示物体
    public GameObject enemyRankTarget;    // 敌人段位显示物体
    public GameObject enemyVIPTarget;     // 敌人VIP显示物体
    // ----------------------------------

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
    /// 刷新战斗场景中的角色状态（等级、皮肤、段位、VIP）
    /// </summary>
    public void Refresh()
    {
        if (_dataManager == null) return;

        // ==========================
        // 1. 玩家逻辑 (Player Logic)
        // ==========================
        
        // 玩家等级解锁：当充值轮次 >= 1
        if (_dataManager.topUpT >= 1)
        {
            ReplaceImages(playerLevelReplacements);
        }
        
        // 玩家皮肤解锁：当充值轮次 >= 2
        if (_dataManager.topUpT >= 2)
        {
            ReplaceImages(playerSkinReplacements);
        }

        // --- 新增：玩家段位与VIP刷新 ---
        if (playerRankTarget != null)
        {
            Sprite playerRankSprite = GetSpriteByRank(playerRankAssets, _dataManager.rank);
            UpdateImage(playerRankTarget, playerRankSprite);
        }
        
        if (playerVIPTarget != null)
        {
            Sprite playerVIPSpr = GetSpriteByVIP(playerVIPAssets, _dataManager.VIPRank);
            UpdateImage(playerVIPTarget, playerVIPSpr);
        }
        // ----------------------------

        // ==========================
        // 2. 敌人逻辑 (Enemy Logic)
        // ==========================
        
        // 敌人等级解锁逻辑
        bool enemyLevelCondition = (_dataManager.gameT >= 2 && _dataManager.topUpT == 0) || (_dataManager.topUpT >= 1);
        if (enemyLevelCondition)
        {
            ReplaceImages(enemyLevelReplacements);
        }

        // 敌人皮肤解锁逻辑
        bool enemySkinCondition = (_dataManager.gameT >= 2 && _dataManager.topUpT == 1) || (_dataManager.topUpT >= 2);
        if (enemySkinCondition)
        {
            ReplaceImages(enemySkinReplacements);
        }

        // --- 新增：敌人段位与VIP刷新 ---
        if (enemyRankTarget != null)
        {
            // 敌人段位始终和玩家一致
            Sprite enemyRankSpr = GetSpriteByRank(enemyRankAssets, _dataManager.rank);
            UpdateImage(enemyRankTarget, enemyRankSpr);
        }
        
        if (enemyVIPTarget != null)
        {
            // 敌人VIP逻辑计算
            int enemyVIPLevel;
            if (_dataManager.gameT >= 2)
            {
                enemyVIPLevel = _dataManager.VIPRank + 1; // 高一级
            }
            else
            {
                enemyVIPLevel = _dataManager.VIPRank; // 一致
            }
            
            Sprite enemyVIPSpr = GetSpriteByVIP(enemyVIPAssets, enemyVIPLevel);
            UpdateImage(enemyVIPTarget, enemyVIPSpr);
        }
        // ----------------------------
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

    // --- 新增辅助方法 ---

    /// <summary>
    /// 根据段位数值获取对应的 Sprite
    /// </summary>
    private Sprite GetSpriteByRank(List<RankAsset> assets, int currentRank)
    {
        foreach (var asset in assets)
        {
            if (asset.rankValue == currentRank)
            {
                return asset.displaySprite;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据 VIP 等级数值获取对应的 Sprite
    /// </summary>
    private Sprite GetSpriteByVIP(List<VIPAsset> assets, int currentVIPRank)
    {
        foreach (var asset in assets)
        {
            if (asset.vipLevel == currentVIPRank)
            {
                return asset.displaySprite;
            }
        }
        return null;
    }

    /// <summary>
    /// 简化 Image 组件更新的代码
    /// </summary>
    private void UpdateImage(GameObject target, Sprite sprite)
    {
        if (sprite != null)
        {
            Image img = target.GetComponent<Image>();
            if (img != null) img.sprite = sprite;
        }
    }
}

/// <summary>
/// 序列化类：用于在 Inspector 中配置 物体 -> 图片 的替换关系
/// (注：修复了原脚本中的类名拼写错误)
/// </summary>
[System.Serializable]
public class FightImageReplacement
{
    [Tooltip("需要替换图片的 GameObject")]
    public GameObject targetObject;

    [Tooltip("替换后的新 Sprite")]
    public Sprite newSprite;
}

/// <summary>
/// 段位资产配置
/// </summary>
[System.Serializable]
public class FightRankAsset
{
    [Tooltip("对应的段位数值")]
    public int rankValue;

    [Tooltip("该段位显示的图片")]
    public Sprite displaySprite;
}

/// <summary>
/// VIP等级资产配置
/// </summary>
[System.Serializable]
public class FightVIPAsset
{
    [Tooltip("对应的VIP等级数值")]
    public int vipLevel;

    [Tooltip("该VIP等级显示的图片")]
    public Sprite displaySprite;
}