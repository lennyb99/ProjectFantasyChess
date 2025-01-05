using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PieceHolder pieceHolder;
    public CanvasManager canvas;

    private Piece promotingPawn;
    public string promotionPieceName = "";
    public event Action OnPromotionPieceNameChanged;

    public void RequestMove(Move newMove)
    {
        // Check if values of Move Object are valid
        if (!ValidateMoveRequest(newMove))
        {
            return;
        }

        if (!CheckForCorrectTurn(newMove))
        {
            newMove.movedPiece.ResetPhysicalPosition();
            return;
        }

        // Pre-check for checks. this checks if a move that is supposed to be made, would result in an opponent checking the king, therefore its illegal
        if (DoesMovePutOwnKingInCheck(newMove))
        {
            Debug.Log("Move is not permitted. Own king in check");
            newMove.movedPiece.ResetPhysicalPosition();
            return;
        }

        // Check if movement of pieceType is corresponding with its rules
        if (!CheckMoveIntegrity(newMove))
        {
            Debug.Log("Move is not permitted");
            newMove.movedPiece.ResetPhysicalPosition();
            return;
        }
        
        if (CheckForPromotion(newMove)) 
        {
            InitiatePromotionSequence(newMove);
        }
        else 
        { 
            ExecuteMoveOnBoard(newMove);
        }

        HandleCastling(newMove);

        RegisterMove(newMove);
        Debug.Log("Move is permitted");

        // Post-check for checks
        if (DoesMovePutEnemyKingInCheck(newMove))
        {
            // Handle Check 

            // Check for Checkmate
            if (IsCheckmate(newMove))
            {
                // Handle Checkmate
                Debug.Log("CHECKMATE DETECTED");
            }
        }

        Debug.Log("----");
        Debug.Log("----");
        Debug.Log("----");
    }


    private Piece FindKing(bool white)
    {
        foreach(GameObject piece in GameBoardData.pieces)
        {
            if(piece != null) { 
                if(piece.GetComponent<Piece>().pieceType == PieceType.king && piece.GetComponent<Piece>().isWhite == white)
                {
                    return piece.GetComponent<Piece>();
                }
            }
        }
        return null;
    }

    private bool DoesMovePutOwnKingInCheck(Move newMove)
    {
        bool checkFlag = false;

        // Store all relevant information
        Piece takenPiece = newMove.destinationSquare.GetCurrentPiece();


        // Update position of piece as if move was made
        newMove.originSquare.SetCurrentPiece(null);
        newMove.destinationSquare.SetCurrentPiece(newMove.movedPiece);

        newMove.movedPiece.SetCurrentSquare(newMove.destinationSquare);

        // Check if move got the OWN king into check
        if (King.IsKingInCheck(FindKing(newMove.movedPiece.isWhite)))
        {
            checkFlag = true;
        }

        // Reset position
        newMove.destinationSquare.SetCurrentPiece(takenPiece);
        newMove.originSquare.SetCurrentPiece(newMove.movedPiece);
        newMove.movedPiece.SetCurrentSquare(newMove.originSquare);

        return checkFlag;
    }

    private bool DoesMovePutEnemyKingInCheck(Move newMove)
    {
        if (King.IsKingInCheck(FindKing(!newMove.movedPiece.isWhite)))
        {
            return true;
        }
        return false;
    }

    private bool IsCheckmate(Move newMove)
    {
        Debug.Log("detection start");
        bool whiteKingInCheck = !newMove.movedPiece.isWhite; // to know for which king checkmate should be detected
        Debug.Log(GameBoardData.pieces.Count);
        foreach (GameObject pieceObj in GameBoardData.pieces)
        {
            if (pieceObj != null && pieceObj.GetComponent<Piece>().isWhite == whiteKingInCheck)
            {
                List<Move> possibleMoves = pieceObj.GetComponent<Piece>().GetAllPossibleMoves();

                foreach(Move move in possibleMoves)
                {
                    if (DoesMovePreventCheckmate(move))
                    {
                        Debug.Log("detection finished early");
                        Debug.Log(move.movedPiece.gameObject.name + move.destinationSquare.gameObject.name + move.originSquare.gameObject.name);
                        return false;
                    }
                }
            }
        }

        Debug.Log("detection finished");
        return true;
    }

    private bool DoesMovePreventCheckmate(Move simulatedMove)
    {
        return !DoesMovePutOwnKingInCheck(simulatedMove);
    }

    private void ExecuteMoveOnBoard(Move newMove)
    {
        HandlePieceDeletion(newMove);

        // Reset memory of old square
        newMove.originSquare.SetCurrentPiece(null);

        // Update memory of piece
        newMove.movedPiece.SetCurrentSquare(newMove.destinationSquare);
        newMove.movedPiece.moveCount++;

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
            case PieceType.pawn:
                if (Pawn.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;
            case PieceType.king:
                if (King.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;


        }
        return false;
    }

    private void HandlePieceDeletion(Move newMove)
    {
        // Delete beaten piece from destination Square
        if (newMove.destinationSquare.GetCurrentPiece() != null)
        {
            GameBoardData.pieces.Remove(newMove.destinationSquare.GetCurrentPiece().gameObject);
            Destroy(newMove.destinationSquare.GetCurrentPiece().gameObject);
        }

        // Detect En passant and delete target Piece
        if (newMove.movedPiece.pieceType == PieceType.pawn && 
            newMove.originSquare.file != newMove.destinationSquare.file &&
            newMove.destinationSquare.GetCurrentPiece() == null)
        {
            if (newMove.movedPiece.isWhite)
            {
                Destroy(newMove.destinationSquare.bottomMid.GetCurrentPiece().gameObject);
            }
            else
            {
                Destroy(newMove.destinationSquare.topMid.GetCurrentPiece().gameObject);
            }
        }
    }

    private bool CheckForPromotion(Move newMove)
    {
        if(newMove.movedPiece.pieceType == PieceType.pawn)
        {
            if(newMove.destinationSquare.rank == GameBoardData.whitePawnPromotionRank &&
                newMove.movedPiece.isWhite ||
                newMove.destinationSquare.rank == GameBoardData.blackPawnPromotionRank &&
                !newMove.movedPiece.isWhite)
            {
                this.promotingPawn = newMove.movedPiece;
                return true;
            }
        }
        return false;
    }

    private void InitiatePromotionSequence(Move newMove)
    {
        canvas.OpenPromotingPanel();

        OnPromotionPieceNameChanged = null;
        OnPromotionPieceNameChanged += () =>
        {
            PromotePawn(newMove);
        };
    }

    public void SetPromotionPieceName(string name)
    {
        promotionPieceName = name;
        OnPromotionPieceNameChanged?.Invoke();
    }

    public void PromotePawn(Move newMove)
    {
        if (newMove.movedPiece.isWhite)
        {
            promotionPieceName = "white" + promotionPieceName;
        }
        else
        {
            promotionPieceName = "black" + promotionPieceName;
        }

        ExecuteMoveOnBoard(newMove);

        GameObject piecePrefab = pieceHolder.GetPiece(promotionPieceName);
        if (piecePrefab == null)
        {
            return;
        }
        Piece newPiece = Instantiate(piecePrefab, promotingPawn.gameObject.transform.position, Quaternion.identity).GetComponent<Piece>();

        // swapping of the square with piece relationship
        newPiece.currentSquare = promotingPawn.currentSquare;
        promotingPawn.currentSquare.SetCurrentPiece(newPiece);
        Destroy(promotingPawn.gameObject);
        promotingPawn = null;
        promotionPieceName = "";
        canvas.ClosePromotionPanel();
    }


    private void HandleCastling(Move newMove)
    {
        if (King.castlingFlag)
        {
            Move rookMove = null;
            Debug.Log(newMove.movedPiece.currentSquare);
            if(newMove.movedPiece.currentSquare.midRight.GetCurrentPiece() != null &&
                newMove.movedPiece.currentSquare.midRight.GetCurrentPiece().pieceType == PieceType.rook)
            {
                rookMove = new Move(newMove.movedPiece.currentSquare.midRight,
                                    newMove.movedPiece.currentSquare.midLeft,
                                    newMove.movedPiece.currentSquare.midRight.GetCurrentPiece());
                
            }
            else if(newMove.movedPiece.currentSquare.midLeft.GetCurrentPiece() != null &&
                    newMove.movedPiece.currentSquare.midLeft.GetCurrentPiece().pieceType == PieceType.rook)
            {
                rookMove = new Move(newMove.movedPiece.currentSquare.midLeft,
                                    newMove.movedPiece.currentSquare.midRight,
                                    newMove.movedPiece.currentSquare.midLeft.GetCurrentPiece());
            }

            if(rookMove != null) { 
                ExecuteMoveOnBoard(rookMove);
            }
        }

        King.castlingFlag = false;
    }

    private bool ValidateMoveRequest(Move newMove)
    {
        if (newMove.originSquare == null || newMove.destinationSquare == null || newMove.movedPiece == null)
        {
            return false;
        }
        Debug.Log(newMove.movedPiece.name + " requests move: " + newMove.originSquare.file + "-" + newMove.originSquare.rank + "...to..." + newMove.destinationSquare.file + "-" + newMove.destinationSquare.rank);
        return true;
    }

    private bool CheckForCorrectTurn(Move newMove) { 
        if(newMove.movedPiece.isWhite == GameBoardData.whiteToMove)
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }

    private void RegisterMove(Move newMove)
    {
        GameBoardData.moves.Add(newMove);
        GameBoardData.whiteToMove = !GameBoardData.whiteToMove;
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

public class BoardPosition
{
    public List<Piece> pieces;
    public List<PlaySquare> squares;

    public BoardPosition(List<Piece> pieces, List<PlaySquare> squares)
    {
        this.pieces = pieces;
        this.squares = squares;
    }

    public bool isWhiteKingInCheck()
    {
        Piece whiteKing = pieces.FirstOrDefault(obj => obj.pieceType == PieceType.king && obj.isWhite);
        Debug.Log("no king detected");
        foreach (Piece piece in pieces)
        {
            if (!piece.isWhite)
            {

            }
        }

        return false;
    }

    public bool isBlackKingInCheck()
    {
        Debug.Log("no king detected");
        Piece blackKing = pieces.FirstOrDefault(obj => obj.pieceType == PieceType.king && !obj.isWhite);

        return false;
    }

}
