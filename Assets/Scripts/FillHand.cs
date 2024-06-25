#region

using JetBrains.Annotations;
using UnityEngine;

#endregion


/// <summary>
///     This class is responsible for filling the hand with cards in the game.
/// </summary>
public sealed class FillHand : MonoBehaviour
{
    // Offset values for positioning the cards in the hand
    private const float XOffset = 0.15f;
    private const float YOffset = 0.008f;
    private const float ZOffset = 0.002f;

    // The card prefab to be instantiated
    public GameObject CardPrefab;

    // The array of cards in the hand
    public Card[] Cards { get; private set; }

    /// <summary>
    ///     This method is called at the start of the game.
    ///     It calls the Fill method to fill the hand with cards.
    /// </summary>
    public void Start()
    {
        Fill();
    }

    /// <summary>
    ///     This method fills the hand with cards.
    ///     It generates an array of cards and instantiates a card prefab for each card in the array.
    /// </summary>
    internal void Fill()
    {
        // Generate the array of cards
        Cards = GenerateCard();

        // Instantiate a card prefab for each card in the array
        // Instantiate a card prefab for each card in the array
        for (int i = 0; i < Cards.Length; i++)
        {
            GameObject cardObject = Instantiate(CardPrefab, transform, false);
            cardObject.name = Cards[i].Rank + " of " + Cards[i].Suit;
            cardObject.tag = gameObject.tag;
            cardObject.GetComponent<Renderer>().material = Cards[i].Content;
            cardObject.transform.localPosition = CalculateCardPosition(i);
            cardObject.transform.Rotate(Vector3.right, 35);

            // Set the Card property of the CardController
            CardManager cardManager = cardObject.GetComponent<CardManager>();
            cardManager.Card = Cards[i];
        }
    }

    private static Vector3 CalculateCardPosition(int index)
    {
        return new Vector3((XOffset * (index - 5.5f)), (YOffset - index * YOffset), (ZOffset + index * ZOffset));
    }


    /// <summary>
    ///     This method generates an array of cards.
    ///     It returns an array of 12 cards with different ranks and suits.
    /// </summary>
    /// <returns>An array of 12 cards.</returns>
    [NotNull]
    private Card[] GenerateCard()
    {
        string gameObjectTag = gameObject.tag;

        // Return an array of 12 cards with different ranks and suits
        return new[]
        {
            CreateCard(1, gameObjectTag),
            CreateCard(1, gameObjectTag),
            CreateCard(1, gameObjectTag),
            CreateCard(1, gameObjectTag),
            CreateCard(2, gameObjectTag),
            CreateCard(2, gameObjectTag),
            CreateCard(2, gameObjectTag),
            CreateCard(3, gameObjectTag),
            CreateCard(3, gameObjectTag),
            CreateCard(4, gameObjectTag),
            CreateCard(4, gameObjectTag),
            CreateCard(5, gameObjectTag)
        };
    }

    [NotNull]
    private static Card CreateCard(int rank, string suit)
    {
        return new Card
        {
            Rank = rank,
            Suit = suit,
            Content = Resources.Load<Material>("Materials/" + rank + "/" + rank + "_" + suit)
        };
    }

    /// <summary>
    ///     This method returns the X offset for positioning the cards in the hand.
    /// </summary>
    /// <returns>The X offset.</returns>
    public static float GetXOffset()
    {
        return XOffset;
    }

    /// <summary>
    ///     This method returns the Y offset for positioning the cards in the hand.
    /// </summary>
    /// <returns>The Y offset.</returns>
    public static float GetYOffset()
    {
        return YOffset;
    }

    /// <summary>
    ///     This method returns the Z offset for positioning the cards in the hand.
    /// </summary>
    /// <returns>The Z offset.</returns>
    public static float GetZOffset()
    {
        return ZOffset;
    }
}