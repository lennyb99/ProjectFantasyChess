using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    MultiplayerManager multiplayerManager;

    public PieceHolder pieceHolder;
    public CanvasManager canvas;

    public bool selectionCompleted = true;
    private Piece promotingPawn;
    public string promotionPieceName = "";
    //public event Action OnPromotionPieceNameChanged;

    private TaskCompletionSource<bool> promotionTaskCompletionSource;

    

    public void Start()
    {
        multiplayerManager = MultiplayerManager.Instance;

        RegisterToMultiplayerManager();
    }

    private void RegisterToMultiplayerManager()
    {
        
        if (multiplayerManager != null)
        {
            multiplayerManager.gameManager = this;
        }
        else
        {
            Debug.Log("CRITICAL ERROR. Game Manager not found");
        }
    }

    public async void RequestMove(Move newMove)
    {
        // Collecting information for whether the move was a promoting move and then using that to make a MoveInstruction Object to send.
        bool promotionMove = false;
        bool castlingMove = false;
        

        // Check if values of Move Object are valid
        if (!ValidateMoveRequest(newMove))
        {
            newMove.movedPiece.ResetPhysicalPosition();
            return;
        }

        if (!CheckForCorrectTurn(newMove))
        {
            newMove.movedPiece.ResetPhysicalPosition();
            return;
        }

        // Makes sure, that player cannot move opponents pieces
        //if (!MovedOwnColorPiece(newMove))
        //{
        //    newMove.movedPiece.ResetPhysicalPosition();
        //    return;
        //}

        // Pre-check for checks. this checks if a move that is supposed to be made, would result in an opponent checking the king, therefore its illegal
        if (DoesMovePutOwnKingInCheck(newMove))
        {
            Debug.Log("Move is not permitted. Own king in check");
            newMove.movedPiece.ResetPhysicalPosition();
            return;
        }

        // Check if movement of pieceType is corresponding with its rules
        if (!CheckMoveIntegrity(newMove))
        {
            Debug.Log("Move is not permitted");
            newMove.movedPiece.ResetPhysicalPosition();
            return;
        }

        

        if (!CheckForPromotion(newMove)) {
            IncrementFiftyMoveCounter();
            ExecuteMoveOnBoard(newMove);
        }
        else
        {
            promotionMove = true;
            await HandlePromotion(newMove);
        }

        // Checks if castling is performed and gives true to collect data for MoveInstruction
        if (HandleCastling(newMove))
        {
            castlingMove = true;
        }

        RegisterMove(newMove);
        Debug.Log("Move is permitted");

        // Post-check for checks
        if (DoesMovePutEnemyKingInCheck(newMove))
        {
            // Handle Check 

            // Check for Checkmate
            if (IsCheckmate(newMove))
            {
                // Handle Checkmate
                Debug.LogWarning("CHECKMATE DETECTED");
                HandleCheckmate(newMove);
            }
        }

        if (IsDraw(newMove))
        {
            HandleDraw(newMove);
        }

        // Sending move to other players
        SendMoveToOtherPlayers(newMove, promotionMove, castlingMove);

        // Any class wide data that has been saved to calculate this move specifically
        ResetMoveInformation();

        Debug.Log("----");
        Debug.Log("----");
    }

    private void SendMoveToOtherPlayers(Move move, bool promotionMove, bool castling)
    {
        MoveInstruction moveInstruction = new MoveInstruction();

        moveInstruction = moveInstruction.CreateMoveInformation(move, promotionMove, promotionPieceName, castling);


        multiplayerManager.SendMoveInstruction(moveInstruction.GetSerializedMoveInstruction());

        Debug.Log("castling: " + moveInstruction.castlingMove);

        promotionMove = false;
    }


    public void QueueOpponentTurn(string serializedMoveInstruction)
    {
        if (serializedMoveInstruction == null)
        {
            Debug.Log("Received move is not valid");
            return;
        }

        MoveInstruction moveInstruction = MoveInstruction.GetDeserializedMoveInstruction(serializedMoveInstruction);

        PlaySquare psOrigin = GameBoardData.FindSquareByCoordinates(moveInstruction.originSquareFile, moveInstruction.originSquareRank);
        PlaySquare psDestination = GameBoardData.FindSquareByCoordinates(moveInstruction.destinationSquareFile, moveInstruction.destinationSquareRank);

        if(psOrigin == null || psDestination == null)
        {
            Debug.Log("Invalid Square information");
        }

        Move newMove = new Move(psOrigin, psDestination, psOrigin.GetCurrentPiece());

        IncrementFiftyMoveCounter();
        ExecuteMoveOnBoard(newMove);

        RegisterMove(newMove);

        // If a promotion move was made
        if (moveInstruction.promotionMove)
        {
            // Remove piece from board
            Destroy(psDestination.GetCurrentPiece().gameObject);

            // Spawn new Piece
            GameObject piecePrefab = pieceHolder.GetPiece(moveInstruction.promotionPieceType);
            if (piecePrefab == null)
            {
                Debug.Log("promotion piece NULL");
                return;
            }
            Piece newPiece = Instantiate(piecePrefab, psDestination.gameObject.transform.position, Quaternion.identity).GetComponent<Piece>();
            // swapping of the square with piece relationship
            newPiece.currentSquare = psDestination;
            psDestination.SetCurrentPiece(newPiece);
            newPiece.SyncPiecePositionToCurrentSquare();
        }

        // if castling move was made
        if (moveInstruction.castlingMove)
        {
            Debug.Log("detecting castling move");
            Move rookMove = null;
            if(psDestination.midLeft != null && psDestination.midLeft.GetCurrentPiece() != null &&
               psDestination.midLeft.GetCurrentPiece().pieceType == PieceType.rook)
            {
                rookMove = new Move(psDestination.midLeft, psDestination.midRight, psDestination.midLeft.GetCurrentPiece());
            }
            else
            {
                rookMove = new Move(psDestination.midRight, psDestination.midLeft, psDestination.midRight.GetCurrentPiece());
            }

            ExecuteMoveOnBoard(rookMove);
        }

        // Check for Checkmate
        if (IsCheckmate(newMove))
        {
            // Handle Checkmate
            Debug.LogWarning("CHECKMATE DETECTED");
            HandleCheckmate(newMove);
        }
        if (IsDraw(newMove))
        {
            HandleDraw(newMove);
        }

    }


    private Piece FindKing(bool white)
    {
        foreach(GameObject piece in GameBoardData.pieces)
        {
            if(piece != null) { 
                if(piece.GetComponent<Piece>().pieceType == PieceType.king && piece.GetComponent<Piece>().isWhite == white)
                {
                    return piece.GetComponent<Piece>();
                }
            }
        }
        return null;
    }

    private bool DoesMovePutOwnKingInCheck(Move newMove)
    {
        bool checkFlag = false;

        // Store all relevant information
        Piece takenPiece = newMove.destinationSquare.GetCurrentPiece();


        // Update position of piece as if move was made
        newMove.originSquare.SetCurrentPiece(null);
        newMove.destinationSquare.SetCurrentPiece(newMove.movedPiece);

        newMove.movedPiece.SetCurrentSquare(newMove.destinationSquare);

        // Check if move got the OWN king into check
        if (King.IsKingInCheck(FindKing(newMove.movedPiece.isWhite)))
        {
            checkFlag = true;
        }

        // Reset position
        newMove.destinationSquare.SetCurrentPiece(takenPiece);
        newMove.originSquare.SetCurrentPiece(newMove.movedPiece);
        newMove.movedPiece.SetCurrentSquare(newMove.originSquare);

        return checkFlag;
    }

    private bool DoesMovePutEnemyKingInCheck(Move newMove)
    {
        if (King.IsKingInCheck(FindKing(!newMove.movedPiece.isWhite)))
        {
            return true;
        }
        return false;
    }

    private bool IsCheckmate(Move newMove)
    {
        bool whiteKingInCheck = !newMove.movedPiece.isWhite; // to know for which king checkmate should be detected
        foreach (GameObject pieceObj in GameBoardData.pieces)
        {
            if (pieceObj != null && pieceObj.GetComponent<Piece>().isWhite == whiteKingInCheck)
            {
                List<Move> possibleMoves = pieceObj.GetComponent<Piece>().GetAllPossibleMoves();

                foreach(Move move in possibleMoves)
                {
                    if (DoesMovePreventCheckmate(move))
                    {
                        Debug.Log(move.movedPiece.gameObject.name + move.destinationSquare.gameObject.name + move.originSquare.gameObject.name);
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private void HandleCheckmate(Move newMove)
    {
        if (newMove.movedPiece.isWhite)
        {
            canvas.OpenResultPanel(1);
        }
        else
        {
            canvas.OpenResultPanel(2);
        }
    }

    private void IncrementFiftyMoveCounter()
    {
        GameBoardData.fiftyMoveRuleCounter++;
    }

    private bool IsDraw(Move newMove)
    {
        // Check for repetition
        BoardPosition currentBoardPosition = GameBoardData.boardPositions[GameBoardData.boardPositions.Count - 1];

        int counter = 0;
        foreach (BoardPosition bP in GameBoardData.boardPositions)
        {
            if (currentBoardPosition.CompareBoardPosition(bP))
            {
                counter++;
            }
        }
        if (counter >= 3)
        {
            return true;
        }

        // Check for 50 move rule
        if (newMove.movedPiece.pieceType == PieceType.pawn)
        {
            GameBoardData.fiftyMoveRuleCounter = 0;
        }
        if( GameBoardData.fiftyMoveRuleCounter >= 50)
        {
            return true;
        }

        List<Move> possibleMoves = new List<Move>();

        // Check if opponent has any legal moves on the board
        foreach (GameObject pieceObj in GameBoardData.pieces)
        {
            if (pieceObj.GetComponent<Piece>().isWhite != newMove.movedPiece.isWhite)
            {
                possibleMoves.AddRange(pieceObj.GetComponent<Piece>().GetAllPossibleMoves());
            }
        }

        foreach(Move move in possibleMoves)
        {
            if (DoesMovePutOwnKingInCheck(move) == false)
            {
                return false;
            }
        }
        
        
        return true;
    }

    private void HandleDraw(Move newMove)
    {
        canvas.OpenResultPanel(0);
    }

    private bool DoesMovePreventCheckmate(Move simulatedMove)
    {
        return !DoesMovePutOwnKingInCheck(simulatedMove);
    }

    private void ExecuteMoveOnBoard(Move newMove)
    {
        
        HandlePieceDeletion(newMove);

        // Reset memory of old square
        newMove.originSquare.SetCurrentPiece(null);

        // Update memory of piece
        newMove.movedPiece.SetCurrentSquare(newMove.destinationSquare);
        newMove.movedPiece.moveCount++;

        // Update memory of new square
        newMove.destinationSquare.SetCurrentPiece(newMove.movedPiece);

        // Actual movement of the physical representation of the peace to its square
        newMove.movedPiece.SyncPiecePositionToCurrentSquare();
    }

    private bool CheckMoveIntegrity(Move newMove)
    {
        switch (newMove.movedPiece.pieceType) {
            case PieceType.rook:
                if (Rook.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;
            case PieceType.bishop:
                if (Bishop.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;
            case PieceType.queen:
                if (Queen.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;
            case PieceType.knight:
                if (Knight.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;
            case PieceType.pawn:
                if (Pawn.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;
            case PieceType.king:
                if (King.CheckMoveIntegrity(newMove))
                {
                    return true;
                }
                break;
        }
        return false;
    }

    private void HandlePieceDeletion(Move newMove)
    {
        // Delete beaten piece from destination Square
        if (newMove.destinationSquare.GetCurrentPiece() != null)
        {
            GameBoardData.pieces.Remove(newMove.destinationSquare.GetCurrentPiece().gameObject);
            GameBoardData.fiftyMoveRuleCounter = 0;
            Destroy(newMove.destinationSquare.GetCurrentPiece().gameObject);
        }

        // Detect En passant and delete target Piece
        if (newMove.movedPiece.pieceType == PieceType.pawn && 
            newMove.originSquare.file != newMove.destinationSquare.file &&
            newMove.destinationSquare.GetCurrentPiece() == null)
        {
            if (newMove.movedPiece.isWhite)
            {
                Destroy(newMove.destinationSquare.bottomMid.GetCurrentPiece().gameObject);
            }
            else
            {
                Destroy(newMove.destinationSquare.topMid.GetCurrentPiece().gameObject);
            }
        }
    }

    private bool CheckForPromotion(Move newMove)
    {
        if(newMove.movedPiece.pieceType == PieceType.pawn)
        {
            //if(newMove.destinationSquare.rank == GameBoardData.whitePawnPromotionRank &&
            //    newMove.movedPiece.isWhite ||
            //    newMove.destinationSquare.rank == GameBoardData.blackPawnPromotionRank &&
            //    !newMove.movedPiece.isWhite)

            if(newMove.movedPiece.isWhite && newMove.destinationSquare.topMid == null || 
                !newMove.movedPiece.isWhite && newMove.destinationSquare.bottomMid == null)
            {
                this.promotingPawn = newMove.movedPiece;
                return true;
            }
        }
        return false;
    }

    private async Task HandlePromotion(Move newMove)
    {
        selectionCompleted = false;
            
        InitiatePromotionSequence(newMove);

        await WaitForPromotionSelection();

        
        selectionCompleted = true;
        PromotePawn(newMove);
    }

    private Task WaitForPromotionSelection()
    {
        promotionTaskCompletionSource = new TaskCompletionSource<bool>();

        canvas.OnPromotionSelected += OnPromotionSelected;
        return promotionTaskCompletionSource.Task;
    }

    private void OnPromotionSelected()
    {
        canvas.OnPromotionSelected -= OnPromotionSelected;
        promotionTaskCompletionSource.TrySetResult(true);
    }

    private void InitiatePromotionSequence(Move newMove)
    {
        canvas.OpenPromotingPanel();
    }


    public void SetPromotionPieceName(string name)
    {
        promotionPieceName = name;
        selectionCompleted = true;
    }

    public void PromotePawn(Move newMove)
    {
        if (newMove.movedPiece.isWhite)
        {
            promotionPieceName = "white" + promotionPieceName;
        }
        else
        {
            promotionPieceName = "black" + promotionPieceName;
        }

        ExecuteMoveOnBoard(newMove);
        GameBoardData.fiftyMoveRuleCounter = 0;

        GameObject piecePrefab = pieceHolder.GetPiece(promotionPieceName);
        if (piecePrefab == null)
        {
            Debug.Log("error while receiving prefab");
            return;
        }
        Piece newPiece = Instantiate(piecePrefab, newMove.destinationSquare.gameObject.transform.position, Quaternion.identity).GetComponent<Piece>();
        GameBoardData.pieces.Add(newPiece.gameObject);

        // swapping of the square with piece relationship
        newPiece.SetCurrentSquare(promotingPawn.currentSquare);
        newMove.destinationSquare.SetCurrentPiece(newPiece);

        Destroy(promotingPawn.gameObject);
        promotingPawn = null;
        selectionCompleted = false;
        //promotionPieceName = "";
        canvas.ClosePromotionPanel();
    }


    private bool HandleCastling(Move newMove)
    {
        if (King.castlingFlag)
        {
            Move rookMove = null;
            if(newMove.movedPiece.currentSquare.midRight.GetCurrentPiece() != null &&
                newMove.movedPiece.currentSquare.midRight.GetCurrentPiece().pieceType == PieceType.rook)
            {
                rookMove = new Move(newMove.movedPiece.currentSquare.midRight,
                                    newMove.movedPiece.currentSquare.midLeft,
                                    newMove.movedPiece.currentSquare.midRight.GetCurrentPiece());
                
            }
            else if(newMove.movedPiece.currentSquare.midLeft.GetCurrentPiece() != null &&
                    newMove.movedPiece.currentSquare.midLeft.GetCurrentPiece().pieceType == PieceType.rook)
            {
                rookMove = new Move(newMove.movedPiece.currentSquare.midLeft,
                                    newMove.movedPiece.currentSquare.midRight,
                                    newMove.movedPiece.currentSquare.midLeft.GetCurrentPiece());
            }

            if(rookMove != null) { 
                ExecuteMoveOnBoard(rookMove);
            }
            King.castlingFlag = false;
            return true;
        }

        King.castlingFlag = false;
        return false;
    }

    private bool ValidateMoveRequest(Move newMove)
    {
        if (newMove.originSquare == null || newMove.destinationSquare == null || newMove.movedPiece == null)
        {
            return false;
        }
        Debug.Log(newMove.movedPiece.name + " requests move: " + newMove.originSquare.file + "-" + newMove.originSquare.rank + "...to..." + newMove.destinationSquare.file + "-" + newMove.destinationSquare.rank);
        return true;
    }

    private bool CheckForCorrectTurn(Move newMove) { 
        if(newMove.movedPiece.isWhite == GameBoardData.whiteToMove)
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    private bool MovedOwnColorPiece(Move newMove)
    {
        if(newMove.movedPiece.isWhite == GameData.GetBoardLayout().whitePov)
        {
            return true;
        }
        Debug.LogError("wrong color tried to move!");
        return false;
    }

    private void ResetMoveInformation()
    {
        promotionPieceName = "";
    }

    private void RegisterMove(Move newMove)
    {
        GameBoardData.moves.Add(newMove);
        GameBoardData.whiteToMove = !GameBoardData.whiteToMove;
        GameBoardData.boardPositions.Add(new BoardPosition());
    }

    public void LeaveGame()
    {
        multiplayerManager.LeaveRoom();
    }

}



public class Move
{
    public PlaySquare originSquare;
    public PlaySquare destinationSquare;

    public Piece movedPiece;

    public Move(PlaySquare oriSquare, PlaySquare destSquare, Piece movedPiece)
    {
        originSquare = oriSquare;
        destinationSquare = destSquare;
        this.movedPiece = movedPiece;
    }
}

[System.Serializable]
public class MoveInstruction
{
    public int originSquareFile;
    public int originSquareRank;
    public int destinationSquareFile;
    public int destinationSquareRank;

    public bool castlingMove;

    public bool promotionMove;
    public string promotionPieceType;

    public MoveInstruction CreateMoveInformation(Move move, bool promotionMove, string promotionPieceType, bool castling)
    {
        originSquareFile = move.originSquare.file;
        originSquareRank = move.originSquare.rank;
        destinationSquareFile = move.destinationSquare.file;
        destinationSquareRank = move.destinationSquare.rank;
        castlingMove = castling;

        this.promotionMove = promotionMove;
        this.promotionPieceType = promotionPieceType;

        return this;
    }

    public string GetSerializedMoveInstruction()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static MoveInstruction GetDeserializedMoveInstruction(string serializedMoveInstruction)
    {
        return JsonConvert.DeserializeObject<MoveInstruction>(serializedMoveInstruction);
    }

}

public class BoardPosition
{
    // array of squares that a given boardlayout has. each square has a value for the piece that sits on it. Since the squares of the gameBoardData are not to be changed,
    // they are supposed to remain in the same order
    private int[] occupations;
    private int pieceCount;
    public BoardPosition()
    {
        occupations = new int[GameBoardData.squares.Count-1];
        pieceCount = GameBoardData.squares.Count;

        for (int i = 0; i < occupations.Length; i++)
        {
            if (GameBoardData.squares[i].GetComponent<PlaySquare>().GetCurrentPiece() != null)
            {
                occupations[i] = GameBoardData.squares[i].GetComponent<PlaySquare>().GetCurrentPiece().GetPieceIdentifier();
            }
            else
            {
                pieceCount -= 1;
            }
        }
    }

    // returns true if boardposition is the same
    public bool CompareBoardPosition(BoardPosition other)
    {
        if (pieceCount != other.pieceCount)
        {
            return false;
        }
        for (int i = 0; i < occupations.Length; i++)
        {
            if (occupations[i] != other.occupations[i])
            {
                return false;
            }
        }
        return true;
    }
}
