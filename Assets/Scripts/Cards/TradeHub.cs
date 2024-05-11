using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeHub : MonoBehaviour
{
    public Button bankTradeButton;
    public Button playerTradeButton;
    public GameObject tradeBox;

    public CardHub bankCards;
    public CardHub infiniteCards;
    public CardHub takingCards;
    public CardHub givingCards;
    public CardHub playerCards;

    private static readonly Type[] resources = { typeof(Wood), typeof(Brick), typeof(Wheat), typeof(Sheep), typeof(Stone) };

    private void Start() {
        infiniteCards.woodButton.onClick.AddListener(delegate { MoveResource(typeof(Wood), null, takingCards); });
        infiniteCards.brickButton.onClick.AddListener(delegate { MoveResource(typeof(Brick), null, takingCards); });
        infiniteCards.sheepButton.onClick.AddListener(delegate { MoveResource(typeof(Sheep), null, takingCards); });
        infiniteCards.wheatButton.onClick.AddListener(delegate { MoveResource(typeof(Wheat), null, takingCards); });
        infiniteCards.stoneButton.onClick.AddListener(delegate { MoveResource(typeof(Stone), null, takingCards); });

        takingCards.woodButton.onClick.AddListener(delegate { MoveResource(typeof(Wood), takingCards, null); });
        takingCards.brickButton.onClick.AddListener(delegate { MoveResource(typeof(Brick), takingCards, null); });
        takingCards.sheepButton.onClick.AddListener(delegate { MoveResource(typeof(Sheep), takingCards, null); });
        takingCards.wheatButton.onClick.AddListener(delegate { MoveResource(typeof(Wheat), takingCards, null); });
        takingCards.stoneButton.onClick.AddListener(delegate { MoveResource(typeof(Stone), takingCards, null); });

        givingCards.woodButton.onClick.AddListener(delegate { MoveResource(typeof(Wood), givingCards, playerCards); });
        givingCards.brickButton.onClick.AddListener(delegate { MoveResource(typeof(Brick), givingCards, playerCards); });
        givingCards.sheepButton.onClick.AddListener(delegate { MoveResource(typeof(Sheep), givingCards, playerCards); });
        givingCards.wheatButton.onClick.AddListener(delegate { MoveResource(typeof(Wheat), givingCards, playerCards); });
        givingCards.stoneButton.onClick.AddListener(delegate { MoveResource(typeof(Stone), givingCards, playerCards); });

        playerCards.woodButton.onClick.AddListener(delegate { MoveResource(typeof(Wood), playerCards, givingCards); });
        playerCards.brickButton.onClick.AddListener(delegate { MoveResource(typeof(Brick), playerCards, givingCards); });
        playerCards.sheepButton.onClick.AddListener(delegate { MoveResource(typeof(Sheep), playerCards, givingCards); });
        playerCards.wheatButton.onClick.AddListener(delegate { MoveResource(typeof(Wheat), playerCards, givingCards); });
        playerCards.stoneButton.onClick.AddListener(delegate { MoveResource(typeof(Stone), playerCards, givingCards); });
    }

    /*
     * Enables or Disables the trade window.
     */
    public void ToggleTradeWindow() {
        playerTradeButton.gameObject.SetActive(!playerTradeButton.gameObject.activeSelf);
        bankTradeButton.gameObject.SetActive(!bankTradeButton.gameObject.activeSelf);
        tradeBox.SetActive(!tradeBox.activeSelf);

        bankTradeButton.interactable = false;
    }

    /*
     * Move card from one card hub to another.
     */
    public void MoveResource(Type resource, CardHub fromHub, CardHub toHub) {
        if (!tradeBox.activeSelf)
            return;

        if (fromHub)
            fromHub.LoseCard(resource);

        if (toHub)
            toHub.ReceiveCard(resource);

        CheckBankTradeValid();
    }

    /*
     * Check if the currently proposed trade is valid as a bank trade.
     * Activate / Deactivate the bank trade button accordingly.
     */ 
    private void CheckBankTradeValid() {
        bool isValid = true;
        int numResourcesGiving = 0;
        int numResourcesTaking = 0;
        int numCardsGiving = 0;
        int numCardsTaking = 0;

        foreach (Type resource in resources) {
            int bankResourceAmount = bankCards.GetResourceAmount(resource);
            int takingResourceAmount = takingCards.GetResourceAmount(resource);
            int givingResourceAmount = givingCards.GetResourceAmount(resource);

            if (bankResourceAmount < takingResourceAmount)
                isValid = false;

            if (givingResourceAmount > 0)
                numResourcesGiving++;

            if (takingResourceAmount > 0)
                numResourcesTaking++;

                numCardsGiving += givingResourceAmount;
            numCardsTaking += takingResourceAmount;
        }

        if (numCardsGiving != 4 || numCardsTaking != 1 || numResourcesGiving != 1 || numResourcesTaking != 1)
            isValid = false;

        if (isValid)
            bankTradeButton.interactable = true;
        else
            bankTradeButton.interactable = false;
    }

    /*
     * Called when the bank trade button is pressed. 
     * Attempts to execute the current trade to the bank.
     */
    public void BankTrade() {
        //TODO: Add harbor stuff
        
        foreach (Type resource in resources) {
            int takingResourceAmount = takingCards.GetResourceAmount(resource);
            int givingResourceAmount = givingCards.GetResourceAmount(resource);

            for (int i = 0; i < takingResourceAmount; i++) {
                MoveResource(resource, bankCards, playerCards);
                MoveResource(resource, takingCards, null);
            }

            for (int i = 0; i < givingResourceAmount; i++)
                MoveResource(resource, givingCards, bankCards);
        }

    }

    /*
     * Called when the player trade button is pressed. 
     * Attempts to propose the current trade to the other players.
     */
    public void PlayerTrade() {

    }

}
