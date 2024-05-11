using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public DiceRoller diceRoller;
    public static TurnManager instance;
    List<PlayerHub> playerHubs = new List<PlayerHub>();

    public PlayerHub currentPlayerTurn = null;
    private int currentPlayerIndex = 0;
    public BuildMode currentBuildMode = BuildMode.Rolling;

    public enum BuildMode{ 
        CityMode,
        SettleMode,
        RoadMode,
        Rolling,
        Idle
    };

    void Start() {
        instance = this;

        PlayerHub[] playerHubsAndBank = GameObject.FindObjectsOfType<PlayerHub>(true);

        foreach (PlayerHub cardHub in playerHubsAndBank) {
            playerHubs.Add(cardHub);
        }

        BeginGame();
    }

    private void BeginGame() {
        GivePlayerTurn(playerHubs[currentPlayerIndex]);
    }

    public void PassTurn() {
        // Increment the current player index
        currentPlayerIndex++;

        // Wrap around the player index if it exceeds the number of players
        if (currentPlayerIndex >= playerHubs.Count) {
            currentPlayerIndex = 0;
        }

        // Give the turn to the next player
        GivePlayerTurn(playerHubs[currentPlayerIndex]);
    }

    private void GivePlayerTurn(PlayerHub playerHub) {
        currentPlayerTurn = playerHub;
        DisableOtherPlayersTurns();

        if(currentPlayerTurn.startingSettlements > 0) {
            diceRoller.DisableDieRoll();
            MakePlayerPlaceSettlementAndRoad();
        } else {
            DisableEndTurn();
            RollingMode();
        }
    }

    private void MakePlayerPlaceSettlementAndRoad() {
        DisableEndTurn();
        SettlementBuildMode();
    }

    // Elaborate on for photon view
    private void DisableOtherPlayersTurns() {
        foreach (PlayerHub playerHub in playerHubs) {
            if (playerHub != currentPlayerTurn) {
                playerHub.gameObject.SetActive(false);
            } else {
                playerHub.gameObject.SetActive(true);
            }
        }
    }
    private void DisableEndTurn() {
        currentPlayerTurn.endTurnButton.interactable = false;
    }
    public void EnableEndTurn() {
        currentPlayerTurn.endTurnButton.interactable = true;
    }

    public void SettlementBuildMode() {
        currentBuildMode = BuildMode.SettleMode;
    }
    public void CityBuildMode() {
        currentBuildMode = BuildMode.CityMode;
    }
    public void RoadBuildMode() {
        currentBuildMode = BuildMode.RoadMode;
    }
    public void RollingMode() {
        currentBuildMode = BuildMode.Rolling;
        DiceRoller.instance.EnableDieRoll();
    }
    public void IdleMode() {
        currentBuildMode = BuildMode.Idle;
        TurnManager.instance.EnableEndTurn();
    }

}
