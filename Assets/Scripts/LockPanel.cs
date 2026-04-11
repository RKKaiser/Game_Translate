using System.Collections;
using UnityEngine;

public class LockPanel : MonoBehaviour
{
    public GameObject lockPanel;
    bool pd=false;
    void Start()
    {

    }
    void Update()
    {
        if(pd==false)
        {
            StartCoroutine(SelfClose());
            pd=true;
        }
    }

    // 协程必须返回 IEnumerator
    IEnumerator SelfClose()
    {
        // 等待 1 秒
        yield return new WaitForSeconds(1f);
        
        // 关闭面板
        pd=false;
        lockPanel.SetActive(false);
    }
}