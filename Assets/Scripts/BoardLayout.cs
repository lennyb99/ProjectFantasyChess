using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static BoardLayout boardLayout;

    public static BoardLayout GetBoardLayout()
    {
        return boardLayout;
    }

    public static void SetBoardLayout(BoardLayout newBoardLayout)
    {
        boardLayout = newBoardLayout;
    }
}


public class BoardLayout
{
    public int fileCount;
    public int rowCount;
    public Dictionary<(int, int), (bool, string)> squares;

    public BoardLayout(int fileCount, int rowCount, Dictionary<(int,int),(bool,string)> squares)
    {
        this.fileCount = fileCount;
        this.rowCount = rowCount;
        this.squares = squares;
    }
}
