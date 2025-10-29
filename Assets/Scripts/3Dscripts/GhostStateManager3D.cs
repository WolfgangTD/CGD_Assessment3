using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class GhostStateManager3D : MonoBehaviour
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
        StopAllCoroutines();
        GetComponent<GhostController3D>().isScared = true;
        StartCoroutine(ChangeGameState(scaredFor, recoveringFor));
    }
    public void Scared(float scaredFor, float recoveringFor)
    {
        StopAllCoroutines();
        GetComponent<GhostController3D>().isScared = true;
        StartCoroutine(ChangeGameState(scaredFor, recoveringFor));
    }

    public void Die()
    {
        StopAllCoroutines();
        isDead = true;
        state = 3;
        animator.SetTrigger("isDead");
        GetComponent<GhostController3D>().hasExitedSpawn = false;
    }
    public void Revive()
    {
        GetComponent<GhostController3D>().isScared = false;
        isDead = false;
        state = 0;
        animator.SetTrigger("playerNotBuffed");
    }

    
    private IEnumerator ChangeGameState(float scaredFor, float recoveringFor)
    {
        animator.ResetTrigger("isDead");
        animator.ResetTrigger("playerNotBuffed");
        animator.ResetTrigger("goToRecovery");
        animator.ResetTrigger("playerBuffed");

        state = 1; // scared
        animator.SetTrigger("playerBuffed");

        yield return new WaitForSeconds(scaredFor);

        //warning
        state = 2;
        animator.SetTrigger("goToRecovery");
        yield return new WaitForSeconds(recoveringFor);
        

        // Normal
        state = 0;
        GetComponent<GhostController3D>().isScared = false;
        animator.SetTrigger("playerNotBuffed");
        if (gameObject.name == "Ghost4")
        {
            GetComponent<GhostController3D>().ResetCornerTarget();
        }
    }
}
