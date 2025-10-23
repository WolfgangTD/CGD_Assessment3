using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;
    public int lives;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        currentScore = 0;
        lives = 0;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }
}
