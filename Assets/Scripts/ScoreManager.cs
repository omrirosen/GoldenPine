using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int currentScore;
    private int pointsPerKill = 1;

    private void Awake()
    {
        currentScore = 0;
    }
    private void Start()
    {
       
        scoreText.text = currentScore.ToString();
    }
    public void AddToScore()
    {
        currentScore = currentScore + pointsPerKill;
        scoreText.text = currentScore.ToString();
    }
}
