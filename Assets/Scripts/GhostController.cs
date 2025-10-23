using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEditor.Build;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    GameObject player;
    PacStudentController playerCont;
    public float scaredFor = 7f;
    public float recoveringFor = 3f;
    public float deadFor = 3f;
    public int state = 0;
    public bool isDead = false;
    private Animator animator;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerCont = player.GetComponent<PacStudentController>();
        animator = gameObject.GetComponent<Animator>();
    }
    public void Scared()
    {
        StartCoroutine(ChangeGameState());
    }

    public void Die()
    {
        float timeAdj = playerCont.buffTime;
        if (state == 1)
        {
            if(scaredFor - timeAdj > recoveringFor)
            {
                StartCoroutine(HandleDeath());
            } else
            {
                StartCoroutine(HandleDeath3());
            }
        }
        else if (state == 2)
        {
            StartCoroutine(HandleDeath2());
        } 
    }

    private IEnumerator HandleDeath()
    {
        state = 3; // dead
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(3f); 

        state = 2;
        animator.SetTrigger("goToRecovery");
        yield return new WaitForSeconds(3f); 
    }
    private IEnumerator HandleDeath3()
    {
        state = 3; // dead
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(3f); 
        float timeAdj = playerCont.buffTime;
        float timeLeft = 10f - timeAdj;
        state = 2;
        animator.SetTrigger("goToRecovery");
        yield return new WaitForSeconds(timeLeft); 
    }
    private IEnumerator HandleDeath2()
    {
        state = 3; // dead
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(3f); 

        state = 0;
        animator.SetTrigger("playerNotBuffed");
    }

    
    private IEnumerator ChangeGameState()
    {
        state = 1; // scared
        animator.SetTrigger("playerBuffed");

        yield return new WaitForSeconds(scaredFor);
        if (state != 3)
        {
            // Recovery phase
            state = 2;
            animator.SetTrigger("goToRecovery");
            yield return new WaitForSeconds(recoveringFor);
        }
        if (state != 3)
        {
             // Normal
            state = 0;
            animator.SetTrigger("playerNotBuffed");
        }
    }
}
