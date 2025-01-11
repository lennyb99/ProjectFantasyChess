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

    [Header("Room Panel View")]
    public GameObject multiplayerPanel;
    public TMP_InputField createRoomInput;
    public TMP_InputField findRoomInput;
    public TMP_Dropdown boardLayoutDropdown;

    [Header("Room Panel View")]
    public GameObject roomPanel;
    public List<TMP_Text> playerPanelTexts;
    public TMP_Text roomNamePanel;

    // Start is called before the first frame update
    void Start()
    {
        multiplayer = MultiplayerManager.Instance;
        app = AppManager.Instance;
        currentPanel = startMenuPanel;

        RegisterToMultiplayerManager();
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
        multiplayer.StartServerConnection();
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
        SwitchToPanelView(roomPanel);
    }

    public void SelectStartMatch()
    {
        if(app.selectedBoardLayout != null) { 
            GameData.SetBoardLayout(app.selectedBoardLayout);
            SceneManager.LoadScene("Playground");
        }
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
            List<string> boardNames = new List<string>();
            app.GetBoardLayouts().ForEach(layout =>
            {
                boardNames.Add(layout.Item1);
            });
            FillDropDown(boardLayoutDropdown, boardNames);
        }
    }

    public void UpdateRoomPanelInformation(List<string> playernames=null, string roomName="default")
    {
        if (!roomPanel.activeSelf)
        {
            return;
        }

        if (playernames != null)
        {
            for (int i = 0; i < playernames.Count; i++)
            {
                playerPanelTexts[i].text = playernames[i];
            }
        }

        if (roomName != null)
        {
            roomNamePanel.text = roomName;
        }
    }

    private void FillDropDown(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }
    
    
}
