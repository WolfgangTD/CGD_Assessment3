using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    public Tween currentTween;
    private GameObject player;
    private Animator aniController;
    private string lastInput;
    private string currentInput;
    private bool isTweening;
    private Vector3 movement;
    public GameObject levelGen;
    Dictionary<Vector3, string> tileMap;
    float stepSize = 0.32f;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        aniController = GetComponent<Animator>();
        levelGen = GameObject.FindWithTag("LevelGenerator");
        tileMap = levelGen.GetComponent<LevelGeneratort>().tileMap;
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        if (!isTweening)
        {
            CheckNextMove();
        }
    }
    void GetMovementInput()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        movement = Vector3.ClampMagnitude(movement, 1.0f);
        if (Input.GetAxis("Horizontal") < 0)
        {
            lastInput = "left";
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            lastInput = "right";
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            lastInput = "down";
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            lastInput = "up";
        }
    }
    void UpdatePlayer(string direction, Vector3 endpos)
    {
        if (direction == "up")
        {
            StartCoroutine(PlayerMove(player.transform.position, endpos, 0.5f, "up"));
            aniController.SetInteger("Direction", 3);
        }
        else if (direction == "down")
        {
            StartCoroutine(PlayerMove(player.transform.position, endpos, 0.5f, "down"));
            aniController.SetInteger("Direction", 1);
        }
        else if (direction == "left")
        {
            StartCoroutine(PlayerMove(player.transform.position, endpos, 0.5f, "left"));
            aniController.SetInteger("Direction", 2);
        }
        else if (direction == "right")
        {
            StartCoroutine(PlayerMove(player.transform.position, endpos, 0.5f, "right"));
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

    void CheckNextMove()
    {
        Vector3 checker = player.transform.position;
        Vector3 nextPos = checker;
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
        } else
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
            }
        }
        
    }
    IEnumerator PlayerMove(Vector3 startPos, Vector3 endPos, float duration, string direction)
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
