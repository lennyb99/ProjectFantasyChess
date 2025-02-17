using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartRoutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>() != null)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("not found");
        }


        //StartCoroutine(DjangoBackendAPI.RegisterUser("TestUser2", "123", (success, response) =>
        //{
        //    Debug.Log($"Register: Erfolg = {success}, Antwort = {response}");
        //}));

        //StartCoroutine(DjangoBackendAPI.Login("TestUser2", "123", (success, response) =>
        //{
        //    if (success)
        //    {


        //    }
        //}));


        //StartCoroutine(DjangoBackendAPI.SaveBoard("TestUser2", "unityboard", "iusdhfsfsdifsdfs-fgregioerig", (saveSuccess, saveResponse) =>
        //{
        //    Debug.Log($"Register: Erfolg = {saveSuccess}, Antwort = {saveResponse}");
        //}));


        //StartCoroutine(DjangoBackendAPI.SaveBoard("TestUser2", "moooin", "sdoufhsuref-ewfuw28r23r-g49gujj", (success, response) =>
        //{
        //    Debug.Log("moin");
        //}));

        //StartCoroutine(DjangoBackendAPI.GetBoards("lenny", (success, response) =>
        //{
        //    Debug.Log(response.ToString());
        //}));

        //StartCoroutine(DjangoBackendAPI.GetBoards("lenny", (success, response) =>
        //{
        //    if (success)
        //    {
        //        string[] jsonBoards = DjangoBackendAPI.SplitJsonObjects(response);
        //        foreach(string j in jsonBoards){

        //            BoardResponse board = JsonUtility.FromJson<BoardResponse>(j);
        //            Debug.Log(board.title);
        //            Debug.Log(board.position_data);
        //        }

        //    }
        //    else
        //    {
        //        Debug.LogError(response.ToString());
        //    }
        //}));
    }

}
