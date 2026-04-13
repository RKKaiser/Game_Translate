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
    public GameObject Trash1_tanChuang;
    public GameObject Trash2_tanChuang;
    public GameObject Trash3_tanChuang;
    public GameObject Trash4_tanChuang;

    private DataManager _dataManager;

    void Awake()
    {
        _dataManager = DataManager.Instance;
        CloseAllTanChuang();
    }

    // ===================================================================================
    // �򿪸�������
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
    public void OpenTrash1TanChuang()
    {
        CloseAllTanChuang();
        Trash1_tanChuang.SetActive(true);
    }
    public void OpenTrash2TanChuang()
    {
        CloseAllTanChuang();
        Trash2_tanChuang.SetActive(true);
    }
    public void OpenTrash3TanChuang()
    {
        CloseAllTanChuang();
        Trash3_tanChuang.SetActive(true);
    }
    public void OpenTrash4TanChuang()
    {
        CloseAllTanChuang();
        Trash4_tanChuang.SetActive(true);
    }

    // ===================================================================================
    // �رո�������
    // ===================================================================================
    public void CloseLevelTanChuang() => Level_tanChuang.SetActive(false);
    public void CloseSkinTanChuang() => Skin_tanChuang.SetActive(false);
    public void ClosePetTanChuang() => Pet_tanChuang.SetActive(false);
    public void CloseTitleTanChuang() => Title_tanChuang.SetActive(false);
    public void CloseMingwenTanChuang() => Mingwen_tanChuang.SetActive(false);
    public void CloseMainPageTanChuang() => MainPage_tanChuang.SetActive(false);
    public void CloseDiamondTanChuang() => Diamond_tanChuang.SetActive(false);
    public void CloseGreetingTanChuang() => Greeting_tanChuang.SetActive(false);
    public void CloseTrash1TanChuang() => Trash1_tanChuang.SetActive(false);
    public void CloseTrash2TanChuang() => Trash2_tanChuang.SetActive(false);
    public void CloseTrash3TanChuang() => Trash3_tanChuang.SetActive(false);
    public void CloseTrash4TanChuang() => Trash4_tanChuang.SetActive(false);

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
        Trash1_tanChuang.SetActive(false);
        Trash2_tanChuang.SetActive(false);
        Trash3_tanChuang.SetActive(false);
        Trash4_tanChuang.SetActive(false);
    }

    // ===================================================================================
    // ���������Ĺ���ť���Զ��޸���ֵ + �رյ�����
    // ===================================================================================
    public void Buy_Level()
    {
        _dataManager.gameT =0;
        _dataManager.OnPlayerTopUp();
        CloseLevelTanChuang();
    }

    public void Buy_Skin()
    {
        _dataManager.gameT =0;
        _dataManager.OnPlayerTopUp();
        CloseSkinTanChuang();
    }

    public void Buy_Pet()
    {
        _dataManager.gameT =0;
        _dataManager.OnPlayerTopUp();
        ClosePetTanChuang();
    }

    public void Buy_Title()
    {
        _dataManager.gameT =0;
        _dataManager.OnPlayerTopUp();
        CloseTitleTanChuang();
    }

    public void Buy_Mingwen()
    {
        _dataManager.gameT =0;
        _dataManager.OnPlayerTopUp();
        CloseMingwenTanChuang();
    }

    public void Buy_MainPage()
    {
        _dataManager.gameT =0;
       _dataManager.OnPlayerTopUp();
        CloseMainPageTanChuang();
    }

    public void Buy_Diamond()
    {
        _dataManager.gameT =0;
        _dataManager.OnPlayerTopUp();
        CloseDiamondTanChuang();
    }
    public void Buy_Greeting()
    {
        //��Ϸ�����߼�����
        CloseDiamondTanChuang();
    }
}