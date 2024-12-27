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
            currentSquare.topMid.currentPiece == null && //checks if squares in front of it are empty
            currentSquare.topMid.topMid.currentPiece == null)
        {
            possibleSquares.Add(currentSquare.topMid.topMid);
        }

        // one square forward
        if (currentSquare.topMid != null && 
            currentSquare.topMid.currentPiece == null)
        {
            possibleSquares.Add(currentSquare.topMid);
        }

        // diagonal left
        if (currentSquare.topLeft != null &&
            currentSquare.topLeft.currentPiece != null)
        {
            if(PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.topLeft.currentPiece))
            {
                possibleSquares.Add(currentSquare.topLeft);
            }   
        }

        // diagonal right
        if (currentSquare.topRight != null &&
            currentSquare.topRight.currentPiece != null)
        {
            if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.topRight.currentPiece))
            {
                possibleSquares.Add(currentSquare.topRight);
            }
        }

        // en passant
        // will be checked by looking at the piece on the square next to it.
        // If its a pawn of the opposite color and it made a two step movement on the last move, en passant is possible.
        if (currentSquare.midLeft != null &&
            currentSquare.midLeft.currentPiece != null &&
            currentSquare.midLeft.currentPiece.pieceType == PieceType.pawn && 
            !currentSquare.midLeft.currentPiece.isWhite && 
            currentSquare.midLeft.currentPiece == GameBoardData.GetLastMove().movedPiece &&
            GameBoardData.GetLastMove().originSquare.rank - GameBoardData.GetLastMove().destinationSquare.rank == 2)
        {
            possibleSquares.Add(currentSquare.topLeft);
        }

        if (currentSquare.midRight != null &&
            currentSquare.midRight.currentPiece != null &&
            currentSquare.midRight.currentPiece.pieceType == PieceType.pawn &&
            !currentSquare.midRight.currentPiece.isWhite &&
            currentSquare.midRight.currentPiece == GameBoardData.GetLastMove().movedPiece &&
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
            currentSquare.bottomMid.currentPiece == null && //checks if squares in front of it are empty
            currentSquare.bottomMid.bottomMid.currentPiece == null)
        {
            possibleSquares.Add(currentSquare.bottomMid.bottomMid);
        }

        // one square forward
        if (currentSquare.bottomMid != null &&
            currentSquare.bottomMid.currentPiece == null)
        {
            possibleSquares.Add(currentSquare.bottomMid);
        }

        // diagonal left
        if (currentSquare.bottomLeft != null &&
            currentSquare.bottomLeft.currentPiece != null)
        {
            if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.bottomLeft.currentPiece))
            {
                possibleSquares.Add(currentSquare.bottomLeft);
            }
        }

        // diagonal right
        if (currentSquare.bottomRight != null &&
            currentSquare.bottomRight.currentPiece != null)
        {
            if (PieceMovement.IsFieldWithTargetPieceTakeable(movedPiece, currentSquare.bottomRight.currentPiece))
            {
                possibleSquares.Add(currentSquare.bottomRight);
            }
        }

        // en passant
        // will be checked by looking at the piece on the square next to it.
        // If its a pawn of the opposite color and it made a two step movement on the last move, en passant is possible.
        if (currentSquare.midLeft != null &&
            currentSquare.midLeft.currentPiece != null &&
            currentSquare.midLeft.currentPiece.pieceType == PieceType.pawn &&
            currentSquare.midLeft.currentPiece.isWhite &&
            currentSquare.midLeft.currentPiece == GameBoardData.GetLastMove().movedPiece &&
            GameBoardData.GetLastMove().originSquare.rank - GameBoardData.GetLastMove().destinationSquare.rank == -2)
        {
            possibleSquares.Add(currentSquare.bottomLeft);
        }

        if (currentSquare.midRight != null &&
            currentSquare.midRight.currentPiece != null &&
            currentSquare.midRight.currentPiece.pieceType == PieceType.pawn &&
            currentSquare.midRight.currentPiece.isWhite &&
            currentSquare.midRight.currentPiece == GameBoardData.GetLastMove().movedPiece &&
            GameBoardData.GetLastMove().originSquare.rank - GameBoardData.GetLastMove().destinationSquare.rank == -2)
        {
            possibleSquares.Add(currentSquare.bottomRight);
        }

        return possibleSquares;
    }

    

}
