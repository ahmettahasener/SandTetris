using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisManager : MonoBehaviour
{
    public GridManager gridManager;
    public bool hasActivePiece;
    public GameObject activePiece;
    public bool isTouchingRightBorder;
    public bool isTouchingLeftBorder;
    public List<GameObject> tetrisPieces;
    public float speed;
    public List<Color> pieceColors;
    private Coroutine createNewPiece;
    private void Start()
    {
        StartCoroutine(SpawnShape());
    }

    void Update()
    {
        MovePiece();
        CheckAndClearConnectedRows();
        if (Input.GetMouseButtonDown(1)) {
            CreateNewPiece();
        }
    }

    private void MovePiece()
    {
        if (activePiece != null && hasActivePiece) {
            activePiece.transform.position += Vector3.down * Time.deltaTime * speed;
            if (Input.GetKey(KeyCode.RightArrow) && !isTouchingRightBorder)
            {
                activePiece.transform.position += Vector3.right * Time.deltaTime * speed;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !isTouchingLeftBorder)
            {
                activePiece.transform.position += Vector3.left * Time.deltaTime * speed;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                activePiece.transform.position += Vector3.down * Time.deltaTime * speed;
            }
        }
    }

    public void CreateNewPiece()
    {
        //Invoke("SpawnShape", 2f);
        createNewPiece = StartCoroutine(SpawnShape());
    }

    private IEnumerator SpawnShape()
    {
        yield return new WaitForSecondsRealtime(2);
        hasActivePiece = true;
        int xPos = Random.Range(7, -7);
        Vector3 pos = new Vector3(xPos, 6, 0);
        activePiece = Instantiate(ChooseRandomShape(), pos, Quaternion.identity);
        gridManager.sandColor = ChooseRandomColor();
        for (int i = 0; i < activePiece.transform.childCount; i++)
        {
            activePiece.transform.GetChild(i).GetComponent<SpriteRenderer>().color = gridManager.sandColor;
        }
        Debug.Log(activePiece + " created.");
    }
    private GameObject ChooseRandomShape()
    {
        return tetrisPieces[Random.Range(0, tetrisPieces.Count)];
    }

    private Color ChooseRandomColor()
    {
        //return pieceColors[Random.Range(0, pieceColors.Count)];
        return pieceColors[Random.Range(0, 2)];
    }
    void CheckAndClearConnectedRows()
    {
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        for (int y = 0; y < gridManager.rows; y++)
        {
            for (int x = 0; x < gridManager.columns; x++)
            {
                // If the cell is unvisited and full
                if (!visited.Contains(new Vector2Int(x, y)) && gridManager.grid[x, y] == 1)
                {
                    Color targetColor = gridManager.cellSRenderersComponents[x, y].color;

                    // Find linked cells of this color with Flood-Fill
                    List<Vector2Int> connectedCells = GetConnectedCellsByColor(x, y, targetColor);

                    if (IsRowConnectedForColor(connectedCells, targetColor, y))
                    {
                        ClearCells(connectedCells);
                    }

                    // Save visited cells
                    foreach (var cell in connectedCells)
                    {
                        visited.Add(cell);
                    }
                }
            }
        }
    }

    List<Vector2Int> GetConnectedCellsByColor(int startX, int startY, Color targetColor)
    {
        List<Vector2Int> connected = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        Vector2Int[] directions = {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

        queue.Enqueue(new Vector2Int(startX, startY));
        visited.Add(new Vector2Int(startX, startY));

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            connected.Add(current);

            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;

                if (neighbor.x >= 0 && neighbor.x < gridManager.columns &&
                    neighbor.y >= 0 && neighbor.y < gridManager.rows &&
                    !visited.Contains(neighbor) &&
                    gridManager.grid[neighbor.x, neighbor.y] == 1 &&
                    AreColorsSame(gridManager.cellSRenderersComponents[neighbor.x, neighbor.y].color, targetColor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return connected;
    }

    bool IsRowConnectedForColor(List<Vector2Int> connectedCells, Color targetColor, int row)
    {
        for (int x = 0; x < gridManager.columns; x++)
        {
            Vector2Int cell = new Vector2Int(x, row);
            if (!connectedCells.Contains(cell) ||
                !AreColorsSame(gridManager.cellSRenderersComponents[cell.x, cell.y].color, targetColor))
            {
                return false;
            }
        }
        return true;
    }

    void ClearCells(List<Vector2Int> cells)
    {
        StartCoroutine(ClearAnimation(cells));
    }

    IEnumerator ClearAnimation(List<Vector2Int> cells)
    {
        if (createNewPiece != null) {
            StopCoroutine(createNewPiece);
            createNewPiece = null;
        }
        gridManager.pause = true;
        speed = 0;
        foreach (var cell in cells)
        {
            gridManager.cellSRenderersComponents[cell.x, cell.y].color = Color.white;
        }
        yield return new WaitForSecondsRealtime(0.5f);

        // Grid'i temizle
        foreach (var cell in cells)
        {
            gridManager.grid[cell.x, cell.y] = 0;
            gridManager.cellSRenderersComponents[cell.x, cell.y].color = gridManager.backgroundColor;
        }
        StartCoroutine(ContinueGame());

        
    }
    IEnumerator ContinueGame()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        gridManager.pause = false;
        speed = 3;
        if (createNewPiece == null)
        {
            createNewPiece = StartCoroutine(SpawnShape());
        }
    }
    bool AreColorsSame(Color color1, Color color2)
    {
        return color1 == color2;
    }

}
