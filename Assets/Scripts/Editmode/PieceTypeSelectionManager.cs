using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionType
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
    editSquareWand,
    trashcan,
}

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

    public Button editSquareWandButton;
    public Button trashCanButton;

    private List<Button> selectPieceTypeButtons = new List<Button>();
    public SelectionType selectedPieceType;

    public static PieceTypeSelectionManager Instance {  get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RegisterButtons();
        selectedPieceType = SelectionType.editSquareWand;
    }

    void Update()
    {
        
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
        selectPieceTypeButtons.Add(trashCanButton);
        selectPieceTypeButtons.Add(editSquareWandButton);
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
        UnselectAllColors();
        if (selectedPieceType == SelectionType.whitePawn)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.whitePawn;
            AdjustColors(whitePawnButton, Color.green);
        }
    }

    public void SelectBlackPawn()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.blackPawn)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.blackPawn;
            AdjustColors(blackPawnButton, Color.green);
        }
    }

    public void SelectWhiteBishop()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.whiteBishop)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.whiteBishop;
            AdjustColors(whiteBishopButton, Color.green);
        }
    }

    public void SelectBlackBishop()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.blackBishop)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.blackBishop;
            AdjustColors(blackBishopButton, Color.green);
        }
    }

    public void SelectWhiteKnight()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.whiteKnight)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.whiteKnight;
            AdjustColors(whiteKnightButton, Color.green);
        }
    }

    public void SelectBlackKnight()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.blackKnight)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.blackKnight;
            AdjustColors(blackKnightButton, Color.green);
        }
    }

    public void SelectWhiteRook()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.whiteRook)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.whiteRook;
            AdjustColors(whiteRookButton, Color.green);
        }
    }

    public void SelectBlackRook()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.blackRook)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.blackRook;
            AdjustColors(blackRookButton, Color.green);
        }
    }

    public void SelectWhiteQueen()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.whiteQueen)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.whiteQueen;
            AdjustColors(whiteQueenButton, Color.green);
        }
    }

    public void SelectBlackQueen()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.blackQueen)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.blackQueen;
            AdjustColors(blackQueenButton, Color.green);
        }
    }

    public void SelectWhiteKing()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.whiteKing)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else {
            selectedPieceType = SelectionType.whiteKing;
            AdjustColors(whiteKingButton, Color.green);
        }
    }

    public void SelectBlackKing()
    {
        UnselectAllColors();
        if (selectedPieceType == SelectionType.blackKing)
        {
            selectedPieceType = SelectionType.editSquareWand;
        }
        else
        {
            selectedPieceType = SelectionType.blackKing;
            AdjustColors(blackKingButton, Color.green);
        }
    }

    public void SelectEditSquareWand()
    {
        UnselectAllColors();
        
            
        selectedPieceType = SelectionType.editSquareWand;

        AdjustColors(editSquareWandButton, Color.green);
    }

    public void SelectTrashCan()
    {
        UnselectAllColors();

        selectedPieceType = SelectionType.trashcan;

        AdjustColors(trashCanButton, Color.green);
    }

}
