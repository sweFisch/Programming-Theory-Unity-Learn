using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI scoreDisplay;
    MainManager mainManager;

    private void Awake()
    {
        scoreDisplay = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        mainManager = MainManager.Instance;

    }

    private void FixedUpdate()
    {
        if (mainManager != null)
        {
            scoreDisplay.text = $"{mainManager.score}" ;
        }
    }
}
