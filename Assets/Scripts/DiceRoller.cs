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

    public void RollDice() {
        int firstNumber = Random.Range(1, 7);
        int secondNumber = Random.Range(1, 7);

        firstDieText.text = firstNumber.ToString();
        secondDieText.text = secondNumber.ToString();

        resourceDistributor.DistributedRolledResources(firstNumber + secondNumber);
    }
}
