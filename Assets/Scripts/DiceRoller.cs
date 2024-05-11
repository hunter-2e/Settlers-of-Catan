using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    public static DiceRoller instance;
    public Button diceButton;
    [SerializeField]
    private TextMeshProUGUI firstDieText, secondDieText;
    [SerializeField]
    private ResourceDistributor resourceDistributor;
    private CanAffordChecker[] canAffordChecks;
    private void Start() {
        instance = this;
        canAffordChecks = FindObjectsOfType<CanAffordChecker>(true);
        UpdateAffordabilities();
        MakeDieRollableOnce();
    }

    private void MakeDieRollableOnce() {
        diceButton.onClick.AddListener(DisableDieRoll);
    }

    public void EnableDieRoll() {
        diceButton.interactable = true;
    }
    public void DisableDieRoll() {
        diceButton.interactable = false;
        TurnManager.instance.IdleMode();
    }

    public void RollDice() {
        int firstNumber = Random.Range(1, 7);
        int secondNumber = Random.Range(1, 7);

        firstDieText.text = firstNumber.ToString();
        secondDieText.text = secondNumber.ToString();

        resourceDistributor.DistributedRolledResources(firstNumber + secondNumber);

        UpdateAffordabilities();
    }

    public void UpdateAffordabilities() {
        foreach (CanAffordChecker affordChecker in canAffordChecks) {
            affordChecker.UpdateAffordAbility();
        }
    }
}
