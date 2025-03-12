using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("PromotionPanel")]
    public GameObject promotionPanel;
    public Button queenButton;
    public Button rookButton;
    public Button bishopButton;
    public Button knightButton;

    [Header("EscapeMenu")]
    public GameObject menuPanel;

    public PlayerManager playerManager;
    public GameManager gameManager;

    public event Action OnPromotionSelected;

    [Header("Result Panel")]
    public GameObject resultPanel;
    public GameObject whiteWonPanel;
    public GameObject blackWonPanel;
    public GameObject drawPanel;


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

    /*
     * 0 = draw
     * 1 = win for white
     * 2 = win for black
     */
    public void OpenResultPanel(int result)
    {
        if (result == 0)
        {
            drawPanel.SetActive(true);
        }
        else if (result == 1)
        {
            whiteWonPanel.SetActive(true);
        }else if (result == 2)
        {
            blackWonPanel.SetActive(true);  
        }
        else
        {
            Debug.LogError("unvalid result. Please enter int 0,1 or 2");
        }


        resultPanel.SetActive(true);
    }

    public void CloseResultPanel()
    {
        drawPanel.SetActive(false);
        whiteWonPanel.SetActive(false);
        blackWonPanel.SetActive(false);
        resultPanel.SetActive(false);
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

    private void OpenMenuPanel()
    {
        playerManager.allowPieceMovements=false;
        menuPanel.SetActive(true);
    }

    private void CloseMenuPanel()
    {
        playerManager.allowPieceMovements = true;
        menuPanel.SetActive(false);
    }

    public void ToggleMenuPanel()
    {
        if (menuPanel.activeSelf)
        {
            CloseMenuPanel();
        }
        else
        {
            OpenMenuPanel();
        }
    }

    public void SelectBackToMenu()
    {
        gameManager.LeaveGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void SelectSurrender()
    {

    }

    public void SelectDrawOffer()
    {

    }

}
