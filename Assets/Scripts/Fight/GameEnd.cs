using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
 public void EndGame()
    {
        SoundManager.Instance.PlayButtonClickSound();
        DataManager.Instance.OnGameEnd();
        SceneManager.LoadScene("Game");
    } 
}
