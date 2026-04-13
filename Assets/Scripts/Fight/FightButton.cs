using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FightButton : MonoBehaviour 
{
    private DataManager dataManager;
    
    // 引用战斗表现脚本
    public BattleVisuals battleVisuals; 

    [Header("UI 结果面板")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("按钮引用")]
    public Button btnShi; // 石头
    public Button btnJian; // 剪刀
    public Button btnBu; // 布

    private int currentPlayerChoice = -1; // 暂存玩家当前选择

    private void Awake() 
    {
        dataManager = DataManager.Instance;
        
        // 绑定按钮事件
        if (btnShi != null) btnShi.onClick.AddListener(() => OnChoiceButtonClick(0));
        if (btnJian != null) btnJian.onClick.AddListener(() => OnChoiceButtonClick(1));
        if (btnBu != null) btnBu.onClick.AddListener(() => OnChoiceButtonClick(2));
    }

    private void OnChoiceButtonClick(int choice)
    {
        currentPlayerChoice = choice;
        SetButtonsInteractable(false); // 禁用按钮防止连点

        // 1. 先进行逻辑判定（算出输赢）
        bool isWin = CalculateWinCondition();
        Debug.Log($"[逻辑层] 玩家出: {choice}, 结果: {(isWin ? "胜利" : "失败")}");

        // 2. 调用战斗表现脚本
        // 注意：这里传入一个回调函数，等动画播完了再显示UI
        battleVisuals.PlayBattle(choice, isWin,  () => OnBattleAnimationFinished(isWin));
    }

    private bool CalculateWinCondition()
    {
        // 这里复用你之前的逻辑
        if (dataManager.gameT == 1 && dataManager.topUpT == 0) return true;
        if (dataManager.gameT >= 2 && dataManager.topUpT == 0) return false;
        if (dataManager.gameT == 1 && dataManager.topUpT == 1) return true;
        if (dataManager.gameT >= 2 && dataManager.topUpT == 1) return false; // 特殊保留
        
        // 新增档位 2-5
        if (dataManager.topUpT >= 2 && dataManager.topUpT <= 5) 
        {
            return (dataManager.gameT == 1);
        }
        
        return false;
    }

    /// <summary>
    /// 战斗动画播放完毕后的回调
    /// </summary>
    private void OnBattleAnimationFinished(bool isAnimDone)
    {
        if (isAnimDone)
        {
            // 此时显示结果面板
            // 简单的延迟一下让画面更平滑
            Invoke("ShowResultPanel", 0.2f);
        }
    }

    private void ShowResultPanel()
    {
        bool isWin = CalculateWinCondition(); // 再次确认或直接存为变量
        
        if (isWin)
        {
            if (winPanel != null) winPanel.SetActive(true);
        }
        else
        {
            if (losePanel != null) losePanel.SetActive(true);
        }
    }

    private void SetButtonsInteractable(bool state)
    {
        if (btnShi != null) btnShi.interactable = state;
        if (btnJian != null) btnJian.interactable = state;
        if (btnBu != null) btnBu.interactable = state;
    }
}