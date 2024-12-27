using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySquare : MonoBehaviour
{
    public int file;
    public int rank;

    [Header("Adjacent Fields")]
    public PlaySquare topLeft;
    public PlaySquare topMid;
    public PlaySquare topRight;
    public PlaySquare midLeft;
    public PlaySquare midRight;
    public PlaySquare bottomLeft;
    public PlaySquare bottomMid;
    public PlaySquare bottomRight;


    public BoardBuilder board;

    public Piece currentPiece;

    public void SetCurrentPiece(Piece piece)
    {
        currentPiece = piece;
    }

    public void AssignAdjacentSquares()
    {
        topLeft = board.FindSquareByCoordinates(file-1, rank+1);
        topMid = board.FindSquareByCoordinates(file, rank+1);
        topRight = board.FindSquareByCoordinates(file+1, rank + 1);
        midLeft = board.FindSquareByCoordinates(file-1, rank);
        midRight = board.FindSquareByCoordinates(file+1, rank);
        bottomLeft = board.FindSquareByCoordinates(file - 1, rank-1);
        bottomMid = board.FindSquareByCoordinates(file, rank-1);
        bottomRight = board.FindSquareByCoordinates(file+1, rank-1);
    }
}
