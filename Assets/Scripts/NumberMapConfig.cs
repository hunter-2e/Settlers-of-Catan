using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberMapConfig : MonoBehaviour
{
    public GameObject baseNumberChip;

    public Dictionary<int, int> numberDots = new Dictionary<int, int>{ { 2,1}, { 3, 2 }, { 4, 3 },{ 5, 4 }, { 6, 5 }, { 8, 5 },{ 9, 4 },{ 10, 3 },{ 11, 2 },{ 12, 1 }  };

    public GameObject CreateChip(int number) {
        GameObject createdChip = Instantiate(baseNumberChip);
        TextMeshProUGUI chipText = createdChip.GetComponentInChildren<TextMeshProUGUI>();

        chipText.text = number.ToString() + "\n" + new string('.', numberDots[number]);
        if(number == 8 || number == 6) {
            chipText.color = Color.red;
        }

        return createdChip;
    }
}
