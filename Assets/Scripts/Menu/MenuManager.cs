using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private MultiplayerManager multiplayer;
    private AppManager app;

    private GameObject currentPanel;

    

    public GameObject startMenuPanel;
    public GameObject selectModePanel;

    [Header("Multiplayer screen View")]
    public GameObject multiplayerPanel;
    public TMP_InputField createRoomInput;
    public TMP_InputField findRoomInput;
    public TMP_Dropdown boardLayoutDropdown;

    [Header("Login Screen View")]
    public GameObject accountScreenPanel;
    public GameObject createAccountPanel;
    public GameObject loginAccountPanel;
    public TMP_InputField loginUsernameInput;
    public TMP_InputField loginPasswordInput;
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerPasswordInput;
    public TMP_Text usernameLabel;


    [Header("Room Panel View")]
    public GameObject roomPanel;
    public List<UIPlayerPanel> playerPanels;
    public TMP_Text roomNamePanel;

    [Header("Create Room Panel View")]
    public GameObject createRoomPanel;


    [Header("Find Room Panel View")]
    public GameObject findRoomPanel;

    [Header("Notice Boards")]
    public GameObject mediumNoticeBoard;
    public TMP_Text mediumNoticeBoardText;

    // Start is called before the first frame update
    void Start()
    {
        multiplayer = MultiplayerManager.Instance;
        app = AppManager.Instance;
        currentPanel = startMenuPanel;

        RegisterToMultiplayerManager();

        if(!multiplayer.loggedIn)
        {
            SwitchToPanelView(accountScreenPanel);
        }
        else
        {
            startMenuPanel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RegisterToMultiplayerManager()
    {
        multiplayer= GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>();
        if (multiplayer != null)
        {
           multiplayer.menuManager = this;
        }
        else
        {
            Debug.Log("CRITICAL ERROR. Menu Manager not found");
        }
    }

    public void SelectStartApp()
    {
        SwitchToPanelView(selectModePanel);
        
    }

    public void SelectMultiplayerScreen()
    {
        SwitchToPanelView(multiplayerPanel);
        UpdateMultiplayerPanelInformation();
    }

    public void SelectCreateRoom()
    {
        multiplayer.CreateRoom(createRoomInput.text);
        SwitchToPanelView(roomPanel);

        app.SelectBoardLayout(app.GetBoardLayout(boardLayoutDropdown.options[boardLayoutDropdown.value].text));
    }

    public void SelectFindRoom()
    {
        multiplayer.FindRoom(findRoomInput.text);
        SwitchToPanelView(roomPanel);
    }

    public void SelectEditMode()
    {
        SceneManager.LoadScene("EditMode");
    }

    public void SelectStartMatch()
    {
        if (multiplayer.IsPlayerMasterClient())
        {
            multiplayer.SendLobbyStart();
        }
        else
        {
            if (!mediumNoticeBoard.activeSelf)
            {
                StartCoroutine(SendNotice("Only the Lobby Leader can start the match.",3f));
            }
        }
    }

    IEnumerator SendNotice(string notice, float seconds)
    {
        mediumNoticeBoardText.text = notice;
        mediumNoticeBoard.SetActive(true);
        yield return new WaitForSeconds(seconds);
        mediumNoticeBoard.SetActive(false);
    }

    public void SelectExitRoom()
    {
        SwitchToPanelView(multiplayerPanel);
        multiplayer.LeaveRoom();
    }

    public void SelectBackToMenu()
    {
        SwitchToPanelView(selectModePanel);
    }

    public void ExecuteStartMatch()
    {
        if (app.selectedBoardLayout != null)
        {
            GameData.SetBoardLayout(app.selectedBoardLayout);
            SceneManager.LoadScene("Playground");
        }
    }

    public void OpenLoginPanelView()
    {
        loginAccountPanel.SetActive(true);
    }

    public void CloseLoginPanelView()
    {
        loginAccountPanel.SetActive(false);
    }

    public void OpenRegisterPanelView()
    {
        createAccountPanel.SetActive(true);
    }

    public void CloseRegisterPanelView()
    {
        createAccountPanel.SetActive(false);
    }

    public void SelectLoginAccount()
    {
        string usernameInput = loginUsernameInput.text;
        string passwordInput = loginPasswordInput.text;

        StartCoroutine(DjangoBackendAPI.Login(usernameInput, passwordInput, (success, response) =>
        {
            if(success)
            {
                Debug.Log("logged in successfully");
                usernameLabel.text = usernameInput;
                multiplayer.SetUsername(usernameInput);
                CloseLoginPanelView();
                SwitchToPanelView(selectModePanel);
            }
            else
            {
                if (!mediumNoticeBoard.activeSelf)
                {
                    StartCoroutine(SendNotice("Login failed.", 2f));
                }
            }
        }));
    }

    public void SelectRegisterAccount()
    {
        string registerInput = registerUsernameInput.text;
        string passwordInput = registerPasswordInput.text;

        if(registerInput == "" || passwordInput == "")
        {
            if (!mediumNoticeBoard.activeSelf)
            {
                StartCoroutine(SendNotice("Please fill out both fields!", 2f));
            }
            return;
        }

        StartCoroutine(DjangoBackendAPI.RegisterUser(registerInput, passwordInput, (success, response) =>
        {
        if (success)
            {
                Debug.Log("registered successfully");

                if (!mediumNoticeBoard.activeSelf)
                {

                    StartCoroutine(SendNotice("Created Account successfully! Please log in.", 1.5f));

                }

                //DjangoBackendAPI.Login(registerInput, passwordInput, (LoginSuccess, LoginResponse) =>
                //{
                //    Debug.Log("handling login..");
                //    if (LoginSuccess)
                //    {
                //        multiplayer.SetUsername(registerInput);
                //        usernameLabel.text = registerInput;
                //        if (!mediumNoticeBoard.activeSelf)
                //        {

                //            StartCoroutine(SendNotice("Created Account successfully!", 2f));

                //        }
                //        SwitchToPanelView(selectModePanel);
                //    }
                //    else
                //    {
                //        if (!mediumNoticeBoard.activeSelf)
                //        {
                //            StartCoroutine(SendNotice("Account created, try to log in.",2f));
                //        }
                //    }
                //    CloseRegisterPanelView();
                //});
            }
            else
            {
                if (!mediumNoticeBoard.activeSelf)
                {
                    StartCoroutine(SendNotice("Error occured while creating account, please try again.", 2f));
                }
            }
        }));

        
    }


    public void SwitchToPanelView( GameObject newPanel)
    {
        newPanel.SetActive(true);
        if(currentPanel != null) { 
            currentPanel.SetActive(false);
        }
        currentPanel = newPanel;
    }

    public void UpdateMultiplayerPanelInformation()
    {
        if (multiplayerPanel.activeSelf)
        {
            StartCoroutine(app.GetBoardLayouts(layouts =>
            {
                List<string> boardNames = new List<string>();
                Debug.Log("boardlayouts:" + layouts.Count);

                foreach (var layout in layouts)
                {
                    boardNames.Add(layout.Item1);
                }

                FillDropDown(boardLayoutDropdown, boardNames);
            }));
        }
    }

    public void OpenCreateRoomPanelView()
    {
        createRoomPanel.SetActive(true);
    }

    public void OpenFindRoomPanelView()
    {
        findRoomPanel.SetActive(true);
    }

    public void CloseCreateRoomPanelView()
    {
        createRoomPanel.SetActive(false);
    }

    public void CloseFindRoomPanelView()
    {
        findRoomPanel.SetActive(false);
    }
    public void UpdateRoomPanelInformation(string roomName=null)
    {
        if (!roomPanel.activeSelf)
        {
            return;
        }

        

        if (roomName != null)
        {
            roomNamePanel.text = roomName;
        }

    }

    public void UpdateUIPlayerpanels(List<string> playerid = null)
    {
        if (!roomPanel.activeSelf)
        {
            return;
        }

        if (playerid != null)
        {
            for (int i = 0; i < playerid.Count; i++)
            {
                playerPanels[i].UpdatePlayerID(playerid[i]);
                playerPanels[i].SetPlayerNumber(i);
            }
        }
    }


    private void FillDropDown(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public List<string> GetAllWhiteUserIds()
    {
        List<string> ids = new List<string>();
        foreach (UIPlayerPanel playerPan in playerPanels)
        {
            if (playerPan.isWhite)
            {
                ids.Add(playerPan.playerId);
            }
        }
        return ids;
    }

    public List<string> GetAllBlackUserIds()
    {
        List<string> ids = new List<string>();
        foreach (UIPlayerPanel playerPan in playerPanels)
        {
            if (!playerPan.isWhite)
            {
                ids.Add(playerPan.playerId);
            }
        }
        return ids;
    }

    public void ChangeTeamIcon(string id, bool isWhite)
    {
        foreach (UIPlayerPanel playerPan in playerPanels)
        {
            if (id == playerPan.playerId)
            {
                playerPan.ToggleTeamImage();
            }
        }
    }


    public bool IsWhitePovSelected()
    {
        return playerPanels[0].isWhite;
    }
    
}
