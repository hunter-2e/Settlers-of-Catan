using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class CardHub : MonoBehaviour
{
    public int startingCardAmount;

    public Dictionary<Type, int> resourceAndAmount = new Dictionary<Type, int>();
    TextMeshProUGUI sheepCount, brickCount, wheatCount, stoneCount, woodCount;
    private void Start() {
        sheepCount = transform.GetComponentInChildren<Sheep>().transform.GetComponentInChildren<TextMeshProUGUI>();
        brickCount = transform.GetComponentInChildren<Brick>().transform.GetComponentInChildren<TextMeshProUGUI>();
        wheatCount = transform.GetComponentInChildren<Wheat>().transform.GetComponentInChildren<TextMeshProUGUI>();
        stoneCount = transform.GetComponentInChildren<Stone>().transform.GetComponentInChildren<TextMeshProUGUI>();
        woodCount = transform.GetComponentInChildren<Wood>().transform.GetComponentInChildren<TextMeshProUGUI>();

        resourceAndAmount.Add(typeof(Sheep), startingCardAmount);
        resourceAndAmount.Add(typeof(Brick), startingCardAmount);
        resourceAndAmount.Add(typeof(Wheat), startingCardAmount);
        resourceAndAmount.Add(typeof(Stone), startingCardAmount);
        resourceAndAmount.Add(typeof(Wood), startingCardAmount);

        UpdateCardAmounts();
    }


    private void UpdateCardAmounts() {
        sheepCount.text = resourceAndAmount[typeof(Sheep)].ToString();
        brickCount.text = resourceAndAmount[typeof(Brick)].ToString();
        wheatCount.text = resourceAndAmount[typeof(Wheat)].ToString();
        stoneCount.text = resourceAndAmount[typeof(Stone)].ToString();
        woodCount.text = resourceAndAmount[typeof(Wood)].ToString();
        Debug.Log(resourceAndAmount[typeof(Wood)]);
    }

    public void ReceiveCard(Type resource) {
        resourceAndAmount[resource] += 1;
        UpdateCardAmounts();
    }
    public void ReceiveCard(List<Type> resources) {
        foreach(Type resource in resources) {
            ReceiveCard(resource);
        }
    }
    public void LoseCard(Type resource) {
        resourceAndAmount[resource] -= 1;
        UpdateCardAmounts();
    }
    public void LoseCard(List<Type> resources) {
        foreach(Type resource in resources) {
            LoseCard(resource);
        }
    }

}
