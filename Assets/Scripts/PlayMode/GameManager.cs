using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Move> moves = new List<Move>();


    public void RequestMove(Move newMove)
    {
        // Check if values of Move Object are valid
        ValidateMoveRequest(newMove);

        // Check if movement of pieceType is corresponding with its rules
        if (!CheckMoveIntegrity(newMove))
        {
            Debug.Log("Move is not permitted");
            newMove.movedPiece.ResetPhysicalPosition();
            return;
        }

        // Check if upcoming position is valid:
        // king didnt put himself in check
        
        ExecuteMoveOnBoard(newMove);
        Debug.Log("Move is permitted");
    }

    private void ExecuteMoveOnBoard(Move newMove)
    {
        // Delete beaten piece from destination Square
        if(newMove.destinationSquare.currentPiece != null) { 
            Destroy(newMove.destinationSquare.currentPiece.gameObject);
        }

        // Reset memory of old square
        newMove.originSquare.SetCurrentPiece(null);

        // Update memory of piece
        newMove.movedPiece.SetCurrentSquare(newMove.destinationSquare);

        // Update memory of new square
        newMove.destinationSquare.SetCurrentPiece(newMove.movedPiece);

        // Actual movement of the physical representation of the peace to its square
        newMove.movedPiece.SyncPiecePositionToCurrentSquare();
    }

    private bool CheckMoveIntegrity(Move newMove)
    {
        switch (newMove.movedPiece.pieceType) {
            case PieceType.rook:
                if (Rook.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;
            case PieceType.bishop:
                if (Bishop.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;
            case PieceType.queen:
                if (Queen.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;
            case PieceType.knight:
                if (Knight.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;


        }
        return false;
    }

    private void ValidateMoveRequest(Move newMove)
    {
        if (newMove.originSquare == null || newMove.destinationSquare == null || newMove.movedPiece == null)
        {
            return;
        }
        Debug.Log(newMove.movedPiece.name + " requests move: " + newMove.originSquare.file + "-" + newMove.originSquare.rank + "...to..." + newMove.destinationSquare.file + "-" + newMove.destinationSquare.rank);
    }

    private void RegisterMove(Move newMove)
    {
        moves.Add(newMove);
    }

}



public class Move
{
    public PlaySquare originSquare;
    public PlaySquare destinationSquare;

    public Piece movedPiece;

    public Move(PlaySquare oriSquare, PlaySquare destSquare, Piece movedPiece)
    {
        originSquare = oriSquare;
        destinationSquare = destSquare;
        this.movedPiece = movedPiece;
    }
}
