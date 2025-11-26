using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Drawing;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public TextMeshProUGUI scoreText;

    public Button BatteryButton;

    public float timer;

    public float score;

    public float battery;

    // Update is called once per frame
    void Update()
    {
        battery = PlayerMovement.Battery;
        UpdateTimer();
        timer = TsunamiBehaivour.spawnstaticTimer;
        score = PlayerMovement.Score;
        UpdateScore();

        // ...existing code...

// ...existing code...
    }

    public void UpdateTimer()
    {
        timerText.text = "Time: " + timer;
    }
    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void OnBatteryButtonClick()
    {
        Debug.Log("Battery Button Clicked!");
        // Implement battery-related functionality here
        battery += 1f;
    }

}
