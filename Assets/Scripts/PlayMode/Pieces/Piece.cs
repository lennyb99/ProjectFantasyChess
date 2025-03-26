using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType{
    king,
    queen,
    rook,
    bishop,
    knight,
    pawn
}

public class Piece : MonoBehaviour
{
    public bool isWhite;
    public Transform physicalPosition;
    public PlaySquare currentSquare;
    public int moveCount; // Stores information on how many times this piece was moved in the game

    public PieceType pieceType;

    public List<PlaySquare> guardedSquares;
    
    // Start is called before the first frame update
    void Start()
    {
        moveCount = 0;
        guardedSquares = new List<PlaySquare>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDestroy()
    {
        GameBoardData.pieces.Remove(this.gameObject);
        foreach (PlaySquare square in guardedSquares)
        {
            square.UnsubscribeAsGuardingPiece(this);
        }
    }

    public void SetCurrentSquare(PlaySquare playSquare)
    {
        currentSquare = playSquare;
        SyncPiecePositionToCurrentSquare();
    }

    public PlaySquare GetCurrentSquare() { return currentSquare; }

    public void SyncPiecePositionToCurrentSquare()
    {
        if (currentSquare != null)
        {
            AssignPieceToSquarePosition(currentSquare.gameObject.transform);
        }
    }

    public void AssignPieceToSquarePosition(Transform newTransform)
    {
        transform.position = new Vector3(newTransform.position.x, newTransform.position.y, newTransform.position.z);
        physicalPosition = newTransform;
    }

    public bool CheckingEnemyKing(Piece enemyKing)
    {
        switch (pieceType)
        {
            case PieceType.king:

                break;
            case PieceType.queen:

                break;
            case PieceType.knight:

                break;
            case PieceType.rook:

                break;
            case PieceType.bishop:

                break;
            case PieceType.pawn:

                break;

        }
        return false;
    }

    
    public void ResetPhysicalPosition()
    {
        transform.position = new Vector3(physicalPosition.position.x, physicalPosition.position.y, physicalPosition.position.z);
    }

    public int GetPieceIdentifier()
    {
        int id = 0;
        switch (pieceType)
        {
            case PieceType.king:
                id = id + 10;
                break;
            case PieceType.queen:
                id = id + 20;
                break;
            case PieceType.knight:
                id = id + 30;
                break;
            case PieceType.rook:
                id = id + 40;
                break;
            case PieceType.bishop:
                id = id + 50;
                break;
            case PieceType.pawn:
                id = id + 60;
                break;
        }

        if(isWhite)
        {
            id = id + 1;
        }

        return id;
    }

    public List<Move> GetAllPossibleMoves()
    {
        List<Move> possibleMoves = new List<Move>();

        List<PlaySquare> destinationSquares = new List<PlaySquare>();
        switch (pieceType)
        {
            case PieceType.king:
                destinationSquares.AddRange(King.GetAllPossibleKingMovesFromSquare(this, currentSquare, false));
                break;
            case PieceType.queen:
                destinationSquares.AddRange(Queen.GetAllPossibleQueenMovesFromSquare(this, currentSquare));
                break;
            case PieceType.knight:
                destinationSquares.AddRange(Knight.GetAllPossibleKnightMovesFromSquare(this, currentSquare));
                break;
            case PieceType.rook:
                destinationSquares.AddRange(Rook.GetAllPossibleRookMovesFromSquare(this, currentSquare));
                break;
            case PieceType.bishop:
                destinationSquares.AddRange(Bishop.GetAllPossibleBishopMovesFromSquare(this, currentSquare));
                break;
            case PieceType.pawn:
                if (isWhite) { 
                    destinationSquares.AddRange(Pawn.GetAllPossibleWhitePawnMovesFromSquare(this, currentSquare));
                }
                else
                {
                    destinationSquares.AddRange(Pawn.GetAllPossibleBlackPawnMovesFromSquare(this, currentSquare));
                }
                break;
        }

        foreach (PlaySquare destSquare in destinationSquares)
        {
            possibleMoves.Add(new Move(currentSquare, destSquare, this));
        }

        return possibleMoves;
    }

    private void GetAllPossibleMovesFromSinglePiece(Func<List<PlaySquare>> staticMethod)
    {
        staticMethod();
    }

    public void CalibrateGuardedSquares()
    {
        guardedSquares.Clear();

        // Let guarded squares be calculated
        switch (pieceType)
        {
            case PieceType.king:

                break;
            case PieceType.queen:

                break;
            case PieceType.knight:

                break;
            case PieceType.rook:

                break;
            case PieceType.bishop:

                break;
            case PieceType.pawn:

                break;
        }
            


        // notify squares
        SubscribeToGuardedSquares();
    }

    private void SubscribeToGuardedSquares()
    {
        foreach(PlaySquare square in guardedSquares)
        {
            square.SubscribeAsGuardingPiece(this);
        }
    }

    
}
