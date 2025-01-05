using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rook 
{
    public static bool CheckMoveIntegrity(Move newMove)
    {
        if (GetAllPossibleRookMovesFromSquare(newMove.movedPiece,newMove.originSquare).Contains(newMove.destinationSquare))
        {
            return true;
        }
        return false;
    }

    public static List<PlaySquare> GetAllPossibleRookMovesFromSquare(Piece movedPiece,PlaySquare currentSquare)
    {
        List<PlaySquare> possibleSquares = new List<PlaySquare>();
        PlaySquare tempCheckSquare = currentSquare; // This initially stores the position of the rook.

        List<Func<PlaySquare, PlaySquare>> directions = new List<Func<PlaySquare, PlaySquare>>
        {
            field => field.topMid,
            field => field.bottomMid,
            field => field.midLeft,
            field => field.midRight,
        };

        foreach (var direction in directions)
        {
            PieceMovement.GetAllFieldsTowardsDirection(movedPiece,direction, tempCheckSquare, possibleSquares);
            tempCheckSquare = currentSquare;
        }
        return possibleSquares;
    }
}
