using LibGameAI.NaiveBayes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This script is responsible for managing the game state.
/// </summary>
public class TrashGame : MonoBehaviour
{
    // These 3 values are available for change in the inspector, the size of the grid
    // the max amount of moves and the chance for trash to spawn
    [Tooltip("A number above 8 will have tiles offscren"), Header("[CONFIGURATIONS]"),
     SerializeField] private int gridSize;
    [SerializeField] private int maxMoves;
    [SerializeField] private bool playerInstantMovement;
    [SerializeField, Header("Rng")] private bool seeded;
    [SerializeField] private string seed;
    [SerializeField, Range(0, 100)] private int chanceForTrash;

    private System.Random random;

    /// <summary>
    /// Player can move instantly or not.
    /// </summary>
    public bool PlayerInstantMovement => playerInstantMovement;

    /// <summary>
    /// The size of the grid.
    /// </summary>
    public int GridSize => gridSize;

    // The size of the grid + 2 to account for the walls
    private int borderSize;
    // Keeps the states of the grid, 0 for empty, 1 for wall and 2 for trash
    private TileType[,] grid;
    public TileType[,] Grid => grid;

    // AI variables
    private Attrib tileUp, tileRight, tileDown, tileLeft, tileMiddle;
    public Attrib TileUp => tileUp;
    public Attrib TileRight => tileRight;
    public Attrib TileDown => tileDown;
    public Attrib TileLeft => tileLeft;
    public Attrib TileMiddle => tileMiddle;
    public NaiveBayesClassifier NbClassifier { get; private set; }
    private int aiObservations = 0;
    public bool AI { get; set; }

    // The prefabs to instantiate
    [SerializeField, Header("[PREFABS]")] private GameObject robot;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject trash;

    // Keeps the a grid with the objects inside
    private GameObject[,] gridGameObjects;
    public GameObject[,] GridGameObjects => gridGameObjects;
    // Keeps the player object
    private GameObject player;

    // The initial position for the player
    private Vector2Int initialPlayerPosition;
    public Vector2Int InitialPlayerPosition => initialPlayerPosition;

    // The score and moves for the UI
    private int score;
    private int[] bestScores;
    private int movesMade;

    // The UI objects
    [SerializeField, Header("[UI]")] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI movesUI;
    [SerializeField] private TextMeshProUGUI aiObservationsUI;
    [SerializeField] private TextMeshProUGUI bestScoresUI;
    [SerializeField] private TextMeshProUGUI bestScoresLeaderboardUI;
    [SerializeField] private GameObject gameOverUI;

    /// <summary>
    /// Boolean to check if the game is over.
    /// </summary>
    /// <value> States if the game is over. </value>
    public bool GameOver { get; private set; }

    private ButtonsManager buttonsManager;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        // Get the buttons manager
        buttonsManager = FindObjectOfType<ButtonsManager>();

        bestScores = new int[6] { -1000000, -1000000, -1000000, -1000000, -1000000, -1000000 };

        player = null;

        GameOver = false;

        // Get the border size
        borderSize = gridSize + 2;

        // Bool to keep track of who is playing
        AI = false;

        // Initalize the arrays
        gridGameObjects = new GameObject[borderSize, borderSize];
        grid = new TileType[borderSize, borderSize];

        // Fill objects with null to avoid errors
        for (int i = 0; i < borderSize; i++)
        {
            for (int j = 0; j < borderSize; j++)
            {
                gridGameObjects[i, j] = null;
            }
        }

