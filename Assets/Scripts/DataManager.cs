using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public int gameT; // 游戏游玩次数
    public int topUpT; // 充值次数
    public int VIPRank; 
    public int rank; 
    public int pet;
    public int inscription;

    // 新增：用于标记充值状态，判断是否是充值后的第一局
    private bool hasJustTopUp = false;

    void Awake()
    {
        // 单例模式初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 新增方法：充值逻辑
    public void OnPlayerTopUp()
    {
        topUpT += 1;
        
        // 逻辑1：每次充值 VIPRank + 1
        VIPRank += 1;
        
        // 逻辑2：标记为刚刚充值状态，等待下一局游戏触发段位提升
        hasJustTopUp = true;
    }

    // 新增方法：游戏结算逻辑（需在每局游戏结束时调用）
    public void OnGameEnd()
    {
        // 如果是充值后的第一局游戏
        if (hasJustTopUp)
        {
            rank += 1; // 段位+1
            hasJustTopUp = false; // 重置标记
        }
    }
}