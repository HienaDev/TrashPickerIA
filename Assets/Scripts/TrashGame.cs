using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGame : MonoBehaviour
{

    [SerializeField] private int gridSize;
    public int GridSize => gridSize;

    private int[,] grid;

    [SerializeField] private GameObject robot;
    private bool emptyPlaceForPlayer = false;

    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject trash;

    // Start is called before the first frame update
    void Start()
    {
        grid = new int[gridSize, gridSize];

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
                    GameObject fixedObject = Instantiate(wall);
                    fixedObject.transform.position = new Vector2(i * 32 + 16, j * 32 + 16);
                }  
                if (grid[i, j] == 2)
                {
                    GameObject fixedObject = Instantiate(trash);
                    fixedObject.transform.position = new Vector2(i * 32 + 16, j * 32 + 16);
                }
                    

                
            }
        }
        string gridDisplay = "";
        for (int i = 0; i < gridSize; i++)
        {
            gridDisplay += "|";
            for (int j = 0; j < gridSize; j++)
            {
                gridDisplay += grid[i, j];
                gridDisplay += ", ";
            }
            gridDisplay += "|\n";
        }

        Debug.Log(gridDisplay);

        Vector2Int positionForPlayer = Vector2Int.zero;


        do
        {
            positionForPlayer.x = Random.Range(0, gridSize);
            positionForPlayer.y = Random.Range(0, gridSize);

            if (grid[positionForPlayer.x, positionForPlayer.y] != 1)
            {
                emptyPlaceForPlayer = true;
            }
        } while (!emptyPlaceForPlayer);

        GameObject player = Instantiate(robot);
        player.transform.position = new Vector2(positionForPlayer.x * 32 + 16, positionForPlayer.y * 32 + 16);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
