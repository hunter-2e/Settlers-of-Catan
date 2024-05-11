using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public static TurnManager instance;
    List<PlayerHub> playerHubs = new List<PlayerHub>();

    public PlayerHub currentPlayerTurn = null;
    private int currentPlayerIndex = 0;

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
}
