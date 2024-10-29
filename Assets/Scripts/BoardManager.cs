using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;

public class BoardManager : MonoBehaviour
{

    public List<GameObject> squares;
    int fileCount = 0;
    int rowCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSquaresList();
        SortSquares();
        CountDimensions();
        AssignSquareCoordinates();
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

    private void SortSquares()
    {
        squares = squares.OrderBy(obj => obj.transform.position.y).ThenBy(obj => obj.transform.position.x).ToList();   
    }

    private void CountDimensions()
    {
        fileCount = squares.Select(obj => obj.transform.position.x).Distinct().Count();
        rowCount = squares.Select(obj => obj.transform.position.y).Distinct().Count();

        Debug.Log("Registered "+fileCount+" files and "+rowCount + " rows");
    }

    private void AssignSquareCoordinates()
    {
        int squareCounter = 0;
        for (int i = 1; i <= rowCount; i++)
        {
            for (int j = 1; j <= fileCount; j++)
            {
                squares[squareCounter].GetComponent<Square>().file = j;
                squares[squareCounter].GetComponent<Square>().row = i;
                squareCounter++; 
            }
        }
    }
    public void test()
    {
        Debug.Log("test");
    }

    private void addFiles()
    {
        

    }

}
