using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageScaleCenter_AutoHide : MonoBehaviour
{
    [Header("放大速度设置")]
    public float scaleSpeed = 0.1f;
    [Header("最大放大倍数")]
    public float maxScale = 2f;
    [Header("自动开始放大")]
    public bool autoStart = true;

    private RectTransform _rectTrans;
    private Vector3 _originScale;
    private bool _isScaling = false;

    void Awake()
    {
        _rectTrans = GetComponent<RectTransform>();
        _originScale = _rectTrans.localScale;
        _rectTrans.pivot = new Vector2(0.5f, 0.5f);
    }

    void Start()
    {
        _isScaling = autoStart;
    }

    void Update()
    {
        if (_isScaling && scaleSpeed > 0)
        {
            ScaleByCenterAndAutoHide();
        }
    }

    private void ScaleByCenterAndAutoHide()
    {
        float currentScale = _rectTrans.localScale.x;
        float targetMaxScale = _originScale.x * maxScale;

        // 到达最大放大
        if (currentScale >= targetMaxScale)
        {
            _isScaling = false;
            StartCoroutine(HideAfterDelay(1f)); // 等待1秒再隐藏
            return;
        }

        // 放大逻辑
        Vector3 scaleStep = new Vector3(scaleSpeed, scaleSpeed, 0) * Time.deltaTime;
        _rectTrans.localScale += scaleStep;
    }

    // 延迟隐藏协程
    IEnumerator HideAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime); // 等待指定秒数
        gameObject.SetActive(false);
    }

    public void StartScaling()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            _rectTrans.localScale = _originScale;
        }
        _isScaling = true;
    }

    public void PauseScaling()
    {
        _isScaling = false;
    }

    public void ResetScale()
    {
        _rectTrans.localScale = _originScale;
        _isScaling = autoStart;
        if (!gameObject.activeSelf) gameObject.SetActive(true);
    }
}