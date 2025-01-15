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

[System.Serializable]
public class BoardLayout
{
    public int fileCount;
    public int rowCount;
    public Dictionary<(int, int), (bool, string)> squares;
    public bool whitePov;
    public bool whiteToMove;

    public BoardLayout(int fileCount, int rowCount, Dictionary<(int,int),(bool,string)> squares, bool whitePov, bool whiteToMove)
    {
        this.fileCount = fileCount;
        this.rowCount = rowCount;
        this.squares = squares;
        this.whitePov = whitePov;
        this.whiteToMove = whiteToMove;
    }
}

[System.Serializable]
public class ChallengeSettings
{
    public bool whitePov;

    public ChallengeSettings(bool whitePov)
    {
        this.whitePov = whitePov;
    }
}

