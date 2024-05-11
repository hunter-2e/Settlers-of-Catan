using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        int woodOwned = TurnManager.instance.currentPlayerTurn.resourceAndAmount[typeof(Wood)];
        int wheatOwned = TurnManager.instance.currentPlayerTurn.resourceAndAmount[typeof(Wheat)];
        int stoneOwned = TurnManager.instance.currentPlayerTurn.resourceAndAmount[typeof(Stone)];
        int brickOwned = TurnManager.instance.currentPlayerTurn.resourceAndAmount[typeof(Brick)];
        int sheepOwned = TurnManager.instance.currentPlayerTurn.resourceAndAmount[typeof(Sheep)];
        canAfford = true;

        if (woodOwned < woodNeeded) {
            canAfford = false;
            Debug.Log("You need " + (woodNeeded - woodOwned) + " more wood.");
        }

        if (stoneOwned < stoneNeeded) {

        if (wheatOwned < wheatNeeded) {
            canAfford = false;
            Debug.Log("You need " + (wheatNeeded - wheatOwned) + " more wheat.");
        }
            canAfford = false;
            Debug.Log("You need " + (stoneNeeded - stoneOwned) + " more stone.");
        }

        if (brickOwned < brickNeeded) {
            canAfford = false;
            Debug.Log("You need " + (brickNeeded - brickOwned) + " more brick.");
        }

        if (sheepOwned < sheepNeeded) {
            canAfford = false;
            Debug.Log("You need " + (sheepNeeded - sheepOwned) + " more sheep.");
        }

        button.interactable = canAfford;

    }
}
