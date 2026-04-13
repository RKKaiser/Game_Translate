using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 自动弹窗触发器
/// 挂载在Game场景的任意GameObject上，用于在从Fight场景返回时根据条件自动弹出购买窗口
/// </summary>
public class AutoPopupTrigger : MonoBehaviour
{
    private PurchaseTanChuang _purchaseManager;
    private bool _hasTriggered = false; // 防止同一轮循环多次触发

    private void Awake()
    {
        // 获取场景中已有的 PurchaseTanChuang 实例
        _purchaseManager = FindObjectOfType<PurchaseTanChuang>();
        if (_purchaseManager == null)
        {
            Debug.LogError("场景中未找到 PurchaseTanChuang 脚本实例！");
        }
    }

    private void OnEnable()
    {
        // 注册场景加载完成事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 注销事件，防止内存泄漏
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 场景加载完成后的回调函数
    /// </summary>
    /// <param name="scene">加载的场景</param>
    /// <param name="mode">加载模式</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 重置触发状态，准备下一次触发
        _hasTriggered = false;
        
        // 如果加载的是Game场景（请确保场景名匹配），且尚未触发过
        if (scene.name == "Game" && !_hasTriggered)
        {
            Open();
        }
    }

    /// <summary>
    /// 核心触发函数
    /// 对应你要求的 Open 函数逻辑
    /// </summary>
    public void Open()
    {
        // 安全检查
        if (_purchaseManager == null) return;

        // 获取 DataManager 的单例实例（因为 PurchaseTanChuang 内部也是这么获取的）
        var dataManager = DataManager.Instance;
        
        // 防止重复执行
        if (_hasTriggered) return;
        _hasTriggered = true;

        // 核心逻辑：按照你的条件判断弹窗
        // 条件：gameT == 1
        if (dataManager.gameT >= 2)
        {
            switch (dataManager.topUpT)
            {
                case 0:
                    _purchaseManager.OpenLevelTanChuang();
                    break;
                case 1:
                    _purchaseManager.OpenSkinTanChuang();
                    break;
                case 2:
                    _purchaseManager.OpenMingwenTanChuang();
                    break;
                case 3:
                    _purchaseManager.OpenDiamondTanChuang();
                    break;
                case 4:
                    _purchaseManager.OpenPetTanChuang();
                    break;
                default:
                    // 如果 topUpT 大于4，不弹窗或者你可以定义其他逻辑
                    Debug.Log("topUpT 超出预设范围，不触发弹窗");
                    break;
            }
        }
        // 如果 gameT 不等于 1，不执行任何操作
    }
}