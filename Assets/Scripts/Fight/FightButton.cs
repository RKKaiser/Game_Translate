using UnityEngine;
using UnityEngine.UI;

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

        // 【新增】确保游戏开始时，两个面板都是隐藏的
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

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
        // 注意：这里传入 isWin，动画结束后直接用这个结果
        battleVisuals.PlayBattle(choice, isWin, () => OnBattleAnimationFinished(isWin));
    }

    private bool CalculateWinCondition()
    {
        // 这里复用你之前的逻辑
        if (dataManager.gameT == 1 && dataManager.topUpT == 0) return true;
        if (dataManager.gameT >= 2 && dataManager.topUpT == 0) return false;
        if (dataManager.gameT == 1 && dataManager.topUpT == 1) return true;
        if (dataManager.gameT >= 2 && dataManager.topUpT == 1) return false;

        // 特殊保留
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
    /// <param name="isWin">动画开始时计算好的胜负结果</param>
    private void OnBattleAnimationFinished(bool isWin)
    {
        // 此时显示结果面板
        // 简单的延迟一下让画面更平滑
        // 关键：把结果传给 ShowResultPanel
        Invoke("ShowResultPanel", 0.2f); 
        
        // 【临时存储】为了配合 Invoke 传参，我们需要一个临时变量
        // 或者更简单的做法：直接在这里调用 ShowResultPanel(isWin)
        // 但是为了不破坏你的 Invoke 延迟逻辑，我们用一个小技巧
        // 这里为了代码清晰，我建议直接调用，或者用一个临时字段存 isWin
        // 我们稍微调整一下逻辑
        StartCoroutine(DelayShowResult(isWin));
    }

    // 为了解决 Invoke 不能传参的问题，我们用协程代替
    private System.Collections.IEnumerator DelayShowResult(bool isWin)
    {
        yield return new WaitForSeconds(0.2f);
        ShowResultPanel(isWin);
    }

    /// <summary>
    /// 显示结果面板
    /// </summary>
    /// <param name="isWin">胜负结果</param>
    private void ShowResultPanel(bool isWin)
    {
        // 【关键修复】不要重新计算！直接使用传进来的结果
        // bool isWin = CalculateWinCondition(); // 【删除这行！】

        if (isWin)
        {
            if (winPanel != null)
            {
                winPanel.SetActive(true);
                Debug.Log("【UI层】显示胜利面板");
            }
        }
        else
        {
            if (losePanel != null)
            {
                losePanel.SetActive(true);
                Debug.Log("【UI层】显示失败面板"); // 加个日志看它到底进没进
            }
        }
    }

    private void SetButtonsInteractable(bool state)
    {
        if (btnShi != null) btnShi.interactable = state;
        if (btnJian != null) btnJian.interactable = state;
        if (btnBu != null) btnBu.interactable = state;
    }
}