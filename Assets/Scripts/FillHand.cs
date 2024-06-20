using System.Collections.Generic;
using UnityEngine;

public sealed class FillHand : MonoBehaviour
{
    public GameObject CardPrefab; // The card prefab
    public List<GameObject> CardsInHand; // The list of cards in hand
    public float CardOffset; // The offset between cards
    public float YOffset; // The offset in the y direction to prevent overlap

    public void Start()
    {
        // Fill the hand with cards
        for (int i = 0; i < 12; i++)
        {
            GameObject card = Instantiate(CardPrefab, transform, false);
            card.transform.localPosition = new Vector3(CardOffset * (i - 5.5f), i * YOffset, -(i * YOffset / 2));
            card.transform.Rotate(Vector3.right, 40);
            CardsInHand.Add(card);

            // Set the reference to the hand in the CardController
            CardController cardController = card.GetComponent<CardController>();
            if (cardController != null)
            {
                cardController.CardsInHand = CardsInHand;
            }
        }
    }
}