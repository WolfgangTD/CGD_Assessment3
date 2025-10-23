using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool gameOver = false;
    public GameObject scoreObj;
    private TextMeshProUGUI scoreText;
    public GameObject timeObj;
    private TextMeshProUGUI timeText;
    private GameObject GameController;
    private GameStateController scoreManager;
    public TextMeshProUGUI startGameText;
    public GameObject screenCoverUI;
    public bool countDownDone = false;
    private GameObject cherryController;

    void Start()
    {
        GameController = GameObject.FindWithTag("LevelGenerator");
        scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        timeText = timeObj.GetComponent<TextMeshProUGUI>();
        scoreManager = GameController.GetComponent<GameStateController>();
        cherryController = GameObject.FindWithTag("CherryController");
        StartCoroutine(CountDown(screenCoverUI, startGameText));
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Score: {scoreManager.currentScore:000000}";
        float time = scoreManager.time;
        int mins = (int)(time / 60);
        int secs = (int)(time % 60);
        int millisecs = (int)(time * 100 % 100);

        timeText.text = $"Time: {mins:00}:{secs:00}:{millisecs:00}";
    }

    IEnumerator CountDown(GameObject screenCoverUI, TextMeshProUGUI startGameText)
    {
        int countdownTime=3;
        while (countdownTime >= 0)
        {
            if (countdownTime == 0)
            {
                startGameText.text = "GO!";
            }
            else
            {
                startGameText.text = $"{countdownTime}";
            }
            yield return new WaitForSeconds(1f);
            countdownTime -= 1;
        }
        screenCoverUI.SetActive(false);
        countDownDone = true;
        StartCoroutine(cherryController.GetComponent<CherryController>().CherrySpawner());
    }
}
