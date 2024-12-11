using UnityEngine;

public class SandManager : MonoBehaviour
{
    private bool isHold;
    public float spriteSize = 0.1f;
    public GridManager gridManager;
    public int touchSize; //Touched area size

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isHold = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isHold = false;
        }

        if (isHold)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Vector3 hitPosition = hit.collider.gameObject.transform.position;

                // Convert world coordinates to grid index
                int xIndex = Mathf.FloorToInt((hitPosition.x - gridManager.gridStartPosition.x) / spriteSize);
                int yIndex = Mathf.FloorToInt((hitPosition.y - gridManager.gridStartPosition.y) / spriteSize);

                if (xIndex >= 0 && xIndex < gridManager.columns && yIndex >= 0 && yIndex < gridManager.rows)
                {
                    // Change grid's value
                    gridManager.grid[xIndex, yIndex] = 1;

                    for(int i = xIndex-touchSize; i < xIndex+touchSize; i++)
                    {
                        if(i>0 && i < gridManager.columns)
                        {
                            for (int j = yIndex - touchSize; j < yIndex + touchSize; j++)
                            {
                                if(j>0 && j < gridManager.rows)
                                {
                                    gridManager.grid[i, j] = 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Clicked outside the grid!");
                }
            }
        }
    }
}
