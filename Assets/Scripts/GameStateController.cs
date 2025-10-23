using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    private GameObject player;
    private GameObject HUD;
    private GameObject audioController;
    private ScoreManager scoreManager;
    private GameObject[] ghosts;
    private Coroutine deadStateRoutine;
    private float buffDuration = 10f;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        HUD = GameObject.FindGameObjectWithTag("HUD");
        audioController = GameObject.FindGameObjectWithTag("AudioManager");
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
    }
    public void StartBuffState()
    {
        if (deadStateRoutine != null)
        {
            StopCoroutine(deadStateRoutine);
        }
        deadStateRoutine = StartCoroutine(ChangeGameState());
    }
    private IEnumerator ChangeGameState()
    {
        player.GetComponent<PacStudentController>().isBuffed = true;
        foreach (GameObject ghost in ghosts)
        {
            ghost.GetComponent<GhostController>().Scared();
        }

        yield return new WaitForSeconds(7f);

        foreach (GameObject ghost in ghosts)
        {
            if (!ghost.GetComponent<GhostController>().isDead)
            {
                ghost.GetComponent<GhostController>().Recovering();
            }
        }
        yield return new WaitForSeconds(buffDuration - 7f);
        foreach (GameObject ghost in ghosts)
        {
            if (!ghost.GetComponent<GhostController>().isDead)
            {
                ghost.GetComponent<GhostController>().BackToNormal();
                ghost.GetComponent<GhostController>().canGoToNormal = true;
            }
        }
        player.GetComponent<PacStudentController>().isBuffed = false;
        
        deadStateRoutine = null;
    }
}
