using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 自动弹窗与界面切换触发器
/// 挂载在Game场景的任意GameObject上
/// </summary>
public class AutoPopupTrigger : MonoBehaviour
{
    private PurchaseTanChuang _purchaseManager; // 购买窗口管理器引用
    private bool _hasTriggered = false; // 防止同一轮循环多次触发

    [Header("手机界面配置")]
    public float phoneDelay = 1f; // 手机界面弹出前的延时（秒）

    [Header("场景跳转配置")]
    public float endSceneDelay = 3f; // Greeting弹窗后跳转至End场景的延时（秒）

    [Header("UI管理器引用")]
    public UIZoomToRight uiZoomManager; // 拖入挂载了 UIZoomToRight 的 GameObject

    private void Awake()
    {
        // 获取场景中已有的 PurchaseTanChuang 实例
        _purchaseManager = FindObjectOfType<PurchaseTanChuang>();
        if (_purchaseManager == null)
        {
            Debug.LogError("场景中未找到 PurchaseTanChuang 脚本实例！");
        }

        // 如果 Inspector 没有拖入，则尝试自动查找
        if (uiZoomManager == null)
        {
            uiZoomManager = FindObjectOfType<UIZoomToRight>();
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

        // 如果加载的是Game场景（请确保场景名匹配）
        if (scene.name == "Game")
        {
            var dataManager = DataManager.Instance;

            // 检查是否需要触发弹窗逻辑
            if (dataManager != null && !_hasTriggered)
            {
                Open();
            }
        }
    }

    /// <summary>
    /// 处理Greeting弹窗后跳转场景的协程
    /// </summary>
    private System.Collections.IEnumerator GreetingAndJumpTo()
    {
        // 1. 弹出 Greeting 弹窗
        if (_purchaseManager != null)
        {
            _purchaseManager.OpenGreetingTanChuang(); 
        }

        // 2. 延时
        yield return new WaitForSeconds(endSceneDelay);

        // 3. 跳转至 End 场景
        Debug.Log("正在跳转至 End 场景...");
        SceneManager.LoadScene("End");
    }

    /// <summary>
    /// 核心触发函数
    /// </summary>
    public void Open()
    {
        // 安全检查
        if (_purchaseManager == null) return;

        // 防止重复执行
        if (_hasTriggered) return;
        _hasTriggered = true;

        // 获取 DataManager 的单例实例
        var dataManager = DataManager.Instance;

        if (dataManager != null)
        {
            // --- 逻辑分支 A: gameT >= 2 ---
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
                    
                    // 铭文弹窗 (topUpT == 2) -> 触发手机界面
                    case 2: 
                        _purchaseManager.OpenMingwenTanChuang();
                        StartCoroutine(TriggerPhoneInterface());
                        break;

                    // 宝石弹窗 (topUpT == 3) -> 触发手机界面
                    case 3: 
                        _purchaseManager.OpenDiamondTanChuang();
                        StartCoroutine(TriggerPhoneInterface());
                        break;

                    // 宠物弹窗 (topUpT == 4) -> 触发手机界面
                    case 4: 
                        _purchaseManager.OpenPetTanChuang();
                        StartCoroutine(TriggerPhoneInterface());
                        break;

                    // 称号弹窗 (topUpT == 5) -> 触发手机界面
                    case 5:
                        _purchaseManager.OpenTitleTanChuang();
                        StartCoroutine(TriggerPhoneInterface());
                        break;

                    default: 
                        Debug.Log("topUpT 超出预设范围"); 
                        break;
                }
            }
            // --- 逻辑分支 B: gameT == 1 ---
            else if (dataManager.gameT == 1)
            {
                // Greeting弹窗 (topUpT == 6)
                if (dataManager.topUpT == 6)
                {
                    StartCoroutine(GreetingAndJumpTo());
                }
            }
        }
    }

    /// <summary>
    /// 触发手机界面显示的协程
    /// 独立出来方便在多个 case 中复用
    /// </summary>
    private System.Collections.IEnumerator TriggerPhoneInterface()
    {
        // 安全检查
        if (uiZoomManager == null)
        {
            Debug.LogError("UIZoomToRight 引用为空！");
            yield break;
        }

        // 延时一段时间后触发手机界面弹出
        yield return new WaitForSeconds(phoneDelay);
        
        // 触发UIZoomToRight脚本中的函数
        uiZoomManager.TriggerZoomAndShowPhone();
    }
}