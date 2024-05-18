using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGame : MonoBehaviour
{

    [SerializeField] private int gridSize;
    public int GridSize => gridSize;

    private int[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new int[gridSize, gridSize];

        for(int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                grid[i, j] = Random.Range(0, 3);
                
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
