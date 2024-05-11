using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceRoller : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI firstDieText, secondDieText;
    [SerializeField]
    private ResourceDistributor resourceDistributor;
    private CanAffordChecker[] canAffordChecks;
    private void Start() {
        canAffordChecks = FindObjectsOfType<CanAffordChecker>(true);
    }
    public void RollDice() {
        int firstNumber = Random.Range(1, 7);
        int secondNumber = Random.Range(1, 7);

        firstDieText.text = firstNumber.ToString();
        secondDieText.text = secondNumber.ToString();

        resourceDistributor.DistributedRolledResources(firstNumber + secondNumber);

        foreach(CanAffordChecker affordChecker in canAffordChecks) {
            affordChecker.UpdateAffordAbility();
        }
    }
}
