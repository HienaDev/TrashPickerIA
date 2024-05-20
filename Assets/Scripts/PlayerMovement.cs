using LibGameAI.NaiveBayes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private TrashGame trashScript;
    // Keeps track of where the player is to validate moves
    private Vector2Int playerPosition;
    // Saves the possible movements for the random move
    private PlayerInputs[] moves;

    [SerializeField] private float timeForEachAITurn;
    private WaitForSeconds waitForSecondsAI;

    // Start is called before the first frame update
    void Start()
    {
        trashScript = FindObjectOfType<TrashGame>();

        waitForSecondsAI = new WaitForSeconds(timeForEachAITurn);

        Debug.Log(timeForEachAITurn);

        playerPosition = trashScript.InitialPlayerPosition;

        moves = new PlayerInputs[] { PlayerInputs.Left, PlayerInputs.Right, PlayerInputs.Up, PlayerInputs.Down };

        if(trashScript.AI)
        {
            StartCoroutine(AIPlay());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If it's not AI
        if (!trashScript.AI)
        {
            // Move Right
            if (Input.GetKeyDown(KeyCode.D))
            {
                trashScript.UpdateAI(PlayerInputs.Right, playerPosition);
                CheckPossibleMove(PlayerInputs.Right);
            }
            // Move Left
            if (Input.GetKeyDown(KeyCode.A))
            {
                trashScript.UpdateAI(PlayerInputs.Left, playerPosition);
                CheckPossibleMove(PlayerInputs.Left);
            }
            // Move Up
            if (Input.GetKeyDown(KeyCode.W))
            {
                trashScript.UpdateAI(PlayerInputs.Up, playerPosition);
                CheckPossibleMove(PlayerInputs.Up);
            }
            // Move Down
            if (Input.GetKeyDown(KeyCode.S))
            {
                trashScript.UpdateAI(PlayerInputs.Down, playerPosition);
                CheckPossibleMove(PlayerInputs.Down);
            }
            // Pick up trash
            if (Input.GetKeyDown(KeyCode.E))
            {
                trashScript.UpdateAI(PlayerInputs.Pick, playerPosition);
                CheckPossibleMove(PlayerInputs.Pick);
            }
            // Don't do anything
            if (Input.GetKeyDown(KeyCode.Space))
            {
                trashScript.UpdateAI(PlayerInputs.Stay, playerPosition);
                CheckPossibleMove(PlayerInputs.Stay);
            }
            // Move randomly
            if (Input.GetKeyDown(KeyCode.R))
            {
                trashScript.UpdateAI(PlayerInputs.Random, playerPosition);
                PlayerInputs randomMove = moves[UnityEngine.Random.Range(0, moves.Length)];
                Debug.Log($"Random chose {randomMove}");
                CheckPossibleMove(randomMove);
            }
        }

    }


    private IEnumerator AIPlay()
    {
        
        while (!trashScript.GameOver)
        {
            Debug.Log("AI Playing");

            string move;
            PlayerInputs enumMove;
            move = trashScript.NbClassifier.Predict(new Dictionary<Attrib, string>()
            {
                {trashScript.TileUp, trashScript.Grid[playerPosition.x, playerPosition.y + 1].ToString()},
                {trashScript.TileRight, trashScript.Grid[playerPosition.x + 1, playerPosition.y].ToString()},
                {trashScript.TileDown,trashScript. Grid[playerPosition.x, playerPosition.y - 1].ToString()},
                {trashScript.TileLeft, trashScript.Grid[playerPosition.x - 1, playerPosition.y].ToString()},
                {trashScript.TileMiddle, trashScript.Grid[playerPosition.x, playerPosition.y].ToString()}
            });

            Debug.Log("AI chose " + move);
            Enum.TryParse<PlayerInputs>(move, out enumMove);

            if(enumMove == PlayerInputs.Random)
            {
                PlayerInputs randomMove = moves[UnityEngine.Random.Range(0, moves.Length)];
                Debug.Log($"Random chose {randomMove}");
                CheckPossibleMove(randomMove);
            }
            else
                CheckPossibleMove(enumMove);

            yield return waitForSecondsAI;

            Debug.Log("AI Turn Done");
        }
        
    }

    /// <summary>
    /// Checks if the last move used is a valid move and awards points
    /// based on what it checked for, if it is valid
    /// it returns true, if not it returns false
    /// </summary>
    /// <param name="move">The name of the move to be checked</param>
    /// <returns></returns>
    private void CheckPossibleMove(PlayerInputs move)
    {

        bool possible = false;

        switch (move)
        {
            case PlayerInputs.Left:
                // Checks if its the border of the grid a.k.a wall
                if (trashScript.Grid[playerPosition.x - 1, playerPosition.y] == TileType.Wall)
                {
                    possible = false;
                    trashScript.AddScore(-5);
                }
                else
                {
                    possible = true;
                }

                break;

            case PlayerInputs.Right:
                // Checks if its the border of the grid a.k.a wall
                if (trashScript.Grid[playerPosition.x + 1, playerPosition.y] == TileType.Wall)
                {
                    possible = false;
                    trashScript.AddScore(-5);
                }
                else
                {
                    possible = true;
                }
                break;

            case PlayerInputs.Down:
                // Checks if its the border of the grid a.k.a wall
                if (trashScript.Grid[playerPosition.x, playerPosition.y - 1] == TileType.Wall)
                {
                    possible = false;
                    trashScript.AddScore(-5);
                }
                else
                {
                    possible = true;
                }
                break;

            case PlayerInputs.Up:
                // Checks if its the border of the grid a.k.a wall
                if (trashScript.Grid[playerPosition.x, playerPosition.y + 1] == TileType.Wall)
                {
                    possible = false;
                    trashScript.AddScore(-5);
                }
                else
                {
                    possible = true;
                }
                break;

            case PlayerInputs.Pick:
                // Checks if the tile the player is on has trash
                if (trashScript.Grid[playerPosition.x, playerPosition.y] == TileType.Trash)
                {
                    trashScript.AddScore(10);
                    trashScript.RemoveTrash(playerPosition);
                    possible = true;
                }
                else
                {
                    trashScript.AddScore(-1);
                    possible = false;
                }
                break;

            case PlayerInputs.Stay:
                possible = true;
                break;

            default:
                possible = false;
                break;
        }

        // If the game is over we disable the player movement
        if (trashScript.AddMove())
        {
            gameObject.GetComponent<PlayerMovement>().enabled = false;
        }

        if (possible)
            ExecuteMove(move);

    }

    /// <summary>
    /// Executes a move
    /// </summary>
    /// <param name="move">The name of the move to be executed</param>
    private void ExecuteMove(PlayerInputs move)
    {
        switch (move)
        {
            // Moves the player left
            case PlayerInputs.Left:
                transform.position = new Vector2(transform.position.x - 32, transform.position.y);
                playerPosition.x -= 1;
                break;
            // Moves the player right
            case PlayerInputs.Right:
                transform.position = new Vector2(transform.position.x + 32, transform.position.y);
                playerPosition.x += 1;
                break;
            // Moves the player down
            case PlayerInputs.Down:
                transform.position = new Vector2(transform.position.x, transform.position.y - 32);
                playerPosition.y -= 1;
                break;
            // Moves the player up
            case PlayerInputs.Up:
                transform.position = new Vector2(transform.position.x, transform.position.y + 32);
                playerPosition.y += 1;
                break;
            // Destroys the trash under the player
            case PlayerInputs.Pick:
                Destroy(trashScript.GridGameObjects[playerPosition.x, playerPosition.y]);
                break;
            // Doesn't do anything, could use default but we keep "stay" for consistency
            case PlayerInputs.Stay:
                break;
            default:
                break;
        }


    }
}
