using System;
using System.Collections;
using System.Collections.Generic;
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
        ValidateMoveRequest(newMove);

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



        RegisterMove(newMove);
        Debug.Log("Move is permitted");
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


        }
        return false;
    }

    private void HandlePieceDeletion(Move newMove)
    {
        // Delete beaten piece from destination Square
        if (newMove.destinationSquare.currentPiece != null)
        {
            Destroy(newMove.destinationSquare.currentPiece.gameObject);
        }

        // Detect En passant and delete target Piece
        if (newMove.movedPiece.pieceType == PieceType.pawn && 
            newMove.originSquare.file != newMove.destinationSquare.file &&
            newMove.destinationSquare.currentPiece == null)
        {
            if (newMove.movedPiece.isWhite)
            {
                Destroy(newMove.destinationSquare.bottomMid.currentPiece.gameObject);
            }
            else
            {
                Destroy(newMove.destinationSquare.topMid.currentPiece.gameObject);
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
        promotingPawn.currentSquare.currentPiece = newPiece;
        Destroy(promotingPawn.gameObject);
        promotingPawn = null;
        promotionPieceName = "";
        canvas.ClosePromotionPanel();
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
        GameBoardData.moves.Add(newMove);
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
