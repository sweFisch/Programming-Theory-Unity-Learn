using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int scoreFromCollectables;
    public int scoreFromDistance;
    public int currentScore;

    MainManager mainManager;

    // if player have lives save and add scores during the session..

    private void Start()
    {
        ResetScore();

        mainManager = MainManager.Instance;
    }

    private void Update()
    {
        scoreFromDistance = (int)transform.position.z;
        currentScore = scoreFromDistance + scoreFromCollectables;

        if(mainManager != null)
        {
            mainManager.score = currentScore;
        }
    }

    public void ScoreValueForCollectable(int value)
    {
        scoreFromCollectables += value;
    }

    public void ResetScore()
    {
        scoreFromCollectables = 0;
        scoreFromDistance = 0;
    }

}
