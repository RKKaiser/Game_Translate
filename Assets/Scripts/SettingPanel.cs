using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SettingPanel : MonoBehaviour
{
    public GameObject settingPanel;
    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }
    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }
    public void QuitGame()
    {
        // 在 Unity 编辑器中停止播放模式
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 在构建的应用程序中退出
            Application.Quit();
#endif
    }
}
