using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 单Image 从放大状态 → 缩放到原始大小 → 等待1秒 → 自动隐藏自己
/// 用于Ending后显示后续页面
/// </summary>
[RequireComponent(typeof(Image))]
public class EndingScaleToHide : MonoBehaviour
{
    [Header("=== 缩放配置 ===")]
    [Tooltip("初始放大倍数")]
    public float startScale = 1.5f;
    [Tooltip("缩小速度")]
    public float scaleSpeed = 0.3f;
    [Tooltip("自动开始")]
    public bool autoStart = true;

    [Header("=== 延迟隐藏 ===")]
    [Tooltip("缩放到原始大小后等待多久隐藏自己")]
    public float waitBeforeHide = 1f;

    private RectTransform _rect;
    private Vector3 _originScale;
    private bool _isScaling = false;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _originScale = _rect.localScale;

        // 初始设置为放大状态
        _rect.localScale = _originScale * startScale;

        // 强制中心缩放不偏移
        _rect.pivot = new Vector2(0.5f, 0.5f);
    }

    void Start()
    {
        _isScaling = autoStart;
    }

    void Update()
    {
        if (_isScaling)
        {
            ScaleDown();
        }
    }

    void ScaleDown()
    {
        // 逐渐缩小
        _rect.localScale = Vector3.Lerp(
            _rect.localScale,
            _originScale,
            scaleSpeed * Time.deltaTime
        );

        // 接近原始大小时停止
        if (Vector3.Distance(_rect.localScale, _originScale) < 0.01f)
        {
            _rect.localScale = _originScale;
            _isScaling = false;

            // 延迟后隐藏自己
            StartCoroutine(HideAfterDelay());
        }
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(waitBeforeHide);

        // 直接隐藏自己，露出后面的UI
        gameObject.SetActive(false);
    }

    // 外部可调用：重新播放动画
    public void PlayAgain()
    {
        gameObject.SetActive(true);
        _rect.localScale = _originScale * startScale;
        _isScaling = true;
    }
}