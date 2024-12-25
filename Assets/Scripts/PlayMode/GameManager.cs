using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Move> moves = new List<Move>();


    public bool RequestMove(Move newMove)
    {
        Debug.Log(newMove.originSquare.name + newMove.destinationSquare.name + newMove.movedPiece.name);


        // Check if values of Move Object are valid
        if (newMove.originSquare == null || newMove.destinationSquare == null || newMove.movedPiece == null)
        {
            return false;
        }

        // Check if movement of pieceType is corresponding with its rules
        Rook.CheckMoveIntegrity(newMove);

        // Check if upcoming position is valid:
        // king didnt put himself in check

        
        return false;
    }

    public void RegisterMove(Move newMove)
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
