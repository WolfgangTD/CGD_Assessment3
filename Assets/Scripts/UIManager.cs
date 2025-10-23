using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject scoreObj;
    private TextMeshProUGUI scoreText;
    public GameObject timeObj;
    private TextMeshProUGUI timeText;
    public GameObject GameController;
    private ScoreManager scoreManager;

    void Start()
    {
        scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        timeText = timeObj.GetComponent<TextMeshProUGUI>();
        scoreManager = GameController.GetComponent<ScoreManager>();
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

    void StartGameCountdown()
    {
        
    }
}
