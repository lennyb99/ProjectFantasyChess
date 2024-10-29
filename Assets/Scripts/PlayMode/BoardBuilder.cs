using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{

    private void Start()
    {
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
            Debug.Log(boardLayout.fileCount);
            Debug.Log(boardLayout.rowCount);

            foreach (KeyValuePair<(int,int),(bool, string)> pair in boardLayout.squares){
                Debug.Log(pair.Key +" "+ pair.Value);
            }
        }
    }
}
