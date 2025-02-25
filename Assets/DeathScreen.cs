using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public TextMeshProUGUI timeText, scoreText;

    // Run when set active
    void Awake()
    {
        int totalXP = HUD.xpTotal;
        scoreText.text = string.Format(scoreText.text, totalXP.ToString());

        int timeSeconds = Mathf.RoundToInt(HUD.surviveSeconds);
        int timeHour = 0;

        while (timeSeconds >= 60) {
            timeSeconds -= 60;
            timeHour += 1;
        }

        timeText.text = string.Format(timeText.text, timeHour.ToString("D2"), timeSeconds.ToString("D2"));
    }

    // Restart Button
    public void RestartGame() {
        Time.timeScale = 1.0f; // Unpause game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
