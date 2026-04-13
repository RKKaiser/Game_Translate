using UnityEngine;
using DG.Tweening;

/// <summary>
/// ���Խ���ƽ������+�ֻ����浯��
/// ���ص�Canvas�����ṩ�����ӿڣ��޻ָ�����
/// </summary>
public class UIZoomToRight : MonoBehaviour
{
    public static UIZoomToRight Instance; // ������ȫ�ֿɵ���

    [Header("������ϷUI�����壨�������е���UI�ĸ����壩")]
    public RectTransform gameUIRoot;
    [Header("�ֻ���������壨�����ֻ�UI�ĸ����壩")]
    public RectTransform phoneUIRoot;

    [Header("��С���ã�����Inspector������")]
    public float targetScale = 0.7f; // ���Խ����������ű���
    public float targetPosX = 300f;  // ���Խ�������X���꣨���ƣ�
    public float animDuration = 2f; // ƽ������ʱ�����룩
    public Ease animEase = Ease.OutQuad; // �������ߣ��ȿ����������Ȼ��

    private bool isZoomed = false; // ����Ƿ�����С����ֹ�ظ�����

    void Awake()
    {
        // ������ʼ����ȫ��Ψһ
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // ��ʼ�����ֻ�����Ĭ�����أ����Խ���Ĭ��ԭʼ״̬
        if (phoneUIRoot != null)
        {
            phoneUIRoot.gameObject.SetActive(false);
            // �ֻ������ʼ͸����Ϊ�˵���Ч����
            CanvasGroup phoneCG = phoneUIRoot.GetComponent<CanvasGroup>();
            if (phoneCG == null) phoneCG = phoneUIRoot.gameObject.AddComponent<CanvasGroup>();
            phoneCG.alpha = 0;
        }
    }

    /// <summary>
    /// ���⹫���Ĵ����ӿڣ�ƽ����С���Խ���+�����ֻ�����
    /// ��������ű��е��ã�UIZoomToRight.Instance.TriggerZoomAndShowPhone();
    /// </summary>
    public void TriggerZoomAndShowPhone()
    {
        // ��ֹ�ظ���������С����ִ�У�
        if (isZoomed || gameUIRoot == null || phoneUIRoot == null) return;

        // ֻ���ţ�����λ�ã���Ϊ���½��Ѿ�������
        gameUIRoot.DOScale(new Vector3(targetScale, targetScale, 1), animDuration).SetEase(animEase);

        // 2. �ֻ����棺�ȼ������ƽ�����루�͵�����С����ͬ����
        phoneUIRoot.gameObject.SetActive(true);
        CanvasGroup phoneCG = phoneUIRoot.GetComponent<CanvasGroup>();
        phoneCG.DOFade(1, animDuration)
            .SetEase(animEase)
            .OnComplete(() =>
            {
                // ������ɺ���Ϊ����С
                isZoomed = true;
                Debug.Log("���Խ�������С���ֻ����浯�����");
            });
    }

    public void TriggerRestoreComputer() 
{
    // 防止在未缩小状态下触发，或者在恢复动画进行中重复触发
    if (!isZoomed || gameUIRoot == null || phoneUIRoot == null) return;

    CanvasGroup phoneCG = phoneUIRoot.GetComponent<CanvasGroup>();

    // 1. 手机界面：平滑淡出
    phoneCG.DOFade(0, animDuration)
            .SetEase(animEase)
            .OnComplete(() => 
            {
                // 动画完成后隐藏手机界面物体
                phoneUIRoot.gameObject.SetActive(false);
            });

    // 2. 电脑界面：恢复原始状态 (Scale=1, Position=0)
    // 注意：原脚本中虽然定义了targetPosX，但实际代码并未移动位置，因此这里只重置缩放
    gameUIRoot.DOScale(Vector3.one, animDuration)
              .SetEase(animEase)
              .OnComplete(() => 
              {
                  // 恢复标记位
                  isZoomed = false;
                  Debug.Log("手机界面已隐藏，电脑界面恢复完成");
              });
}
}