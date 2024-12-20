using System.Collections.Generic;
using UnityEngine;
using static TetrisPiece;

public class TetrisManager : MonoBehaviour
{
    public GridManager gridManager;
    public bool hasActivePiece;
    public List<Vector2Int> activePieceCells = new List<Vector2Int>();

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    CreateNewPiece();
        //}



        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    MoveActivePiece(Vector2Int.left);
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    MoveActivePiece(Vector2Int.right);
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    MoveActivePiece(Vector2Int.down);
        //}
        PieceDown();
    }

    public void CreateNewPiece(int[,] shape, int startX, int startY)
    {
        Debug.Log("Created new piece");
        startX = Random.Range(15, gridManager.columns - 15); //Tam ekranýn dibi olmasýn diye
        startY = gridManager.rows - 2;
        shape = ChooseRandomShape();
        hasActivePiece = true;
        activePieceCells.Clear();

        for (int i = 0; i < shape.GetLength(0); i++)
        {
            for (int j = 0; j < shape.GetLength(1); j++)
            {
                if (shape[i, j] == 1)
                {
                    int x = startX + j;
                    int y = startY - i;

                    if (x >= 0 && x < gridManager.columns && y >= 0 && y < gridManager.rows)
                    {
                        gridManager.grid[x, y] = 1;
                        activePieceCells.Add(new Vector2Int(x, y));
                    }
                }
            }
        }
    }

    void MoveActivePiece(Vector2Int direction)
    {
        // Yeni pozisyonlarý hesaplayýn
        List<Vector2Int> newPositions = new List<Vector2Int>();
        foreach (var cell in activePieceCells)
        {
            Vector2Int newPos = cell + direction;
            if (newPos.x < 0 || newPos.x >= gridManager.columns || newPos.y < 0 || newPos.y >= gridManager.rows || gridManager.grid[newPos.x, newPos.y] == 1)
            {
                return; // Hareket geçersiz
            }
            newPositions.Add(newPos);
        }

        // Eski pozisyonlarý temizleyin
        foreach (var cell in activePieceCells)
        {
            gridManager.grid[cell.x, cell.y] = 0;
        }

        // Yeni pozisyonlarý uygulayýn
        foreach (var cell in newPositions)
        {
            gridManager.grid[cell.x, cell.y] = 1;
        }

        // Aktif parçanýn koordinatlarýný güncelleyin
        activePieceCells = newPositions;
    }

    private void PieceDown()
    {
        // Sað ve sol limit kontrolü için geçici bir liste
        List<Vector2Int> tempCells = new List<Vector2Int>(activePieceCells);

        // Eski pozisyonlarý temizleyin
        foreach (var cell in activePieceCells)
        {
            gridManager.grid[cell.x, cell.y] = 0;
        }

        // Hareket yönü belirleyin
        Vector2Int moveDirection = Vector2Int.down; // Varsayýlan aþaðý hareket
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = Vector2Int.right + Vector2Int.down; // Saða ve aþaðý
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = Vector2Int.left + Vector2Int.down; // Sola ve aþaðý
        }

        // Yeni pozisyonlarý kontrol et
        for (int i = 0; i < tempCells.Count; i++)
        {
            Vector2Int newCell = tempCells[i] + moveDirection;

            // Grid sýnýrlarýnýn dýþýna çýkmayý engelle
            if (newCell.x < 0 || newCell.x >= gridManager.columns || newCell.y == 0)
            {
                Debug.Log("There is no active piece");
                hasActivePiece = false;
                // Yeni pozisyonlarý uygula
                activePieceCells = tempCells;
                foreach (var cell in activePieceCells)
                {
                    gridManager.grid[cell.x, cell.y] = 1;
                }
                return; // Hareketi durdur
            }

            // Çarpýþma kontrolü
            if (gridManager.grid[newCell.x, newCell.y] == 1)
            {
                return; // Hareketi durdur
            }

            // Yeni pozisyonlarý kaydet
            tempCells[i] = newCell;
        }

        // Yeni pozisyonlarý uygula
        activePieceCells = tempCells;
        foreach (var cell in activePieceCells)
        {
            gridManager.grid[cell.x, cell.y] = 1;
        }
    }


    //public void CreateNewPiece()
    //{
    //    int xIndex = Random.Range(15, gridManager.columns-15); //Tam ekranýn dibi olmasýn diye
    //    hasActivePiece = true;
    //    SpawnShape(xIndex, gridManager.rows-2, ChooseRandomShape()); //en üstte doðsun diye

    //}

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

        return shapes[Random.Range(0, shapes.Count)];
        //return TetrisShapes.O;
    }
}
