using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Data.SqlTypes;

public class BoardManager : MonoBehaviour
{

    public List<GameObject> squares = new List<GameObject>();
    int fileCount = 0;
    int rowCount = 0;

    public GameObject squarePrefab;
    public GameObject squareCollection;

    AppManager appManager;


    // Start is called before the first frame update
    void Start()
    {
        appManager = AppManager.Instance;
        InstantiateSquares();
        SortSquares();
        CountDimensions();
        AssignSquareCoordinates();
        AssignSquareColorTheme();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InstantiateSquares()
    {
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                //squares.Add(Instantiate(squarePrefab,new Vector3(i, j , 0), Quaternion.identity, squareCollection.transform));
                squares.Add(Instantiate(squarePrefab, new Vector3(i+.5f, j+.5f, 0), Quaternion.identity));
            }
        }
    }

    private void AssignSquareColorTheme()
    {
        foreach (GameObject squareObj in squares)
        {
            EditSquare square = squareObj.GetComponent<EditSquare>();
            if (square!= null)
            {
                if(square.file % 2 == 0){
                    if(square.rank % 2 == 0)
                    {
                        square.SetColorTheme(true);
                    }
                    else
                    {
                        square.SetColorTheme(false);
                    }
                }
                else
                {
                    if (square.rank % 2 == 0)
                    {
                        square.SetColorTheme(false);
                    }
                    else
                    {
                        square.SetColorTheme(true);
                    }
                }
            }
        }
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
                squares[squareCounter].GetComponent<EditSquare>().file = j;
                squares[squareCounter].GetComponent<EditSquare>().rank = i;
                squareCounter++; 
            }
        }
    }

    public void CreateBoardLayout(string boardname)
    {
        Dictionary<(int, int), (bool, string)> squareOccupations = new Dictionary<(int, int), (bool, string)>();
        foreach (var squareObject in squares)
        {
            EditSquare square = squareObject.GetComponent<EditSquare>();

            squareOccupations.Add((square.file, square.rank), (square.isActive, square.GetSquareOccupationAsString()));
        }
        BoardLayout boardLayout = new BoardLayout(fileCount, rowCount, squareOccupations, true, true);

        //GameData.SetBoardLayout(boardLayout);  OLD WAY
        appManager.AddBoardLayout(boardname,boardLayout);
    }

    public List<GameObject> ReturnMatrixOfSquareObjects(EditSquare firstSquare, EditSquare secondSquare)
    {
        List<GameObject> squaresBetweenTargetSquares = new List<GameObject>();
       
        foreach (var squareObject in squares)
        {
            if(IsValueBetween(firstSquare.file, secondSquare.file, squareObject.GetComponent<EditSquare>().file))
            {
                if(IsValueBetween(firstSquare.rank, secondSquare.rank, squareObject.GetComponent<EditSquare>().rank))
                {
                    squaresBetweenTargetSquares.Add(squareObject);
                }
            }
        }

        return squaresBetweenTargetSquares;
    }

    private bool IsValueBetween(int value1, int value2, int checkValue)
    {
        int min = Mathf.Min(value1, value2);
        int max = Mathf.Max(value1, value2);
        return checkValue >= min && checkValue <= max;
    }


    private void addFiles()
    {
        

    }

}
