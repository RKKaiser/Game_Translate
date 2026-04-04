using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ImageLoopScroll : MonoBehaviour
{
    [Header("滚动速度（正数向右/向上，负数向左/向下）")]
    public float scrollSpeed = 50f;

    [Header("滚动方向")]
    public ScrollDirection direction;

    [Header("滚动区域宽度/高度（自动获取可不填）")]
    public float areaSize;

    private RectTransform _rect;
    private Vector2 _startPos;

    public enum ScrollDirection
    {
        LeftRight, // 左右滚动
        UpDown     // 上下滚动
    }

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _startPos = _rect.anchoredPosition;

        // 自动获取图片宽度/高度
        if (areaSize <= 0)
        {
            areaSize = direction == ScrollDirection.LeftRight
                ? _rect.rect.width
                : _rect.rect.height;
        }
    }

    void Update()
    {
        // 移动
        if (direction == ScrollDirection.LeftRight)
        {
            _rect.anchoredPosition += Vector2.right * scrollSpeed * Time.deltaTime;

            // 循环重置
            if (scrollSpeed > 0 && _rect.anchoredPosition.x >= _startPos.x + areaSize)
            {
                _rect.anchoredPosition = _startPos;
            }
            else if (scrollSpeed < 0 && _rect.anchoredPosition.x <= _startPos.x - areaSize)
            {
                _rect.anchoredPosition = _startPos;
            }
        }
        else
        {
            _rect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (scrollSpeed > 0 && _rect.anchoredPosition.y >= _startPos.y + areaSize)
            {
                _rect.anchoredPosition = _startPos;
            }
            else if (scrollSpeed < 0 && _rect.anchoredPosition.y <= _startPos.y - areaSize)
            {
                _rect.anchoredPosition = _startPos;
            }
        }
    }
}