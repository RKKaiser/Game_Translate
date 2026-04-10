using UnityEngine;
using DG.Tweening;

/// <summary>
/// 电脑界面平滑右缩+手机界面弹出
/// 挂载到Canvas，仅提供触发接口，无恢复功能
/// </summary>
public class UIZoomToRight : MonoBehaviour
{
    public static UIZoomToRight Instance; // 单例，全局可调用

    [Header("电脑游戏UI根物体（拖入所有电脑UI的父物体）")]
    public RectTransform gameUIRoot;
    [Header("手机界面根物体（拖入手机UI的父物体）")]
    public RectTransform phoneUIRoot;

    [Header("缩小配置（可在Inspector调整）")]
    public float targetScale = 0.7f; // 电脑界面最终缩放比例
    public float targetPosX = 300f;  // 电脑界面最终X坐标（右移）
    public float animDuration = 0.8f; // 平滑动画时长（秒）
    public Ease animEase = Ease.OutQuad; // 动画曲线（先快后慢，更自然）

    private bool isZoomed = false; // 标记是否已缩小，防止重复触发

    void Awake()
    {
        // 单例初始化，全局唯一
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 初始化：手机界面默认隐藏，电脑界面默认原始状态
        if (phoneUIRoot != null)
        {
            phoneUIRoot.gameObject.SetActive(false);
            // 手机界面初始透明（为了淡入效果）
            CanvasGroup phoneCG = phoneUIRoot.GetComponent<CanvasGroup>();
            if (phoneCG == null) phoneCG = phoneUIRoot.gameObject.AddComponent<CanvasGroup>();
            phoneCG.alpha = 0;
        }
    }

    /// <summary>
    /// 对外公开的触发接口：平滑缩小电脑界面+弹出手机界面
    /// 可在任意脚本中调用：UIZoomToRight.Instance.TriggerZoomAndShowPhone();
    /// </summary>
    public void TriggerZoomAndShowPhone()
    {
        // 防止重复触发（缩小后不再执行）
        if (isZoomed || gameUIRoot == null || phoneUIRoot == null) return;

        // 1. 电脑界面：平滑缩放+平滑右移（同时执行）
        gameUIRoot.DOScale(new Vector3(targetScale, targetScale, 1), animDuration)
            .SetEase(animEase);
        gameUIRoot.DOAnchorPosX(targetPosX, animDuration)
            .SetEase(animEase);

        // 2. 手机界面：先激活→再平滑淡入（和电脑缩小动画同步）
        phoneUIRoot.gameObject.SetActive(true);
        CanvasGroup phoneCG = phoneUIRoot.GetComponent<CanvasGroup>();
        phoneCG.DOFade(1, animDuration)
            .SetEase(animEase)
            .OnComplete(() =>
            {
                // 动画完成后标记为已缩小
                isZoomed = true;
                Debug.Log("电脑界面已缩小，手机界面弹出完成");
            });
    }
}