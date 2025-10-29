using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerTweening : MonoBehaviour
{
    public Tween currentTween;
    private GameObject player;
    private Animator aniController;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        aniController = GetComponent<Animator>();

        StartCoroutine(PlayerMove());
    }

    // Update is called once per frame
    IEnumerator PlayerMove()
    {
        int counter = 0;
        while (true)
        {
            AddTween(player.transform, player.transform.position, AddDirection(counter), 3f);
            aniController.SetInteger("Direction", counter);
            float timeSince = 0f;
            while (timeSince < currentTween.Duration)
            {
                timeSince += Time.deltaTime;
                float duration = timeSince / currentTween.Duration;
                currentTween.Target.position = Vector3.Lerp(currentTween.StartPos, currentTween.EndPos, duration);
                yield return null;
            }

            currentTween.Target.position = currentTween.EndPos;

            counter++;
            if (counter > 3)
                counter = 0;
        }
    }

    void AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        currentTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
    }
    Vector3 AddDirection(int counter)
    {
        if (counter == 0)
        {
            //right
            
            return new Vector3(player.transform.position.x + 1.6f, player.transform.position.y, player.transform.position.z);
        } else if( counter == 1)
        {
            //down
            return new Vector3(player.transform.position.x, player.transform.position.y - 1.28f, player.transform.position.z);
        } else if (counter == 2)
        {
            //left
            return new Vector3(player.transform.position.x - 1.6f, player.transform.position.y, player.transform.position.z);
        }else if (counter == 3)
        {
            //up
            return new Vector3(player.transform.position.x, player.transform.position.y + 1.28f, player.transform.position.z);
        } else
        {
            Debug.Log(counter);
            return new Vector3(0, 0, 0);
        }
    }
}
