using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{



    public void StartGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        // # gives instruction to the compiler to only compile for that application case
        // //usefull for IOS - Android - Windows difference
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif   
    }
}
