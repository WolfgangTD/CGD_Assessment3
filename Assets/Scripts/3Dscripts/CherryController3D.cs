using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CherryController3D : MonoBehaviour
{
    public GameObject bonusCrystal;
    public GameObject spawnedCrystal;
    public SpriteMask mask;
    GameObject levelGen;
    //spawn at x -0.3 or 8.9
    //y can be 0.35 to -9.35
    void Start()
    {
        levelGen = GameObject.FindWithTag("LevelGenerator");
    }
    public void KillCrystal(GameObject crystal)
    {
        StopAllCoroutines();
        Destroy(crystal);
        StartCoroutine(CherrySpawner());
    }

    public IEnumerator CherrySpawner()
{
    yield return new WaitForSeconds(5f);


    List<Vector3> emptyTiles = new List<Vector3>();

    foreach (var tile in levelGen.GetComponent<LevelGenerator3D>().tileMap)
    {
        if (tile.Value == "Empty")
        {
            emptyTiles.Add(tile.Key);
        }
    }

    if (emptyTiles.Count == 0)
    {
        Debug.LogWarning("No empty tiles found to spawn cherry!");
        yield break;
    }

    Vector3 spawnPoint = emptyTiles[Random.Range(0, emptyTiles.Count)];

    spawnedCrystal = Instantiate(bonusCrystal, spawnPoint, Quaternion.identity);
    }
}
