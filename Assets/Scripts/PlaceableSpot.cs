using UnityEngine;

public class PlaceableSpot : MonoBehaviour {
    public GameObject objectToPlace; // The object to place when hovering

    private bool spotTaken = false;
    private GameObject placedObject; // The object placed at the location
    private SphereCollider sphereCollider;

    void Start() {
        // Ensure there's a SphereCollider attached to the GameObject
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null) {
            Debug.LogError("No SphereCollider found on this GameObject. Please attach a SphereCollider component.");
            enabled = false;
        }
    }

    void OnMouseEnter() {
        // Place the object when mouse enters the collider
        PlaceObject();
    }

    void OnMouseExit() {
        // Remove the object when mouse exits the collider, if not confirmed
        RemoveObject();
    }

    void OnMouseDown() {
        // Confirm the location when mouse clicks
        ConfirmPlacement();
    }

    void PlaceObject() {
        // If the object is not already placed and the spot is not taken, instantiate it at the collider position
        if (!spotTaken && placedObject == null && objectToPlace != null) {
            placedObject = Instantiate(objectToPlace, transform.position, Quaternion.identity);
        }
    }

    void RemoveObject() {
        // If object is placed, destroy it when mouse exits the collider
        if (placedObject != null) {
            Destroy(placedObject);
            placedObject = null;
        }
    }

    void ConfirmPlacement() {
        if (!spotTaken) {
            // Do something when the placement is confirmed, like keeping the object or triggering an event
            Debug.Log("Placement confirmed at: " + transform.position);
            Instantiate(objectToPlace, transform.position, Quaternion.identity, transform);

            spotTaken = true;

            // Toggle isTrigger based on spotTaken status
            sphereCollider.isTrigger = false;

            // Disable other nearby spots within the collider's range
            Collider[] colliders = Physics.OverlapSphere(transform.position, sphereCollider.radius);
            foreach (Collider col in colliders) {
                // Check if the collider belongs to a different GameObject and is a PlaceableSpot
                if (col.gameObject != gameObject && col.TryGetComponent(out PlaceableSpot otherSpot)) {
                    // Ensure we don't mark spots within the same object
                    if (otherSpot && !otherSpot.spotTaken) {
                        // Check if the other spot is within this spot's sphere collider
                        if (Vector3.Distance(transform.position, otherSpot.transform.position) <= .75f) {
                            otherSpot.spotTaken = true;
                        }
                    }
                }
            }
        }
    }
}
