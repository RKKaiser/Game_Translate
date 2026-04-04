using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 成组Image替换控制器（多Image、多套图、按组/状态/进度批量切换）
/// 功能：
/// 1. 管理 N 个 Image（目标组）
/// 2. 管理 M 套图（每套 = N 张 Sprite）
/// 3. 按 组索引 / 进度(0~1) / 状态 一键整组替换
/// 4. 防越界、防空、自动初始化
/// </summary>
public class UIController_GroupImage : MonoBehaviour
{
    [Header("=== 1. 要控制的一组Image（按顺序填）===")]
    public Image[] targetImages;

    [Header("=== 2. 多套图片组（每套 = 对应上面Image数量）===")]
    [Tooltip("第0套、第1套、第2套... 每套长度必须 = targetImages.Length")]
    public Sprite[][] spriteGroups;

    [Header("=== 3. 编辑器下可视化套装（必须手动赋值）===")]
    public SpriteGroupSet[] spriteGroupSets;

    [System.Serializable]
    public class SpriteGroupSet
    {
        public string groupName = "套装0";
        public Sprite[] sprites;
    }

    [Header("=== 4. 初始显示第几套（默认0）===")]
    public int defaultGroupIndex = 0;

    private int _imageCount;
    private int _groupCount;
    private bool _inited;

    void Awake()
    {
        Init();
    }

    void OnValidate()
    {
        // 编辑器下把 spriteGroupSets 转成 spriteGroups（方便代码访问）
        if (spriteGroupSets != null)
        {
            spriteGroups = new Sprite[spriteGroupSets.Length][];
            for (int i = 0; i < spriteGroupSets.Length; i++)
            {
                spriteGroups[i] = spriteGroupSets[i].sprites;
            }
        }
        if (Application.isPlaying == false && _inited == false)
            Init();
    }

    public void Init()
    {
        if (_inited) return;

        _imageCount = targetImages.Length;
        _groupCount = spriteGroups?.Length ?? 0;

        // 安全校验
        if (_imageCount == 0)
        {
            Debug.LogError("【GroupImageController】未设置任何 targetImages！");
            return;
        }
        if (_groupCount == 0)
        {
            Debug.LogError("【GroupImageController】未设置任何 spriteGroups！");
            return;
        }
        // 校验每套图数量是否匹配
        for (int g = 0; g < _groupCount; g++)
        {
            if (spriteGroups[g] == null || spriteGroups[g].Length != _imageCount)
            {
                Debug.LogError($"【GroupImageController】第{g}套图数量不匹配！需要{_imageCount}张");
                return;
            }
        }

        // 初始显示默认套装
        if (defaultGroupIndex >= 0 && defaultGroupIndex < _groupCount)
            ChangeGroup(defaultGroupIndex);

        _inited = true;
    }

    #region ===================== 核心：成组替换 =====================
    /// <summary>
    /// 切换到第 index 套图（整组替换）
    /// </summary>
    public void ChangeGroup(int groupIndex)
    {
        if (!_inited) Init();
        if (_imageCount == 0 || _groupCount == 0) return;

        groupIndex = Mathf.Clamp(groupIndex, 0, _groupCount - 1);
        Sprite[] targetGroup = spriteGroups[groupIndex];

        for (int i = 0; i < _imageCount; i++)
        {
            if (targetImages[i] != null && targetGroup[i] != null)
            {
                targetImages[i].sprite = targetGroup[i];
                // 可选：保持原图大小，注释掉就用Image当前尺寸
                // targetImages[i].SetNativeSize();
            }
        }
    }

    /// <summary>
    /// 按进度(0~1) 切换套装（0=第0套，1=最后一套）
    /// </summary>
    public void ChangeGroupByProgress(float progress01)
    {
        progress01 = Mathf.Clamp01(progress01);
        int groupIndex = Mathf.RoundToInt(progress01 * (_groupCount - 1));
        ChangeGroup(groupIndex);
    }

    /// <summary>
    /// 下一套（循环/不循环）
    /// </summary>
    public void NextGroup(bool loop = true)
    {
        int current = GetCurrentGroupIndex();
        int next = current + 1;
        if (next >= _groupCount)
            next = loop ? 0 : _groupCount - 1;
        ChangeGroup(next);
    }

    /// <summary>
    /// 上一套（循环/不循环）
    /// </summary>
    public void PrevGroup(bool loop = true)
    {
        int current = GetCurrentGroupIndex();
        int prev = current - 1;
        if (prev < 0)
            prev = loop ? _groupCount - 1 : 0;
        ChangeGroup(prev);
    }
    #endregion

    #region ===================== 辅助 =====================
    // 粗略获取当前是第几套（只对比第一张图）
    public int GetCurrentGroupIndex()
    {
        if (!_inited || _groupCount == 0 || _imageCount == 0)
            return defaultGroupIndex;

        Sprite currentSprite = targetImages[0].sprite;
        for (int g = 0; g < _groupCount; g++)
        {
            if (spriteGroups[g][0] == currentSprite)
                return g;
        }
        return defaultGroupIndex;
    }

    // 重置为默认套装
    public void ResetToDefault()
    {
        ChangeGroup(defaultGroupIndex);
    }

    // 清空所有Image
    public void ClearAllImages()
    {
        foreach (var img in targetImages)
        {
            if (img != null) img.sprite = null;
        }
    }
    #endregion
}