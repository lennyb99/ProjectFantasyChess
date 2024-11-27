using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{
    public GameObject squarePrefab;
    public List<GameObject> squares;
    public List<GameObject> pieces;

    public PieceHolder pieceHolder;


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
                    InitializeSquare(pair.Key.Item1, pair.Key.Item2, boardLayout.whitePov);
                    InitializePiece(pair.Key.Item1, pair.Key.Item2, boardLayout.whitePov, pair.Value.Item2);
                }
            }
        }
    }

    private void InitializeSquare(int file, int rank, bool whitePov)
    {
        GameObject square = Instantiate(squarePrefab, CreateVectorPosition(file, rank, whitePov), Quaternion.identity);
        squares.Add(square);
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

    private void InitializePiece(int file, int rank, bool whitePov, string name)
    {
        GameObject piecePrefab = pieceHolder.GetPiece(name);
        Debug.Log(name);
        if (piecePrefab == null)
        {
            Debug.Log("Piece couldnt be retrieved from Piece Holder");
            return;
        }
        GameObject piece = Instantiate(piecePrefab, CreateVectorPosition(file, rank, whitePov), Quaternion.identity);
        pieces.Add(piece); 
    }

    private GameObject CreatePiece()
    {
        GameObject piece = new GameObject();

        return null;
    }
}
