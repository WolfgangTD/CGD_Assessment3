using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class GhostStateManager : MonoBehaviour
{
    public float scaredFor = 7f;
    public float recoveringFor = 3f;
    public float deadFor = 3f;
    public bool isDead = false;
    public int state = 0;
    private Animator animator;


    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    public void Scared()
    {
        StartCoroutine(ChangeGameState());
    }

    public void Die()
    {
        StopAllCoroutines();
        isDead = true;
        state = 3;
        animator.SetTrigger("isDead");
        GetComponent<GhostController>().hasExitedSpawn = false;
    }
    public void Revive()
    {
        isDead = false;
        state = 0;
        animator.SetTrigger("playerNotBuffed");
    }

    
    private IEnumerator ChangeGameState()
    {
        state = 1; // scared
        animator.SetTrigger("playerBuffed");

        yield return new WaitForSeconds(scaredFor);

        //warning
        state = 2;
        animator.SetTrigger("goToRecovery");
        yield return new WaitForSeconds(recoveringFor);
        

        // Normal
        state = 0;
        animator.SetTrigger("playerNotBuffed");
        
    }
}
