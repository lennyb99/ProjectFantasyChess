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

        //Standard chess board setup
        Dictionary<(int, int), (bool, string)> debugDict = new Dictionary<(int, int), (bool, string)>
        {
            { (1,1),(true,"whiteRook") },
            { (1,2),(true,"whitePawn") },
            { (1,3),(true,"") },
            { (1,4),(true,"") },
            { (1,5),(true,"") },
            { (1,6),(true,"") },
            { (1,7),(true,"blackPawn") },
            { (1,8),(true,"blackRook") },
            { (2,1),(true,"whiteKnight") },
            { (2,2),(true,"whitePawn") },
            { (2,3),(true,"") },
            { (2,4),(true,"") },
            { (2,5),(true,"") },
            { (2,6),(true,"") },
            { (2,7),(true,"blackPawn") },
            { (2,8),(true,"blackKnight") },
            { (3,1),(true,"whiteBishop") },
            { (3,2),(true,"whitePawn") },
            { (3,3),(true,"") },
            { (3,4),(true,"") },
            { (3,5),(true,"") },
            { (3,6),(true,"") },
            { (3,7),(true,"blackPawn") },
            { (3,8),(true,"blackBishop") },
            { (4,1),(true,"whiteQueen") },
            { (4,2),(true,"whitePawn") },
            { (4,3),(true,"") },
            { (4,4),(true,"whiteRook") },
            { (4,5),(true,"blackBishop") },
            { (4,6),(true,"") },
            { (4,7),(true,"blackPawn") },
            { (4,8),(true,"blackQueen") },
            { (5,1),(true,"whiteKing") },
            { (5,2),(true,"whitePawn") },
            { (5,3),(true,"") },
            { (5,4),(true,"") },
            { (5,5),(true,"") },
            { (5,6),(true,"") },
            { (5,7),(true,"blackPawn") },
            { (5,8),(true,"blackKing") },
            { (6,1),(true,"whiteBishop") },
            { (6,2),(true,"whitePawn") },
            { (6,3),(true,"") },
            { (6,4),(true,"") },
            { (6,5),(true,"") },
            { (6,6),(true,"") },
            { (6,7),(true,"blackPawn") },
            { (6,8),(true,"blackBishop") },
            { (7,1),(true,"whiteKnight") },
            { (7,2),(true,"whitePawn") },
            { (7,3),(true,"") },
            { (7,4),(true,"") },
            { (7,5),(true,"") },
            { (7,6),(true,"") },
            { (7,7),(true,"blackPawn") },
            { (7,8),(true,"blackKnight") },
            { (8,1),(true,"whiteRook") },
            { (8,2),(true,"whitePawn") },
            { (8,3),(true,"") },
            { (8,4),(true,"") },
            { (8,5),(true,"") },
            { (8,6),(true,"") },
            { (8,7),(true,"blackPawn") },
            { (8,8),(true,"blackRook") },
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
                        square.GetComponent<PlaySquare>().SetCurrentPiece(piece.GetComponent<Piece>());
                        piece.GetComponent<Piece>().SyncPiecePositionToCurrentSquare();
                    }
                }
            }
            SetupSquareNeighborLinks();
            SetTexturesForSquares();
            SendBoardDataToGameBoardData();
            SendPawnRuleData();
        }
    }

    private void SendPawnRuleData()
    {
        GameBoardData.blackPawnBaseRank = 7;
        GameBoardData.whitePawnBaseRank = 2;
        GameBoardData.blackPawnPromotionRank = 1;
        GameBoardData.whitePawnPromotionRank = 8;
    }

    private void SendBoardDataToGameBoardData()
    {
        GameBoardData.squares = this.squares;
        GameBoardData.pieces = this.pieces;
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
