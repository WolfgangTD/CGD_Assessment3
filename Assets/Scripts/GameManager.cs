using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerPrefs HIGHSCORE;
    PlayerPrefs HIGHSCORE_TIME;
    void Awake()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("GameManager"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
