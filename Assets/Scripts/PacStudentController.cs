using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public Tween currentTween;
    private GameObject player;
    private Animator aniController;
    private string lastInput;
    private string currentInput;
    private bool isTweening;
    private Vector3 movement;
    public GameObject mapGenerator;
    private List<GameObject> levelTiles;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        aniController = GetComponent<Animator>();
        levelTiles = mapGenerator.GetComponent<LevelGeneratort>().levelTiles;
        foreach (GameObject tile in levelTiles)
        {
            Vector3 checker = new Vector3(tile.transform.position.x - 5.13999987f, tile.transform.position.y + 7.65999985f, tile.transform.position.z);
            Debug.Log(tile.transform.position);
            if (tile.transform.position == player.transform.position)
            {
                //Vector3(5.13999987,-7.65999985,0) map pos
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        if (!isTweening)
        {
            CheckNextMove();
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            currentInput = "left";
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            currentInput = "right";
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            currentInput = "down";
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            currentInput = "up";
        }
    }
    void GetMovementInput()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        movement = Vector3.ClampMagnitude(movement, 1.0f);
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
    }
    void CheckNextMove()
    {
        UpdatePlayer(lastInput);
        UpdatePlayer(currentInput);
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
