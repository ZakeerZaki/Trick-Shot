using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject gameplayCanvas; // Reference to the gameplay canvas
    [SerializeField] private GameObject howToPlayCanvas; // Reference to the How to Play canvas
    [SerializeField] private GameObject leaderboardCanvas; // Reference to the Leaderboard canvas
    [SerializeField] private GameObject cannonGameObject; // Reference to the Cannon GameObject

    private void Start()
    {
        ShowMainMenu();

        if (cannonGameObject != null)
        {
            cannonGameObject.SetActive(false); // Ensure cannon is inactive at the start
        }

        if (howToPlayCanvas != null)
        {
            howToPlayCanvas.SetActive(false); // Ensure How to Play canvas is inactive at the start
        }

        if (leaderboardCanvas != null)
        {
            leaderboardCanvas.SetActive(false); // Ensure Leaderboard canvas is inactive at the start
        }
    }

    public void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        gameplayCanvas.SetActive(false); // Ensure gameplay canvas is inactive

        if (howToPlayCanvas != null)
        {
            howToPlayCanvas.SetActive(false); // Hide How to Play canvas when showing the main menu
        }

        if (leaderboardCanvas != null)
        {
            leaderboardCanvas.SetActive(false); // Hide Leaderboard canvas when showing the main menu
        }
    }

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false); // Hide main menu
        gameplayCanvas.SetActive(true); // Show gameplay canvas

        if (cannonGameObject != null)
        {
            cannonGameObject.SetActive(true); // Activate cannon when game starts
        }

        // Any additional logic to start the game can be added here.
    }

    public void ShowHowToPlay()
    {
        if (howToPlayCanvas != null)
        {
            howToPlayCanvas.SetActive(true); // Show How to Play canvas
        }

        mainMenuCanvas.SetActive(false); // Hide main menu
        gameplayCanvas.SetActive(false); // Hide gameplay canvas
    }

    public void ShowLeaderboard()
    {
        if (leaderboardCanvas != null)
        {
            leaderboardCanvas.SetActive(true); // Show Leaderboard canvas
        }

        mainMenuCanvas.SetActive(false); // Hide main menu
        gameplayCanvas.SetActive(false); // Hide gameplay canvas
        if (howToPlayCanvas != null)
        {
            howToPlayCanvas.SetActive(false); // Hide How to Play canvas
        }
    }

    public void BackToMainMenu()
    {
        if (howToPlayCanvas != null)
        {
            howToPlayCanvas.SetActive(false); // Hide How to Play canvas
        }

        if (leaderboardCanvas != null)
        {
            leaderboardCanvas.SetActive(false); // Hide Leaderboard canvas
        }

        ShowMainMenu(); // Show the main menu again
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
