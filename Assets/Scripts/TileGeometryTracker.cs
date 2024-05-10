using System;
using System.Collections.Generic;
using UnityEngine;

public class TileGeometryTracker : MonoBehaviour {
    public List<GameObject> tileCorner;
    public List<GameObject> tileEdges;
    public static Dictionary<Tuple<GameObject, GameObject>, GameObject> cornersEdges = new Dictionary<Tuple<GameObject, GameObject>, GameObject>();
}
