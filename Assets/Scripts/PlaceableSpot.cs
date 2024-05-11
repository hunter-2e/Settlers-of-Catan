using UnityEngine;

public class PlaceableSpot : MonoBehaviour {

    public float maxDistance = 0.5f; // Maximum distance to check for taken spots

    public GameObject objectToPlace; // The object to place when hovering

    public bool spotTaken = false;
    public bool spotPlaceable = true;
    public PlayerHub playersBuilding;
    public GameObject placedObject; // The object placed at the location
    public SphereCollider sphereCollider;

    void Start() {
        // Ensure there's a SphereCollider attached to the GameObject
        sphereCollider = GetComponentInChildren<SphereCollider>();
        if (sphereCollider == null) {
            Debug.LogError("No SphereCollider found on this GameObject. Please attach a SphereCollider component.");
            enabled = false;
        }
    }

    public virtual void OnMouseEnter() {
        // Place the object when mouse enters the collider
        if(TurnManager.instance.currentBuildMode == TurnManager.BuildMode.SettleMode) {
            PlaceObject();
        }
    }

    void OnMouseExit() {
        // Remove the object when mouse exits the collider, if not confirmed
        RemoveObject();
    }

    public virtual void OnMouseDown() {
        // Confirm the location when mouse clicks
        if (TurnManager.instance.currentBuildMode == TurnManager.BuildMode.SettleMode) {
            ConfirmPlacement();
        }
    }

    public virtual void PlaceObject() {
        // If the object is not already placed and the spot is not taken, instantiate it at the collider position
        if (spotPlaceable && placedObject == null && objectToPlace != null) {
            bool roadConnectingToSettlement = false;

            //TAKE OUT CONDITION FOR STARTING SETTLEMENT JUST FOR TESTING
            if (!spotTaken && spotPlaceable) {
                Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, maxDistance);
                foreach (Collider col in nearbyColliders) {
                    if (col.gameObject != gameObject && col.TryGetComponent(out RoadPlaceableSpot otherSpot)) {
                        if ((otherSpot.spotPlaceable && !otherSpot.spotTaken) || otherSpot.playersBuilding != TurnManager.instance.currentPlayerTurn ) {
                            roadConnectingToSettlement = false;
                        } else {
                            roadConnectingToSettlement = true;
                            break;
                        }
                    }
                }
            }

            if (!roadConnectingToSettlement && TurnManager.instance.currentPlayerTurn.startingSettlements < 1) {
                Debug.Log("NOT PLACEABLE! ROAD NOT CONNECTING TO SETTLEMENT");
                return;
            }

            placedObject = Instantiate(TurnManager.instance.currentPlayerTurn.settlement, transform.position, Quaternion.identity);
        }
    }

    void RemoveObject() {
        // If object is placed, destroy it when mouse exits the collider
        if (placedObject != null) {
            Destroy(placedObject);
            placedObject = null;
        }
    }

    public virtual void ConfirmPlacement() {
        bool roadConnectingToSettlement = false;

        //TAKE OUT CONDITION FOR STARTING SETTLEMENT JUST FOR TESTING
        if (!spotTaken && spotPlaceable) {
                Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, maxDistance);
                foreach (Collider col in nearbyColliders) {
                    if (col.gameObject != gameObject && col.TryGetComponent(out RoadPlaceableSpot otherSpot)) {
                        if ((otherSpot.spotPlaceable && !otherSpot.spotTaken) || otherSpot.playersBuilding != TurnManager.instance.currentPlayerTurn) {
                            roadConnectingToSettlement = false;
                        } else {
                            roadConnectingToSettlement = true;
                            break;
                        }
                    }
                }
            }

            if (!roadConnectingToSettlement && TurnManager.instance.currentPlayerTurn.startingSettlements < 1) {
            Debug.Log("NOT PLACEABLE! ROAD NOT CONNECTING TO SETTLEMENT");
            return; }


            // Do something when the placement is confirmed, like keeping the object or triggering an event
            Debug.Log("Placement confirmed at: " + transform.position);
            Instantiate(TurnManager.instance.currentPlayerTurn.settlement, transform.position, Quaternion.identity, transform);
            playersBuilding = TurnManager.instance.currentPlayerTurn;

            spotTaken = true;
            spotPlaceable = false;

            // Toggle isTrigger based on spotTaken status
            sphereCollider.isTrigger = false;

            // Disable other nearby spots within the collider's range
            Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance);
            foreach (Collider col in colliders) {
                // Check if the collider belongs to a different GameObject and is a PlaceableSpot
                if (col.gameObject != gameObject && col.TryGetComponent(out SettlementPlaceableSpot otherSpot)) {
                    // Ensure we don't mark spots within the same object
                    if (otherSpot && !otherSpot.spotTaken) {
                        // Check if the other spot is within this spot's sphere collider
                        otherSpot.spotPlaceable = false;
                        
                    }
                }
            }
        if (TurnManager.instance.currentPlayerTurn.startingSettlements == 0) {
            Bank.instance.BuySettlement();
            TurnManager.instance.RollingMode();
        } else {
            TurnManager.instance.currentPlayerTurn.startingSettlements--;
            TurnManager.instance.RoadBuildMode();
        }
        }
    }
