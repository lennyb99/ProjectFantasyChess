using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public List<(string, BoardLayout)> boardLayouts = new List<(string, BoardLayout)>();
    

    public static AppManager Instance { get; private set; }

    public BoardLayout selectedBoardLayout;
    private int count = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        boardLayouts.Add(("default",GenerateStandardChessBoardLayout()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBoardLayout(BoardLayout bl)
    {
        boardLayouts.Add(("board"+count,bl));
    }

    public void SelectBoardLayout(BoardLayout bl)
    {
        selectedBoardLayout = bl;
    }

    public BoardLayout GetBoardLayout(string name)
    {
        foreach (var layout in boardLayouts)
        {
            if (layout.Item1 == name)
            {
                return layout.Item2;
            }
        }
        return null;
    }

    public List<(string,BoardLayout)> GetBoardLayouts()
    {
        return boardLayouts;
    }

    private BoardLayout GenerateStandardChessBoardLayout()
    {
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
            { (4,4),(true,"") },
            { (4,5),(true,"") },
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
        BoardLayout debugLayout = new BoardLayout(3, 4, debugDict, true, true);

        return debugLayout;
    }
}