        InitAI();
    }

    /// <summary>
    /// Initializes the AI.
    /// </summary>
    private void InitAI()
    {
        tileUp = new Attrib("tileUp", Enum.GetNames(typeof(TileType)));
        tileRight = new Attrib("tileRight", Enum.GetNames(typeof(TileType)));
        tileDown = new Attrib("tileDown", Enum.GetNames(typeof(TileType)));
        tileLeft = new Attrib("tileLeft", Enum.GetNames(typeof(TileType)));
        tileMiddle = new Attrib("tileMiddle", Enum.GetNames(typeof(TileType)));

        Debug.Log(tileUp.Name);
        foreach (string s in tileUp.Values)
        {
            Debug.Log(s);
        }

        Debug.Log("");
        Debug.Log(tileRight.Name);
        foreach (string s in tileRight.Values)
        {
            Debug.Log(s);
        }

        Debug.Log("");
        Debug.Log(tileDown.Name);
        foreach (string s in tileDown.Values)
        {
            Debug.Log(s);
        }

        Debug.Log("");
        Debug.Log(tileLeft.Name);
        foreach (string s in tileLeft.Values)
        {
            Debug.Log(s);
        }

        Debug.Log("");
        Debug.Log(tileMiddle.Name);
        foreach (string s in tileMiddle.Values)
        {
            Debug.Log(s);
        }

        Debug.Log("");


        NbClassifier =
            new NaiveBayesClassifier(Enum.GetNames(typeof(PlayerInputs)),
            new Attrib[] { tileUp, tileRight, tileDown, tileLeft, tileMiddle });
    }


    /// <summary>
    /// Start the game.
    /// </summary>
    public void StartGame()
    {
        if (seeded)
        {
            random = new System.Random(seed.GetHashCode());
        }
        else
            random = new System.Random(DateTime.Now.GetHashCode());

        // Restart all variables and objects
        gameOverUI.SetActive(false);
        score = 0;
        movesMade = 0;
        scoreUI.text = $"Score: {score}";
        movesUI.text = $"Moves: {movesMade}/{maxMoves}";
        GameOver = false;

        // Destroy all objects from a previous run
        foreach (GameObject objects in gridGameObjects)
        {
            if (objects != null)
            {
                Destroy(objects);
            }
        }

        // Reiniatilize the game object grid in case this isn't the first run
        for (int i = 0; i < borderSize; i++)
        {
            for (int j = 0; j < borderSize; j++)
            {
                gridGameObjects[i, j] = null;
            }
        }

        // Fill the grid with the possible states 0, 1 and 2
        // and fill the object grid with the corresponding objects
        for (int i = 0; i < borderSize; i++)
        {
            for (int j = 0; j < borderSize; j++)
            {
                // Put walls if we are on the border
                if (i == 0 || i == borderSize - 1 || j == 0 || j == borderSize - 1)
                {
                    grid[i, j] = TileType.Wall;
                    GameObject fixedObject = Instantiate(wall, transform);
                    fixedObject.transform.position = new Vector2(i * 32 + 16, j * 32 + 16);
                    gridGameObjects[i, j] = fixedObject;
                }
                // Spawn trash or leave tile empty based on chance chosen on inspector
                else
                {
                    grid[i, j] = random.Next(0, 100) < chanceForTrash ? TileType.Trash : TileType.Empty;

                    if (grid[i, j] == TileType.Trash)
                    {
                        GameObject fixedObject = Instantiate(trash, transform);
                        fixedObject.transform.position = new Vector2(i * 32 + 16, j * 32 + 16);
                        gridGameObjects[i, j] = fixedObject;
                    }
                }
            }
        }

        // DEBUGGING of grid
        string gridDisplay = "";
        for (int j = borderSize - 1; j >= 0; j--)
        {
            gridDisplay += "|";
            for (int i = 0; i < borderSize; i++)
            {
                gridDisplay += grid[i, j];
                gridDisplay += ", ";
            }
            gridDisplay += "|\n";
        }
        Debug.Log(gridDisplay);

        // If there's a player from a previous run destroy it
        if (player != null)
            Destroy(player);

        // Get random player position to start on that isn't on the borders
        initialPlayerPosition.x = random.Next(1, gridSize + 1);
        initialPlayerPosition.y = random.Next(1, gridSize + 1);

        // Instantite player at random position
        player = Instantiate(robot);
        player.transform.position = new Vector2(initialPlayerPosition.x * 32 + 16, initialPlayerPosition.y * 32 + 16);
    }

    /// <summary>
    /// Remove trash from the grid.
    /// </summary>
    /// <param name="pos"> The position to remove the trash from. </param>
    public void RemoveTrash(Vector2Int pos) => grid[pos.x, pos.y] = 0;

    /// <summary>
    /// Update the AI with the player's move and position.
    /// </summary>
    /// <param name="move"> The player's move. </param>
    /// <param name="playerPosition"> The player's position. </param>
    public void UpdateAI(PlayerInputs move, Vector2Int playerPosition)
    {
        NbClassifier.Update(
            move.ToString(),
            new Dictionary<Attrib, string>()
            {
                {tileUp, grid[playerPosition.x, playerPosition.y + 1].ToString()},
                {tileRight, grid[playerPosition.x + 1, playerPosition.y].ToString()},
                {tileDown, grid[playerPosition.x, playerPosition.y - 1].ToString()},
                {tileLeft, grid[playerPosition.x - 1, playerPosition.y].ToString()},
                {tileMiddle, grid[playerPosition.x, playerPosition.y].ToString()}
            }
        );

        Debug.Log($"When tileUp is {grid[playerPosition.x, playerPosition.y + 1].ToString()}\n" +
            $"When tileRight is {grid[playerPosition.x + 1, playerPosition.y].ToString()}\n" +
            $"When tileDown is {grid[playerPosition.x, playerPosition.y - 1].ToString()} \n" +
            $"When tileLeft is {grid[playerPosition.x - 1, playerPosition.y].ToString()}\n" +
            $"When tileMiddle is {grid[playerPosition.x, playerPosition.y].ToString()} \n" +
            $"Player chose {move.ToString()}");

        aiObservations++;
        aiObservationsUI.text = $"AI Observations: {aiObservations}";
    }

    /// <summary>
    /// Displays the top 6 scores on the screen
    /// </summary>
    public void DisplayLeaderboard()
    {
        if(!bestScoresLeaderboardUI.gameObject.activeSelf)
        {
            bestScoresLeaderboardUI.gameObject.SetActive(true);

            string bestScoreText = "Best Scores:\n";

            for (int i = 0; i < bestScores.Length; i++)
            {
                bestScoreText += $"{i + 1} - ";

                if (bestScores[i] == score)
                    bestScoreText += $"<color=#FFFF00>{bestScores[i]}</color>\n";
                else if (bestScores[i] != -1000000)
                    bestScoreText += $"{bestScores[i]}\n";
                else
                    bestScoreText += $"___\n";
            }

            bestScoresLeaderboardUI.text = bestScoreText;
        }
        else
            bestScoresLeaderboardUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// Add score value and change score UI.
    /// </summary>
    /// <param name="value"> The value to be added to the score. </param>
    public void AddScore(int value)
    {
        score += value;
        scoreUI.text = $"Score: {score}";
        Debug.Log("Added " + value + " to score");
    }

    /// <summary>
    /// Add +1 to amount of moves and updates MoveUI.
    /// </summary>
    /// <returns> Returns true if we've hit the max amout of moves. </returns>
    public bool AddMove()
    {
        movesMade++;
        movesUI.text = $"Moves: {movesMade}/{maxMoves}";
        if (movesMade == maxMoves)
        {
            gameOverUI.SetActive(true);
            gameOverUI.GetComponentInChildren<TextMeshProUGUI>().text = $"GAME OVER\n\nScore:\n{score}";
            buttonsManager.TurnOnButtons();

            for (int i = bestScores.Length - 1; i >= 0; i--)
            {
                if (bestScores[i] < score)
                {
                    bestScores[i] = score;
                    break;
                }
            }
            Array.Sort(bestScores);
            Array.Reverse(bestScores);
            string bestScoreText = "Best Scores:\n";

            for (int i = 0; i < bestScores.Length; i++)
            {
                bestScoreText += $"{i + 1} - ";

                if (bestScores[i] == score)
                    bestScoreText += $"<color=#FFFF00>{bestScores[i]}</color>\n";
                else if (bestScores[i] != -1000000)
                    bestScoreText += $"{bestScores[i]}\n";
                else
                    bestScoreText += $"___\n";
            }

            bestScoresUI.text = bestScoreText;
            bestScoresLeaderboardUI.text = bestScoreText;   
            GameOver = true;
            return true;
        }
        return false;
    }
}