using UnityEngine;
using static TetrisPiece;

public class GridManager : MonoBehaviour
{
    public GameObject cellObject; //The gameobject created for each grid cell
    public int[,] grid;
    public GameObject[,] cellSRenderers; // Grid for SpriteRenderer's
    public int columns; //Grid columns
    public int rows; //Grid rows
    float spriteSize = 0.1f;
    public Vector3 gridStartPosition; // Grid's start position
    private Color[,] cellColors;
    public Color sandColor;
    public Color backgroundColor;
    public TetrisManager tetrisManager;
    public bool tetrisMode;
    void Start()
    {
        // Calculating camera borders
        gridStartPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));


        grid = new int[columns, rows];
        cellSRenderers = new GameObject[columns, rows];

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                grid[i, j] = 0;
                SpawnTile(i, j, grid[i, j], spriteSize, gridStartPosition);
            }
        }

        //cellColors = new Color[columns, rows];
        //for (int x = 0; x < columns; x++)
        //{
        //    for (int y = 0; y < rows; y++)
        //    {
        //        cellColors[x, y] = sandColor;
        //        //cellColors[x, y] = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        //    }
        //}
    }

    private void SpawnTile(int x, int y, float value, float spriteSize, Vector3 bottomLeft)
    {
        GameObject cell = Instantiate(cellObject);
        cellSRenderers[x, y] = cell;
        //Set position based on bottom left corner position
        cell.transform.position = new Vector3(bottomLeft.x + (x * spriteSize), bottomLeft.y + (y * spriteSize), 0);
    }

    void Update()
    {
        if (tetrisMode && tetrisManager != null && !tetrisManager.hasActivePiece)
        {
            Debug.Log("updating grid");
            UpdateGrid();
        }
        else if (!tetrisMode) {
            UpdateGrid();
        }
        UpdateCellSprites();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tetrisManager.CreateNewPiece(TetrisShapes.T,0,0);
        }
    }

    void UpdateGrid()
    {
        for (int y = 0; y < rows - 1; y++) // Movement towards the bottom line
        {
            for (int x = 0; x < columns; x++) 
            {
                if (grid[x, y] == 1 && y != 0 && grid[x, y - 1] == 0)
                {
                    grid[x, y] = 0;      // Empty the current cell
                    grid[x, y - 1] = 1; // The lower cell becomes sand
                    //HandleTetrisPieces(x,y);
                }
                //Randomness if both right and left cells are empty
                else if (grid[x, y] == 1 && x > 0 && x < columns - 1 && y != 0 && grid[x + 1, y - 1] == 0 && grid[x - 1, y - 1] == 0) 
                {
                    int randomSide = Random.Range(0, 2); //0 for right side, 1 for left side
                    grid[x, y] = 0;
                    if (randomSide == 0)
                    {
                        grid[x + 1, y - 1] = 1;
                    }
                    else
                    {
                        grid[x - 1, y - 1] = 1;
                    }
                }
                // Right spread if left side is full
                else if (grid[x, y] == 1 && x < columns - 1 && y != 0 && grid[x + 1, y - 1] == 0)
                {
                    grid[x, y] = 0;
                    grid[x + 1, y - 1] = 1;

                }
                // Left spread if right side is full
                else if (grid[x, y] == 1 && x > 0 && y != 0 && grid[x - 1, y - 1] == 0)
                {
                    grid[x, y] = 0;
                    grid[x - 1, y - 1] = 1;
                }
            }
        }
    }

    void UpdateCellSprites()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                UpdateCellColor(x, y);
            }
        }
    }

    void UpdateCellColor(int x, int y)
    {
        SpriteRenderer sr = cellSRenderers[x, y].GetComponentInChildren<SpriteRenderer>(); //bir kez alalým sr leri onu güncelleyelim her update olmasýn
        if (grid[x, y] == 1)
        {
            //sr.color = cellColors[x, y]; // Use calculated color
            sr.color = sandColor;
        }
        else
        {
            sr.color = backgroundColor; // Empty cells
        }
    }

    //void HandleTetrisPieces(int x, int y)
    //{
    //    if (tetrisManager != null && tetrisManager.hasActivePiece)
    //    {
    //        if (Input.GetKey(KeyCode.RightArrow))
    //        {
    //            if (x + 1 < columns && grid[x + 1, y - 1] == 0)
    //            {
    //                Debug.Log("Moving right");

    //                grid[x, y - 1] = 0;
    //                grid[x + 1, y - 1] = 1;
    //            }
    //        }
    //        else if (Input.GetKey(KeyCode.LeftArrow))
    //        {
    //            if (x - 1 >= 0 && grid[x - 1, y - 1] == 0)
    //            {
    //                Debug.Log("Moving left");

    //                grid[x, y - 1] = 0;
    //                grid[x - 1, y - 1] = 1;
    //            }
    //        }
    //    }
    //}

}