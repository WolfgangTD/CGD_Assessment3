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
    void UpdatePlayer(string direction)
    {
        if (direction == "up")
        {
            StartCoroutine(PlayerMove(player.transform.position, player.transform.position + (Vector3.up * 0.32f), 1f, "up"));
            aniController.SetInteger("Direction", 3);
        }
        else if (direction == "down")
        {
            StartCoroutine(PlayerMove(player.transform.position, player.transform.position + (Vector3.down * 0.32f), 1f, "down"));
            aniController.SetInteger("Direction", 1);
        }
        else if (direction == "left")
        {
            StartCoroutine(PlayerMove(player.transform.position, player.transform.position + (Vector3.left * 0.32f), 1f, "left"));
            aniController.SetInteger("Direction", 2);
        }
        else if (direction == "right")
        {
            StartCoroutine(PlayerMove(player.transform.position, player.transform.position + (Vector3.right * 0.32f), 1f, "Right"));
            aniController.SetInteger("Direction", 0);
        }
        currentInput = lastInput;
    }
    Vector3 RoundToGrid(Vector3 pos)
{
    return new Vector3(
        Mathf.Round(pos.x / 0.32f) * 0.32f,
        Mathf.Round(pos.y / 0.32f) * 0.32f,
        0f
    );
}
    void CheckNextMove()
    {
        Vector3 checker = player.transform.position + (Vector3.right * 0.16f);
        Debug.Log(checker);
        if (lastInput == "up")
        {
            if (tileMap.TryGetValue(RoundToGrid(checker + (Vector3.up * 0.32f)), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(lastInput);
            }
        }
        else if (lastInput == "down")
        {
            if (tileMap.TryGetValue(RoundToGrid(checker + (Vector3.down * 0.32f)), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(lastInput);
            }
        }
        else if (lastInput == "left")
        {
            if (tileMap.TryGetValue(RoundToGrid(checker + (Vector3.left * 0.32f)), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(lastInput);
            }
        }
        else if (lastInput == "right")
        {
            if (tileMap.TryGetValue(RoundToGrid(checker + (Vector3.right * 0.32f)), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(lastInput);
            }
        }
        else if (currentInput == "up")
        {
            if(tileMap.TryGetValue(RoundToGrid(checker + (Vector3.up * 0.32f)), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(currentInput);
            }
        }
        else if (currentInput == "down")
        {
            if (tileMap.TryGetValue(RoundToGrid(checker + (Vector3.down * 0.32f)), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(currentInput);
            }
        }
        else if (currentInput == "left")
        {
            if(tileMap.TryGetValue(RoundToGrid(checker + (Vector3.left * 0.32f)), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(currentInput);
            }
        }
        else if(currentInput == "right")
        {
            if(tileMap.TryGetValue(RoundToGrid(checker + (Vector3.right * 0.32f)), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(currentInput);
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
