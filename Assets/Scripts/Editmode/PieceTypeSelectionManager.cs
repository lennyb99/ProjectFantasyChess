using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PieceTypeSelectionManager : MonoBehaviour
{
    public Button whitePawnButton;
    public Button blackPawnButton;
    public Button whiteKnightButton;
    public Button blackKnightButton;
    public Button whiteBishopButton;
    public Button blackBishopButton;
    public Button whiteRookButton;
    public Button blackRookButton;
    public Button whiteQueenButton;
    public Button blackQueenButton;
    public Button whiteKingButton;
    public Button blackKingButton;

    public Button UnselectAllButton;

    private List<Button> selectPieceTypeButtons = new List<Button>();
    public string activePieceTypeSelect;

    // Start is called before the first frame update
    void Start()
    {
        RegisterButtons();
    }
    private void RegisterButtons()
    {
        selectPieceTypeButtons.Add(whitePawnButton);
        selectPieceTypeButtons.Add(blackPawnButton);
        selectPieceTypeButtons.Add(whiteKnightButton);
        selectPieceTypeButtons.Add(blackKnightButton);
        selectPieceTypeButtons.Add(whiteBishopButton);
        selectPieceTypeButtons.Add(blackBishopButton);
        selectPieceTypeButtons.Add(whiteRookButton);
        selectPieceTypeButtons.Add(blackRookButton);
        selectPieceTypeButtons.Add(whiteQueenButton);
        selectPieceTypeButtons.Add(blackQueenButton);
        selectPieceTypeButtons.Add(whiteKingButton);
        selectPieceTypeButtons.Add(blackKingButton);
    }
   
    private void AdjustColors(Button selectButton, Color color)
    {
        ColorBlock colors = selectButton.colors;
        colors.normalColor = color;
        colors.highlightedColor = color;
        colors.selectedColor = color;
        selectButton.colors = colors;
    }

    public void UnselectAllColors()
    {
        foreach (Button selectButton in selectPieceTypeButtons)
        {
            AdjustColors(selectButton, Color.white);
        }
    }

    public void SelectWhitePawn()
    {
        
        activePieceTypeSelect = "whitePawn";
        UnselectAllColors();
        AdjustColors(whitePawnButton, Color.green);
    }

    public void SelectBlackPawn()
    {
        activePieceTypeSelect = "blackPawn";
        UnselectAllColors();
        AdjustColors(blackPawnButton, Color.green);
    }

    public void SelectWhiteBishop()
    {
        activePieceTypeSelect = "whiteBishop";
        UnselectAllColors();
        AdjustColors(whiteBishopButton, Color.green);
    }

    public void SelectBlackBishop()
    {
        activePieceTypeSelect = "blackBishop";
        UnselectAllColors();
        AdjustColors(blackBishopButton, Color.green);
    }

    public void SelectWhiteKnight()
    {
        activePieceTypeSelect = "whiteKnight";
        UnselectAllColors();
        AdjustColors(whiteKnightButton, Color.green);
    }

    public void SelectBlackKnight()
    {
        activePieceTypeSelect = "blackKnight";
        UnselectAllColors();
        AdjustColors(blackKnightButton, Color.green);
    }

    public void SelectWhiteRook()
    {
        activePieceTypeSelect = "whiteRook";
        UnselectAllColors();
        AdjustColors(whiteRookButton, Color.green);
    }

    public void SelectBlackRook()
    {
        activePieceTypeSelect = "blackRook";
        UnselectAllColors();
        AdjustColors(blackRookButton, Color.green);
    }

    public void SelectWhiteQueen()
    {
        activePieceTypeSelect = "whiteQueen";
        UnselectAllColors();
        AdjustColors(whiteQueenButton, Color.green);
    }

    public void SelectBlackQueen()
    {
        activePieceTypeSelect = "blackQueen";
        UnselectAllColors();
        AdjustColors(blackQueenButton, Color.green);
    }

    public void SelectWhiteKing()
    {
        activePieceTypeSelect = "whiteKing";
        UnselectAllColors();
        AdjustColors(whiteKingButton, Color.green);
    }

    public void SelectBlackKing()
    {
        activePieceTypeSelect = "blackKing";
        UnselectAllColors();
        AdjustColors(blackKingButton, Color.green);
    }

    public void UnselectAll()
    {
        activePieceTypeSelect = "unselected";
        UnselectAllColors();
    }
    
}
