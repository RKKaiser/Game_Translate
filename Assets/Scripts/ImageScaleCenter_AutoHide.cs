using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DualImageScaleAutoSwitch : MonoBehaviour
{
    [Header("=== 缩放设置 ===")]
    [Tooltip("放大速度")]
    public float scaleSpeed = 0.1f;
    [Tooltip("最大放大倍数")]
    public float maxScale = 2f;
    [Tooltip("自动开始播放")]
    public bool autoStart = true;

    [Header("=== 两张图片配置 ===")]
    [Tooltip("第一张图片")]
    public Image firstImage;
    [Tooltip("第二张图片")]
    public Image secondImage;

    [Header("=== 切换与场景设置 ===")]
    [Tooltip("图片放大到最大后停留多久再切换")]
    public float stayTimeAfterMaxScale = 1f;
    [Tooltip("第二张播放完成后等待多久切换场景")]
    public float waitTimeBeforeSwitchScene = 2f;
    [Tooltip("要切换到的场景名称（必须在构建设置中添加）")]
    public string nextSceneName;

    private RectTransform _currentRectTrans;
    private Vector3 _originScale;
    private bool _isScaling = false;
    private int _currentImageIndex = 1; // 1=第一张 2=第二张

    void Awake()
    {
        // 初始化：默认显示第一张，隐藏第二张
        if (firstImage != null)
        {
            firstImage.gameObject.SetActive(true);
            _currentRectTrans = firstImage.rectTransform;
        }
        if (secondImage != null)
        {
            secondImage.gameObject.SetActive(false);
        }

        // 记录原始缩放
        if (_currentRectTrans != null)
        {
            _originScale = _currentRectTrans.localScale;
            _currentRectTrans.pivot = new Vector2(0.5f, 0.5f);
        }
    }

    void Start()
    {
        _isScaling = autoStart;
    }

    void Update()
    {
        if (_isScaling && scaleSpeed > 0 && _currentRectTrans != null)
        {
            ScaleByCenter();
        }
    }

    /// <summary>
    /// 中心缩放逻辑
    /// </summary>
    private void ScaleByCenter()
    {
        float currentScale = _currentRectTrans.localScale.x;
        float targetMaxScale = _originScale.x * maxScale;

        // 到达最大放大
        if (currentScale >= targetMaxScale)
        {
            _isScaling = false;
            HandleImageSwitch(); // 处理切换逻辑
            return;
        }

        // 匀速放大
        Vector3 scaleStep = new Vector3(scaleSpeed, scaleSpeed, 0) * Time.deltaTime;
        _currentRectTrans.localScale += scaleStep;
    }

    /// <summary>
    /// 处理图片切换逻辑
    /// </summary>
    private void HandleImageSwitch()
    {
        StartCoroutine(ImageSwitchCoroutine());
    }

    IEnumerator ImageSwitchCoroutine()
    {
        // 等待最大放大后的停留时间
        yield return new WaitForSeconds(stayTimeAfterMaxScale);

        // 如果是第一张 → 切换第二张
        if (_currentImageIndex == 1)
        {
            SwitchToSecondImage();
        }
        // 如果是第二张 → 等待后切换场景
        else if (_currentImageIndex == 2)
        {
            yield return new WaitForSeconds(waitTimeBeforeSwitchScene);
            SwitchToNextScene();
        }
    }

    /// <summary>
    /// 切换到第二张图片并重新开始缩放
    /// </summary>
    private void SwitchToSecondImage()
    {
        if (firstImage == null || secondImage == null) return;

        // 隐藏第一张，显示第二张
        firstImage.gameObject.SetActive(false);
        secondImage.gameObject.SetActive(true);

        // 更新当前图片信息
        _currentRectTrans = secondImage.rectTransform;
        _currentRectTrans.pivot = new Vector2(0.5f, 0.5f);
        _currentRectTrans.localScale = _originScale; // 重置为原始大小
        _currentImageIndex = 2;

        // 重新开始缩放
        _isScaling = true;
    }

    /// <summary>
    /// 切换到下一个场景
    /// </summary>
    private void SwitchToNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("未设置要切换的场景名称！");
        }
    }

    #region 外部调用方法
    public void StartScaling()
    {
        ResetToFirstImage();
        _isScaling = true;
    }

    public void PauseScaling()
    {
        _isScaling = false;
    }

    public void ResetScale()
    {
        ResetToFirstImage();
    }
    #endregion

    /// <summary>
    /// 重置到第一张初始状态
    /// </summary>
    private void ResetToFirstImage()
    {
        if (firstImage != null)
        {
            firstImage.gameObject.SetActive(true);
            _currentRectTrans = firstImage.rectTransform;
            _currentRectTrans.localScale = _originScale;
        }
        if (secondImage != null)
        {
            secondImage.gameObject.SetActive(false);
        }

        _currentImageIndex = 1;
        _isScaling = autoStart;
    }
}