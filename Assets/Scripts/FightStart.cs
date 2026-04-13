using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightStart : MonoBehaviour
{
    public void StartFight() 
    { 
        // 1. 首先给 DataManager 中的 gameT 加 1
        // 假设 DataManager 使用的是标准单例模式
        if (DataManager.Instance != null)
        {
            DataManager.Instance.gameT++; // 执行 +1 操作
            Debug.Log("gameT 已增加，当前值: " + DataManager.Instance.gameT);
        }
        else
        {
            Debug.LogError("DataManager 实例未找到！");
        }

        // 2. 然后加载 Fight 场景
        SceneManager.LoadScene("Fight"); 
    } 
}
