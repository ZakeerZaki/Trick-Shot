using UnityEngine;
using TMPro;
using System.Drawing;

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    private const string ScoreKey = "PlayerScore";

    private void Start()
    {
        // Load the score from PlayerPrefs when the game starts
        currentScore = PlayerPrefs.GetInt(ScoreKey, 0); // Default score is 0 if not found
        UpdateScoreUI();
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        SaveScore();
        UpdateScoreUI();
    }

    public void SubtractScore(int scoreToSubtract)
    {
        currentScore -= scoreToSubtract;
        SaveScore();
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    private void SaveScore()
    {
        // Save the current score to PlayerPrefs
        PlayerPrefs.SetInt(ScoreKey, currentScore);
        PlayerPrefs.Save(); // Ensure the changes are saved\
        PlayfabManager.Instance.UpdatePlayerStatistics(currentScore);
    }

    private void OnApplicationQuit()
    {
        SaveScore(); // Save the score when the application is quitting
    }

    private void OnDestroy()
    {
        SaveScore(); // Save the score when the object is destroyed (optional, for safety)
    }
}
