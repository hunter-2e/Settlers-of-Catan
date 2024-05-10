using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Transform cardParent; // Parent transform for the cards
    public float cardSpacing = 10f; // Spacing between cards

    private bool isDragging = false;
    private Vector2 startPosition;

    public void OnDrag(PointerEventData eventData) {
        if (!isDragging) {
            isDragging = true;
            startPosition = transform.position;
        }

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        isDragging = false;

        // Snap the card to the nearest position
        SnapToNearestPosition();
    }

    private void SnapToNearestPosition() {
        Transform nearestCard = null;

        // Find the nearest card
        foreach (Transform card in cardParent) {
            RectTransform cardRect = card.GetComponent<RectTransform>();
            RectTransform thisRect = GetComponent<RectTransform>();

            if (thisRect != cardRect && RectOverlap(thisRect, cardRect)) {
                nearestCard = card;
                break;
            }
        }

        // If the released card is hovering over another card, swap their positions
        if (nearestCard != null) {
            Vector3 tempPosition = transform.position;
            transform.position = nearestCard.position;
            nearestCard.position = tempPosition;
        }

        // Reorder cards
        ReorderCards();
    }

    private bool RectOverlap(RectTransform rectTrans1, RectTransform rectTrans2) {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);
        return rect1.Overlaps(rect2);
    }

    private void ReorderCards() {
        // Sort cards by x position
        int childCount = cardParent.childCount;
        Transform[] cards = new Transform[childCount];
        for (int i = 0; i < childCount; i++) {
            cards[i] = cardParent.GetChild(i);
        }

        // Sort the cards based on their x position
        System.Array.Sort(cards, (x, y) => x.position.x.CompareTo(y.position.x));

        // Reposition cards with spacing
        float xOffset = 0f;
        for (int i = 0; i < childCount; i++) {
            Vector3 newPosition = new Vector3(cardParent.position.x + xOffset, cardParent.position.y, cardParent.position.z);
            cards[i].position = newPosition;
            xOffset += cards[i].GetComponent<RectTransform>().sizeDelta.x + cardSpacing;

            // Update the sibling index to match the new order
            cards[i].SetSiblingIndex(i);
        }
    }
}
