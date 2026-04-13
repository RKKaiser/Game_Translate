using UnityEngine;
using DG.Tweening; // 需要导入 DOTween 插件
using System;
using System.Collections;

public class BattleVisuals : MonoBehaviour
{
    // 枚举定义出拳类型，方便对应数组下标
    // 0=石头, 1=剪刀, 2=布 (请确保顺序和你代码里的逻辑一致)
    public enum HandType { Rock = 0, Scissors = 1, Paper = 2 }

    [Header("UI 资产引用 (左侧玩家)")]
    public GameObject playerRock;
    public GameObject playerScissors;
    public GameObject playerPaper;

    [Header("UI 资产引用 (右侧敌人)")]
    public GameObject enemyRock;
    public GameObject enemyScissors;
    public GameObject enemyPaper;

    [Header("配置")]
    public Transform clashPoint; // 屏幕中间碰撞点，可以在 Inspector 拖入一个空物体作为中心点
    public float moveDuration = 0.4f; // 飞向中间的时间
    public float stayDuration = 0.5f; // 在中间停留多久后弹窗

    // 用于存储左右两边的图片数组，方便通过索引调用
    private GameObject[] playerHands;
    private GameObject[] enemyHands;

    private void Awake()
    {
        // 初始化数组，方便管理
        playerHands = new GameObject[] { playerRock, playerScissors, playerPaper };
        enemyHands = new GameObject[] { enemyRock, enemyScissors, enemyPaper };

        // 确保初始状态都在原位 (假设Inspector里已经摆好了位置)
        ResetPositions();
    }

    // 重置所有图片位置，准备下一局
    public void ResetPositions()
    {
        foreach (var go in playerHands)
        {
            if (go != null) go.transform.localPosition = go.transform.parent.InverseTransformPoint(go.transform.position); // 保持原位
        }
        foreach (var go in enemyHands)
        {
            if (go != null) go.transform.localPosition = go.transform.parent.InverseTransformPoint(go.transform.position);
        }
    }

    /// <summary>
    /// 入口函数：由 FightButton 调用
    /// </summary>
    /// <param name="playerHandIndex">玩家出的手势 (0石头, 1剪刀, 2布)</param>
    /// <param name="isWin">是否是胜利局</param>
    /// <param name="onFinish">动画结束后的回调</param>
    public void PlayBattle(int playerHandIndex, bool isWin, Action onFinish)
    {
        StartCoroutine(BattleRoutine(playerHandIndex, isWin, onFinish));
    }

    private IEnumerator BattleRoutine(int playerIndex, bool isWin, Action onFinish)
    {
        // 1. 计算敌人出什么
        // 如果赢：敌人出被玩家克制的 (例如玩家剪刀(1)，敌人出布(2))
        // 如果输：敌人出克制玩家的 (例如玩家剪刀(1)，敌人出石头(0))
        // 如果平局：敌人出和玩家一样的
        int enemyIndex = playerIndex; // 默认平局

        if (isWin)
        {
            // 玩家赢：敌人出“被玩家克制”的
            // 石头(0)克剪刀(1), 剪刀(1)克布(2), 布(2)克石头(0)
            // 公式：(玩家 + 1) % 3
            enemyIndex = (playerIndex + 1) % 3;
        }
        else if (!isWin) // 输的情况
        {
            // 玩家输：敌人出“克制玩家”的
            // 公式：(玩家 + 2) % 3
            enemyIndex = (playerIndex + 2) % 3;
        }

        // 2. 获取对应的 GameObject
        GameObject pHand = playerHands[playerIndex];
        GameObject eHand = enemyHands[enemyIndex];

        if (pHand == null || eHand == null)
        {
            Debug.LogError("缺少对应的图像资产！");
            onFinish?.Invoke();
            yield break;
        }

        // 3. 播放动画 (DOTween)
        // 记录原始位置，以便动画结束后归位（如果需要）
        Vector3 pOrigin = pHand.transform.position;
        Vector3 eOrigin = eHand.transform.position;
        Vector3 target = clashPoint.position;

        // 同时飞向中间
        pHand.transform.DOMove(target, moveDuration).SetEase(Ease.OutBack);
        eHand.transform.DOMove(target, moveDuration).SetEase(Ease.OutBack);

        // 等待动画完成
        yield return new WaitForSeconds(moveDuration);

        // 4. 可以在这里加一点碰撞抖动效果
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(stayDuration); // 停留一会展示结果

        // 动画完全结束后，调用回调函数（弹窗）
        seq.OnComplete(() =>
        {
            // 重置位置（可选，看你想不想让它们飞回去）
            // pHand.transform.position = pOrigin;
            // eHand.transform.position = eOrigin;

            onFinish?.Invoke();
        });
    }
}