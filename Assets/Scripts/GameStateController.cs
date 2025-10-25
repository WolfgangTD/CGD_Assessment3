using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        player.GetComponent<PacStudentController>().StopMovement();
        if(currentScore > PlayerPrefs.GetInt("HIGHSCORE") || (currentScore == PlayerPrefs.GetInt("HIGHSCORE") && time < PlayerPrefs.GetInt("HIGHSCORE_TIME")))
        {
            sm.SetNewScore(currentScore, time);
        }
        HUD.GetComponent<UIManager>().EndGame();
    }
    
    public void StartBuffState()
    {
        HUD.GetComponent<UIManager>().StartGhostTimer();
        foreach(GameObject ghost in ghosts)
        {
            ghost.GetComponent<GhostStateManager>().Scared();
        }
        StartCoroutine(SoundManaging());
    }
    IEnumerator SoundManaging()
    {
        audioController.GetComponent<SoundController>().ScaredState();
        List<GameObject> ghostsToWatch = new List<GameObject>();
        bool anyGhostDead = false;
        int allNormal = 0;

        while(allNormal != 4)
        {
            allNormal = 0;
            for(int i = 0; i < ghosts.Length; i++){
                if (ghosts[i].GetComponent<GhostStateManager>().state == 3)
                {
                    ghostsToWatch.Add(ghosts[i]);
                    if (!anyGhostDead)
                    {
                        audioController.GetComponent<SoundController>().DeadState();
                        anyGhostDead = true;
                    }
                }
                if (!ghostsToWatch.Any() && i == ghosts.Length)
                {
                    audioController.GetComponent<SoundController>().ScaredState();
                    anyGhostDead = false;
                }
                if (ghosts[i].GetComponent<GhostStateManager>().state == 0)
                {
                    allNormal ++;
                }
            }
            yield return null;
        }
        audioController.GetComponent<SoundController>().BackToNormal();
    }
}
