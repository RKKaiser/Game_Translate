using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SettingPanel : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject LockPanel;
    public void OpenSettingPanel()
    {
        SoundManager.Instance.PlayButtonClickSound();
        settingPanel.SetActive(true);
    }
    public void OpenLockPanel()
    {
        SoundManager.Instance.PlayButtonClickSound();
        LockPanel.SetActive(true);
    }
    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }
    public void QuitGame()
    {
        // �� Unity �༭����ֹͣ����ģʽ
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // �ڹ�����Ӧ�ó������˳�
            Application.Quit();
#endif
    }
}
