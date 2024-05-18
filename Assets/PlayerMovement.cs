using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private TrashGame trashScript;
    private Vector2Int playerPosition;

    private string[] moves;

    // Start is called before the first frame update
    void Start()
    {
        trashScript = FindObjectOfType<TrashGame>();

        playerPosition = trashScript.InitialPlayerPosition;

        moves = new string[] { "left", "right", "up", "down" };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (CheckPossibleMove("right"))
            {
                ExecuteMove("right");
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (CheckPossibleMove("left"))
            {
                ExecuteMove("left");
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (CheckPossibleMove("up"))
            {
                ExecuteMove("up");
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (CheckPossibleMove("down"))
            {
                ExecuteMove("down");
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CheckPossibleMove("pick"))
            {
                ExecuteMove("pick");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckPossibleMove("stay"))
            {
                ExecuteMove("stay");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            string randomMove = moves[Random.Range(0, moves.Length)];
            Debug.Log($"Random chose {randomMove}");
            if (CheckPossibleMove(randomMove))
            {
                ExecuteMove(randomMove);
            }
        }
    }

    private bool CheckPossibleMove(string move)
    {

        bool possible = false;

        switch (move)
        {
            case "left":
                if ((transform.position.x - 48) / 32 >= 0)
                {
                    possible = true;
                }
                else
                {
                    trashScript.AddScore(-5);
                    possible = false;
                }

                break;

            case "right":
                if ((transform.position.x + 48) / 32 <= trashScript.GridSize)
                {
                    possible = true;
                }
                else
                {
                    trashScript.AddScore(-5);
                    possible = false;
                }
                break;

            case "down":
                if ((transform.position.y - 48) / 32 >= 0)
                {
                    possible = true;
                }
                else
                {
                    trashScript.AddScore(-5);
                    possible = false;
                }
                break;

            case "up":
                if ((transform.position.y + 48) / 32 <= trashScript.GridSize)
                {
                    possible = true;
                }
                else
                {
                    trashScript.AddScore(-5);
                    possible = false;
                }
                break;

            case "pick":
                if (trashScript.Grid[playerPosition.x, playerPosition.y] == 2)
                {
                    trashScript.AddScore(10);
                    possible = true;
                }
                else
                {
                    trashScript.AddScore(-1);
                    possible = false;
                }
                break;

            case "stay":
                possible = true;
                break;

            default:
                possible = false;
                break;
        }

        if (trashScript.AddMove())
        {
            gameObject.GetComponent<PlayerMovement>().enabled = false;
        }

        return possible;
    }

    private void ExecuteMove(string move)
    {
        switch (move)
        {
            case "left":
                transform.position = new Vector2(transform.position.x - 32, transform.position.y);
                playerPosition.x -= 1;
                break;
            case "right":
                transform.position = new Vector2(transform.position.x + 32, transform.position.y);
                playerPosition.x += 1;
                break;
            case "down":
                transform.position = new Vector2(transform.position.x, transform.position.y - 32);
                playerPosition.y -= 1;
                break;
            case "up":
                transform.position = new Vector2(transform.position.x, transform.position.y + 32);
                playerPosition.y += 1;
                break;
            case "pick":
                Destroy(trashScript.GridGameObjects[playerPosition.x, playerPosition.y]);
                break;
            case "stay":
                break;
            default:
                break;
        }


    }
}
