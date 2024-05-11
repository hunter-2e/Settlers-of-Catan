using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDistributor : MonoBehaviour {
    public Dictionary<int, GameObject> numberTile = new Dictionary<int, GameObject>();
    public Bank bank; // Reference to the ResourceBank script

    public void DistributedRolledResources(int rolledNumber) {
        if (numberTile.ContainsKey(rolledNumber)) {
            TileGeometryTracker tileGeometryTracker = numberTile[rolledNumber].GetComponent<TileGeometryTracker>();

            foreach (GameObject corner in tileGeometryTracker.tileCorner) {
                Settlement settlement = corner.GetComponentInChildren<Settlement>();
                City city = corner.GetComponentInChildren<City>();
                if (settlement != null) {
                    if (numberTile[rolledNumber].TryGetComponent(out Resource resource)) {
                        // Pass the type of resource instead of the resource object
                        Debug.Log("HAPPENING");
                        Bank.instance.LoseCard(resource.GetType());
                        settlement.playerCardHub.ReceiveCard(resource.GetType());
                    }
                }
                if(city != null) {
                    if (numberTile[rolledNumber].TryGetComponent(out Resource resource)) {
                        // Pass the type of resource instead of the resource object
                        Debug.Log("HAPPENING");
                        Bank.instance.LoseCard(resource.GetType());
                        Bank.instance.LoseCard(resource.GetType());

                        settlement.playerCardHub.ReceiveCard(resource.GetType());
                        settlement.playerCardHub.ReceiveCard(resource.GetType());
                    }
                }
            }
        }
    }
}
