using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EditSquarePieceTypes
{
    whitePawn,
    whiteKnight,
    whiteBishop,
    whiteRook,
    whiteQueen,
    whiteKing,
    blackPawn,
    blackKnight,
    blackBishop,
    blackRook,
    blackQueen,
    blackKing,
    unselected
}
public class EditSquare : MonoBehaviour
{
    public bool isActive;
    public SelectionType selectedPieceType;
    public EditSquarePieceTypes currentPiece;

    public Sprite activeFieldSpriteBright;
    public Sprite activeFieldSpriteDark;
    public Sprite inactiveFieldSpriteDark;
    public Sprite inactiveFieldSpriteBright;

    public bool darkTheme;

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

    public int file;
    public int rank;

    public SpriteRenderer pieceOnSquareRenderer;

    public PieceTypeSelectionManager pieceTypeSelectionManager;

    public void Start()
    {
        pieceTypeSelectionManager = PieceTypeSelectionManager.Instance;
        currentPiece = EditSquarePieceTypes.unselected;
    }

    public void HandleClickedOn()
    {
        if (pieceTypeSelectionManager.selectedPieceType == SelectionType.editSquareWand)
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
            ChangeSquareOccupation(pieceTypeSelectionManager.selectedPieceType);
        }
    }

    public void ChangeSquareOccupation(SelectionType selectedPieceType)
    {
        this.selectedPieceType = selectedPieceType;
        switch (selectedPieceType)
        {
            case SelectionType.whitePawn:
                pieceOnSquareRenderer.sprite = whitePawn;
                currentPiece = EditSquarePieceTypes.whitePawn;
                break;
            case SelectionType.whiteKnight:
                pieceOnSquareRenderer.sprite = whiteKnight;
                currentPiece = EditSquarePieceTypes.whiteKnight;
                break;
            case SelectionType.whiteBishop:
                pieceOnSquareRenderer.sprite = whiteBishop;
                currentPiece = EditSquarePieceTypes.whiteBishop;
                break;
            case SelectionType.whiteRook:
                pieceOnSquareRenderer.sprite = whiteRook;
                currentPiece = EditSquarePieceTypes.whiteRook;
                break;
            case SelectionType.whiteQueen:
                pieceOnSquareRenderer.sprite = whiteQueen;
                currentPiece = EditSquarePieceTypes.whiteQueen;
                break;
            case SelectionType.whiteKing:
                pieceOnSquareRenderer.sprite = whiteKing;
                currentPiece = EditSquarePieceTypes.whiteKing;
                break;
            case SelectionType.blackPawn:
                pieceOnSquareRenderer.sprite = blackPawn;
                currentPiece = EditSquarePieceTypes.blackPawn;
                break;
            case SelectionType.blackKnight:
                pieceOnSquareRenderer.sprite = blackKnight;
                currentPiece = EditSquarePieceTypes.blackKnight;
                break;
            case SelectionType.blackBishop:
                pieceOnSquareRenderer.sprite = blackBishop;
                currentPiece = EditSquarePieceTypes.blackBishop;
                break;
            case SelectionType.blackRook:
                pieceOnSquareRenderer.sprite = blackRook;
                currentPiece = EditSquarePieceTypes.blackRook;
                break;
            case SelectionType.blackQueen:
                pieceOnSquareRenderer.sprite = blackQueen;
                currentPiece = EditSquarePieceTypes.blackQueen;
                break;
            case SelectionType.blackKing:
                pieceOnSquareRenderer.sprite = blackKing;
                currentPiece = EditSquarePieceTypes.blackKing;
                break;
            case SelectionType.trashcan:
                pieceOnSquareRenderer.sprite = null;
                currentPiece = EditSquarePieceTypes.unselected;
                break;
            default:
                break;
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
        UpdateColor();    
    }

    public void SetColorTheme(bool darkTheme)
    {
        this.darkTheme = darkTheme;
        UpdateColor();
    }

    public void UpdateColor()
    {
        SpriteRenderer ren = GetComponent<SpriteRenderer>();
        if (ren == null)
        {
            Debug.Log("Sprite Renderer error");
            return;
        }
        if (isActive)
        {
            if (darkTheme)
            {
                ren.sprite = activeFieldSpriteDark;
            }
            else
            {
                ren.sprite = activeFieldSpriteBright;
            }
                
        }
        else
        {
            if (darkTheme)
            {
                ren.sprite = inactiveFieldSpriteDark;
            }
            else
            {
                ren.sprite = inactiveFieldSpriteBright;
            }
            
        }
    }

    public string GetSquareOccupationAsString()
    {
        switch (currentPiece)
        {
            case EditSquarePieceTypes.whitePawn:
                return "whitePawn";
            case EditSquarePieceTypes.whiteKnight:
                return "whiteKnight";
            case EditSquarePieceTypes.whiteBishop:
                return "whiteBishop";
            case EditSquarePieceTypes.whiteRook:
                return "whiteRook";
            case EditSquarePieceTypes.whiteQueen:
                return "whiteQueen";
            case EditSquarePieceTypes.whiteKing:
                return "whiteKing";
            case EditSquarePieceTypes.blackPawn:
                return "blackPawn";
            case EditSquarePieceTypes.blackKnight:
                return "blackKnight";
            case EditSquarePieceTypes.blackBishop:
                return "blackBishop";
            case EditSquarePieceTypes.blackRook:
                return "blackRook";
            case EditSquarePieceTypes.blackQueen:
                return "blackQueen";
            case EditSquarePieceTypes.blackKing:
                return "blackKing";
            case EditSquarePieceTypes.unselected:
                return "";
            default:
                return "";
        }
    }
}
