using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHub : CardHub
{
    public Button endTurnButton;
    public int startingSettlements = 2;
    public int startingRoads = 2;
    public GameObject settlement, city, road;
}
