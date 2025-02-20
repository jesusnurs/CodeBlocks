using System;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int _currentScore;
    
    private void Start()
    {
        ScoreSystem.Instance.OnScoreChanged += UpdateScoreUI;
    }

    private void UpdateScoreUI()
    {
        int newScore = ScoreSystem.Instance.Score;
        
        if(newScore == _currentScore)
            return;
        
        if(newScore > _currentScore)
            IncreaseScoreAnim(newScore);
        else
            DecreaseScoreAnim(newScore);
        
        _currentScore = newScore;
    }

    private void IncreaseScoreAnim(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    private void DecreaseScoreAnim(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
