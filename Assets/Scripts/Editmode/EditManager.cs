using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    public BoardManager boardManager;

    public void Submit()
    {
        boardManager.CreateBoardLayout();
        SceneManager.LoadScene(0);
    }
}