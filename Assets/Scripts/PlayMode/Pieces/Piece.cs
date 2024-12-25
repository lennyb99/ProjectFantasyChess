using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType{
    king,
    queen,
    rook,
    bishop,
    knight,
    pawn
}

public class Piece : MonoBehaviour
{
    public bool isWhite;
    public Transform physicalPosition;
    //public int file;
    //public int rank;
    public PlaySquare currentSquare;

    public PieceType pieceType;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentSquare(PlaySquare playSquare)
    {
        currentSquare = playSquare;
    }

    public PlaySquare GetCurrentSquare() { return currentSquare; }

    public void SyncPiecePositionToCurrentSquare()
    {
        if (currentSquare != null)
        {
            AssignPieceToSquarePosition(currentSquare.gameObject.transform);
        }
    }

    public void AssignPieceToSquarePosition(Transform newTransform)
    {
        transform.position = new Vector3(newTransform.position.x, newTransform.position.y, newTransform.position.z);
        physicalPosition = newTransform;
    }

    
    public void ResetPhysicalPosition()
    {
        transform.position = new Vector3(physicalPosition.position.x, physicalPosition.position.y, physicalPosition.position.z);
    }

}
