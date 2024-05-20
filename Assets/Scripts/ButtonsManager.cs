using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    private TrashGame trashScript;

    void Start()
    {
        trashScript = FindObjectOfType<TrashGame>();
        // Ensure the game starts in a paused state
        Time.timeScale = 0f;
    }

    public void StartWithHuman()
    {
        Time.timeScale = 1f;
        trashScript.AI = false;
        trashScript.StartGame();
        buttons.SetActive(false);
    }

    public void StartWithAI()
    {
        Time.timeScale = 1f;
        trashScript.AI = true;
        trashScript.StartGame();
        buttons.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TurnOnButtons()
    {
        buttons.SetActive(true);
    }
}
