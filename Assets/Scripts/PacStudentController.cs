using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    private GameObject gameController;
    private GameObject CherryController;
    public ParticleSystem wallHitEffect;
    public ParticleSystem deathEffect;
    private bool wallHit = false;
    public Tween currentTween;
    private GameObject player;
    public int livesLeft;
    private Animator aniController;
    private string lastInput;
    private string currentInput;
    private bool isTweening;
    private Vector3 movement;
    public GameObject levelGen;
    Dictionary<Vector3, string> tileMap;
    float stepSize = 0.32f;
    public AudioClip walking;
    public AudioClip walkingEating;
    public AudioClip hitWall;
    public AudioSource audioSource;
    public Vector3 spawnPoint;
    private GameObject HUD;
    public bool isBuffed;

    private float walkSpeed = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        livesLeft = 3;
        player = gameObject;
        aniController = GetComponent<Animator>();
        levelGen = GameObject.FindWithTag("LevelGenerator");
        tileMap = levelGen.GetComponent<LevelGeneratort>().tileMap;
        audioSource = player.GetComponent<AudioSource>();
        CherryController = GameObject.FindWithTag("CherryController");
        gameController = GameObject.FindWithTag("LevelGenerator");
        HUD = GameObject.FindGameObjectWithTag("HUD");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HUD.GetComponent<UIManager>().countDownDone && !levelGen.GetComponent<GameStateController>().gameOver)
        {
            GetMovementInput();
            if (!isTweening)
            {
                CheckNextMove();
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
        audioSource.Stop();
    }
    public void ResetGame()
    {
        StopMovement();
        StartCoroutine(DeadMode(1)); 
    }
    IEnumerator DeadMode(int secs)
    {
        yield return new WaitForSeconds(secs);
        player.transform.position = spawnPoint;
        aniController.SetTrigger("reset");
    }
    void GetMovementInput()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        movement = Vector3.ClampMagnitude(movement, 1.0f);
        if (Input.GetAxis("Horizontal") < 0)
        {
            lastInput = "left";
            wallHitEffect.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            lastInput = "right";
            wallHitEffect.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            lastInput = "down";
            wallHitEffect.transform.rotation = Quaternion.identity;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            lastInput = "up";
            wallHitEffect.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleRight"))
        {
            StopAllCoroutines();
            isTweening = false;
            StartCoroutine(ReenableCollider());
            player.transform.position = new Vector3(0f, -4.54f, 0);
        }
        else if (other.CompareTag("TeleLeft"))
        {
            StopAllCoroutines();
            isTweening = false;
            StartCoroutine(ReenableCollider());
            player.transform.position = new Vector3(8.639999389648438f, -4.54f, 0);
        }
        else if (other.CompareTag("BonusCrystal"))
        {
            CherryController.GetComponent<CherryController>().KillCrystal(other.gameObject);
            gameController.GetComponent<GameStateController>().currentScore += 100;
        }
        else if (other.CompareTag("PowerPellet"))
        {
            Destroy(other.gameObject);
            gameController.GetComponent<GameStateController>().currentScore += 50;
            gameController.GetComponent<GameStateController>().totalPellets--;
        }
        else if (other.CompareTag("Pellet") && lastInput != null)
        {
            Destroy(other.gameObject);
            gameController.GetComponent<GameStateController>().currentScore += 10;
            gameController.GetComponent<GameStateController>().totalPellets--;
        }
        
        if(other.CompareTag("Ghost") && !isBuffed && !other.GetComponent<GhostStateManager>().isDead)
        {
            GameObject[] HUDLives = HUD.GetComponent<UIManager>().lives;
            HUDLives[livesLeft-1].SetActive(false);
            livesLeft --;
            deathEffect.Play();
            aniController.SetTrigger("isDead");
            levelGen.GetComponent<GameStateController>().ResetGame();
            ResetGame();
        }else if(other.CompareTag("Ghost") && isBuffed && !other.GetComponent<GhostStateManager>().isDead)
        {
            gameController.GetComponent<GameStateController>().currentScore += 300;
            other.GetComponent<GhostStateManager>().Die();
        }
    }
    IEnumerator ReenableCollider()
    {
        player.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1f);
        player.GetComponent<BoxCollider>().enabled = true;
    }
}
