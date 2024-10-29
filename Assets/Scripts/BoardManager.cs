using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public List<GameObject> squares;
    

    // Start is called before the first frame update
    void Start()
    {
        InitializeSquaresList();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeSquaresList()
    {
        squares = new List<GameObject>();
        squares.AddRange(GameObject.FindGameObjectsWithTag("Square"));
        Debug.Log("Registered " + squares.Count + " squares to Board Manager");
    }

    public void test()
    {
        Debug.Log("test");
    }

    private void addFiles()
    {
        

    }

}
