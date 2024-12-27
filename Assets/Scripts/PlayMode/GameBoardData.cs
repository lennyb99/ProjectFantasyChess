using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameBoardData
{
    public static List<GameObject> squares;
    public static List<GameObject> pieces;



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
}
