using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 按钮点击直接跳转到名为 Start 的场景
/// 挂载到任意Button对象上即可使用
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonJumpToScene1 : MonoBehaviour
{
    private Button _jumpButton;

    void Awake()
    {
        _jumpButton = GetComponent<Button>();
        _jumpButton.onClick.AddListener(OnJumpButtonClick);
    }

    private void OnJumpButtonClick()
    {
        try
        {
            // 直接跳转到场景名字叫 Start 的场景
            SceneManager.LoadScene("Start");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"跳转到 Start 场景失败：{e.Message}\n请确认场景已加入 Build Settings", this);
        }
    }

    public void JumpToScene1()
    {
        OnJumpButtonClick();
    }
}