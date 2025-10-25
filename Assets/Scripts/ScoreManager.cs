using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public void SetNewScore(int score, float time)
    {
        PlayerPrefs.SetInt("HIGHSCORE", score);
        PlayerPrefs.SetFloat("HIGHSCORE_TIME", time);
    }

}
