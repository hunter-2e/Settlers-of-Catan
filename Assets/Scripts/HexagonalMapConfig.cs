using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "HexagonalMapConfig", menuName = "Custom/Hexagonal Map Configuration")]
public class HexagonalMapConfig : ScriptableObject {
    [Range(1, 10)]
    public int totalRings = 1;

    public List<GameObject> hexFields = new List<GameObject>();
    public List<int> numberFields = new List<int>();
}