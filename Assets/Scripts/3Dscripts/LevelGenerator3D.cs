using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class LevelGenerator3D : MonoBehaviour
{
    public List<GameObject> levelTiles = new List<GameObject>();
    private GameObject map;
    public Dictionary<Vector3, string> tileMap = new Dictionary<Vector3, string>();
    public GameObject player;
    public GameObject teleporterLeft;
    public GameObject teleporterRight;
    public Vector3 teleLeftPos;
    public Vector3 teleRightPos;
    public GameObject ghost1;
    public GameObject ghost2;
    public GameObject ghost3;
    public GameObject ghost4;
    private GameStateController3D scoreManager;
    public List<Vector3> spawnPoints = new List<Vector3>();

    // Start is called before the first frame update
    int[,] levelMap =
    {
    {1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,2},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,4,4,5,3,4,4,4,3,5,3,4,4,3,5,2},
    {2,6,4,4,4,4,5,4,4,4,4,4,5,4,4,5,4,4,4,4,4,5,4,4,4,4,6,2},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,3,3,5,3,4,4,4,3,5,3,4,4,3,5,2},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
    {2,5,3,4,4,3,5,3,3,5,3,4,4,4,4,4,4,3,5,3,3,5,3,4,4,3,5,2},
    {2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2},
    {2,5,5,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,5,5,2},
    {1,2,2,2,2,1,5,4,3,4,4,3,0,4,4,0,3,4,4,3,4,5,1,2,2,2,2,1},
    {20,20,20,20,20,2,5,4,3,4,4,3,0,3,3,0,3,4,4,3,4,5,1,20,20,20,20,20},
    {20,20,20,20,20,2,5,4,4,0,0,0,0,0,0,0,0,0,0,4,4,5,2,20,20,20,20,20},
    {20,20,20,20,20,2,5,4,4,0,3,4,4,8,8,4,4,3,0,4,4,5,2,20,20,20,20,20},
    {2,2,2,2,2,1,5,3,3,0,4,20,20,11,12,20,20,4,0,3,3,5,1,2,2,2,2,2},
    {9,0,0,0,0,0,5,0,0,0,4,20,20,20,20,20,20,4,0,0,0,5,0,0,0,0,0,10},
    {2,2,2,2,2,1,5,3,3,0,4,20,20,13,14,20,20,4,0,3,3,5,1,2,2,2,2,2},
    {20,20,20,20,20,2,5,4,4,0,3,4,4,8,8,4,4,3,0,4,4,5,2,20,20,20,20,20},
    {20,20,20,20,20,2,5,4,4,0,0,0,0,0,0,0,0,0,0,4,4,5,2,20,20,20,20,20},
    {20,20,20,20,20,2,5,4,3,4,4,3,0,3,3,0,3,4,4,3,4,5,1,20,20,20,20,20},
    {1,2,2,2,2,1,5,4,3,4,4,3,0,4,4,0,3,4,4,3,4,5,1,2,2,2,2,1},
    {2,5,5,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,5,5,2},
    {2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2},
    {2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2},
    {2,5,3,4,4,3,5,3,3,5,3,4,4,4,4,4,4,3,5,3,3,5,3,4,4,3,5,2},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,3,3,5,3,4,4,4,3,5,3,4,4,3,5,2},
    {2,6,4,4,4,4,5,4,4,4,4,4,5,4,4,5,4,4,4,4,4,5,4,4,4,4,6,2},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,4,4,5,3,4,4,4,3,5,3,4,4,3,5,2},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,2},
    {1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1},
    };
    void Start()
    {
        map = GameObject.FindWithTag("Level");
        player = GameObject.FindWithTag("Player");
        scoreManager = gameObject.GetComponent<GameStateController3D>();
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
    void MapGenerationLogic(int[,] mapBlueprint)
    {
        for (int y = 0; y < mapBlueprint.GetLength(0); y++)
        {
            for (int x = 0; x < mapBlueprint.GetLength(1); x++)
            {
                Vector3 transformPos = new Vector3(x * 0.32f, 0, y * 0.32f);
                if (y == 0 && x == 0)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    tileMap.Add(transformPos, "OutsideWall");
                }
                else if (y == 1 && x == 1)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(90f,0f,0f), map.transform);
                    tileMap.Add(transformPos, "Pellet");
                    scoreManager.totalPellets++;

                    player.transform.position = transformPos;
                    if (player.GetComponent<PacStudentController3D>().spawnPoint != transformPos)
                    {
                       player.GetComponent<PacStudentController3D>().spawnPoint = transformPos;
                    }
                }
                else if (mapBlueprint[y, x] == 0)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(90f, 0f, 0f), map.transform);
                    tileMap.Add(transformPos, "Empty");
                    
                }
                else if (mapBlueprint[y, x] == 5)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(90f,0f,0f), map.transform);
                    tileMap.Add(transformPos, "Pellet");
                    scoreManager.totalPellets++;
                }
                else if (mapBlueprint[y, x] == 6)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(90f,0f,0f), map.transform);
                    tileMap.Add(transformPos, "PowerPellet");
                    scoreManager.totalPellets++;
                }
                else if (mapBlueprint[y, x] == 2)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    tileMap.Add(transformPos, "OutsideWall");
                }
                else if (mapBlueprint[y, x] == 4)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    tileMap.Add(transformPos, "Wall");
                }
                else if (mapBlueprint[y, x] == 7)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    tileMap.Add(transformPos, "Wall");
                }
                else if (mapBlueprint[y, x] == 8)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    tileMap.Add(transformPos, "GhostSpawn");
                }
                else if (mapBlueprint[y, x] == 1)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    tileMap.Add(transformPos, "OutsideWall");
                }
                else if (mapBlueprint[y, x] == 9)
                {
                    Instantiate(teleporterLeft, transformPos, Quaternion.Euler(90f, 0f, 0f), map.transform);
                    tileMap.Add(transformPos, "Empty");
                }
                else if (mapBlueprint[y, x] == 10)
                {
                    Instantiate(teleporterRight, transformPos, Quaternion.Euler(90f, 0f, 0f), map.transform);
                    teleRightPos = transformPos;
                    tileMap.Add(transformPos, "Empty");
                }
                else if (mapBlueprint[y, x] == 3)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    teleLeftPos = transformPos;
                    tileMap.Add(transformPos, "Wall");
                }
                else if (mapBlueprint[y, x] == 20)
                {
                    Instantiate(levelTiles[0], transformPos, Quaternion.Euler(90f, 0f, 0f), map.transform);
                    teleLeftPos = transformPos;
                    tileMap.Add(transformPos, "OutOfBounds");
                }
                else if(mapBlueprint[y, x] == 11)
                {
                        ghost1.transform.position = transformPos;
                        ghost1.GetComponent<GhostController3D>().spawnPoint = transformPos;
                        spawnPoints.Add(transformPos);
                        Instantiate(levelTiles[0], transformPos, Quaternion.Euler(90f, 0f, 0f), map.transform);
                        tileMap.Add(transformPos, "Empty");
                    } else if(mapBlueprint[y, x] == 12)
                    {
                        ghost3.transform.position = transformPos;
                        ghost3.GetComponent<GhostController3D>().spawnPoint = transformPos;
                        Instantiate(levelTiles[0], transformPos, Quaternion.Euler(90f, 0f, 0f), map.transform);
                        tileMap.Add(transformPos, "Empty");
                    }else if(mapBlueprint[y, x] == 13)
                    {
                        ghost2.transform.position = transformPos;
                        ghost2.GetComponent<GhostController3D>().spawnPoint = transformPos;
                        Instantiate(levelTiles[0], transformPos, Quaternion.Euler(90f, 0f, 0f), map.transform);
                        tileMap.Add(transformPos, "Empty");
                    }else if(mapBlueprint[y, x] == 14)
                    {
                        ghost4.transform.position = transformPos;
                        ghost4.GetComponent<GhostController3D>().spawnPoint = transformPos;
                        Instantiate(levelTiles[0], transformPos, Quaternion.Euler(90f, 0f, 0f), map.transform);
                        tileMap.Add(transformPos, "Empty");
                    }
            }
        }
    }

    void GenerateMap(int[,] mapBlueprint)
    {
        MapGenerationLogic(mapBlueprint);
    }
}
