using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Settlement : MonoBehaviour
{
    public CardHub playerCardHub;

    private void Start() {
        CardHub[] cardHubs = FindObjectsOfType<CardHub>();

        foreach(CardHub hub in cardHubs) {
            if(hub as Bank == false) {
                playerCardHub = hub;
            }
        }
    }
}
