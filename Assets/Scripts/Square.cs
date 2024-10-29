using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    private bool isActive;

    public Sprite activeFieldSpriteBright;
    public Sprite activeFieldSpriteDark;
    public Sprite inactiveFieldSpriteDark;
    public Sprite inactiveFieldSpriteBright;

    public Sprite whitePawn;
    public Sprite whiteKnight;
    public Sprite whiteBishop;
    public Sprite whiteRook;
    public Sprite whiteQueen;
    public Sprite whiteKing;

    public Sprite blackPawn;
    public Sprite blackKnight;
    public Sprite blackBishop;
    public Sprite blackRook;
    public Sprite blackQueen;
    public Sprite blackKing;
    

    public SpriteRenderer pieceOnSquareRenderer;

    public PieceTypeSelectionManager pieceTypeSelectionManager;

    public void Start()
    {
        if(pieceOnSquareRenderer == null || pieceTypeSelectionManager == null)
        {
            //Debug.Log("square not correctly initialized");
        }
    }

    public void HandleClickedOn()
    {
        if (pieceTypeSelectionManager.activePieceTypeSelect == "unselected")
        { 
            if (isActive)
            {
                SetActive(false);
            }
            else
            {
                SetActive(true);
            }
        }
        else
        {
            ChangeSquareOccupation(pieceTypeSelectionManager.activePieceTypeSelect);
        }
    }

    public void ChangeSquareOccupation(string pieceType)
    {
        switch(pieceType)
        {
            case "whitePawn":
                pieceOnSquareRenderer.sprite = whitePawn;
                break;
            case "whiteKnight":
                pieceOnSquareRenderer.sprite = whiteKnight;
                break;
            case "whiteBishop":
                pieceOnSquareRenderer.sprite = whiteBishop;
                break;
            case "whiteRook":
                pieceOnSquareRenderer.sprite = whiteRook;
                break;
            case "whiteQueen":
                pieceOnSquareRenderer.sprite = whiteQueen;
                break;
            case "whiteKing":
                pieceOnSquareRenderer.sprite = whiteKing;
                break;
            case "blackPawn":
                pieceOnSquareRenderer.sprite = blackPawn;
                break;
            case "blackKnight":
                pieceOnSquareRenderer.sprite = blackKnight;
                break;
            case "blackBishop":
                pieceOnSquareRenderer.sprite = blackBishop;
                break;
            case "blackRook":
                pieceOnSquareRenderer.sprite = blackRook;
                break;
            case "blackQueen":
                pieceOnSquareRenderer.sprite = blackQueen;
                break;
            case "blackKing":
                pieceOnSquareRenderer.sprite = blackKing;
                break;
            default:
                break;
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;


        SpriteRenderer ren = GetComponent<SpriteRenderer>();
        if (ren == null)
        {
            Debug.Log("Sprite Renderer error");
            return;
        }
        if (isActive)
        {
            ren.sprite = activeFieldSpriteDark;
        }
        else
        {
            ren.sprite = inactiveFieldSpriteDark;
        }
    }
}
