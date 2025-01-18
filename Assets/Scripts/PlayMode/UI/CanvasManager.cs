using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public GameObject promotionPanel;
    public Button queenButton;
    public Button rookButton;
    public Button bishopButton;
    public Button knightButton;

    public PlayerManager playerManager;
    public GameManager gameManager;

    public event Action OnPromotionSelected;

    // Start is called before the first frame update
    void Start()
    {
        queenButton.onClick.AddListener(() => OnButtonClicked("Queen"));
        rookButton.onClick.AddListener(() => OnButtonClicked("Rook"));
        bishopButton.onClick.AddListener(() => OnButtonClicked("Bishop"));
        knightButton.onClick.AddListener(() => OnButtonClicked("Knight"));
    }


    private void OnButtonClicked(string pieceName)
    {
        Debug.Log($"{pieceName} ausgewählt.");
        gameManager.SetPromotionPieceName(pieceName);
        gameManager.selectionCompleted = true;

        // Promotion-Panel schließen
        ClosePromotionPanel();

        // Event auslösen
        OnPromotionSelected?.Invoke();
    }

    public void OpenPromotingPanel()
    {
        promotionPanel.SetActive(true);
        playerManager.allowPieceMovements = false;
    }

    public void ClosePromotionPanel()
    {
        promotionPanel.SetActive(false);
        playerManager.allowPieceMovements = true;
    }

    

}
