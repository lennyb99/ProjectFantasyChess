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

    public Sprite whiteSquareTexture;
    public Sprite blackSquareTexture;
   
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
            case "":
                return null;
            default:
                Debug.Log("couldnt find '" + pieceName + "'.");
                return null;
        }
    }

    public int GetPieceIdentifier(string pieceName)
    {
        switch (pieceName)
        {
            case "whitePawn":
                return 10;
            case "blackPawn":
                return 20;
            case "whiteBishop":
                return 12;
            case "blackBishop":
                return 22;
            case "whiteKnight":
                return 11;
            case "blackKnight":
                return 21;
            case "whiteRook":
                return 13;
            case "blackRook":
                return 23;
            case "whiteQueen":
                return 14;
            case "blackQueen":
                return 24;
            case "whiteKing":
                return 15;
            case "blackKing":
                return 25;
            default:
                return 0;
        }
    }

    public string GetPieceName(int pieceId)
    {
        switch (pieceId)
        {
            case 10:
                return "whitePawn";
            case 11:
                return "whiteKnight";
            case 12:
                return "whiteBishop";
            case 13:
                return "whiteRook";
            case 14:
                return "whiteQueen";
            case 15:
                return "whiteKing";

            case 20:
                return "blackPawn";
            case 21:
                return "blackKnight";
            case 22:
                return "blackBishop";
            case 23:
                return "blackRook";
            case 24:
                return "blackQueen";
            case 25:
                return "blackKing";
            default:
                return null;
        }
    }
}
