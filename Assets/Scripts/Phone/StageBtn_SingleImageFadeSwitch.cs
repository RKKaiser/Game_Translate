using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[DisallowMultipleComponent]
public class StageBtn_SingleImageFadeSwitch : MonoBehaviour
{
    [Header("=== 全局唯一目标Image ===")]
    public Image targetImage;

    [Header("=== 本Button专属轮播配置 ===")]
    public Sprite[] mySpriteGroup;
    public float singleDelay = 1f;
    public bool isLoop = true;

    [Header("=== 点击后延时多久才开始播放 ===")]
    public float startDelay = 0f;

    [Header("=== 过渡动画 ===")]
    public float fadeDuration = 0.2f;

    [Header("=== 自定义每张图延时（可选）===")]
    public bool useCustomDelay;
    public float[] customDelayList;

    private Button _selfBtn;
    private CanvasGroup _imageCg;
    private bool _isMySwitching = false;

    private void Start()
    {
        _selfBtn = GetComponent<Button>();
        InitFadeComponent();
        _selfBtn.onClick.AddListener(OnMyBtnClick);
    }

    private void InitFadeComponent()
    {
        if (targetImage == null) return;
        _imageCg = targetImage.GetComponent<CanvasGroup>();
        if (_imageCg == null)
            _imageCg = targetImage.gameObject.AddComponent<CanvasGroup>();
        _imageCg.alpha = 1f;
    }

    public void OnMyBtnClick()
    {
        if (_isMySwitching) return;
        StopAllCoroutines();
        StartCoroutine(MyImageFadeSwitchCoroutine());
    }

    private IEnumerator MyImageFadeSwitchCoroutine()
    {
        _isMySwitching = true;

        if (startDelay > 0)
            yield return new WaitForSeconds(startDelay);

        int index = 0;

        // --------------------------
        // 修复：非循环也会完整播完所有图
        // --------------------------
        while (index < mySpriteGroup.Length)
        {
            // 淡出
            yield return FadeImage(0, fadeDuration);
            // 切图
            targetImage.sprite = mySpriteGroup[index];
            // 淡入
            yield return FadeImage(1, fadeDuration);

            // 等待当前图的延时
            float delay = useCustomDelay ? customDelayList[index] : singleDelay;
            yield return new WaitForSeconds(delay);

            // 下一张
            index++;

            // 如果不循环，播到最后一张就停住
            if (!isLoop && index >= mySpriteGroup.Length)
            {
                break;
            }

            // 如果循环，回到第一张
            if (isLoop && index >= mySpriteGroup.Length)
            {
                index = 0;
            }
        }

        _isMySwitching = false;
    }

    private IEnumerator FadeImage(float targetAlpha, float duration)
    {
        if (_imageCg == null) yield break;
        float start = _imageCg.alpha;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            _imageCg.alpha = Mathf.Lerp(start, targetAlpha, t / duration);
            yield return null;
        }
        _imageCg.alpha = targetAlpha;
    }

    public void StopMySwitch()
    {
        StopAllCoroutines();
        _isMySwitching = false;
        if (_imageCg != null) _imageCg.alpha = 1;
    }

    public void OnMyBtnHide()
    {
        StopMySwitch();
    }

    private void OnDestroy()
    {
        if (_selfBtn != null)
            _selfBtn.onClick.RemoveListener(OnMyBtnClick);
        StopMySwitch();
    }
}