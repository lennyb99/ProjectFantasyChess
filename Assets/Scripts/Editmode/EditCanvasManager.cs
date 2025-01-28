using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditCanvasManager : MonoBehaviour
{
    public PlayerController playerController;

    public EditManager editManager;

    [Header("EscapeMenu")]
    public GameObject escapeMenu;

    [Header("SaveMenu")]
    public GameObject saveMenu;
    public TMP_InputField saveNameInputField;

    [Header("Notice Boards")]
    public GameObject mediumNoticeBoard;
    public TMP_Text mediumNoticeBoardText;


    public void ToggleEscapeMenu()
    {
        if (escapeMenu.activeSelf) {
            CloseEscapeMenu();
        }
        else
        {
            OpenEscapeMenu();
        }
    }

    private void OpenEscapeMenu()
    {
        playerController.allowSquareActivation = false;
        escapeMenu.SetActive(true);
    }

    private void CloseEscapeMenu()
    {
        playerController.allowSquareActivation = true;
        escapeMenu.SetActive(false);
    }

    public void OpenSaveMenu()
    {
        CloseEscapeMenu();
        saveMenu.SetActive(true);
        playerController.allowSquareActivation = false;
    }

    public void CloseSaveMenu()
    {
        CloseEscapeMenu();
        saveMenu.SetActive(false);
        playerController.allowSquareActivation = true;
    }

    public void SaveCurrentBoard()
    {
        if (!IsValidName(saveNameInputField.text))
        {
            if (!mediumNoticeBoard.activeSelf)
            {
                StartCoroutine(SendNotice("Please enter a valid name for your board."));
            }
        }
        else {
            editManager.Submit(saveNameInputField.text);
            if (!mediumNoticeBoard.activeSelf)
            {
                StartCoroutine(SendNotice("Board has been saved as '" + saveNameInputField.text+"'"));
            }
            CloseSaveMenu();
            OpenEscapeMenu();
        }
    }

    public void SelectBackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private bool IsValidName(string name)
    {
        if (name.Length > 0 && 
            name.Replace(" ", "").Length > 0 &&
            name.Length < 20)
        {
            return true;
        }
        return false;
    }

    IEnumerator SendNotice(string notice)
    {
        mediumNoticeBoardText.text = notice;
        mediumNoticeBoard.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        mediumNoticeBoard.SetActive(false);
    }



}
