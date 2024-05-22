using UnityEngine;

/// <summary>
/// This script is responsible for managing the buttons in the main menu.
/// </summary>
public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    
    private TrashGame trashScript;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        trashScript = FindObjectOfType<TrashGame>();
        // Ensure the game starts in a paused state
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Displays the top 6 scores on the screen
    /// </summary>
    public void DisplayLeaderboard()
    {
        if(!trashScript.BestScoresLeaderboardUI.gameObject.activeSelf)
        {
            trashScript.BestScoresLeaderboardUI.gameObject.SetActive(true);

            string bestScoreText = "Best Scores:\n";

            for (int i = 0; i < trashScript.BestScores.Length; i++)
            {
                bestScoreText += $"{i + 1} - ";

                if (trashScript.BestScores[i] == trashScript.Score)
                    bestScoreText += $"<color=#FFFF00>{trashScript.BestScores[i]}</color>\n";
                else if (trashScript.BestScores[i] != -1000000)
                    bestScoreText += $"{trashScript.BestScores[i]}\n";
                else
                    bestScoreText += $"___\n";
            }

            trashScript.BestScoresLeaderboardUI.text = bestScoreText;
        }
        else
            trashScript.BestScoresLeaderboardUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// Start the game with a human player.
    /// </summary>
    public void StartWithHuman()
    {
        Time.timeScale = 1f;
        trashScript.AI = false;
        trashScript.StartGame();
        buttons.SetActive(false);
    }

    /// <summary>
    /// Start the game with an AI player.
    /// </summary>
    public void StartWithAI()
    {
        Time.timeScale = 1f;
        trashScript.AI = true;
        trashScript.StartGame();
        buttons.SetActive(false);
    }

    /// <summary>
    /// Quit the game.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_STANDALONE
            // Quit application if running standalone
            Application.Quit();
#endif
#if UNITY_EDITOR
            // Stop game if running in editor
            UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    /// <summary>
    /// Turn on the buttons.
    /// </summary>
    public void TurnOnButtons()
    {
        buttons.SetActive(true);
    }
}
