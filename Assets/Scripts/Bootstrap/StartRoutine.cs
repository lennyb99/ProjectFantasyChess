using System;
using System.Collections;
using System.Collections.Generic;
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

        StartCoroutine(DjangoBackendAPI.GetBoards("TestUser2", (success, response) =>
        {
            Debug.Log(response.ToString());
        }));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
