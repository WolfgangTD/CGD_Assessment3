using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public bool gameOver = false;
    private GameObject player;
    private GameObject HUD;
    private GameObject audioController;
    private GameObject[] ghosts;
    public int totalPellets;
    public int lives;
    public float time;
    public int currentScore;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        HUD = GameObject.FindGameObjectWithTag("HUD");
        audioController = GameObject.FindGameObjectWithTag("AudioManager");
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        time = 0;
        currentScore = 0;
        lives = 3;
    }
    void Update()
    {
        if (HUD.GetComponent<UIManager>().countDownDone && !gameOver)
        {
            time += Time.deltaTime;
            Debug.Log(totalPellets);
        }
        if (lives == 0 || totalPellets == 0)
        {
            GameOver();
        }
    }
    void GameOver()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        GameObject levelController = GameObject.FindWithTag("LevelController");
        ScoreManager sm = gameController.GetComponent<ScoreManager>();
        LevelController lc = levelController.GetComponent<LevelController>();
        if(currentScore > PlayerPrefs.GetInt("HIGHSCORE"))
        {
            sm.SetNewScore(currentScore, time);
            lc.LoadLevel0();
        }
    }
    public void StartBuffState()
    {
        foreach(GameObject ghost in ghosts)
        {
            ghost.GetComponent<GhostController>().Scared();
        }
    }
}
