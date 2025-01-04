using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class King
{
    public static bool CheckMoveIntegrity(Move newMove)
    {
        if (GetAllPossibleKingMovesFromSquare(newMove.movedPiece, newMove.originSquare).Contains(newMove.destinationSquare))
        {
            return true;
        }
        return false;
    }



    private static List<PlaySquare> GetAllPossibleKingMovesFromSquare(Piece movedPiece, PlaySquare currentSquare)
    {
        List<PlaySquare> possibleFields = new List<PlaySquare>();

        int file = currentSquare.file;
        int rank = currentSquare.rank;

        if(movedPiece.moveCount == 0) { 
            IsCastlingPossible(movedPiece, currentSquare, possibleFields);
        }

        



        // Top Mid
        if (currentSquare.topMid != null) { 
            if (currentSquare.topMid.GetCurrentPiece() == null) // Checks if square exists, then if piece is on it
            {
                possibleFields.Add(currentSquare.topMid);
            }
            else
            {
                if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.topMid.GetCurrentPiece()))
                {
                    possibleFields.Add(currentSquare.topMid);
                }
            }
        }

        // Top Right
        if (currentSquare.topRight != null) { 
            if(currentSquare.topRight.GetCurrentPiece() == null) // Checks if square exists, then if piece is on it
            {
                possibleFields.Add(currentSquare.topRight);
            }
            else
            {
                if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.topRight.GetCurrentPiece()))
                {
                    possibleFields.Add(currentSquare.topRight);
                }
            }
        }

        // Mid Right
        if (currentSquare.midRight != null) { 
            if (currentSquare.midRight.GetCurrentPiece() == null) // Checks if square exists, then if piece is on it
            {
                possibleFields.Add(currentSquare.midRight);
            }
            else
            {
                if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.midRight.GetCurrentPiece()))
                {
                    possibleFields.Add(currentSquare.midRight);
                }
            }
        }

        // Bottom Right
        if (currentSquare.bottomRight != null) { 
            if (currentSquare.bottomRight.GetCurrentPiece() == null) // Checks if square exists, then if piece is on it
            {
                possibleFields.Add(currentSquare.bottomRight);
            }
            else
            {
                if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.bottomRight.GetCurrentPiece()))
                {
                    possibleFields.Add(currentSquare.bottomRight);
                }
            }
        }

        // Bottom Mid
        if (currentSquare.bottomMid != null) {
            if (currentSquare.bottomMid.GetCurrentPiece() == null) // Checks if square exists, then if piece is on it
            {
                possibleFields.Add(currentSquare.bottomMid);
            }
            else
            {
                if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.bottomMid.GetCurrentPiece()))
                {
                    possibleFields.Add(currentSquare.bottomMid);
                }
            }
        }

        // Bottom left
        if (currentSquare.bottomLeft != null) {
            if (currentSquare.bottomLeft.GetCurrentPiece() == null) // Checks if square exists, then if piece is on it
            {
                possibleFields.Add(currentSquare.bottomLeft);
            }
            else
            {
                if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.bottomLeft.GetCurrentPiece()))
                {
                    possibleFields.Add(currentSquare.bottomLeft);
                }
            }
        }

        // Mid Left
        if (currentSquare.midLeft != null) { 
            if (currentSquare.midLeft.GetCurrentPiece() == null) // Checks if square exists, then if piece is on it
            {
                possibleFields.Add(currentSquare.midLeft);
            }
            else
            {
                if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.midLeft.GetCurrentPiece()))
                {
                    possibleFields.Add(currentSquare.midLeft);
                }
            }
        }

        // Top Left
        if (currentSquare.topLeft != null) { 
            if (currentSquare.topLeft.GetCurrentPiece() == null) // Checks if square exists, then if piece is on it
            {
                possibleFields.Add(currentSquare.topLeft);
            }
            else
            {
                if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.topLeft.GetCurrentPiece()))
                {
                    possibleFields.Add(currentSquare.topLeft);
                }
            }
        }
        return possibleFields;
    }

    public static bool IsKingInCheck(Piece king)
    {
        List<Piece> piecesThatLookAtKing = GetPiecesThatLookAtSquare(king.currentSquare);
        
        for (int i = piecesThatLookAtKing.Count - 1; i >= 0; i--)
        {
            // filter pieces out, that have same color as the king
            if (piecesThatLookAtKing[i].isWhite == king.isWhite)
            {
                piecesThatLookAtKing.RemoveAt(i);
                continue;
            }

            // filter pawns out that look in the wrong direction
            // this has to be done, because the previous method only checks if its a diagonal movement with a distance of 1
            if (piecesThatLookAtKing[i].pieceType == PieceType.pawn)
            {
                if (!IsEnemyPawnLookingAtKing(piecesThatLookAtKing[i], king))
                {
                    piecesThatLookAtKing.RemoveAt(i);
                }
            }
        }

        if(piecesThatLookAtKing.Count > 0)
        {
            Debug.Log("King is in check by " + piecesThatLookAtKing.Count + " pieces.");
            return true;
        }
        else {
            Debug.Log("King is not in check");
            return false;
        }

    }

    private static bool IsEnemyPawnLookingAtKing(Piece pawn, Piece king)
    {
        if(king.isWhite)
        {
            if (king.currentSquare.topRight == pawn.currentSquare ||
                king.currentSquare.topLeft == pawn.currentSquare)
            {
                return true;
            }
        }
        else
        {
            if (king.currentSquare.bottomRight == pawn.currentSquare ||
                king.currentSquare.bottomLeft == pawn.currentSquare)
            {
                return true;
            }
        }
        return false;
    }

    private static List<Piece> GetPiecesThatLookAtSquare(PlaySquare square)
    {
        List<Piece> piecesThatLookAtSquare = new List<Piece>();

        List<(Func<PlaySquare, PlaySquare>, (Piece, int))> directions = new List<(Func<PlaySquare, PlaySquare>, (Piece, int))>
        {
            (field => field.topRight, (null, 0)),
            (field => field.bottomRight,(null, 0)),
            (field => field.bottomLeft,(null, 0)),
            (field => field.topLeft,(null, 0)),
            (field => field.topMid,(null, 0)),
            (field => field.bottomMid,(null, 0)),
            (field => field.midLeft,(null, 0)),
            (field => field.midRight,(null, 0)),
        };

        for (int i = 0; i < directions.Count; i++)
        {
            // Update the direction with the piece that is looking at the square
            var (key, value) = directions[i];
            (Piece,int) newValue = GetPieceThatReachesFromThatDirection(square, directions[i].Item1);
            directions[i] = (key, newValue);
        }

        // Check for diagonal moving pieces, eg. queen, bishop, pawns & kings
        for (int i = 0; i < 4;i++)
        {
            if (directions[i].Item2.Item1 == null) // If no piece is looking at the square
            {
                continue;
            }

            Piece lookingPiece = directions[i].Item2.Item1;
            if (lookingPiece.pieceType == PieceType.bishop || 
                 lookingPiece.pieceType == PieceType.queen ||
                  lookingPiece.pieceType == PieceType.pawn && directions[i].Item2.Item2 == 1 ||
                   lookingPiece.pieceType == PieceType.king && directions[i].Item2.Item2 == 1)
            {
                piecesThatLookAtSquare.Add(lookingPiece);
            }
        }

        // Check for diagonal moving pieces, eg. queen, rooks & kings
        for (int i = 4; i < 8; i++)
        {
            if (directions[i].Item2.Item1 == null) // If no piece is looking at the square
            {
                continue;
            }

            Piece lookingPiece = directions[i].Item2.Item1;
            if (lookingPiece.pieceType == PieceType.rook ||
                 lookingPiece.pieceType == PieceType.queen ||
                  lookingPiece.pieceType == PieceType.pawn && directions[i].Item2.Item2 == 1 ||
                   lookingPiece.pieceType == PieceType.king && directions[i].Item2.Item2 == 1)
            {
                piecesThatLookAtSquare.Add(lookingPiece);
            }
        }

        // Check for Knights
        piecesThatLookAtSquare.AddRange(GetKnightsThatLookAtSquareFromDirection(square));

        return piecesThatLookAtSquare;
    }

    private static (Piece, int) GetPieceThatReachesFromThatDirection(PlaySquare square, Func<PlaySquare, PlaySquare> getNextField)
    {
        PlaySquare tempCheckSquare = square; // Resets position to current position
        int distance = 0;

        while (getNextField(tempCheckSquare) != null) // Checks whether an end of a board is reached
        {
            distance++;
            PlaySquare nextField = getNextField(tempCheckSquare);

            if (nextField.GetCurrentPiece() == null) // empty square: look further
            {
                tempCheckSquare = nextField;
                continue;
            }
            else // square has a piece on it: return piece
            {
                return (nextField.GetCurrentPiece(), distance);
            }
        }
        return (null, distance);
    }

    private static List<Piece> GetKnightsThatLookAtSquareFromDirection(PlaySquare currentSquare)
    {
        List<Piece> knights = new List<Piece>();

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
                if (square.GetCurrentPiece() != null)
                {
                    if(square.GetCurrentPiece().pieceType == PieceType.knight)
                    {
                        knights.Add(square.GetCurrentPiece());
                    }
                }
            }
        }

        return knights;
    }

    private static void IsCastlingPossible(Piece movedPiece, PlaySquare currentSquare, List<PlaySquare> possibleFields)
    {
        PlaySquare tempSquare = currentSquare;
        while (tempSquare.midRight != null)
        {
            if (tempSquare.midRight.GetCurrentPiece() == null) // Clear way
            {
                tempSquare = tempSquare.midRight;
                continue;
            }
            else if (tempSquare.midRight.GetCurrentPiece().pieceType == PieceType.rook && // Rook available for castling
                     tempSquare.midRight.GetCurrentPiece().isWhite == movedPiece.isWhite &&
                     tempSquare.midRight.GetCurrentPiece().moveCount == 0)
            {
                possibleFields.Add(tempSquare);
                break;
            }
            else
            {
                break;
            }
        }

        tempSquare = currentSquare;
        while (tempSquare.midLeft != null)
        {
            if (tempSquare.midLeft.GetCurrentPiece() == null) // Clear way
            {
                tempSquare = tempSquare.midLeft;
            }
            else if (tempSquare.midLeft.GetCurrentPiece().pieceType == PieceType.rook && // Rook available for castling
                     tempSquare.midLeft.GetCurrentPiece().isWhite == movedPiece.isWhite &&
                     tempSquare.midLeft.GetCurrentPiece().moveCount == 0)
            {
                possibleFields.Add(tempSquare);
                break;
            }
            else
            {
                break;
            }
        }
    }

    

}
