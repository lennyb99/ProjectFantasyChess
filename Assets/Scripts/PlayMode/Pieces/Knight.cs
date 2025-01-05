using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public static bool CheckMoveIntegrity(Move newMove)
    {
        if (GetAllPossibleKnightMovesFromSquare(newMove.movedPiece, newMove.originSquare).Contains(newMove.destinationSquare))
        {
            return true;
        }
        return false;
    }



    public static List<PlaySquare> GetAllPossibleKnightMovesFromSquare(Piece movedPiece, PlaySquare currentSquare)
    {
        List<PlaySquare> possibleFields = new List<PlaySquare>();

        int file = currentSquare.file;
        int rank = currentSquare.rank;

        List<(int, int)> destinationSquareCoordinates = new List<(int, int)>
        {
            // 1 clock 
            (file + 1, rank + 2),
            // 2 clock
            (file + 2, rank + 1),
            // 4 clock
            (file + 2, rank - 1),
            // 5 clock
            (file + 1, rank - 2),
            // 7 clock
            (file - 1, rank - 2),
            // 8 clock
            (file - 2, rank - 1),
            // 10 clock
            (file - 2, rank + 1),
            // 11 clock
            (file - 1, rank + 2),
        };


        foreach (var pair in destinationSquareCoordinates)
        {
            if (GameBoardData.FindSquareByCoordinates(pair.Item1, pair.Item2) != null)
            {
                PlaySquare square = GameBoardData.FindSquareByCoordinates(pair.Item1, pair.Item2);
                if (square.GetCurrentPiece() == null)
                {
                    possibleFields.Add(square);
                }
                else
                {
                    if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, square.GetCurrentPiece()))
                    {
                        possibleFields.Add(square); // Add Square to the possible destination squares of the rook
                    }
                }
            }
        }

        return possibleFields;
    }
}
