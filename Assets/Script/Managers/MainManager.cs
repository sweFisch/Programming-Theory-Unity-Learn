using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public HighScoreEntry currentHighScore;

    public HighScoreEntry currentPlayerScore;

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
        currentPlayerScore.score = 0;
        currentPlayerScore.name = "AAA";
    }

    // Compare highscore and save it if over

    // keep player name active during session

    // Limit characters in name

}
