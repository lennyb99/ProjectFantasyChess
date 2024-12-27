using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bishop
{
    public static bool CheckMoveIntegrity(Move newMove)
    {
        if (GetAllPossibleBishopMovesFromSquare(newMove.movedPiece, newMove.originSquare).Contains(newMove.destinationSquare))
        {
            return true;
        }
        return false;
    }

    private static List<PlaySquare> GetAllPossibleBishopMovesFromSquare(Piece movedPiece, PlaySquare currentSquare)
    {
        List<PlaySquare> possibleFields = new List<PlaySquare>();
        PlaySquare tempCheckField = currentSquare; // This initially stores the position of the rook.

        List<Func<PlaySquare, PlaySquare>> directions = new List<Func<PlaySquare, PlaySquare>>
        {
            field => field.topRight,
            field => field.bottomRight,
            field => field.bottomLeft,
            field => field.topLeft,
        };

        foreach (var direction in directions)
        {
            PieceMovement.GetAllFieldsTowardsDirection(movedPiece, direction, tempCheckField, possibleFields);
            tempCheckField = currentSquare;
        }
        return possibleFields;
    }
}
