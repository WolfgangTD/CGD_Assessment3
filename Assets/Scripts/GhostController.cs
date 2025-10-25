using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    string ghostName;
    bool isTweening = false;
    private GameObject HUD;
    public GameObject levelGen;
    public Vector3 spawnPoint1;
    public Vector3 spawnPoint2;
    public Vector3 spawnPoint3;
    public Vector3 spawnPoint4;
    // Start is called before the first frame update
    void Start()
    {
        levelGen = GameObject.FindWithTag("LevelGenerator");
        HUD = GameObject.FindGameObjectWithTag("HUD");
        ghostName = gameObject.name;
        
    }
    void StartGhosts()
    {
        if (ghostName == "Ghost1")
        {
            
        }else if(ghostName == "Ghost2")
        {
            
        }else if(ghostName == "Ghost3")
        {
            
        }else if(ghostName == "Ghost4")
        {
            
        }
    }
    void Update()
    {
        if (HUD.GetComponent<UIManager>().countDownDone && !levelGen.GetComponent<GameStateController>().gameOver)
        {
            GetMovementInput();
            if (!isTweening)
            {
                StartGhosts();
            }
        }
    }
    void CheckNextMove()
    {
        Vector3 checker = player.transform.position;
        Vector3 nextPos = new Vector3(0f,0f,0f);
        if (lastInput == "up")
        {
            nextPos = PosToTileMap(checker + (Vector3.up * stepSize));
        }
        else if (lastInput == "down")
        {
            nextPos = PosToTileMap(checker + (Vector3.down * stepSize));
        }
        else if (lastInput == "left")
        {
            nextPos = PosToTileMap(checker + (Vector3.left * stepSize));
        }
        else if (lastInput == "right")
        {
            nextPos = PosToTileMap(checker + (Vector3.right * stepSize));
        }
        if (tileMap.TryGetValue(nextPos, out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
        {
            UpdatePlayer(lastInput, nextPos);
            currentInput = lastInput;
            if (tileType == "Pellet")
            {
                audioSource.clip = walkingEating;
                audioSource.Play();
                wallHit = false;
                tileMap[nextPos] = "Empty";
                
            } else if(tileType == "PowerPellet")
            {
                audioSource.clip = walkingEating;
                audioSource.Play();
                wallHit = false;
                isBuffed = true;
                gameController.GetComponent<GameStateController>().StartBuffState();
                StartCoroutine(BuffCounter());
                tileMap[nextPos] = "Empty";
            }
            else
            {
                audioSource.clip = walking;
                audioSource.Play();
                wallHit = false;
            }
        }
        else
        {
            if (currentInput == "up")
            {
                nextPos = PosToTileMap(checker + (Vector3.up * stepSize));
            }
            else if (currentInput == "down")
            {
                nextPos = PosToTileMap(checker + (Vector3.down * stepSize));
            }
            else if (currentInput == "left")
            {
                nextPos = PosToTileMap(checker + (Vector3.left * stepSize));
            }
            else if (currentInput == "right")
            {
                nextPos = PosToTileMap(checker + (Vector3.right * stepSize));
            }
            if (tileMap.TryGetValue(nextPos, out string tileType2) && tileType2 != "Wall" && tileType2 != "GhostSpawn")
            {
                UpdatePlayer(currentInput, nextPos);
                if (tileType2 == "Pellet")
                {
                    audioSource.clip = walkingEating;
                    audioSource.Play();
                    wallHit = false;
                    tileMap[nextPos] = "Empty";
                }else if(tileType2 == "PowerPellet")
                {
                    audioSource.clip = walkingEating;
                    audioSource.Play();
                    wallHit = false;
                    isBuffed = true;
                    gameController.GetComponent<GameStateController>().StartBuffState();
                    StartCoroutine(BuffCounter());
                    tileMap[nextPos] = "Empty";
                }
                else
                {
                    audioSource.clip = walking;
                    audioSource.Play();
                    wallHit = false;
                }
            } else
            {
                if(lastInput != null && !wallHit)
                {
                    audioSource.clip = hitWall;
                    audioSource.Play();
                    wallHit = true;
                    wallHitEffect.Play();
                }
            }
        }
    }
    void UpdatePlayer(string direction, Vector3 endpos)
    {
        if (direction == "up")
        {
            StartCoroutine(PlayerMove(player.transform.position, endpos, walkSpeed, "up"));
            aniController.SetInteger("Direction", 3);
        }
        else if (direction == "down")
        {
            StartCoroutine(PlayerMove(player.transform.position, endpos, walkSpeed, "down"));
            aniController.SetInteger("Direction", 1);
        }
        else if (direction == "left")
        {
            StartCoroutine(PlayerMove(player.transform.position, endpos, walkSpeed, "left"));
            aniController.SetInteger("Direction", 2);
        }
        else if (direction == "right")
        {
            StartCoroutine(PlayerMove(player.transform.position, endpos, walkSpeed, "right"));
            aniController.SetInteger("Direction", 0);
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
    IEnumerator GhostMove(Vector3 startPos, Vector3 endPos, float duration, string direction)
    {
        isTweening = true;
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float timeLen = timeElapsed / duration;
            player.transform.position = Vector3.Lerp(startPos, endPos, timeLen);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        player.transform.position = endPos;
        isTweening = false;
        lastInput = direction;
    }

}
