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
        activePieceTypeSelect = "unselected";
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
        if (activePieceTypeSelect == "whitePawn")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "whitePawn";
            AdjustColors(whitePawnButton, Color.green);
        }
    }

    public void SelectBlackPawn()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == "blackPawn")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "blackPawn";
            AdjustColors(blackPawnButton, Color.green);
        }
    }

    public void SelectWhiteBishop()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == "whiteBishop")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "whiteBishop";
            AdjustColors(whiteBishopButton, Color.green);
        }
    }

    public void SelectBlackBishop()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == "blackBishop")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "blackBishop";
            AdjustColors(blackBishopButton, Color.green);
        }
    }

    public void SelectWhiteKnight()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == "whiteKnight")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "whiteKnight";
            AdjustColors(whiteKnightButton, Color.green);
        }
    }

    public void SelectBlackKnight()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == "blackKnight")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "blackKnight";
            AdjustColors(blackKnightButton, Color.green);
        }
    }

    public void SelectWhiteRook()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == "whiteRook")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "whiteRook";
            AdjustColors(whiteRookButton, Color.green);
        }
    }

    public void SelectBlackRook()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == "blackRook")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "blackRook";
            AdjustColors(blackRookButton, Color.green);
        }
    }

    public void SelectWhiteQueen()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == " whiteQueen")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "whiteQueen";
            AdjustColors(whiteQueenButton, Color.green);
        }
    }

    public void SelectBlackQueen()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == "blackQueen")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "blackQueen";
            AdjustColors(blackQueenButton, Color.green);
        }
    }

    public void SelectWhiteKing()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == "whiteKing")
        {
            activePieceTypeSelect = "unselected";
        }
        else { 
            activePieceTypeSelect = "whiteKing";
            AdjustColors(whiteKingButton, Color.green);
        }
    }

    public void SelectBlackKing()
    {
        UnselectAllColors();
        if (activePieceTypeSelect == "blackKing")
        {
            activePieceTypeSelect = "unselected";
        }
        else
        {
            activePieceTypeSelect = "blackKing";
            AdjustColors(blackKingButton, Color.green);
        }
    }

    public void UnselectAll()
    {
        activePieceTypeSelect = "unselected";
        UnselectAllColors();
    }
    
}
