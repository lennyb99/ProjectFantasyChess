using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerPanel : MonoBehaviour
{
    public TMP_Text playerLabel;

    public Image swapTeamImage;
    public Sprite whitePawn;
    public Sprite blackPawn;
    public bool isWhite;
    

    public string playerId;
    public int playerNumber;

    private MultiplayerManager multiplayer;

    

    public void Start()
    {
        multiplayer = MultiplayerManager.Instance;
    }


    public void UpdatePlayerID(string playerId)
    {
        this.playerId = playerId;


        UpdatePlayerLabel();
    }

    public void SetPlayerNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
    }

    public void UpdatePlayerLabel()
    {
        playerLabel.text = multiplayer.GetPlayernameFromId(playerId);
    }

    public void ToggleTeam()
    {
        isWhite = !isWhite;

        ToggleTeamImage();

        multiplayer.SendTeamIconChange(playerId, isWhite);
    }

    public void ToggleTeamImage()
    {
        if (swapTeamImage.sprite == whitePawn)
        {
            swapTeamImage.sprite = blackPawn;
        }
        else
        {
            swapTeamImage.sprite = whitePawn;
        }
    }

}
