using System.Collections.Generic;
using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    public GridManager gridManager;
    public List<string> touchedCells = new List<string>(); // Stores the names of touched cells
    public TetrisManager tetrisManager;
    private void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        tetrisManager = FindObjectOfType<TetrisManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sand"))
        {
            string cellName = collision.transform.parent.name;
            if (!touchedCells.Contains(cellName))
            {
                touchedCells.Add(cellName);
            }
        }
        else if (collision.CompareTag("RightBorder"))
        {
            tetrisManager.isTouchingRightBorder = true;
        }
        else if (collision.CompareTag("LeftBorder"))
        {
            tetrisManager.isTouchingLeftBorder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Sand"))
        {
            string cellName = collision.transform.parent.name;
            if (touchedCells.Contains(cellName))
            {
                touchedCells.Remove(cellName);
            }
        }
        else if (collision.CompareTag("RightBorder"))
        {
            tetrisManager.isTouchingRightBorder = false;
        }
        else if (collision.CompareTag("LeftBorder"))
        {
            tetrisManager.isTouchingLeftBorder = false;
        }
    }

    private void Update()
    {
        bool isGrounded = false;

        if (touchedCells.Count > 0)
        {
            foreach (string cellName in touchedCells)
            {
                string[] coordinates = cellName.Split('_');
                if (coordinates.Length == 2 &&
                    int.TryParse(coordinates[0], out int x) &&
                    int.TryParse(coordinates[1], out int y))
                {
                    if (y == 0 || (y > 0 && gridManager.grid[x, y - 1] == 1))
                    {
                        isGrounded = true; // If there is contact with a occupied cell
                    }
                }
            }
        }

        if (isGrounded)
        {
            FinalizePiece(); // Update all grids in TouchedCells and close the fragment
        }
    }


    private void FinalizePiece()
    {
        // Set the grid values ​​of all cells in TouchedCells to 1
        foreach (string cellName in touchedCells)
        {
            // Parse x and y coordinates from cell name
            string[] coordinates = cellName.Split('_');
            if (coordinates.Length == 2 &&
                int.TryParse(coordinates[0], out int x) &&
                int.TryParse(coordinates[1], out int y))
            {
                if (x >= 0 && x < gridManager.columns && y >= 0 && y < gridManager.rows)
                {
                    gridManager.grid[x, y] = 1;
                }
            }
        }

        touchedCells.Clear();

        Debug.Log("Piece settled and deactivating.");
        tetrisManager.hasActivePiece = false;
        if (!tetrisManager.hasActivePiece) {
            tetrisManager.CreateNewPiece();
        }
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
