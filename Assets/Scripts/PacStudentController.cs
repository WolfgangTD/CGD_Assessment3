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
        if (!isTweening)
        {
            CheckNextMove();
        }
        GetMovementInput();
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
        currentInput = lastInput;
    }

    void CheckNextMove()
    {
        Vector3 checker = player.transform.position;
        if (lastInput == "up")
        {
            if (tileMap.TryGetValue(checker + (Vector3.up * stepSize), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(lastInput, checker + (Vector3.up * stepSize));
            }
        }
        else if (lastInput == "down")
        {
            if (tileMap.TryGetValue(checker + (Vector3.down * stepSize), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(lastInput, checker + (Vector3.down * stepSize));
            }
        }
        else if (lastInput == "left")
        {
            if (tileMap.TryGetValue(checker + (Vector3.left * stepSize), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(lastInput, checker + (Vector3.left * stepSize));
            }
        }
        else if (lastInput == "right")
        {
            if (tileMap.TryGetValue(checker + (Vector3.right * stepSize), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                Debug.Log(tileType);
                UpdatePlayer(lastInput, checker + (Vector3.right * stepSize));
            }
        }
        else if (currentInput == "up")
        {
            if(tileMap.TryGetValue(checker + (Vector3.up * stepSize), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(currentInput, checker + (Vector3.up * stepSize));
            }
        }
        else if (currentInput == "down")
        {
            if (tileMap.TryGetValue(checker + (Vector3.down * stepSize), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(currentInput, checker + (Vector3.down * stepSize));
            }
        }
        else if (currentInput == "left")
        {
            if(tileMap.TryGetValue(checker + (Vector3.left * stepSize), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(currentInput, checker + (Vector3.left * stepSize));
            }
        }
        else if(currentInput == "right")
        {
            if(tileMap.TryGetValue(checker + (Vector3.right * stepSize), out string tileType) && tileType != "Wall" && tileType != "GhostSpawn")
            {
                UpdatePlayer(currentInput, checker + (Vector3.right * stepSize));
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
