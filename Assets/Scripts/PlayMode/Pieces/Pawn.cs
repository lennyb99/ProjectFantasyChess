using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pawn
{
    public static bool CheckMoveIntegrity(Move newMove)
    {
        if (newMove.movedPiece.isWhite){
            if (GetAllPossibleWhitePawnMovesFromSquare(newMove.movedPiece, newMove.originSquare).Contains(newMove.destinationSquare))
            {
                return true;
            }
        }else
        {
            if (GetAllPossibleBlackPawnMovesFromSquare(newMove.movedPiece, newMove.originSquare).Contains(newMove.destinationSquare))
            {
                return true;
            }
        }
        return false;
    }

    private static List<PlaySquare> GetAllPossibleWhitePawnMovesFromSquare(Piece movedPiece, PlaySquare currentSquare)
    {
        List<PlaySquare> possibleSquares = new List<PlaySquare>();
        int rank = currentSquare.rank;

        // two squares forward
        if (rank == GameBoardData.whitePawnBaseRank && //checks if pawn is allowed to push two ranks from its position
            currentSquare.topMid != null &&  //checks if squares infront of it exist
            currentSquare.topMid.topMid != null &&
            currentSquare.topMid.GetCurrentPiece() == null && //checks if squares in front of it are empty
            currentSquare.topMid.topMid.GetCurrentPiece() == null)
        {
            possibleSquares.Add(currentSquare.topMid.topMid);
        }

        // one square forward
        if (currentSquare.topMid != null && 
            currentSquare.topMid.GetCurrentPiece() == null)
        {
            possibleSquares.Add(currentSquare.topMid);
        }

        // diagonal left
        if (currentSquare.topLeft != null &&
            currentSquare.topLeft.GetCurrentPiece() != null)
        {
            if(PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.topLeft.GetCurrentPiece()))
            {
                possibleSquares.Add(currentSquare.topLeft);
            }   
        }

        // diagonal right
        if (currentSquare.topRight != null &&
            currentSquare.topRight.GetCurrentPiece() != null)
        {
            if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.topRight.GetCurrentPiece()))
            {
                possibleSquares.Add(currentSquare.topRight);
            }
        }

        // en passant
        // will be checked by looking at the piece on the square next to it.
        // If its a pawn of the opposite color and it made a two step movement on the last move, en passant is possible.
        if (currentSquare.midLeft != null &&
            currentSquare.midLeft.GetCurrentPiece() != null &&
            currentSquare.midLeft.GetCurrentPiece().pieceType == PieceType.pawn && 
            !currentSquare.midLeft.GetCurrentPiece().isWhite && 
            currentSquare.midLeft.GetCurrentPiece() == GameBoardData.GetLastMove().movedPiece &&
            GameBoardData.GetLastMove().originSquare.rank - GameBoardData.GetLastMove().destinationSquare.rank == 2)
        {
            possibleSquares.Add(currentSquare.topLeft);
        }

        if (currentSquare.midRight != null &&
            currentSquare.midRight.GetCurrentPiece() != null &&
            currentSquare.midRight.GetCurrentPiece().pieceType == PieceType.pawn &&
            !currentSquare.midRight.GetCurrentPiece().isWhite &&
            currentSquare.midRight.GetCurrentPiece() == GameBoardData.GetLastMove().movedPiece &&
            GameBoardData.GetLastMove().originSquare.rank - GameBoardData.GetLastMove().destinationSquare.rank == 2)
        {
            possibleSquares.Add(currentSquare.topRight);
        }

        return possibleSquares;
    }

    private static List<PlaySquare> GetAllPossibleBlackPawnMovesFromSquare(Piece movedPiece, PlaySquare currentSquare)
    {
        List<PlaySquare> possibleSquares = new List<PlaySquare>();
        int rank = currentSquare.rank;

        // two squares forward
        if (rank == GameBoardData.blackPawnBaseRank && //checks if pawn is allowed to push two ranks from its position
            currentSquare.bottomMid != null &&  //checks if squares infront of it exist
            currentSquare.bottomMid.bottomMid != null &&
            currentSquare.bottomMid.GetCurrentPiece() == null && //checks if squares in front of it are empty
            currentSquare.bottomMid.bottomMid.GetCurrentPiece() == null)
        {
            possibleSquares.Add(currentSquare.bottomMid.bottomMid);
        }

        // one square forward
        if (currentSquare.bottomMid != null &&
            currentSquare.bottomMid.GetCurrentPiece() == null)
        {
            possibleSquares.Add(currentSquare.bottomMid);
        }

        // diagonal left
        if (currentSquare.bottomLeft != null &&
            currentSquare.bottomLeft.GetCurrentPiece() != null)
        {
            if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.bottomLeft.GetCurrentPiece()))
            {
                possibleSquares.Add(currentSquare.bottomLeft);
            }
        }

        // diagonal right
        if (currentSquare.bottomRight != null &&
            currentSquare.bottomRight.GetCurrentPiece() != null)
        {
            if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.bottomRight.GetCurrentPiece()))
            {
                possibleSquares.Add(currentSquare.bottomRight);
            }
        }

        // en passant
        // will be checked by looking at the piece on the square next to it.
        // If its a pawn of the opposite color and it made a two step movement on the last move, en passant is possible.
        if (currentSquare.midLeft != null &&
            currentSquare.midLeft.GetCurrentPiece() != null &&
            currentSquare.midLeft.GetCurrentPiece().pieceType == PieceType.pawn &&
            currentSquare.midLeft.GetCurrentPiece().isWhite &&
            currentSquare.midLeft.GetCurrentPiece() == GameBoardData.GetLastMove().movedPiece &&
            GameBoardData.GetLastMove().originSquare.rank - GameBoardData.GetLastMove().destinationSquare.rank == -2)
        {
            possibleSquares.Add(currentSquare.bottomLeft);
        }

        if (currentSquare.midRight != null &&
            currentSquare.midRight.GetCurrentPiece() != null &&
            currentSquare.midRight.GetCurrentPiece().pieceType == PieceType.pawn &&
            currentSquare.midRight.GetCurrentPiece().isWhite &&
            currentSquare.midRight.GetCurrentPiece() == GameBoardData.GetLastMove().movedPiece &&
            GameBoardData.GetLastMove().originSquare.rank - GameBoardData.GetLastMove().destinationSquare.rank == -2)
        {
            possibleSquares.Add(currentSquare.bottomRight);
        }

        return possibleSquares;
    }

    

}
