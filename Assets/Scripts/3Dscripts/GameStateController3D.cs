using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class GameStateController3D : MonoBehaviour
{
    public bool gameOver = false;
    private GameObject player;
    private GameObject HUD;
    private GameObject audioController;
    private GameObject cherryController;
    private GameObject[] ghosts;
    public int totalPellets;
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
    }
    void Update()
    {
        if (HUD.GetComponent<UIManager3D>().countDownDone && !gameOver)
        {
            time += Time.deltaTime;
        }
        if (player.GetComponent<PacStudentController3D>().livesLeft == 0 || totalPellets == 0)
        {
            GameOver();
        }
    }
    void GameOver()
    {
        gameOver = true;
        cherryController.GetComponent<CherryController3D>().StopAllCoroutines();
        player.GetComponent<PacStudentController3D>().StopMovement();
        foreach(GameObject ghost in ghosts)
        {
            ghost.GetComponent<GhostController3D>().StopMovement();
        }
        HUD.GetComponent<UIManager3D>().EndGame();
    }
    
    public void StartBuffState()
    {
        HUD.GetComponent<UIManager3D>().StartGhostTimer();
        foreach(GameObject ghost in ghosts)
        {
            if (!ghost.GetComponent<GhostStateManager3D>().isDead)
            {
                ghost.GetComponent<GhostStateManager3D>().Scared();
            }
        }
        StartCoroutine(SoundManaging());
    }
    public void ResetGame()
    {
        foreach(GameObject ghost in ghosts)
        {
            ghost.GetComponent<GhostController3D>().ResetGame();
        }
    }
    IEnumerator SoundManaging()
    {
        audioController.GetComponent<SoundController3D>().ScaredState();
        List<GameObject> ghostsToWatch = new List<GameObject>();
        bool anyGhostDead = false;
        int allNormal = 0;

        while(allNormal != 4)
        {
            allNormal = 0;
            for(int i = 0; i < ghosts.Length; i++){
                if (ghosts[i].GetComponent<GhostStateManager3D>().state == 3)
                {
                    ghostsToWatch.Add(ghosts[i]);
                    if (!anyGhostDead)
                    {
                        audioController.GetComponent<SoundController3D>().DeadState();
                        anyGhostDead = true;
                    }
                }
                if (!ghostsToWatch.Any() && i == ghosts.Length)
                {
                    audioController.GetComponent<SoundController3D>().ScaredState();
                    anyGhostDead = false;
                }
                if (ghosts[i].GetComponent<GhostStateManager3D>().state == 0)
                {
                    allNormal ++;
                }
            }
            yield return null;
        }
        audioController.GetComponent<SoundController3D>().BackToNormal();
    }
}
