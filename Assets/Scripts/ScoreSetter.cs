using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreSetter : MonoBehaviour
{
    public ScoreManager scoreManager;
    public TextMeshProUGUI newHighScoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.FindWithTag("GameController").GetComponent<ScoreManager>();
        newHighScoreText.enabled = scoreManager.newHighScore;

        SetTitleValues();
    }
    void SetTitleValues()
    {
        scoreText.text = $"{PlayerPrefs.GetInt("HIGHSCORE")}";
        float time = PlayerPrefs.GetFloat("HIGHSCORE_TIME");
        int mins = (int)(time / 60);
        int secs = (int)(time % 60);
        int millisecs = (int)(time * 100 % 100);

        timeText.text = $"{mins:00}:{secs:00}:{millisecs:00}";
        if (scoreManager.newHighScore)
        {
            scoreManager.newHighScore = false;
        }
    }
}
