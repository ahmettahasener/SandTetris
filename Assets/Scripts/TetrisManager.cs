using System.Collections.Generic;
using UnityEngine;
using static TetrisPiece;

public class TetrisManager : MonoBehaviour
{
    public GridManager gridManager;
    public bool hasActivePiece;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateNewPiece();
        }
    }

    public void CreateNewPiece()
    {
        int xIndex = Random.Range(15, gridManager.columns-15); //Tam ekranýn dibi olmasýn diye
        hasActivePiece = true;
        SpawnShape(xIndex, gridManager.rows-2, ChooseRandomShape()); //en üstte doðsun diye
        
    }

    void SpawnShape(int startX, int startY, int[,] shape)
    {
        for (int i = 0; i < shape.GetLength(0); i++)
        {
            for (int j = 0; j < shape.GetLength(1); j++)
            {
                int x = startX + j;
                int y = startY - i;

                if (x >= 0 && x < gridManager.columns && y >= 0 && y < gridManager.rows && shape[i, j] == 1)
                {
                    gridManager.grid[x, y] = 1;
                }
            }
        }
    }

    private int[,] ChooseRandomShape()
    {
        List<int[,]> shapes = new List<int[,]>
        {
            TetrisShapes.I,
            TetrisShapes.O,
            TetrisShapes.S,
            TetrisShapes.Z,
            TetrisShapes.L,
            TetrisShapes.T
        };

        //return shapes[Random.Range(0, shapes.Count)];
        return TetrisShapes.O;
    }
}
