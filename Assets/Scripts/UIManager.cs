using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 通用UI图片切换器
/// 挂载在每一个需要根据数值切换图片的物体上
/// 支持根据DataManager中的不同变量值，切换对应的Sprite
/// </summary>
public class UIManager : MonoBehaviour
{
    // 1. Inspector 配置区域
    // ========================================================================================================

    [Header("基础设置")]
    public DataManager dataManager; // 需手动拖拽引用

    [Header("变量映射")]
    [Tooltip("输入DataManager中对应的Public变量名，如: VIPRank, Rank, gameT, topUpT")]
    public string targetVariableName = "VIPRank"; // 允许Inspector修改，实现“每个脚本变量不一样”

    [Header("图片映射表")]
    [Tooltip("按数值顺序排列的图片列表，Index即为数值")]
    public Sprite[] sprites; // Element 0 对应数值 0, Element 1 对应数值 1...

    [Tooltip("当数值超出图片数组范围时显示的默认图片")]
    public Sprite defaultSprite;

    // 2. 缓存与组件引用
    // ========================================================================================================
    private Image imageComponent; // 自身的Image组件
    private int lastKnownValue = -1; // 缓存上一次读取的值，用于优化性能

    void Awake()
    {
        // 获取自身的Image组件
        imageComponent = GetComponent<Image>();
        if (imageComponent == null)
        {
            Debug.LogError($"UIManager on {name} requires an Image component!", this);
        }
    }

    void Start()
    {
        // 初始化检查
        if (dataManager == null)
        {
            Debug.LogError($"UIManager on {name} is missing DataManager reference!", this);
            enabled = false; // 停用脚本
            return;
        }

        // 初始显示
        UpdateImage();
    }

    void Update()
    {
        // 检查数据是否发生变化
        // 修正点：调用完整的方法名 GetCurrentDataValue()
        int currentValue = GetCurrentDataValue();
        
        // 只有当数值发生变化时才更新图片（性能优化）
        if (currentValue != lastKnownValue)
        {
            lastKnownValue = currentValue;
            UpdateImage();
        }
    }

    // 3. 核心逻辑方法
    // ========================================================================================================

    /// <summary>
    /// 通用的图片更新方法 (文档中明确要求包含)
    /// </summary>
    /// <param name="targetImage">要更新的目标Image组件，传入null则更新自己</param>
    /// <param name="newSprite">新的Sprite图片</param>
    public void UpdateImage(Image targetImage = null, Sprite newSprite = null)
    {
        Image target = targetImage ?? imageComponent;
        
        if (target == null) return;

        // 如果传入了特定Sprite，直接使用
        if (newSprite != null)
        {
            target.sprite = newSprite;
            return;
        }

        // 否则根据当前配置的变量值获取Sprite
        int value = GetCurrentDataValue();
        Sprite spriteToUse = GetSpriteForValue(value);

        target.sprite = spriteToUse;
        target.enabled = (spriteToUse != null); // 如果Sprite为空，隐藏Image
    }

    /// <summary>
    /// 根据当前数值获取对应的图片
    /// </summary>
    private Sprite GetSpriteForValue(int value)
    {
        // 1. 尝试根据数值索引获取
        if (sprites != null && value >= 0 && value < sprites.Length)
        {
            return sprites[value];
        }

        // 2. 数值超出范围，返回默认图片
        return defaultSprite;
    }

    /// <summary>
    /// 读取DataManager中指定变量的当前值
    /// 使用简单的条件判断来映射变量名
    /// </summary>
    private int GetCurrentDataValue()
    {
        if (dataManager == null) return 0;

        // 根据Inspector中配置的变量名，返回对应的值
        // 这里涵盖了文档中提到的所有Public变量：gameT, topUpT, VIPRank, Rank
        switch (targetVariableName.ToLower())
        {
            case "viprank":
                return dataManager.VIPRank;
            case "rank":
                return dataManager.rank;
            case "gamet":
                return dataManager.gameT;
            case "topupt":
                return dataManager.topUpT;
            // 可以在这里扩展其他变量...
            default:
                Debug.LogWarning($"UIManager: Unknown variable name '{targetVariableName}' on object {name}. Using 0.", this);
                return 0;
        }
    }
}