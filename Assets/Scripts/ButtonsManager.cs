using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    private TrashGame trashGame;
    private bool isAI = false;

    void Start()
    {
        trashGame = FindObjectOfType<TrashGame>();
        // Ensure the game starts in a paused state
        Time.timeScale = 0f;
    }

    public void StartWithHuman()
    {
        isAI = false;
        trashGame.StartGameHuman();
        buttons.SetActive(false);
    }

    public void StartWithAI()
    {
        isAI = true;
        //StartGameAI();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public bool IsAI()
    {
        return isAI;
    }

    public void TurnOnButtons()
    {
        buttons.SetActive(true);
    }
}
