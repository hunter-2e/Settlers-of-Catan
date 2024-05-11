using UnityEngine;

public class RoadPlaceableSpot : PlaceableSpot {

    public override void OnMouseEnter() {
        base.OnMouseEnter();
        Debug.Log("ENTERING ROUD SPOT");
    }

    public override void PlaceObject() {
        // If the object is not already placed and the spot is not taken, instantiate it at the collider position
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, maxDistance);
        foreach (Collider col in nearbyColliders) {
            if (col.gameObject != gameObject && col.TryGetComponent(out PlaceableSpot otherSpot)) {
                if (!otherSpot.spotPlaceable && otherSpot.spotTaken && otherSpot.playersBuilding == TurnManager.instance.currentPlayerTurn) {
                    // If a taken spot is found within the distance, confirm placement
                    Debug.Log("Placement confirmed at: " + transform.position);
                    placedObject = Instantiate(TurnManager.instance.currentPlayerTurn.road, transform.position, Quaternion.identity, transform);
                    placedObject.transform.localRotation = Quaternion.Euler(0, 0, 0);


                    return;
                }
            }
        }
    }

    public override void ConfirmPlacement() {
        if (!spotTaken) {
            // Check if there's another taken spot within a certain distance
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, maxDistance);
            foreach (Collider col in nearbyColliders) {
                if (col.gameObject != gameObject && col.TryGetComponent(out PlaceableSpot otherSpot)) {
                    if (!otherSpot.spotPlaceable && otherSpot.spotTaken && otherSpot.playersBuilding == TurnManager.instance.currentPlayerTurn) {
                        // If a taken spot is found within the distance, confirm placement
                        Debug.Log("Placement confirmed at: " + transform.position);
                        GameObject roadPlaced = Instantiate(TurnManager.instance.currentPlayerTurn.road, transform.position, Quaternion.identity, transform);
                        roadPlaced.transform.localRotation = Quaternion.Euler(0, 0, 0);

                        playersBuilding = TurnManager.instance.currentPlayerTurn;
                        spotTaken = true;
                        spotPlaceable = false;

                        if (TurnManager.instance.currentPlayerTurn.startingRoads < 1) {
                            Bank.instance.BuyRoad();
                        } else {
                            TurnManager.instance.currentPlayerTurn.startingRoads--;
                        }
                    }
                }
            }

        }
    }
}
