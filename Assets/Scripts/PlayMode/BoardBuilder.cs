using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{
    public GameObject squarePrefab;
    private void Start()
    {
        Dictionary<(int, int), (bool, string)> debugDict = new Dictionary<(int, int), (bool, string)>
        {
            { (1,1),(true,"whiteQueen") },
            { (1,2),(true,"blackQueen") },
            { (1,3),(true,"whiteKing") },
            { (2,1),(true,"whiteQueen") },
            { (2,2),(true,"blackQueen") },
            { (2,3),(true,"whiteKing") },
            { (3,1),(true,"whiteQueen") },
            { (3,2),(true,"blackQueen") },
            { (3,3),(true,"whiteKing") },
            { (4,1),(true,"whiteQueen") },
            { (4,2),(true,"blackQueen") },
            { (4,3),(true,"whiteKing") },
        };
        BoardLayout debugLayout = new BoardLayout(3, 4, debugDict);
        //buildBoard(debugLayout);
        
        
        buildBoard(GameData.GetBoardLayout());
    }

    public void buildBoard(BoardLayout boardLayout)
    {
        if (boardLayout == null)
        {
            Debug.Log("board is null");
        }
        else
        {
            foreach (KeyValuePair<(int,int),(bool, string)> pair in boardLayout.squares){
                Vector3 spawnPosition = new Vector3(pair.Key.Item1 *1.0f, pair.Key.Item2 * 1.0f, 0);
               
                Instantiate(squarePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
