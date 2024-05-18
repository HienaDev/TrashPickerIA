using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TrashGame : MonoBehaviour
{

    [SerializeField] private int gridSize;
    public int GridSize => gridSize;

    private int[,] grid;
    public int[,] Grid => grid;

    [SerializeField] private GameObject robot;
    private bool emptyPlaceForPlayer = false;

    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject trash;
    private GameObject[,] gridGameObjects;
    public GameObject[,] GridGameObjects => gridGameObjects;
    private GameObject player;

    private Vector2Int initialPlayerPosition;
    public Vector2Int InitialPlayerPosition => initialPlayerPosition;

    private int score;
    private int movesMade;

    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI movesUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject playAgainButton;

    // Start is called before the first frame update
    void Start()
    {
        player = null;
        gridGameObjects = new GameObject[gridSize, gridSize];
        grid = new int[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                gridGameObjects[i, j] = null;
            }
        }

        StartGame();
    }

    public void StartGame()
    {
        gameOverUI.SetActive(false);
        playAgainButton.SetActive(false);
        score = 0;
        movesMade = 0;
        emptyPlaceForPlayer = false;
        scoreUI.text = $"Score: {score}";
        movesUI.text = $"Moves: {movesMade}/20";

        foreach (GameObject objects in gridGameObjects)
        {
            if(objects != null)
            {
                Destroy(objects);
            }
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                gridGameObjects[i, j] = null;
            }
        }
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                // 0 = Empty
                // 1 = Wall
                // 2 = Trash
                grid[i, j] = Random.Range(0, 3);



                if (grid[i, j] == 1)
                {
                    GameObject fixedObject = Instantiate(wall, transform);
                    fixedObject.transform.position = new Vector2(i * 32 + 16, j * 32 + 16);
                    gridGameObjects[i, j] = fixedObject;
                }
                if (grid[i, j] == 2)
                {
                    GameObject fixedObject = Instantiate(trash, transform);
                    fixedObject.transform.position = new Vector2(i * 32 + 16, j * 32 + 16);
                    gridGameObjects[i, j] = fixedObject;
                }



            }
        }

        string gridDisplay = "";
        for (int j = gridSize - 1; j >= 0; j--)
        {
            gridDisplay += "|";
            for (int i = 0; i < gridSize; i++)
            {
                gridDisplay += grid[i, j];
                gridDisplay += ", ";
            }
            gridDisplay += "|\n";
        }

        Debug.Log(gridDisplay);

        initialPlayerPosition = Vector2Int.zero;

        do
        {
            initialPlayerPosition.x = Random.Range(0, gridSize);
            initialPlayerPosition.y = Random.Range(0, gridSize);

            if (grid[initialPlayerPosition.x, initialPlayerPosition.y] != 1)
            {
                emptyPlaceForPlayer = true;
            }
        } while (!emptyPlaceForPlayer);

        if(player != null)
            Destroy(player);

        player = Instantiate(robot);
        player.transform.position = new Vector2(initialPlayerPosition.x * 32 + 16, initialPlayerPosition.y * 32 + 16);
    }

    public void AddScore(int value)
    { 
        score += value;
        scoreUI.text = $"Score: {score}";
        Debug.Log("Added " + value + " to score");
    }

    public bool AddMove()
    {
        movesMade++;
        movesUI.text = $"Moves: {movesMade}/20";
        if (movesMade == 20)
        {
            gameOverUI.SetActive(true);
            gameOverUI.GetComponent<TextMeshProUGUI>().text = $"GAME OVER\n\nScore: {score}";
            playAgainButton.SetActive(true);
            return true;
        }
        return false;
    }

}
