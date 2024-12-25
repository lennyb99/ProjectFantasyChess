using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{
    public GameObject squarePrefab;
    public List<GameObject> squares;
    public List<GameObject> pieces;

    public PieceHolder pieceHolder;

    int DebugCount = 0;

    private void Start()
    {
        squares = new List<GameObject>();

        Dictionary<(int, int), (bool, string)> debugDict = new Dictionary<(int, int), (bool, string)>
        {
            { (1,1),(true,"whiteRook") },
            { (1,2),(true,"blackPawn") },
            { (1,3),(true,"blackRook") },
            { (2,1),(true,"whiteQueen") },
            { (2,2),(true,"whitePawn") },
            { (2,3),(true,"blackQueen") },
            { (3,1),(true,"whiteKing") },
            { (3,2),(true,"blackPawn") },
            { (3,3),(true,"blackKing") },
            { (4,1),(true,"whiteRook") },
            { (4,2),(true,"whitePawn") },
            { (4,3),(true,"blackBishop") },
        };
        BoardLayout debugLayout = new BoardLayout(3, 4, debugDict, true);
        BuildBoard(debugLayout);
        
        //buildBoard(GameData.GetBoardLayout());
    }

    public void BuildBoard(BoardLayout boardLayout)
    {
        if (boardLayout == null)
        {
            Debug.Log("board is null");
        }
        else
        {
            foreach (KeyValuePair<(int,int),(bool, string)> pair in boardLayout.squares){
                Vector3 spawnPosition = new Vector3(pair.Key.Item1 *1.0f, pair.Key.Item2 * 1.0f, 0);

                if (pair.Value.Item1) { 
                    GameObject square = InitializeAndReturnSquare(pair.Key.Item1, pair.Key.Item2, boardLayout.whitePov);
                    GameObject piece = InitializeAndReturnPiece(pair.Key.Item1, pair.Key.Item2, boardLayout.whitePov, pair.Value.Item2);

                    if (piece != null) { 
                    piece.GetComponent<Piece>().SetCurrentSquare(square.GetComponent<PlaySquare>());
                    piece.GetComponent<Piece>().SyncPiecePositionToCurrentSquare();
                    }
                }
            }
            SetupSquareNeighborLinks();
            SetTexturesForSquares();
        }
    }
    
    private void SetupSquareNeighborLinks()
    {
        foreach(GameObject square in squares)
        {
            square.GetComponent<PlaySquare>().AssignAdjacentSquares();
        }
    }

    private GameObject InitializeAndReturnSquare(int file, int rank, bool whitePov)
    {
        GameObject square = Instantiate(squarePrefab, CreateVectorPosition(file, rank, whitePov), Quaternion.identity);
        square.name = "square" + DebugCount;
        DebugCount++;

        square.GetComponent<PlaySquare>().file = file;
        square.GetComponent<PlaySquare>().rank = rank;
        square.GetComponent<PlaySquare>().board = this;

        squares.Add(square);

        return square;
    }

    /*
     * Returns a vector3 that represents the position of the square for whites/blacks perspective
    */
    private Vector3 CreateVectorPosition(int file, int rank, bool whitePov)
    {
        if (whitePov)
        {
            return new Vector3(file, rank, 0);
        }
        else
        {
            return new Vector3(9 - file, 9 - rank, 0);
        }
        
    }

    private GameObject InitializeAndReturnPiece(int file, int rank, bool whitePov, string name)
    {
        GameObject piecePrefab = pieceHolder.GetPiece(name);
        if (piecePrefab == null)
        {
            Debug.Log("Piece couldnt be retrieved from Piece Holder");
            return null;
        }
        GameObject piece = Instantiate(piecePrefab, CreateVectorPosition(file, rank, whitePov), Quaternion.identity);
        pieces.Add(piece);

        return piece;
    }

    private void SetTexturesForSquares()
    {
        foreach (GameObject square in squares)
        {
            SpriteRenderer spriteRen = square.GetComponent<SpriteRenderer>();
            if (square.GetComponent<PlaySquare>() != null)
            {
                if(square.GetComponent<PlaySquare>().file % 2 == 0)
                {
                    if(square.GetComponent<PlaySquare>().rank % 2 == 0)
                    {
                        // black
                        spriteRen.sprite = pieceHolder.blackSquareTexture;
                    }
                    else
                    {
                        // white
                        spriteRen.sprite = pieceHolder.whiteSquareTexture;
                    }
                }
                else
                {
                    if (square.GetComponent<PlaySquare>().rank % 2 == 0)
                    {
                        // white
                        spriteRen.sprite = pieceHolder.whiteSquareTexture;
                    }
                    else
                    {
                        // black
                        spriteRen.sprite = pieceHolder.blackSquareTexture;
                    }
                }
            }
        }
    }

    private GameObject CreatePiece()
    {
        GameObject piece = new GameObject();

        return null;
    }


    /*
     * Function to provide a certain square in the list of squares that are present.
     * returns null if no square is found
     */
    public PlaySquare FindSquareByCoordinates(int file, int rank)
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
