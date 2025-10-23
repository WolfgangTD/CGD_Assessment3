using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject bonusCrystal;
    public GameObject spawnedCrystal;
    public SpriteMask mask;
    //spawn at x -0.3 or 8.9
    //y can be 0.35 to -9.35
    public void KillCrystal(GameObject crystal)
    {
        StopAllCoroutines();
        Destroy(crystal);
        StartCoroutine(CherrySpawner());
    }

    public IEnumerator CherrySpawner()
    {
        yield return new WaitForSeconds(5f);

        // Sprite mask world data
        Vector2 maskCenter = new Vector2(4.33f, -4.46f);
        float leftMax = -0.25f;
        float rightMax = 8.95f;
        float topMax = 0.4f;
        float botMax = -9.4f;
        // Randomly pick which side to spawn from
        int side = Random.Range(0, 4);
        Vector3 spawnPoint = Vector3.zero;
        switch (side)
        {
            case 0: //up
                spawnPoint = new Vector3(Random.Range(leftMax, rightMax), topMax);
                break;

            case 1: //down
                spawnPoint = new Vector3(Random.Range(leftMax, rightMax), botMax);
                break;
            case 2: //left
                spawnPoint = new Vector3(leftMax, Random.Range(botMax, topMax));
                break;
            case 3: //right
                spawnPoint = new Vector3(rightMax, Random.Range(botMax, topMax));
                break;
        }
    Vector3 endPoint = (Vector3)(2 * maskCenter - (Vector2)spawnPoint);

    spawnedCrystal = Instantiate(bonusCrystal, spawnPoint, Quaternion.identity);

    StartCoroutine(MoveCherry(spawnedCrystal, 20f, spawnPoint, endPoint));
}

    IEnumerator MoveCherry(GameObject cherry, float duration, Vector3 startPos, Vector3 endPos)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float timeLen = timeElapsed / duration;
            cherry.transform.position = Vector3.Lerp(startPos, endPos, timeLen);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        KillCrystal(cherry);
    }
}
