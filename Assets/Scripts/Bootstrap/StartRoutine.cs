using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartRoutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>() != null)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
