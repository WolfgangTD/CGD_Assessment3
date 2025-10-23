using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int highScore;
    public float highScoreTime;
    // Update is called once per frame
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreTime = PlayerPrefs.GetFloat("HighScoreTime");
    }

    public void SetNewScore(int score, float time)
    {
        PlayerPrefs.SetInt("HighScore", score);
        PlayerPrefs.SetFloat("HighScoreTime", time);
    }

}
