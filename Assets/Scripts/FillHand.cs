using System.Collections.Generic;
using UnityEngine;

public class FillHand : MonoBehaviour
{
    public GameObject cardPrefab; // The card prefab
    public List<GameObject> cardsInHand; // The list of cards in hand
    public float cardOffset; // The offset between cards
    public float yOffset; // The offset in the y direction to prevent overlap

    void Start()
    {
        // Fill the hand with cards
        for (int i = 0; i < 12; i++)
        {
            GameObject card = Instantiate(cardPrefab, transform, false);
            card.transform.localPosition = new Vector3(cardOffset * (i - 5.5f), i * yOffset, -(i * yOffset / 2));
            card.transform.Rotate(Vector3.right, 40);
            cardsInHand.Add(card);
            
            // Set the reference to the hand in the CardController
            CardController cardController = card.GetComponent<CardController>();
            if (cardController != null)
            {
                cardController.cardsInHand = cardsInHand;
            }
        }
    }
}