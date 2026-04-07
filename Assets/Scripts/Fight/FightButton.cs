using UnityEngine;

/// <summary>
/// FightButton: 石头剪刀布战斗按钮逻辑
/// 负责根据 DataManager 中的 gameT 和 topUpT 数值决定胜负
/// </summary>
public class FightButton : MonoBehaviour
{
    // 1. 声明对 DataManager 的引用
    private DataManager dataManager;

    private void Awake()
    {
        // 2. 尝试获取单例实例
        // 如果 DataManager 是 DontDestroyOnLoad 或者在场景加载前已存在，直接获取
        dataManager = DataManager.Instance;
        
        // 可选：如果获取不到报错，方便调试
        if (dataManager == null)
        {
            Debug.LogError("FightButton 找不到 DataManager 单例！请确保 DataManager 已挂载且 Instance 已初始化。");
        }
    }

    /// <summary>
    /// 该函数绑定在 UI 按钮的 onClick 事件上
    /// </summary>
    public void OnFightButtonClick()
    {
        // 确保数据管理器存在
        if (dataManager == null) return;

        // 打印当前状态用于调试（可选）
        Debug.Log($"[战斗日志] 点击战斗！当前局数(gameT): {dataManager.gameT}, 充值次数(topUpT): {dataManager.topUpT}");

        // 3. 核心胜负判定逻辑
        // 逻辑完全遵循文档：根据 gameT 和 topUpT 的组合判断
        if (dataManager.gameT == 1 && dataManager.topUpT == 0)
        {
            // 文档逻辑: 第一局且未充值 -> 胜利 (新手保护)
            GameWin();
        }
        else if (dataManager.gameT >= 2 && dataManager.topUpT == 0)
        {
            // 文档逻辑: 多局且未充值 -> 失败 (数值膨胀)
            GameLose();
        }
        else if (dataManager.gameT == 1 && dataManager.topUpT == 1)
        {
            // 文档逻辑: 第一局且充值1次 -> 胜利 (短暂强权)
            GameWin();
        }
        else if (dataManager.gameT >= 2 && dataManager.topUpT == 1)
        {
            // 文档逻辑: 多局且充值1次 -> 失败 (对手也变强了)
            GameWin();
        }
        else if (dataManager.gameT == 1 && dataManager.topUpT == 2)
        {
            // 文档逻辑: 第一局且充值2次 -> 胜利 (深度氪金)
            GameWin();
        }
        else
        {
            // 兜底逻辑：如果不符合上述任何情况（例如充值超过2次但局数异常），默认判负以诱导更多充值
            // 或者你可以根据设计决定默认行为
            GameLose();
        }
    }

    /// <summary>
    /// 胜利逻辑占位符
    /// </summary>
    private void GameWin()
    {
        Debug.Log("--- 结算：玩家获胜！---");
        // TODO: 在此处编写具体逻辑，例如：
        // 1. 播放胜利特效
        // 2. 弹出胜利面板 (UIManager.ShowWinPanel())
        // 3. 增加段位经验
    }

    /// <summary>
    /// 失败逻辑占位符
    /// </summary>
    private void GameLose()
    {
        Debug.Log("--- 结算：玩家失败... ---");
        // TODO: 在此处编写具体逻辑，例如：
    }
}