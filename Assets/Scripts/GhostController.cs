using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    string ghostName;
    public bool foundOuterWall = false;
    public bool isTweening = false;
    public bool isScared = false;
    private Animator aniController;
    private GameObject HUD;
    public GameObject levelGen;
    public Vector3 spawnPoint;
    private Vector3 closestSpawn;
    string direction;
    float stepSize = 0.32f;
    float playerMoveSpeed = 0.4f;
    public float walkSpeed;
    private GameObject player;
    Dictionary<Vector3, string> tileMap;
    private string currentDirection;
    public bool hasExitedSpawn = false;
    Vector3 lastTile;
    string lastDir;
    private int currentCornerIndex = 0;
    private List<Vector3> cornerTiles = new List<Vector3>
    {
        new Vector3(8.32f, -8.64f, 0f), 
        new Vector3(0.32f, -8.64f, 0f), 
        new Vector3(0.32f, -0.32f, 0f), 
        new Vector3(8.32f, -0.32f, 0f)  
    };
    void Start()
    {
        aniController = GetComponent<Animator>();
        levelGen = GameObject.FindWithTag("LevelGenerator");
        HUD = GameObject.FindGameObjectWithTag("HUD");
        ghostName = gameObject.name;
        player = GameObject.FindWithTag("Player");
        tileMap = levelGen.GetComponent<LevelGeneratort>().tileMap;
        walkSpeed = playerMoveSpeed*1.1f;
    }
    void StartGhosts()
    {
        var ghostState = GetComponent<GhostStateManager>();

        if (ghostState.isDead)
        {
            BackToSpawn();
            if (transform.position == closestSpawn)
            {
                ReviveGhost();
            }
            return;
        }

        if (isScared)
        {
            walkSpeed = playerMoveSpeed * 1.5f;
            foundOuterWall = false;
            Ghost1Movement(); 
            return;
        }

        if (!hasExitedSpawn)
        {
            InitGhosts();
            return;
        }

        walkSpeed = playerMoveSpeed * 1.1f;
        switch (ghostName)
        {
            case "Ghost1": Ghost1Movement(); break;
            case "Ghost2": Ghost2Movement(); break;
            case "Ghost3": Ghost3Movement(); break;
            case "Ghost4": Ghost4Movement(); break;
        }
    }
    void Update()
    {
        if (HUD.GetComponent<UIManager>().countDownDone && !levelGen.GetComponent<GameStateController>().gameOver)
        {
            if (!isTweening)
            {
                StartGhosts();
            }   
        }
    }
   void BackToSpawn()
    {
        Vector3 currentPos = transform.position;
        closestSpawn = levelGen.GetComponent<LevelGeneratort>().spawnPoints[0];
        float minDist = Vector3.Distance(currentPos, closestSpawn);

        foreach (Vector3 point in levelGen.GetComponent<LevelGeneratort>().spawnPoints)
        {
            float dist = Vector3.Distance(currentPos, point);
            if (dist < minDist)
            {
                minDist = dist;
                closestSpawn = point;
            }
        }


        List<string> validDirs = GetValidDirections();
        Vector3 lastPos = lastTile;

        string oppositeDir = null;
        switch (lastDir)
        {
            case "up": oppositeDir = "down"; break;
            case "down": oppositeDir = "up"; break;
            case "left": oppositeDir = "right"; break;
            case "right": oppositeDir = "left"; break;
        }

        if (!string.IsNullOrEmpty(oppositeDir) && validDirs.Count > 1)
        {
            validDirs.Remove(oppositeDir);
        }

        List<string> possibleDirs = new List<string>();
        float currentDist = Vector3.Distance(currentPos, closestSpawn);

        foreach (string dir in validDirs)
        {
            Vector3 nextPos = PosToTileMap(currentPos + DirToVector(dir) * stepSize);
            float newDist = DistanceToSpawn(nextPos);
            if (newDist < currentDist && nextPos != lastPos)
            {
                possibleDirs.Add(dir);
            }
        }

        if (possibleDirs.Count == 0)
        {
            float closestDist = float.MaxValue;
            string bestDir = validDirs[0];

            foreach (string dir in validDirs)
            {
                Vector3 nextPos = PosToTileMap(currentPos + DirToVector(dir) * stepSize);
                float dist = DistanceToSpawn(nextPos);
                if (dist < closestDist && nextPos != lastPos)
                {
                    closestDist = dist;
                    bestDir = dir;
                }
            }

            possibleDirs.Add(bestDir);
        }

        direction = possibleDirs[Random.Range(0, possibleDirs.Count)];

        if (!isTweening)
        {
            CheckNextMove();
        }
    }
    void Ghost1Movement()
    {
        List<string> validDirs = GetValidDirections();
        Vector3 currentPos = transform.position;
        Vector3 lastPos = lastTile;

        string oppositeDir = null;
        switch (lastDir)
        {
            case "up": oppositeDir = "down"; break;
            case "down": oppositeDir = "up"; break;
            case "left": oppositeDir = "right"; break;
            case "right": oppositeDir = "left"; break;
        }

        if (!string.IsNullOrEmpty(oppositeDir) && validDirs.Count > 1)
        {
            validDirs.Remove(oppositeDir);
        }

        List<string> possibleDirs = new List<string>();
        float currentDist = DistanceToPlayer(currentPos);

        foreach (string dir in validDirs)
        {
            Vector3 nextPos = PosToTileMap(currentPos + DirToVector(dir) * stepSize);
            float newDist = DistanceToPlayer(nextPos);
            if (newDist >= currentDist && nextPos != lastPos)
            {
                possibleDirs.Add(dir);
            }
        }

        if (possibleDirs.Count == 0)
        {
            float closestDist = float.MaxValue;
            string bestDir = validDirs[0];

            foreach (string dir in validDirs)
            {
                Vector3 nextPos = PosToTileMap(currentPos + DirToVector(dir) * stepSize);
                float dist = DistanceToSpawn(nextPos);
                if (dist < closestDist && nextPos != lastPos)
                {
                    closestDist = dist;
                    bestDir = dir;
                }
            }

            possibleDirs.Add(bestDir);
        }

        direction = possibleDirs[Random.Range(0, possibleDirs.Count)];

        if (!isTweening)
        {
            CheckNextMove();
        }
    }
    void Ghost2Movement()
    {
        List<string> validDirs = GetValidDirections();
        Vector3 currentPos = transform.position;
        Vector3 lastPos = lastTile;

        string oppositeDir = null;
        switch (lastDir)
        {
            case "up": oppositeDir = "down"; break;
            case "down": oppositeDir = "up"; break;
            case "left": oppositeDir = "right"; break;
            case "right": oppositeDir = "left"; break;
        }

        if (!string.IsNullOrEmpty(oppositeDir) && validDirs.Count > 1)
        {
            validDirs.Remove(oppositeDir);
        }

        List<string> possibleDirs = new List<string>();
        float currentDist = DistanceToPlayer(currentPos);

        foreach (string dir in validDirs)
        {
            Vector3 nextPos = PosToTileMap(currentPos + DirToVector(dir) * stepSize);
            float newDist = DistanceToPlayer(nextPos);
            if (newDist <= currentDist && nextPos != lastPos)
            {
                possibleDirs.Add(dir);
            }
        }

        if (possibleDirs.Count == 0)
        {
            float closestDist = float.MaxValue;
            string bestDir = validDirs[0];

            foreach (string dir in validDirs)
            {
                Vector3 nextPos = PosToTileMap(currentPos + DirToVector(dir) * stepSize);
                float dist = DistanceToSpawn(nextPos);
                if (dist < closestDist && nextPos != lastPos)
                {
                    closestDist = dist;
                    bestDir = dir;
                }
            }

            possibleDirs.Add(bestDir);
        }

        direction = possibleDirs[Random.Range(0, possibleDirs.Count)];

        if (!isTweening)
        {
            CheckNextMove();
        }
    }
    void Ghost3Movement()
    {
        List<string> validDirs = GetValidDirections();
        string oppositeDir = null;
        switch (lastDir)
        {
            case "up": oppositeDir = "down"; break;
            case "down": oppositeDir = "up"; break;
            case "left": oppositeDir = "right"; break;
            case "right": oppositeDir = "left"; break;
        }
        if (!string.IsNullOrEmpty(oppositeDir) && validDirs.Count > 1)
        {
            validDirs.Remove(oppositeDir);
        }
        if (validDirs.Count > 1 && lastTile != Vector3.zero)
        {
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
        if (validDirs.Count == 0)
        {
            validDirs.Add(oppositeDir);
        }
        direction = validDirs[Random.Range(0, validDirs.Count)];
        if (!isTweening)
        {
            CheckNextMove();
        }
            
    }
    void Ghost4Movement()
    {
        List<string> validDirs = GetValidDirections();
        Vector3 currentPos = transform.position;
        Vector3 lastPos = lastTile;

        string oppositeDir = null;
        switch (lastDir)
        {
            case "up": oppositeDir = "down"; break;
            case "down": oppositeDir = "up"; break;
            case "left": oppositeDir = "right"; break;
            case "right": oppositeDir = "left"; break;
        }
        if (!string.IsNullOrEmpty(oppositeDir) && validDirs.Count > 1)
            validDirs.Remove(oppositeDir);

        if (currentCornerIndex >= cornerTiles.Count)
            currentCornerIndex = 0;

        Vector3 targetCorner = cornerTiles[currentCornerIndex];

        if (Vector3.Distance(currentPos, targetCorner) < 0.1f)
        {
            currentCornerIndex++;
            if (currentCornerIndex >= cornerTiles.Count)
                currentCornerIndex = 0;

            targetCorner = cornerTiles[currentCornerIndex];
        }

        List<string> possibleDirs = new List<string>();
        float currentDist = Vector3.Distance(currentPos, targetCorner);

        foreach (string dir in validDirs)
        {
            Vector3 nextPos = PosToTileMap(currentPos + DirToVector(dir) * stepSize);
            float newDist = Vector3.Distance(nextPos, targetCorner);
            if (newDist < currentDist && nextPos != lastPos)
            {
                possibleDirs.Add(dir);
            }
        }

        if (possibleDirs.Count == 0)
        {
            float bestDist = float.MaxValue;
            string bestDir = validDirs[0];
            foreach (string dir in validDirs)
            {
                Vector3 nextPos = PosToTileMap(currentPos + DirToVector(dir) * stepSize);
                float dist = Vector3.Distance(nextPos, targetCorner);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    bestDir = dir;
                }
            }
            possibleDirs.Add(bestDir);
        }

        direction = possibleDirs[Random.Range(0, possibleDirs.Count)];

        if (!isTweening)
        {
            CheckNextMove();
        }
    }
    
    public void ResetCornerTarget()
    {
        Vector3 currentPos = transform.position;
        float minDist = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < cornerTiles.Count; i++)
        {
            float dist = Vector3.Distance(currentPos, cornerTiles[i]);
            if (dist < minDist)
            {
                minDist = dist;
                closestIndex = i;
            }
        }
        currentCornerIndex = closestIndex;
    }
    void ReviveGhost()
    {
        if (GetComponent<GhostStateManager>().isDead)
        {
            GetComponent<GhostStateManager>().Revive();
        }
        List<Vector3> exitTiles = new List<Vector3>
        {
            new Vector3(4.16f, -5.44f, 0f),
            new Vector3(4.16f, -3.52f, 0f),
            new Vector3(4.48f, -5.44f, 0f),
            new Vector3(4.48f, -3.52f, 0f)
        };
        Vector3 currentPos = transform.position;
        Vector3 closestTile = exitTiles[0];
        float minDist = Vector3.Distance(currentPos, closestTile);

        foreach (Vector3 tile in exitTiles)
        {
            float dist = Vector3.Distance(currentPos, tile);
            if (dist < minDist)
            {
                minDist = dist;
                closestTile = tile;
            }
        }
        Vector3 directionToGo = closestTile - currentPos;

        string dir = directionToGo.y > 0 ? "up" : "down";
        ResetCornerTarget();
        UpdateGhost(dir, PosToTileMap(closestTile));
        foundOuterWall = false;
        hasExitedSpawn = true;
    }
    public void ResetGame()
    {
        StopAllCoroutines(); 
        isTweening = true;   
        foundOuterWall = false;
        transform.position = spawnPoint;
        lastTile = Vector3.zero;
        lastDir = null;
        currentDirection = null;
        hasExitedSpawn = false;
        currentCornerIndex = 0;
        GetComponent<GhostStateManager>().Revive();
        StartCoroutine(WaitSec());
    }
    public void StopMovement()
    {
        StopAllCoroutines(); 
        isTweening = true;   
        foundOuterWall = false;
        transform.position = spawnPoint;
        lastTile = Vector3.zero;
        lastDir = null;
        currentDirection = null;
        hasExitedSpawn = false;
    }

    private IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(1f);
        isTweening = false;
    }
    void InitGhosts()
    {
        Vector3 moveUp = Vector3.up * 0.64f;
        Vector3 moveDown = Vector3.down * 0.64f;
        
        if (ghostName == "Ghost1")
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
        hasExitedSpawn = true;
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
            Vector3 next = PosToTileMap(pos + (dir.Value * stepSize));
            if (tileMap.TryGetValue(next, out string tileType))
            {
                
                if (gameObject.GetComponent<GhostStateManager>().isDead)
                {
                    if (tileType != "Wall" && tileType != "OutsideWall")
                    {
                        validDirs.Add(dir.Key);
                    }
                }
                else
                {
                    if (tileType != "Wall" && tileType != "GhostSpawn" && tileType != "OutsideWall")
                    {
                        validDirs.Add(dir.Key);
                    }
                }
            } 
        }
        return validDirs;
    }

    float DistanceToPlayer(Vector3 pos)
    {
        return Vector3.Distance(player.transform.position, pos);
    }
    float DistanceToSpawn(Vector3 pos)
    {
        return Vector3.Distance(spawnPoint, pos);
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
        Vector3 checker = PosToTileMap(transform.position);
        Vector3 nextPos = checker;
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
        if(gameObject.GetComponent<GhostStateManager>().isDead)
        {
            if (tileMap.TryGetValue(nextPos, out string tileType) && tileType != "Wall" && tileType != "OutsideWall")
            {
                
                UpdateGhost(direction, nextPos);
                currentDirection = direction;
                lastTile = checker;
                lastDir = direction;
            }
            
        } else
        {
            if (tileMap.TryGetValue(nextPos, out string tileType) && tileType != "Wall" && tileType != "GhostSpawn" && tileType != "OutsideWall")
            {
                UpdateGhost(direction, nextPos);
                currentDirection = direction;
                lastTile = checker;
                lastDir = direction;
            }
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