using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreSetter : MonoBehaviour
{
    private GameObject gameController;
    private ScoreManager scoreManager;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    // Start is called before the first frame update
    void Start()
    {
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
    }
}
