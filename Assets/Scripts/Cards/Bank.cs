using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Bank : CardHub {
    public static Bank instance;

    private void Awake() {
        instance = this;
    }

    public void BuySettlement() {
        List<Type> settlementResources = new List<Type>() { typeof(Wheat), typeof(Wood), typeof(Sheep), typeof(Brick) };
        TurnManager.instance.currentPlayerTurn.LoseCard(settlementResources);
        ReceiveCard(settlementResources);
    }

    public void BuyCity() {
        List<Type> cityResources = new List<Type>() { typeof(Stone), typeof(Stone), typeof(Stone), typeof(Wheat), typeof(Wheat) };
        TurnManager.instance.currentPlayerTurn.LoseCard(cityResources);
        ReceiveCard(cityResources);
    }

    public void BuyRoad() {
        List<Type> roadResources = new List<Type>() { typeof(Brick), typeof(Wood) };
        TurnManager.instance.currentPlayerTurn.LoseCard(roadResources);
        ReceiveCard(roadResources);
    }
}
