using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiateGrid : MonoBehaviour
{
    [SerializeField] private TrashGame trashScript;
    private int x;
    private int y;

    [SerializeField] private GameObject gridPrefab;
 
    //-160 -160

    // Start is called before the first frame update
    void Start()
    {
        x = 16;
        y = 16;

        //Debug.Log(Screen.width);
        //Debug.Log(Screen.height);

        for (int i = 0; i < trashScript.GridSize; i++)
        {
            for (int j = 0; j < trashScript.GridSize; j++)
            {
                GameObject temp = Instantiate(gridPrefab, transform);
                temp.transform.position = new Vector2(x + i * 32, y + j * 32);
            }
        }
    }


}
