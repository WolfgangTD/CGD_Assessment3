using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public bool isDead = false;
    public bool canGoToNormal = false;
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    public void Scared()
    {
        if (isDead)
        {
            StopAllCoroutines();
        }
        animator.ResetTrigger("goToRecovery");
        animator.ResetTrigger("isDead");
        animator.SetTrigger("playerBuffed");
    }

    public void Recovering()
    {
        if (!isDead)
        {
            animator.SetTrigger("goToRecovery");
        }
    }

    public void BackToNormal()
    {
        if (!isDead)
        {
            canGoToNormal = false;
            animator.ResetTrigger("playerBuffed");
            animator.ResetTrigger("goToRecovery");
            animator.SetTrigger("playerNotBuffed");
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            StartCoroutine(DeathCountdown());
        }
        else
        {
            return;
        }
    }

    private IEnumerator DeathCountdown()
    {
        isDead = true;
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(3);
        isDead = false;
        animator.ResetTrigger("isDead");

        if (canGoToNormal)
        {
            yield return new WaitUntil(() => canGoToNormal);
            animator.SetTrigger("ForceRevive");
            BackToNormal();
        }
    }
}
