using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private TrashGame trashScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
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
    }

    private bool CheckPossibleMove(string move)
    {
        switch (move)
        {
            case "left":
                return ((transform.position.x - 48) / 32 >= 0);
            case "right":
                return ((transform.position.x + 48) / 32 <= trashScript.GridSize);
            case "down":
                return ((transform.position.y - 48) / 32 >= 0);
            case "up":
                return ((transform.position.y + 48) / 32 <= trashScript.GridSize);
            default:
                return false;
        }
    }

    private void ExecuteMove(string move)
    {
        switch (move)
        {
            case "left":
                transform.position = new Vector2(transform.position.x - 32, transform.position.y);
                break;
            case "right":
                transform.position = new Vector2(transform.position.x + 32, transform.position.y);
                break;
            case "down":
                transform.position = new Vector2(transform.position.x, transform.position.y - 32);
                break;
            case "up":
                transform.position = new Vector2(transform.position.x, transform.position.y + 32);
                break;
            default:
                break;
        }
    }
}
