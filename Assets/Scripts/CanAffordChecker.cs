using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class CanAffordChecker : MonoBehaviour {
    [SerializeField]
    private int woodNeeded, stoneNeeded, brickNeeded, wheatNeeded, sheepNeeded;

    private Button button;
    private bool canAfford = false;

    private void Start() {
        button = GetComponent<Button>();
    }

    public void UpdateAffordAbility() {
        if(button == null) {
            button = GetComponent<Button>();
        }
        if (TurnManager.instance.currentPlayerTurn == null) {
            Debug.Log("current player null");
        }else if (TurnManager.instance == null) {
            Debug.Log("turnmanager null");

        }else if(TurnManager.instance.currentPlayerTurn.resourceAndAmount.Count == 0) {
            Debug.Log("EMPTY DICT");
        }
        int woodOwned = TurnManager.instance.currentPlayerTurn.resourceAndAmount[typeof(Wood)];
        int wheatOwned = TurnManager.instance.currentPlayerTurn.resourceAndAmount[typeof(Wheat)];
        int stoneOwned = TurnManager.instance.currentPlayerTurn.resourceAndAmount[typeof(Stone)];
        int brickOwned = TurnManager.instance.currentPlayerTurn.resourceAndAmount[typeof(Brick)];
        int sheepOwned = TurnManager.instance.currentPlayerTurn.resourceAndAmount[typeof(Sheep)];

        canAfford = false;
        if(gameObject.name == "City Button") {
            Debug.Log("Sheep needed: " + sheepNeeded);
            Debug.Log("Sheep owned: " + sheepOwned);

            if(wheatOwned >= wheatNeeded && stoneOwned >= stoneNeeded && sheepOwned >= sheepNeeded && brickOwned >= brickNeeded && woodOwned >= woodNeeded) {
                canAfford = true;
            }
        }

        button.interactable = canAfford;

    }
}
