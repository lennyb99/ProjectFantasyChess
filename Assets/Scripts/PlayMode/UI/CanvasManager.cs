using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        queenButton.onClick.AddListener(() => OnButtonClicked(queenButton));
        rookButton.onClick.AddListener(() => OnButtonClicked(rookButton));
        bishopButton.onClick.AddListener(() => OnButtonClicked(bishopButton)); 
        knightButton.onClick.AddListener(() => OnButtonClicked(knightButton));
    }


    private void OnButtonClicked(Button clickedButton)
    {
        if (clickedButton == queenButton)
        {
            gameManager.SetPromotionPieceName("Queen");
        }
        if (clickedButton == rookButton)
        {
            gameManager.SetPromotionPieceName("Rook");
        }
        if (clickedButton == bishopButton)
        {
            gameManager.SetPromotionPieceName("Bishop");
        }
        if (clickedButton == knightButton)
        {
            gameManager.SetPromotionPieceName("Knight");
        }
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
