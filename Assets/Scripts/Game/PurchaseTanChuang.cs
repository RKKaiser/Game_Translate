using UnityEngine;

public class PurchaseTanChuang : MonoBehaviour
{
    public GameObject Level_tanChuang;
    public GameObject Skin_tanChuang;
    public GameObject Pet_tanChuang;
    public GameObject Title_tanChuang;
    public GameObject Mingwen_tanChuang;
    public GameObject MainPage_tanChuang;
    public GameObject Diamond_tanChuang;
    public GameObject Greeting_tanChuang;

    private DataManager _dataManager;

    void Awake()
    {
        _dataManager = DataManager.Instance;
        CloseAllTanChuang();
    }

    // ===================================================================================
    // 打开各个弹窗
    // ===================================================================================
    public void OpenLevelTanChuang()
    {
        CloseAllTanChuang();
        Level_tanChuang.SetActive(true);
    }

    public void OpenSkinTanChuang()
    {
        CloseAllTanChuang();
        Skin_tanChuang.SetActive(true);
    }

    public void OpenPetTanChuang()
    {
        CloseAllTanChuang();
        Pet_tanChuang.SetActive(true);
    }

    public void OpenTitleTanChuang()
    {
        CloseAllTanChuang();
        Title_tanChuang.SetActive(true);
    }

    public void OpenMingwenTanChuang()
    {
        CloseAllTanChuang();
        Mingwen_tanChuang.SetActive(true);
    }

    public void OpenMainPageTanChuang()
    {
        CloseAllTanChuang();
        MainPage_tanChuang.SetActive(true);
    }

    public void OpenDiamondTanChuang()
    {
        CloseAllTanChuang();
        Diamond_tanChuang.SetActive(true);
    }
    public void OpenGreetingTanChuang()
    {
        CloseAllTanChuang();
        Greeting_tanChuang.SetActive(true);
    }

    // ===================================================================================
    // 关闭各个弹窗
    // ===================================================================================
    public void CloseLevelTanChuang() => Level_tanChuang.SetActive(false);
    public void CloseSkinTanChuang() => Skin_tanChuang.SetActive(false);
    public void ClosePetTanChuang() => Pet_tanChuang.SetActive(false);
    public void CloseTitleTanChuang() => Title_tanChuang.SetActive(false);
    public void CloseMingwenTanChuang() => Mingwen_tanChuang.SetActive(false);
    public void CloseMainPageTanChuang() => MainPage_tanChuang.SetActive(false);
    public void CloseDiamondTanChuang() => Diamond_tanChuang.SetActive(false);
    public void CloseGreetingTanChuang() => Greeting_tanChuang.SetActive(false);
    public void CloseAllTanChuang()
    {
        Level_tanChuang.SetActive(false);
        Skin_tanChuang.SetActive(false);
        Pet_tanChuang.SetActive(false);
        Title_tanChuang.SetActive(false);
        Mingwen_tanChuang.SetActive(false);
        MainPage_tanChuang.SetActive(false);
        Diamond_tanChuang.SetActive(false);
        Greeting_tanChuang.SetActive(false);
    }

    // ===================================================================================
    // 各个弹窗的购买按钮（自动修改数值 + 关闭弹窗）
    // ===================================================================================
    public void Buy_Level()
    {
        _dataManager.gameT =0;
        _dataManager.topUpT += 1;
        CloseLevelTanChuang();
    }

    public void Buy_Skin()
    {
        _dataManager.gameT =0;
        _dataManager.topUpT += 1;
        CloseSkinTanChuang();
    }

    public void Buy_Pet()
    {
        _dataManager.gameT =0;
        _dataManager.topUpT += 1;
        ClosePetTanChuang();
    }

    public void Buy_Title()
    {
        _dataManager.gameT =0;
        _dataManager.topUpT += 1;
        CloseTitleTanChuang();
    }

    public void Buy_Mingwen()
    {
        _dataManager.gameT =0;
        _dataManager.topUpT += 1;
        CloseMingwenTanChuang();
    }

    public void Buy_MainPage()
    {
        _dataManager.gameT =0;
        _dataManager.topUpT += 1;
        CloseMainPageTanChuang();
    }

    public void Buy_Diamond()
    {
        _dataManager.gameT =0;
        _dataManager.topUpT += 1;
        CloseDiamondTanChuang();
    }
    public void Buy_Greeting()
    {
        //游戏结束逻辑处理
        CloseDiamondTanChuang();
    }
}