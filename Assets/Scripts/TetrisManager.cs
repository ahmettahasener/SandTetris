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

    private void Start()
    {
        SpawnShape();
    }

    void Update()
    {
        MovePiece();
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
        Invoke("SpawnShape", 1);
    }

    private void SpawnShape()
    {
        hasActivePiece = true;
        int xPos = Random.Range(7, -7);
        Vector3 pos = new Vector3(xPos, 6, 0);
        activePiece = Instantiate(ChooseRandomShape(), pos, Quaternion.identity);
        Debug.Log(activePiece + " created.");
    }
    private GameObject ChooseRandomShape()
    {
        return tetrisPieces[Random.Range(0, tetrisPieces.Count)];
    }
}
