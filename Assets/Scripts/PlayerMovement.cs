using LibGameAI.NaiveBayes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is responsible for the player movement.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float timeForEachAITurn;
    [SerializeField] private float howMuchToMovePerFrame;

    private TrashGame trashScript;
    private Vector2Int playerPosition;
    private PlayerInputs[] moves;
    private WaitForSeconds waitForSecondsAI;
    private bool doneMoving;
    private Animator animator;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        trashScript = FindObjectOfType<TrashGame>();

        waitForSecondsAI = new WaitForSeconds(timeForEachAITurn);

        Debug.Log(timeForEachAITurn);

        playerPosition = trashScript.InitialPlayerPosition;

        animator = GetComponent<Animator>();

        moves = new PlayerInputs[] { PlayerInputs.Left, PlayerInputs.Right, PlayerInputs.Up, PlayerInputs.Down };

        doneMoving = true;

        if (trashScript.AI)
        {
            StartCoroutine(AIPlay());
        }
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // If it's not AI
        if (!trashScript.AI && doneMoving)
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

    /// <summary>
    /// AI plays the game.
    /// </summary>
    /// <returns> Returns the seconds to wait for the AI to play. </returns>
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

            
            Enum.TryParse<PlayerInputs>(move, out enumMove);

            if (enumMove == PlayerInputs.Random)
            {
                PlayerInputs randomMove = moves[UnityEngine.Random.Range(0, moves.Length)];
                Debug.Log($"Random chose {randomMove}");
                CheckPossibleMove(randomMove);
            }
            else
                CheckPossibleMove(enumMove);

            Debug.Log("AI chose " + enumMove.ToString());

            yield return waitForSecondsAI;

            Debug.Log("AI Turn Done");
        }

    }

    /// <summary>
    /// Checks if the last move used is a valid move and awards points
    /// based on what it checked for, if it is valid
    /// it returns true, if not it returns false.
    /// </summary>
    /// <param name="move"> The name of the move to be checked. </param>
    /// <returns> Returns true if the move is possible, false otherwise. </returns>
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
                animator.SetTrigger("PickUp");
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
    /// Executes a move.
    /// </summary>
    /// <param name="move"> The name of the move to be executed. </param>
    private void ExecuteMove(PlayerInputs move)
    {
        switch (move)
        {
            // Moves the player left
            case PlayerInputs.Left:
                if (trashScript.PlayerInstantMovement)
                    transform.position = new Vector2(transform.position.x - 32, transform.position.y);
                else
                    StartCoroutine(MoveRobot(new Vector2(transform.position.x - 32, transform.position.y)));

                playerPosition.x -= 1;
                break;
            // Moves the player right
            case PlayerInputs.Right:
                if (trashScript.PlayerInstantMovement)
                    transform.position = new Vector2(transform.position.x + 32, transform.position.y);
                else
                    StartCoroutine(MoveRobot(new Vector2(transform.position.x + 32, transform.position.y)));
                playerPosition.x += 1;
                break;
            // Moves the player down
            case PlayerInputs.Down:
                if (trashScript.PlayerInstantMovement)
                    transform.position = new Vector2(transform.position.x, transform.position.y - 32);
                else
                    StartCoroutine(MoveRobot(new Vector2(transform.position.x, transform.position.y - 32)));

                playerPosition.y -= 1;
                break;
            // Moves the player up
            case PlayerInputs.Up:
                if (trashScript.PlayerInstantMovement)
                    transform.position = new Vector2(transform.position.x, transform.position.y + 32);
                else
                    StartCoroutine(MoveRobot(new Vector2(transform.position.x, transform.position.y + 32)));

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

    /// <summary>
    /// Moves the robot to a new position.
    /// </summary>
    /// <param name="newPos"> The new position to move the robot to. </param>
    /// <returns> Returns null when the robot is done moving. </returns>
    private IEnumerator MoveRobot(Vector2 newPos)
    {
        float lerpValue = 0;

        doneMoving = false;

        animator.SetBool("Moving", !doneMoving);

        Vector2 initialPos = transform.position;

        if (!(initialPos.x == newPos.x))
            GetComponent<SpriteRenderer>().flipX = initialPos.x > newPos.x;
 

        while (!doneMoving)
        {
            lerpValue += howMuchToMovePerFrame * Time.deltaTime;
            if (lerpValue > 1)
            {
                lerpValue = 1;
                doneMoving = true;
            }
            transform.position = Vector2.Lerp(initialPos, newPos, lerpValue);
            yield return null;
        }

        animator.SetBool("Moving", !doneMoving);
    }
}
