using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public GameManager gameManager;
    public CanvasManager canvasManager;

    [SerializeField]
    private GameObject selectedObject;
    private Vector3 offset;

    public bool allowPieceMovements;

    void Start()
    {
        allowPieceMovements = true;
    }

    void Update()
    {
        if (allowPieceMovements)
        {
            CheckForPieceMovements();
        }

        CheckForMenuButtons();
    }

    private void CheckForMenuButtons()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey(KeyCode.Tab))
        {
            canvasManager.ToggleMenuPanel();
        }
    }
    private void CheckForPieceMovements()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectLeftMouseClick();
        }

        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            DragObject();
        }

        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            DropObject();
        }
    }

    private void DetectLeftMouseClick()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Piece"))
            {
                selectedObject = hit.collider.gameObject;
                return;
            }
        }

        
    }

    private void DragObject()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        selectedObject.transform.position = mousePosition;
    }

    private void DropObject()
    {
        //Debug.Log(selectedObject.gameObject.name + " dropping..");
        if (selectedObject.GetComponent<Piece>() == null)
        {
            return;
        }
        RequestMove(selectedObject.GetComponent<Piece>().currentSquare, GetSquareFromDragDestination(), selectedObject);
        selectedObject = null;
    }

    private PlaySquare GetSquareFromDragDestination()
    {
        int layerMask = LayerMask.GetMask("SquareLayer");

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            GameObject obj = hit.collider.gameObject;

            if (obj.tag == "Square")
            {
                return obj.GetComponent<PlaySquare>();
            }
        }
        return null;
    }

    private void RequestMove(PlaySquare oriSquareObj, PlaySquare destSquareObj, GameObject pieceObj)
    {
        if (pieceObj.GetComponent<Piece>() != null) {
            gameManager.RequestMove(new Move(oriSquareObj, destSquareObj, pieceObj.GetComponent<Piece>()));
        }
    }
}
