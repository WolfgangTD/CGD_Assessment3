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
    private GameObject cherryController;
    private GameObject[] ghosts;
    public int totalPellets;
    public int lives;
    public float time;
    public int currentScore;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        cherryController = GameObject.FindWithTag("CherryController");
        HUD = GameObject.FindGameObjectWithTag("HUD");
        audioController = GameObject.FindGameObjectWithTag("AudioManager");
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        time = 0;
        currentScore = 0;
        lives = player.GetComponent<PacStudentController>().livesLeft;
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
        ScoreManager sm = gameController.GetComponent<ScoreManager>();
        gameOver = true;
        cherryController.GetComponent<CherryController>().StopAllCoroutines();
        if(currentScore > PlayerPrefs.GetInt("HIGHSCORE") || (currentScore == PlayerPrefs.GetInt("HIGHSCORE") && time < PlayerPrefs.GetInt("HIGHSCORE_TIME")))
        {
            sm.SetNewScore(currentScore, time);
        }
        HUD.GetComponent<UIManager>().EndGame();
    }
    public void StartBuffState()
    {
        foreach(GameObject ghost in ghosts)
        {
            ghost.GetComponent<GhostController>().Scared();
        }
    }
}
