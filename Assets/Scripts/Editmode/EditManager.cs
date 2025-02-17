using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    public BoardManager boardManager;
    AppManager appManager;
    MultiplayerManager multiplayerManager;

    public void Start()
    {
        multiplayerManager = MultiplayerManager.Instance;
        appManager = AppManager.Instance;
    }
    public void Submit(string boardName)
    {
        BoardLayout bl = boardManager.CreateBoardLayout();
        string serializedBl = appManager.GetSerializedBoardLayoutData(bl);
        StartCoroutine(DjangoBackendAPI.SaveBoard(multiplayerManager.GetUsername(),boardName,serializedBl, (success, response) =>
        {
            if (success)
            {
                Debug.Log("saved successfully to database");
            }
            else
            {
                Debug.Log(response.ToString());
            }
        }));

        // Add board to local data
        appManager.AddBoardLayout(boardName, bl);
    }
}
