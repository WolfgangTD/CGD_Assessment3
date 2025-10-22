using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    bool cherrySpawned = false;
    public GameObject bonusCrystal;
    public GameObject spawnedCrystal;
    //spawn at x -0.3 or 8.9
    //y can be 0.35 to -9.35
    void Start()
    {
        if (!cherrySpawned)
        {
            StartCoroutine(CountFiveSeconds());
        }
    }
    void KillCrystal()
    {
        Destroy(spawnedCrystal);
        StartCoroutine(CountFiveSeconds());
    }

    IEnumerator CountFiveSeconds()
    {
        Vector3 spawnPoint = new Vector3(-0.3f, 0.35f, 0f);
        yield return new WaitForSeconds(5f);
        spawnedCrystal = Instantiate(bonusCrystal, spawnPoint, Quaternion.identity);
        cherrySpawned = true;
        StartCoroutine(MoveCherry(spawnedCrystal, 100f));
    }
    IEnumerator MoveCherry(GameObject cherry, float duration)
    {
        Vector3 endPos = new Vector3(8.9f, -9.35f, 0f);
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float timeLen = timeElapsed / duration;
            cherry.transform.position = Vector3.Lerp(cherry.transform.position, endPos, timeLen);
            
            if (!cherry.GetComponent<Renderer>().isVisible)
            {
                KillCrystal();
                break;
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
    }
}
