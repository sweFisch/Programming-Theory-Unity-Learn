using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public HighScoreEntry highScore;

    public HighScoreEntry currentPlayerScore;

    public int score;


    [System.Serializable] // system.serializable is requiered for saving to json
    public class HighScoreEntry
    {
        public int score;
        public string name;
    }

    private void Awake()
    {
        // Make this an Singelton
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // load highscore from file or player prefs

        // make default player name and set score to 0
        currentPlayerScore = new HighScoreEntry();
        currentPlayerScore.score = 0;
        currentPlayerScore.name = "AAA";

        LoadCurrentHighScoreData();
    }

    private void LoadCurrentHighScoreData()
    {
        string path = Application.persistentDataPath + "/highscore.json";

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreEntry highScoreData = JsonUtility.FromJson<HighScoreEntry>(json);
            highScore.score = highScoreData.score;
            highScore.name = highScoreData.name;
        }
        else
        {
            // Load current highscore data
            highScore = new HighScoreEntry();
            highScore.score = 0;
            highScore.name = "TempPlayer";

        }
    }

    public void SaveHighScoreToJson()
    {
        string json = JsonUtility.ToJson(currentPlayerScore);
        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }

    public void SetCurrentPlayerName(string newName)
    {
        if(newName.Length < 2)
        {
            Debug.Log("no name set, using default");
            currentPlayerScore.name = "AAA";
            return;
        }
        currentPlayerScore.name = newName;
        Debug.Log("New Player Name : " + newName);
    }


    public string GetCurrentHighScore()
    {
        LoadCurrentHighScoreData();

        return $"{highScore.name} : {highScore.score}";
    }

    public void SaveCurrentPlayerScore()
    {
        // this is a bit convoluted i might want to show the current players best try 
        if (score > currentPlayerScore.score)
        {
            currentPlayerScore.score = score;
            SaveTopScore();
        }
    }

    public void SaveTopScore()
    {
        if(currentPlayerScore.score > highScore.score)
        {
            highScore.score = currentPlayerScore.score;
            highScore.name = currentPlayerScore.name;

            SaveHighScoreToJson();
        }

    }


    public void GoToMainMenue()
    {
        SceneManager.LoadScene(0);
    }

    // Compare highscore and save it if over

    // keep player name active during session

    // Limit characters in name

}
