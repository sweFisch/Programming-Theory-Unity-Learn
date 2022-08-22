using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;


    public TextMeshProUGUI playerNameInputFeild;

    private MainManager mainManager;

    private void Start()
    {
        mainManager = MainManager.Instance;
        UpdateHighScoreText();
    }

    public void UpdateHighScoreText()
    {
        highScoreText.text = mainManager.GetCurrentHighScore();
    }

    public void StartGameButton()
    {
        //get name form input field and set it in Main Manager
        mainManager.SetCurrentPlayerName(playerNameInputFeild.text);

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
