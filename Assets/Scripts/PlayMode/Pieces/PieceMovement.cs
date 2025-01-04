using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class PieceMovement
{
    /*
     * Support function to check for diagonal movements, eg. bishop, rook, queen
     */
    public static void GetAllFieldsTowardsDirection(Piece movingPiece,  Func<PlaySquare, PlaySquare> getNextField, PlaySquare currentField, List<PlaySquare> possibleFields)
    {
        PlaySquare tempCheckField = currentField; // Resets position to current position
       
        while (getNextField(tempCheckField) != null) // Checks whether an end of a board is reached
        {
            PlaySquare nextField = getNextField(tempCheckField);

            if (nextField.GetCurrentPiece() == null)
            {
                possibleFields.Add(nextField);
                tempCheckField = nextField;
                continue;
            }

            Piece piece = nextField.GetCurrentPiece(); // Stores reference to the piece on the viewed square
            if (IsFieldWithTargetPieceTakeable(movingPiece,piece))
            {
                possibleFields.Add(nextField); // Add Square to the possible destination squares 
            }
            break;
        }
    }

    //   /*
    //* Support function to check for diagonal movements, eg. bishop, rook, queen
    //*/
    //   public static void GetAllGuardedFieldsTowardsDirection(Func<Field, Field> getNextField, Field currentField, List<Field> possibleFields)
    //   {
    //       Field tempCheckField = currentField; // Resets position to current position
    //       while (getNextField(tempCheckField) != null)
    //       {
    //           Field nextField = getNextField(tempCheckField);

    //           possibleFields.Add(nextField);
    //           tempCheckField = nextField;

    //           if (nextField.GetCurrentGameObject() != null)
    //           {
    //               possibleFields.Add(nextField);
    //               break;
    //           }


    //           //possibleFields.Add(nextField); // Add Square to the possible destination squares 

    //       }
    //   }


    


    /*
     * Support function to reduce redundancy
     * 
     * Returns TRUE if square is available for current piece
     * FALSE if not
     */
    public static bool IsFieldWithTargetPieceTakeable(Piece movingPiece, Piece viewedPiece)
    {
        // Rook is WHITE and target piece is WHITE OR Rook is BLACK and target piece is BLACK
        if (movingPiece.isWhite && viewedPiece.isWhite || !movingPiece.isWhite && !viewedPiece.isWhite)
        {
            return false;
        }
        else        // Rook is WHITE and target piece is BLACK OR Rook is BLACK and target piece is WHITE
                    //if( IsWhite && !piece.IsWhite || !IsWhite && piece.IsWhite)
        {
            return true;
        }
    }
}
