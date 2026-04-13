using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightStart : MonoBehaviour
{
    public void StartFight()
    {
        SceneManager.LoadScene("Fight");

    }
}
