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
