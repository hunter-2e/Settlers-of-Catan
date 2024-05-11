using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class CardHub : MonoBehaviour
{
    public int startingCardAmount;
    public bool showEmpty;

    public Dictionary<Type, int> resourceAndAmount = new Dictionary<Type, int>();

    protected TextMeshProUGUI woodCount, brickCount, sheepCount, wheatCount, stoneCount;
    private GameObject woodObj, brickObj, sheepObj, wheatObj, stoneObj;
    public Button woodButton, brickButton, sheepButton, wheatButton, stoneButton;

    private void Start() {
        woodObj = transform.GetComponentInChildren<Wood>(true).gameObject;
        brickObj = transform.GetComponentInChildren<Brick>(true).gameObject;
        sheepObj = transform.GetComponentInChildren<Sheep>(true).gameObject;
        wheatObj = transform.GetComponentInChildren<Wheat>(true).gameObject;
        stoneObj = transform.GetComponentInChildren<Stone>(true).gameObject;

        woodCount = transform.GetComponentInChildren<Wood>(true).transform.GetComponentInChildren<TextMeshProUGUI>(true);
        brickCount = transform.GetComponentInChildren<Brick>(true).transform.GetComponentInChildren<TextMeshProUGUI>(true);
        sheepCount = transform.GetComponentInChildren<Sheep>(true).transform.GetComponentInChildren<TextMeshProUGUI>(true);
        wheatCount = transform.GetComponentInChildren<Wheat>(true).transform.GetComponentInChildren<TextMeshProUGUI>(true);
        stoneCount = transform.GetComponentInChildren<Stone>(true).transform.GetComponentInChildren<TextMeshProUGUI>(true);
        
        resourceAndAmount.Add(typeof(Wood), startingCardAmount);
        resourceAndAmount.Add(typeof(Brick), startingCardAmount);
        resourceAndAmount.Add(typeof(Sheep), startingCardAmount);
        resourceAndAmount.Add(typeof(Wheat), startingCardAmount);
        resourceAndAmount.Add(typeof(Stone), startingCardAmount);

        UpdateCardAmounts();
        ToggleCards();
    }


    private void UpdateCardAmounts() {
        woodCount.text = resourceAndAmount[typeof(Wood)].ToString();
        brickCount.text = resourceAndAmount[typeof(Brick)].ToString();
        sheepCount.text = resourceAndAmount[typeof(Sheep)].ToString();
        wheatCount.text = resourceAndAmount[typeof(Wheat)].ToString();
        stoneCount.text = resourceAndAmount[typeof(Stone)].ToString();

        Debug.Log(resourceAndAmount[typeof(Wood)]);
    }

    public void ToggleCards() {
        if (!showEmpty) {
            woodObj.SetActive(resourceAndAmount[typeof(Wood)] > 0);
            brickObj.SetActive(resourceAndAmount[typeof(Brick)] > 0);
            sheepObj.SetActive(resourceAndAmount[typeof(Sheep)] > 0);
            wheatObj.SetActive(resourceAndAmount[typeof(Wheat)] > 0);
            stoneObj.SetActive(resourceAndAmount[typeof(Stone)] > 0);
        }
    }

    public void ReceiveCard(Type resource) {
        resourceAndAmount[resource] += 1;
        UpdateCardAmounts();
        ToggleCards();

        Debug.Log("Recieved " + resource);
    }
    public void ReceiveCard(List<Type> resources) {
        foreach(Type resource in resources) {
            ReceiveCard(resource);
        }
    }
    public void LoseCard(Type resource) {
        resourceAndAmount[resource] -= 1;
        UpdateCardAmounts();
        ToggleCards();
    }

    public int GetResourceAmount(Type resource) {
        return resourceAndAmount[resource];
    }
    public void LoseCard(List<Type> resources) {
        foreach(Type resource in resources) {
            LoseCard(resource);
        }
    }

}
