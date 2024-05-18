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

    private Vector2Int initialPlayerPosition;
    public Vector2Int InitialPlayerPosition => initialPlayerPosition;

    private int score;
    private int movesMade;

    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI movesUI;
    [SerializeField] private GameObject gameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        grid = new int[gridSize, gridSize];
        gridGameObjects = new GameObject[gridSize, gridSize];

        movesMade = 0;

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

        GameObject player = Instantiate(robot);
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
            return true;
        }
        return false;
    }

}
