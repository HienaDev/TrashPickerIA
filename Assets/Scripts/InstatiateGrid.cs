using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiateGrid : MonoBehaviour
{
    private TrashGame trashScript;

    [SerializeField] private GameObject gridPrefab;
 
    void Start()
    {

        trashScript = FindObjectOfType<TrashGame>();

        // Instantiates grid tiles for the map layout
        // Each tile is 32x32 so we instantiate them by
        // that interval, and center them by adding 16 to its
        // coordinates
        for (int i = 0; i < trashScript.GridSize; i++)
        {
            for (int j = 0; j < trashScript.GridSize; j++)
            {
                GameObject temp = Instantiate(gridPrefab, transform);
                temp.transform.position = new Vector2(16 + (i + 1) * 32, 16 + (j + 1) * 32);
            }
        }
    }


}
