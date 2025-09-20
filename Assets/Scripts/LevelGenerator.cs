using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LevelGeneratort : MonoBehaviour
{
    public List<GameObject> levelTiles = new List<GameObject>();
    public GameObject map;
    public GameObject player;
    public GameObject ghost1;
    public GameObject ghost2;
    public GameObject ghost3;
    public GameObject ghost4;
    // Start is called before the first frame update
    int[,] levelMap =
    {
    {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
    {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
    {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
    {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
    {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
    {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
    {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
    {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
    {0,0,0,0,0,2,5,4,4,0,3,4,4,8},
    {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
    {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };
    void Start()
    {
        DestroyCurrentMap(map);
        GenerateMap(levelMap);
    }
    void DestroyCurrentMap(GameObject map)
    {
        foreach (Transform child in map.GetComponentsInChildren<Transform>())
        {
            if (child != map.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    void GenerateMap(int[,] mapBlueprint)
    {
        for(int y = 0; y < mapBlueprint.GetLength(0); y++)
        {
            for(int x = 0; x < mapBlueprint.GetLength(1); x++)
            {
                Vector3 transformPos = new Vector3(x * 0.32f, -y * 0.32f, 0);
                if (y == 1 && x == 1)
                {
                    Instantiate(player, transformPos, Quaternion.identity, map.transform);
                }
                Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
            }
        }
    }
}
