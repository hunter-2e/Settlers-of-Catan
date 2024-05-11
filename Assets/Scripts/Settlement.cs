using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Settlement : MonoBehaviour
{
    public PlayerHub playerCardHub;

    private void Start() {
        PlayerHub[] cardHubs = FindObjectsOfType<PlayerHub>();

        foreach(PlayerHub hub in cardHubs) {
            playerCardHub = hub;
        }
    }
}
