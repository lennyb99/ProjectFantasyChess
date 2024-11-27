using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceHolder : MonoBehaviour
{
    public GameObject whitePawn;
    public GameObject blackPawn;

    public GameObject whiteBishop;
    public GameObject blackBishop;

    public GameObject whiteKnight;
    public GameObject blackKnight;

    public GameObject whiteRook;
    public GameObject blackRook;

    public GameObject whiteQueen;
    public GameObject blackQueen;

    public GameObject whiteKing;
    public GameObject blackKing;

   
    public GameObject GetPiece(string pieceName)
    {
        switch (pieceName)
        {
            case "whitePawn":
                return whitePawn;
            case "blackPawn":
                return blackPawn;
            case "whiteBishop":
                return whiteBishop;
            case "blackBishop":
                return blackBishop;
            case "whiteKnight":
                return whiteKnight;
            case "blackKnight":
                return blackKnight;
            case "whiteRook":
                return whiteRook;
            case "blackRook":
                return blackRook;
            case "whiteQueen":
                return whiteQueen;
            case "blackQueen":
                return blackQueen;
            case "whiteKing":
                return whiteKing;
            case "blackKing":
                return blackKing;
            default:
                Debug.Log("couldnt find '" + pieceName + "'.");
                return null;
        }
    }
}
