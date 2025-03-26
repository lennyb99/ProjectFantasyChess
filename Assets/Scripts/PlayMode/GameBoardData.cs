using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameBoardData
{
    public static List<GameObject> squares;
    public static List<GameObject> pieces;
    public static List<Move> moves = new List<Move>();

    public static List<BoardPosition> boardPositions = new List<BoardPosition>();
    public static int fiftyMoveRuleCounter;


    public static bool whiteToMove;

    public static int whitePawnBaseRank;
    public static int blackPawnBaseRank;

    public static int whitePawnPromotionRank;
    public static int blackPawnPromotionRank;

    /*
     * Function to provide a certain square in the list of squares that are present.
     * returns null if no square is found
     */
    public static PlaySquare FindSquareByCoordinates(int file, int rank)
    {
        foreach (GameObject square in squares)
        {
            if (square.GetComponent<PlaySquare>().file == file && square.GetComponent<PlaySquare>().rank == rank)
            {
                return square.GetComponent<PlaySquare>();
            }
        }
        return null;
    }

    public static Move GetLastMove()
    {
        if(moves.Count > 0)
        {
            return moves[moves.Count - 1];
        }
        return null;
    }
}
