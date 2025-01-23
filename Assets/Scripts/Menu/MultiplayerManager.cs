using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Linq;
using System;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public MenuManager menuManager;
    public AppManager appManager;
    public GameManager gameManager;

    public PhotonView roomPhotonView;
    public static MultiplayerManager Instance { get; private set; }


    private string userId;

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
        StartServerConnection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartServerConnection()
    {
        string uniqueUserId = Guid.NewGuid().ToString();
        this.userId = uniqueUserId;
        PhotonNetwork.AuthValues = new AuthenticationValues(uniqueUserId);
        PhotonNetwork.NickName = $"Player_{uniqueUserId.Substring(0, 5)}";


        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to photon..");
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to masterserver");
    }

    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true,
            CleanupCacheOnLeave = true,
            PublishUserId = true
        };
        

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void FindRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(message);
        menuManager.SwitchToPanelView(menuManager.multiplayerPanel);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
        menuManager.SwitchToPanelView(menuManager.multiplayerPanel);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        menuManager.UpdateRoomPanelInformation(roomName:PhotonNetwork.CurrentRoom.Name);
        menuManager.UpdateUIPlayerpanels(playerid: RetrievePlayerIDsFromCurrentRoom());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        menuManager.UpdateUIPlayerpanels(playerid: RetrievePlayerIDsFromCurrentRoom());

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        menuManager.UpdateUIPlayerpanels(playerid: RetrievePlayerIDsFromCurrentRoom());
    }

    private List<string> RetrievePlayerIDsFromCurrentRoom()
    {
        List<string> playerIDs = new List<string>();

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            if(player.Value.UserId == this.userId)
            {
                playerIDs.Insert(0,player.Value.UserId);
            }
            else
            {
                playerIDs.Add(player.Value.UserId);
            }
            
        }

        return playerIDs;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        menuManager.UpdateRoomPanelInformation(roomName: PhotonNetwork.CurrentRoom.Name.ToString());
        menuManager.UpdateUIPlayerpanels(playerid: RetrievePlayerIDsFromCurrentRoom());
    }

    public void SendLobbyStart()
    {
        // Check for Lobby leader

        roomPhotonView.RPC("ReceiveLobbyStart", RpcTarget.All, StringCompresser.CompressString(appManager.GetCurrentSerializedBoardLayout()));
            
        
    }

    [PunRPC]
    public void ReceiveLobbyStart(byte[] serializedBoardLayout)
    {
        Debug.Log("starting game..");
        appManager.SetCurrentBoardLayout(StringCompresser.DecompressString(serializedBoardLayout), menuManager.IsWhitePovSelected());
        menuManager.ExecuteStartMatch();
    }

    public void SendTeamIconChange(string id, bool isWhite)
    {
        roomPhotonView.RPC("ReceiveTeamIconChange", RpcTarget.Others, id, isWhite);
    }

    [PunRPC]
    public void ReceiveTeamIconChange(string id, bool isWhite)
    {
        menuManager.ChangeTeamIcon(id, isWhite);
    }

    public void SendMoveInstruction(string moveInstr)
    {
        roomPhotonView.RPC("ReceiveMoveInstruction", RpcTarget.Others, moveInstr);
    }

    [PunRPC]
    public void ReceiveMoveInstruction(string serializedMoveInstruction)
    {
        gameManager.QueueOpponentTurn(serializedMoveInstruction);
    }

    public string GetPlayernameFromId(string id)
    {
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.UserId == id)
            {
                return player.Value.NickName;
            }
        }
        return null;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public bool IsPlayerMasterClient()
    {
        if(PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
