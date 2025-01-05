using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour
{
    public static bool CheckMoveIntegrity(Move newMove)
    {
        if (GetAllPossibleQueenMovesFromSquare(newMove.movedPiece, newMove.originSquare).Contains(newMove.destinationSquare))
        {
            return true;
        }
        return false;
    }

    public static List<PlaySquare> GetAllPossibleQueenMovesFromSquare(Piece movedPiece, PlaySquare currentSquare)
    {
        List<PlaySquare> possibleSquares = new List<PlaySquare>();
        PlaySquare tempCheckSquare = currentSquare; // This initially stores the position of the rook.

        List<Func<PlaySquare, PlaySquare>> directions = new List<Func<PlaySquare, PlaySquare>>
        {
            field => field.topRight,
            field => field.bottomRight,
            field => field.bottomLeft,
            field => field.topLeft,
            field => field.topMid,
            field => field.bottomMid,
            field => field.midLeft,
            field => field.midRight,
        };

        foreach (var direction in directions)
        {
            PieceMovement.GetAllFieldsTowardsDirection(movedPiece, direction, tempCheckSquare, possibleSquares);
            tempCheckSquare = currentSquare;
        }
        return possibleSquares;
    }
}
