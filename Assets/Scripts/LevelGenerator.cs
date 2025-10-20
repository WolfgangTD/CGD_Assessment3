using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class LevelGeneratort : MonoBehaviour
{
    public List<GameObject> levelTiles = new List<GameObject>();
    public GameObject map;
    public Dictionary<Vector3, string> tileMap = new Dictionary<Vector3, string>();

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
    public float GenerateCorner(int y, int x, int[,] mapBlueprint)
    {
        bool up = (y > 0) && (mapBlueprint[y - 1, x] == 4 || mapBlueprint[y - 1, x] == 3);
        bool down = (y < mapBlueprint.GetLength(0) - 1) && (mapBlueprint[y + 1, x] == 4 || mapBlueprint[y + 1, x] == 3);
        bool left = (x > 0) && (mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3);
        bool right = (x < mapBlueprint.GetLength(1) - 1) && (mapBlueprint[y, x + 1] == 4 || mapBlueprint[y, x + 1] == 3);

        if (up && right && !down && !left) return 90f;
        else if (up && left && !right && !down) return 180f;
        else if (down && right && !left && !up) return 0f;
        else if (down && left && !right && !up) return 270f;
        else if (up && !left && !right && !down) return 90f;
        else if (up && !left && !right && !down && x == mapBlueprint.GetLength(1)) return 90f;
        else if (!up && !left && !right && down && x == mapBlueprint.GetLength(1)) return 0f;
        else if (up && !left && !right && !down && x == 0) return 180f;
        else if (!up && !left && !right && down && x == 0) return 270f;

        if ((y > 0) && (mapBlueprint[y - 1, x] == 3)) { up = false; }
        if ((y < mapBlueprint.GetLength(0) - 1) && (mapBlueprint[y + 1, x] == 3)) { down = false; }
        if ((x > 0) && (mapBlueprint[y, x - 1] == 3)) { left = false; }
        if ((x < mapBlueprint.GetLength(1) - 1) && (mapBlueprint[y, x + 1] == 3)) { right = false; }

        if ((up && left && right))
        {
            if (mapBlueprint[y - 2, x] != 4 && mapBlueprint[y - 2, x] != 3)
            {
                up = false;
            }
            if (mapBlueprint[y, x - 2] != 4 && mapBlueprint[y, x - 2] != 3)
            {
                left = false;
            }
            if (mapBlueprint[y, x + 2] != 4 && mapBlueprint[y, x + 2] != 3)
            {
                right = false;
            }
        }
        else if (down && left && right)
        {
            if (mapBlueprint[y, x - 2] != 4 && mapBlueprint[y, x - 2] != 3)
            {
                left = false;
            }
            if (mapBlueprint[y, x + 2] != 4 && mapBlueprint[y, x + 2] != 3)
            {
                right = false;
            }
            if (mapBlueprint[y + 2, x] != 4 && mapBlueprint[y + 2, x] != 3)
            {
                down = false;
            }
        }
        else if (up && down && right)
        {
            if (mapBlueprint[y - 2, x] != 4 && mapBlueprint[y - 2, x] != 3)
            {
                up = false;
            }
            if (mapBlueprint[y, x + 2] != 4 && mapBlueprint[y, x + 2] != 3)
            {
                right = false;
            }
            if (mapBlueprint[y + 2, x] != 4 && mapBlueprint[y + 2, x] != 3)
            {
                down = false;
            }
        }
        else if (up && down && left)
        {
            if (mapBlueprint[y - 2, x] != 4 && mapBlueprint[y - 2, x] != 3)
            {
                up = false;
            }
            if (mapBlueprint[y, x - 2] != 4 && mapBlueprint[y, x - 2] != 3)
            {
                left = false;
            }
            if (mapBlueprint[y + 2, x] != 4 && mapBlueprint[y + 2, x] != 3)
            {
                down = false;
            }
        }

        if (up && right && !down && !left) return 90f;
        else if (up && left && !right && !down) return 180f;
        else if (down && right && !left && !up) return 0f;
        else if (down && left && !right && !up) return 270f;
        else if (up && !left && !right && !down) return 90f;

        UnityEngine.Debug.Log(up + "+" + down + "+" + left + "+" + right);
        return -45f;
    }
    void MapGenerationLogic2(int[,] mapBlueprint)
    {
        for (int y = 0; y < mapBlueprint.GetLength(0); y++)
        {
            int readX = 0;
            for (int x = mapBlueprint.GetLength(1)-1; x >= 0; x--)
            {
                Vector3 transformPos = new Vector3((mapBlueprint.GetLength(1) + readX) * 0.32f, -y * 0.32f, 0);
                if (y == 0 && x == 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f,0f,270f), map.transform);
                }
                if (mapBlueprint[y, x] == 0 )
                {
                    tileMap.TryAdd(transformPos, "Empty");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 5)
                {
                    tileMap.TryAdd(transformPos, "Pellet");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 6)
                {
                    tileMap.TryAdd(transformPos, "PowerPellet");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 2 && x > 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                else if (mapBlueprint[y, x] == 2 && x <= 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                if (mapBlueprint[y, x] == 4)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (x > 0 && x < mapBlueprint.GetLength(1) - 1)
                    {
                        if ((mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3 || mapBlueprint[y, x - 1] == 8) && (mapBlueprint[y, x + 1] == 4 || mapBlueprint[y, x + 1] == 3 || mapBlueprint[y, x + 1] == 8))
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                        }
                        else
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                        }
                    }
                    if (x == mapBlueprint.GetLength(1) - 1)
                    {
                        if (mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3)
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                        }
                        else
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                        }
                    }
                }
                if (mapBlueprint[y, x] == 7)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (y == 0)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                    }
                    else if (y == mapBlueprint.GetLength(0))
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else if (y > 0 && y < mapBlueprint.GetLength(0))
                    {
                        if (x == 0)
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                        }
                        else if (x == mapBlueprint.GetLength(1))
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 0f), map.transform);
                        }
                    }
                }
                if (mapBlueprint[y, x] == 8)
                {
                    tileMap.TryAdd(transformPos, "GhostSpawn");
                    if (mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                if (mapBlueprint[y, x] == 1)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (y > 0)
                    {
                        if (mapBlueprint[y - 1, x] == 1 || mapBlueprint[y - 1, x] == 2)
                        {
                            if (x > 0)
                            {
                                if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                                }
                            }
                            else if (x == 0)
                            {
                                if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                                }
                            }
                        }
                        else
                        {
                            if (x > 0)
                            {
                                if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                                }
                            }
                            else if (x == 0)
                            {
                                if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
 
                                }
                            }
                        }
                    }
                }
                if (mapBlueprint[y, x] == 3)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    float rotation = GenerateCorner(y, x, mapBlueprint);
                    if (rotation == 90f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                    } else if (rotation == 180f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }else if (rotation == 270f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 0f), map.transform);
                    }else if (rotation == 0f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                    }
                }
                readX++;
            }
        }
    }
    void MapGenerationLogic(int[,] mapBlueprint)
    {
        for (int y = 0; y < mapBlueprint.GetLength(0); y++)
        {
            for (int x = 0; x < mapBlueprint.GetLength(1); x++)
            {
                Vector3 transformPos = new Vector3(x * 0.32f, -y * 0.32f, 0);
                if (y == 0 && x == 0)
                {
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 0 )
                {
                    tileMap.TryAdd(transformPos, "Empty");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 5)
                {
                    tileMap.TryAdd(transformPos, "Pellet");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 6)
                {
                    tileMap.TryAdd(transformPos, "PowerPellet");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 2 && x > 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                else if (mapBlueprint[y, x] == 2 && x <= 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                if (mapBlueprint[y, x] == 4)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (x > 0 && x < mapBlueprint.GetLength(1) - 1)
                    {
                        if ((mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3 || mapBlueprint[y, x - 1] == 8) && (mapBlueprint[y, x + 1] == 4 || mapBlueprint[y, x + 1] == 3 || mapBlueprint[y, x + 1] == 8))
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                        }
                        else
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                        }
                    }
                    if (x == mapBlueprint.GetLength(1) - 1)
                    {
                        if (mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3)
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                        }
                        else
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                        }
                    }
                }
                if (mapBlueprint[y, x] == 7)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (y == 0)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                    }
                    else if (y == mapBlueprint.GetLength(0))
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else if (y > 0 && y < mapBlueprint.GetLength(0))
                    {
                        if (x == 0)
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                        }
                        else if (x == mapBlueprint.GetLength(1))
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                        }
                    }
                }
                if (mapBlueprint[y, x] == 8)
                {
                    tileMap.TryAdd(transformPos, "GhostSpawn");
                    if (mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                if (mapBlueprint[y, x] == 1)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (y > 0)
                    {
                        if (mapBlueprint[y - 1, x] == 1 || mapBlueprint[y - 1, x] == 2)
                        {
                            if (x > 0)
                            {
                                if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                                }
                            }
                            else if (x == 0)
                            {
                                if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                                }
                            }
                        }
                        else
                        {
                            if (x > 0)
                            {
                                if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                                }
                            }
                            else if (x == 0)
                            {
                                if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                                }
                            }
                        }
                    }
                }
                if (mapBlueprint[y, x] == 3)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, GenerateCorner(y, x, mapBlueprint)), map.transform);
                }
            }
        }
    }

    void MapGenerationLogic3(int[,] mapBlueprint)
    {
        int readY = 0;
        for (int y = mapBlueprint.GetLength(0) - 2; y >= 0; y--)
        {
            for (int x = 0; x < mapBlueprint.GetLength(1); x++)
            {
                Vector3 transformPos = new Vector3(x * 0.32f, (-readY - mapBlueprint.GetLength(0)) * 0.32f, 0);
                if (y == 0 && x == 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                }
                if (mapBlueprint[y, x] == 0 )
                {
                    tileMap.TryAdd(transformPos, "Empty");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 5)
                {
                    tileMap.TryAdd(transformPos, "Pellet");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 6)
                {
                    tileMap.TryAdd(transformPos, "PowerPellet");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 2 && x > 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                else if (mapBlueprint[y, x] == 2 && x <= 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                if (mapBlueprint[y, x] == 4)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (x > 0 && x < mapBlueprint.GetLength(1) - 1)
                    {
                        if ((mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3 || mapBlueprint[y, x - 1] == 8) && (mapBlueprint[y, x + 1] == 4 || mapBlueprint[y, x + 1] == 3 || mapBlueprint[y, x + 1] == 8))
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                        }
                        else
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                        }
                    }
                    if (x == mapBlueprint.GetLength(1) - 1)
                    {
                        if (mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3)
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                        }
                        else
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                        }
                    }
                }
                if (mapBlueprint[y, x] == 7)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (y == 0)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else if (y == mapBlueprint.GetLength(0))
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                    }
                    else if (y > 0 && y < mapBlueprint.GetLength(0))
                    {
                        if (x == 0)
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                        }
                        else if (x == mapBlueprint.GetLength(1))
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                        }
                    }
                }
                if (mapBlueprint[y, x] == 8)
                {
                    tileMap.TryAdd(transformPos, "GhostSpawn");
                    if (mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                if (mapBlueprint[y, x] == 1)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (y > 0)
                    {
                        if (mapBlueprint[y - 1, x] == 1 || mapBlueprint[y - 1, x] == 2)
                        {
                            if (x > 0)
                            {
                                if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 0f), map.transform);
                                }
                            }
                            else if (x == 0)
                            {
                                if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 0f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                                }
                            }
                        }
                        else
                        {
                            if (x > 0)
                            {
                                if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                                }
                            }
                            else if (x == 0)
                            {
                                if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                                }
                            }
                        }
                    }
                }
                if (mapBlueprint[y, x] == 3)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    float rotation = GenerateCorner(y, x, mapBlueprint);
                    if (rotation == 90f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 0f), map.transform);
                    }
                    else if (rotation == 180f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                    }
                    else if (rotation == 270f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                    }
                    else if (rotation == 0f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                }
            }
            readY++;
        }
    }
    void MapGenerationLogic4(int[,] mapBlueprint)
    {
        int readY = 0;
        for (int y = mapBlueprint.GetLength(0)-2; y >= 0; y--)
        {
            int readX = 0;
            for (int x = mapBlueprint.GetLength(1) - 1; x >= 0; x--)
            {
                Vector3 transformPos = new Vector3((mapBlueprint.GetLength(1) + readX) * 0.32f, (-readY - mapBlueprint.GetLength(0)) * 0.32f, 0);
                if (y == 0 && x == 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                }
                if (mapBlueprint[y, x] == 0 )
                {
                    tileMap.TryAdd(transformPos, "Empty");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 5)
                {
                    tileMap.TryAdd(transformPos, "Pellet");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 6)
                {
                    tileMap.TryAdd(transformPos, "PowerPellet");
                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                }
                if (mapBlueprint[y, x] == 2 && x > 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                else if (mapBlueprint[y, x] == 2 && x <= 0)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                if (mapBlueprint[y, x] == 4)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (x > 0 && x < mapBlueprint.GetLength(1) - 1)
                    {
                        if ((mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3 || mapBlueprint[y, x - 1] == 8) && (mapBlueprint[y, x + 1] == 4 || mapBlueprint[y, x + 1] == 3 || mapBlueprint[y, x + 1] == 8))
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                        }
                        else
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                        }
                    }
                    if (x == mapBlueprint.GetLength(1) - 1)
                    {
                        if (mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3)
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                        }
                        else
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                        }
                    }
                }
                if (mapBlueprint[y, x] == 7)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (y == 0)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else if (y == mapBlueprint.GetLength(0))
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                    }
                    else if (y > 0 && y < mapBlueprint.GetLength(0))
                    {
                        if (x == 0)
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                        }
                        else if (x == mapBlueprint.GetLength(1))
                        {
                            Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 0f), map.transform);
                        }
                    }
                }
                if (mapBlueprint[y, x] == 8)
                {
                    tileMap.TryAdd(transformPos, "GhostSpawn");
                    if (mapBlueprint[y, x - 1] == 4 || mapBlueprint[y, x - 1] == 3)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.identity, map.transform);
                    }
                }
                if (mapBlueprint[y, x] == 1)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    if (y > 0)
                    {
                        if (mapBlueprint[y - 1, x] == 1 || mapBlueprint[y - 1, x] == 2)
                        {
                            if (x > 0)
                            {
                                if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 0f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                                }
                            }
                            else if (x == 0)
                            {
                                if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 0f), map.transform);
                                }
                            }
                        }
                        else
                        {
                            if (x > 0)
                            {
                                if (mapBlueprint[y, x - 1] == 2 || mapBlueprint[y, x - 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                                }
                            }
                            else if (x == 0)
                            {
                                if (mapBlueprint[y, x + 1] == 2 || mapBlueprint[y, x + 1] == 1)
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                                }
                                else
                                {
                                    Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                                }
                            }
                        }
                    }
                }
                if (mapBlueprint[y, x] == 3)
                {
                    tileMap.TryAdd(transformPos, "Wall");
                    float rotation = GenerateCorner(y, x, mapBlueprint);
                    if (rotation == 90f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 270f), map.transform);
                    }
                    else if (rotation == 180f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 0f), map.transform);
                    }
                    else if (rotation == 270f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 90f), map.transform);
                    }
                    else if (rotation == 0f)
                    {
                        Instantiate(levelTiles[mapBlueprint[y, x]], transformPos, Quaternion.Euler(0f, 0f, 180f), map.transform);
                    }
                }
                readX++;
            }    
        
            readY++;
        }
    }
    void GenerateMap(int[,] mapBlueprint)
    {
        MapGenerationLogic(mapBlueprint);
        MapGenerationLogic2(mapBlueprint);
        MapGenerationLogic3(mapBlueprint);
        MapGenerationLogic4(mapBlueprint);
    }
}
