using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController3D : MonoBehaviour
{
    public AudioSource bonusCherrySound;
    private GameObject gameController;
    private GameObject CherryController;
    private bool wallHit = false;
    public Tween currentTween;
    private GameObject player;
    public int livesLeft;
    private string lastInput;
    private string currentInput;
    private bool isTweening;
    private Vector3 movement;
    private float movementSqrMagnitude; 
    public GameObject levelGen;
    Dictionary<Vector3, string> tileMap;
    float stepSize = 0.32f;
    public AudioClip walking;
    public AudioClip walkingEating;
    public AudioClip hitWall;
    public AudioClip deathSound;
    public AudioSource audioSource;
    public Vector3 spawnPoint;
    private GameObject HUD;
    public bool isBuffed;
    private bool isDead;
    bool cameraRotating = false;
    private float walkSpeed = 0.4f;
    public Transform cameraTransform; 
    public float cameraRotationSpeed = 90f; 
    // Start is called before the first frame update
    void Start()
    {
        livesLeft = 3;
        player = gameObject;
        levelGen = GameObject.FindWithTag("LevelGenerator");
        tileMap = levelGen.GetComponent<LevelGenerator3D>().tileMap;
        audioSource = player.GetComponent<AudioSource>();
        CherryController = GameObject.FindWithTag("CherryController");
        gameController = GameObject.FindWithTag("LevelGenerator");
        HUD = GameObject.FindGameObjectWithTag("HUD");
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (HUD.GetComponent<UIManager3D>().countDownDone && !levelGen.GetComponent<GameStateController3D>().gameOver)
        {
            if (!isDead)
            {
                GetMovementInput();
                if (!isTweening)
                {
                    CheckNextMove();
                }
            }
        }
    }
    
    public void StopMovement()
    {
        StopAllCoroutines();
        currentInput = null;
        lastInput = null;
        isTweening = false;
        movement = Vector3.zero;
    }
    public void ResetGame()
    {
        StopMovement();
        StartCoroutine(ReenableCollider());
        StartCoroutine(DeadMode(1)); 
    }
    IEnumerator DeadMode(int secs)
    {
        yield return new WaitForSeconds(secs);
        player.transform.position = spawnPoint;
        cameraTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        isDead = false;
    }
    void GetMovementInput()
    {
        if (!cameraRotating)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(RotateCamera(-90f, 0.2f));   
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(RotateCamera(90f, 0.2f));
            }
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            currentInput = "forward";
            movement = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentInput = "backward";
            movement = new Vector3(-cameraTransform.forward.x, 0f, -cameraTransform.forward.z).normalized;
        }
        else
        {
            currentInput = null;
            movement = Vector3.zero;
        }
    }
    IEnumerator RotateCamera(float angle, float duration)
    {
        cameraRotating = true;
        float startAngle = cameraTransform.eulerAngles.y;
        float endAngle = startAngle+angle;
        float time = 0;

        while (time < duration)
        {
            float step = time / duration;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, step);
            float angleChange =  currentAngle - cameraTransform.eulerAngles.y;
            cameraTransform.RotateAround(player.transform.position, Vector3.up, angleChange);
            time += Time.deltaTime;
            yield return null;
        }
        float change = startAngle+angle - cameraTransform.eulerAngles.y;
        cameraTransform.RotateAround(player.transform.position, Vector3.up, change);
        Vector3 camAngle = cameraTransform.eulerAngles;
        camAngle.y = Mathf.Round(camAngle.y / 90f) * 90f;
        cameraTransform.rotation = Quaternion.Euler(camAngle);
        cameraRotating = false;
    }

    void CheckNextMove()
    {
        if (currentInput == null || isTweening) return;

        Vector3 nextPos = player.transform.position;
        if (currentInput == "forward" || currentInput == "backward")
        {
            Vector3 dir = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
            if (currentInput == "backward") dir = -dir;
            nextPos += dir * stepSize;
        }

        nextPos = PosToTileMap(nextPos);

        if (tileMap.TryGetValue(nextPos, out string tileType) && tileType != "Wall" && tileType != "GhostSpawn" && tileType != "OutsideWall")
        {
            StartCoroutine(PlayerMove(player.transform.position, nextPos, walkSpeed, currentInput));

            if (tileType == "Pellet" || tileType == "PowerPellet")
            {
                audioSource.clip = walkingEating;
                audioSource.Play();
                wallHit = false;
                if (tileType == "PowerPellet")
                {
                    isBuffed = true;
                    gameController.GetComponent<GameStateController3D>().StartBuffState();
                }
                tileMap[nextPos] = "Empty";
            }
            else
            {
                audioSource.clip = walking;
                audioSource.Play();
            }

            lastInput = currentInput;
            wallHit = false;
        }
        else
        {
            if (!wallHit && lastInput != null)
            {
                audioSource.clip = hitWall;
                audioSource.Play();
                wallHit = true;
            }
        }

    }
    Vector3 PosToTileMap(Vector3 pos)
    {
        pos.x = Mathf.Round(pos.x / stepSize) * stepSize;
        pos.z = Mathf.Round(pos.z / stepSize) * stepSize;
        pos.y = 0f;
        return pos;
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
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleRight"))
        {
            StopAllCoroutines();
            isTweening = false;
            StartCoroutine(ReenableCollider());
            player.transform.position = new Vector3(0.32f, 0f, 4.48f);
            
        }
        else if (other.CompareTag("TeleLeft"))
        {
            StopAllCoroutines();
            isTweening = false;
            StartCoroutine(ReenableCollider());
            player.transform.position = new Vector3(8.32f,0,4.48f);
        }
        else if (other.CompareTag("BonusCrystal"))
        {
            bonusCherrySound.Play();
            CherryController.GetComponent<CherryController3D>().KillCrystal(other.gameObject);
            gameController.GetComponent<GameStateController3D>().currentScore += 100;
        }
        else if (other.CompareTag("PowerPellet"))
        {
            Destroy(other.gameObject);
            gameController.GetComponent<GameStateController3D>().currentScore += 50;
            gameController.GetComponent<GameStateController3D>().totalPellets--;
        }
        else if (other.CompareTag("Pellet") && lastInput != null)
        {
            Destroy(other.gameObject);
            gameController.GetComponent<GameStateController3D>().currentScore += 10;
            gameController.GetComponent<GameStateController3D>().totalPellets--;
        }
        
        if(other.CompareTag("Ghost") && !isBuffed && !other.GetComponent<GhostStateManager3D>().isDead)
        {
            isDead = true;
            GameObject[] HUDLives = HUD.GetComponent<UIManager3D>().lives;
            HUDLives[livesLeft-1].SetActive(false);
            livesLeft --;
            audioSource.clip = deathSound;
            audioSource.Play();
            ResetGame();
            levelGen.GetComponent<GameStateController3D>().ResetGame();
            
        }else if(other.CompareTag("Ghost") && isBuffed && !other.GetComponent<GhostStateManager3D>().isDead)
        {
            gameController.GetComponent<GameStateController3D>().currentScore += 300;
            other.GetComponent<GhostStateManager3D>().Die();
        }
    }
    IEnumerator ReenableCollider()
    {
        player.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1f);
        player.GetComponent<BoxCollider>().enabled = true;
    }
}
