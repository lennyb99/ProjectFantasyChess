using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public MenuManager menuManager;
    public static MultiplayerManager Instance { get; private set; }

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartServerConnection()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to photon..");
        if(PhotonNetwork.NickName != null) { 
            PhotonNetwork.NickName = "hans";
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to masterserver");
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void FindRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        Debug.Log("searching for room");
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
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        Debug.Log(RetrievePlayerNamesFromCurrentRoom().Count);
        menuManager.UpdateRoomPanelInformation(playernames:RetrievePlayerNamesFromCurrentRoom(), roomName:PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        menuManager.UpdateRoomPanelInformation(playernames: RetrievePlayerNamesFromCurrentRoom());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        menuManager.UpdateRoomPanelInformation(playernames: RetrievePlayerNamesFromCurrentRoom());
    }

    private List<string> RetrievePlayerNamesFromCurrentRoom()
    {
        List<string> playerNames = new List<string>();
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            
            playerNames.Add(player.Value.NickName);
        }
        return playerNames; 
    }

    

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        menuManager.UpdateRoomPanelInformation(playernames: RetrievePlayerNamesFromCurrentRoom(), roomName: PhotonNetwork.CurrentRoom.Name.ToString());
    }



}
