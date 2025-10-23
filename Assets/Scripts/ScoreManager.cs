using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;
    public int lives;
    public float time;
    private GameObject HUD;
    // Start is called before the first frame update
    void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");
        time = 0;
        currentScore = 0;
        lives = 3;
    }
    // Update is called once per frame
    void Update()
    {
        if(HUD.GetComponent<UIManager>().countDownDone)
        {
            time += Time.deltaTime;
        }
    }
}
