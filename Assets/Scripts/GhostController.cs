using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    string ghostName;
    bool isTweening = false;
    private Animator aniController;
    private GameObject HUD;
    public GameObject levelGen;
    public Vector3 spawnPoint;
    string direction;
    float stepSize = 0.32f;
    public float walkSpeed = 0.36f;
    private GameObject player;
    Dictionary<Vector3, string> tileMap;
    private string currentDirection;
    private int ghostState;
    bool hasExitedSpawn = false;
    Vector3 lastTile;
    string lastDir;
    // Start is called before the first frame update
    void Start()
    {
        aniController = GetComponent<Animator>();
        levelGen = GameObject.FindWithTag("LevelGenerator");
        HUD = GameObject.FindGameObjectWithTag("HUD");
        ghostName = gameObject.name;
        player = GameObject.FindWithTag("Player");
        tileMap = levelGen.GetComponent<LevelGeneratort>().tileMap;
        ghostState = GetComponent<GhostStateManager>().state;
        
    }
    void StartGhosts()
    {
        if (hasExitedSpawn)
        {
            if (ghostName == "Ghost1" || ghostState != 0)
            {
                Ghost1Movement();
            }else if(ghostName == "Ghost2" && ghostState == 0)
            {
                Ghost2Movement();
            }else if(ghostName == "Ghost3" && ghostState == 0)
            {
                Ghost3Movement();
            }else if(ghostName == "Ghost4" && ghostState == 0)
            {
                Ghost4Movement();
            }
        } else
        {
            InitGhosts();
            hasExitedSpawn = true;
        }
    }
    void Update()
    {
        if (HUD.GetComponent<UIManager>().countDownDone && !levelGen.GetComponent<GameStateController>().gameOver && !isTweening)
        {
                StartGhosts();
        }
    }
    void Ghost1Movement()
    {
        List<string> validDirs = GetValidDirections();
        float currentDist = DistanceToPlayer(transform.position);

        List<string> possibleDirs = new List<string>();
        foreach (string dir in validDirs)
        {
            
            Vector3 nextPos = PosToTileMap(transform.position + DirToVector(dir) * stepSize);
            float newDist = DistanceToPlayer(nextPos);
            if (newDist >= currentDist && nextPos != lastTile)
            {
                if(lastDir == "up")
                {
                    if(dir == "up" || dir == "left" || dir == "right")
                    {
                        possibleDirs.Add(dir);
                    }
                } else if(lastDir == "down")
                {
                    if(dir == "down" || dir == "left" || dir == "right")
                    {
                        possibleDirs.Add(dir);
                    }
                }else if(lastDir == "left")
                {
                    if(dir == "down" || dir == "left" || dir == "up")
                    {
                        possibleDirs.Add(dir);
                    }
                }else if(lastDir == "right")
                {
                    if(dir == "down" || dir == "up" || dir == "right")
                    {
                        possibleDirs.Add(dir);
                    }
                }
            }
        }

        if (possibleDirs.Count == 0)
        {
            possibleDirs = validDirs;
        }
        direction = possibleDirs[Random.Range(0, possibleDirs.Count)];
        CheckNextMove();
    }
    void Ghost2Movement()
    {
        List<string> validDirs = GetValidDirections();
        float currentDist = DistanceToPlayer(transform.position);

        List<string> possibleDirs = new List<string>();
        foreach (string dir in validDirs)
        {
            Vector3 nextPos = PosToTileMap(transform.position + DirToVector(dir) * stepSize);
            float newDist = DistanceToPlayer(nextPos);
            if (newDist <= currentDist && nextPos != lastTile)
            {
                if(lastDir == "up")
                {
                    if(dir == "up" || dir == "left" || dir == "right")
                    {
                        possibleDirs.Add(dir);
                    }
                } else if(lastDir == "down")
                {
                    if(dir == "down" || dir == "left" || dir == "right")
                    {
                        possibleDirs.Add(dir);
                    }
                }else if(lastDir == "left")
                {
                    if(dir == "down" || dir == "left" || dir == "up")
                    {
                        possibleDirs.Add(dir);
                    }
                }else if(lastDir == "right")
                {
                    if(dir == "down" || dir == "up" || dir == "right")
                    {
                        possibleDirs.Add(dir);
                    }
                }
            }
        }

        if (possibleDirs.Count == 0)
            possibleDirs = validDirs;

        direction = possibleDirs[Random.Range(0, possibleDirs.Count)];
        CheckNextMove();
    }
    void Ghost3Movement()
    {
        List<string> validDirs = GetValidDirections();
        if (validDirs == null || validDirs.Count == 0)
            return;

        // Determine opposite of lastDir (the "backtrack" direction we want to forbid)
        string oppositeDir = null;
        switch (lastDir)
        {
            case "up": oppositeDir = "down"; break;
            case "down": oppositeDir = "up"; break;
            case "left": oppositeDir = "right"; break;
            case "right": oppositeDir = "left"; break;
        }

        // Remove the opposite/backtrack direction if there are other options available.
        // If there's only one valid direction, keep it (so the ghost won't get stuck).
        if (!string.IsNullOrEmpty(oppositeDir) && validDirs.Count > 1)
        {
            validDirs.Remove(oppositeDir);
        }

        // Also prefer not to go to the lastTile (the tile we just came from) if alternatives exist
        if (validDirs.Count > 1 && lastTile != Vector3.zero)
        {
            // build a list excluding moves that would step onto lastTile
            List<string> filtered = new List<string>();
            foreach (string dir in validDirs)
            {
                Vector3 nextPos = PosToTileMap(transform.position + DirToVector(dir) * stepSize);
                if (nextPos != lastTile)
                    filtered.Add(dir);
            }
            if (filtered.Count > 0)
                validDirs = filtered;
        }

        // Finally pick a random direction from remaining valid options
        direction = validDirs[Random.Range(0, validDirs.Count)];
        CheckNextMove();
    }
    void Ghost4Movement()
    {
        List<string> validDirs = GetValidDirections();

        if (string.IsNullOrEmpty(currentDirection))
        {
            currentDirection = "right";
        }
            

        Dictionary<string, List<string>> clockwisePriority = new Dictionary<string, List<string>>
        {
            {"up", new List<string>{"right", "up", "left", "down"}},
            {"right", new List<string>{"down", "right", "up", "left"}},
            {"down", new List<string>{"left", "down", "right", "up"}},
            {"left", new List<string>{"up", "left", "down", "right"}}
        };

        foreach (var dir in clockwisePriority[currentDirection])
        {
            if (validDirs.Contains(dir))
            {
                direction = dir;
                CheckNextMove();
                return;
            }
        }
    }
    void InitGhosts()
    {
        Vector3 moveUp = Vector3.up * 0.64f;
        Vector3 moveDown = Vector3.down * 0.64f;
        if (ghostName == "Ghost1" || ghostState != 0)
            {
                UpdateGhost("up", gameObject.transform.position + moveUp);
            }else if(ghostName == "Ghost2")
            {
                UpdateGhost("down", gameObject.transform.position + moveDown);
            }else if(ghostName == "Ghost3")
            {
                UpdateGhost("up", gameObject.transform.position + moveUp);
            }else if(ghostName == "Ghost4")
            {
                UpdateGhost("down", gameObject.transform.position + moveDown);
            }
    }
    List<string> GetValidDirections()
    {
        List<string> validDirs = new List<string>();
        Vector3 pos = transform.position;

        var directions = new Dictionary<string, Vector3>
        {
            {"up", Vector3.up},
            {"down", Vector3.down},
            {"left", Vector3.left},
            {"right", Vector3.right}
        };

        foreach (var dir in directions)
        {
            Vector3 next = PosToTileMap(pos + dir.Value * stepSize);
            if (tileMap.TryGetValue(next, out string tileType))
            {
                if (tileType != "Wall" && tileType != "GhostSpawn")
                {
                    validDirs.Add(dir.Key);
                }
            }
        }
        return validDirs;
    }

    float DistanceToPlayer(Vector3 pos)
    {
        return Vector3.Distance(player.transform.position, pos);
    }

    Vector3 DirToVector(string dir)
    {
        switch (dir)
        {
            case "up": return Vector3.up;
            case "down": return Vector3.down;
            case "left": return Vector3.left;
            case "right": return Vector3.right;
            default: return Vector3.zero;
        }
    }
    void CheckNextMove()
    {
        Vector3 checker = gameObject.transform.position;
        Vector3 nextPos = new Vector3(0f,0f,0f);
        if (direction == "up")
        {
            nextPos = PosToTileMap(checker + (Vector3.up * stepSize));
        }
        else if (direction == "down")
        {
            nextPos = PosToTileMap(checker + (Vector3.down * stepSize));
        }
        else if (direction == "left")
        {
            nextPos = PosToTileMap(checker + (Vector3.left * stepSize));
        }
        else if (direction == "right")
        {
            nextPos = PosToTileMap(checker + (Vector3.right * stepSize));
        }
        if (tileMap.TryGetValue(nextPos, out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdateGhost(direction, nextPos);
                currentDirection = direction;
                lastTile = nextPos;
                lastDir = direction;
            }
    }
        
    void UpdateGhost(string direction, Vector3 endpos)
    {
        if (direction == "up")
        {
            StartCoroutine(GhostMove(gameObject.transform.position, endpos, walkSpeed, "up"));
            aniController.SetInteger("Direction", 1);
        }
        else if (direction == "down")
        {
            StartCoroutine(GhostMove(gameObject.transform.position, endpos, walkSpeed, "down"));
            aniController.SetInteger("Direction", 0);
        }
        else if (direction == "left")
        {
            StartCoroutine(GhostMove(gameObject.transform.position, endpos, walkSpeed, "left"));
            aniController.SetInteger("Direction", 3);
        }
        else if (direction == "right")
        {
            StartCoroutine(GhostMove(gameObject.transform.position, endpos, walkSpeed, "right"));
            aniController.SetInteger("Direction", 2);
        }
    }
    Vector3 PosToTileMap(Vector3 pos)
    {
        return new Vector3(
            Mathf.Round(pos.x / stepSize) * stepSize,
            Mathf.Round(pos.y / stepSize) * stepSize,
            0
        );
    }
    IEnumerator GhostMove(Vector3 startPos, Vector3 endPos, float duration, string dir)
    {
        isTweening = true;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float time = timeElapsed / duration;
            transform.position = Vector3.Lerp(startPos, endPos, time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        direction = dir;
        isTweening = false;
    }
}
