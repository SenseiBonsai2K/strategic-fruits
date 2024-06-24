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
        for (int i = 0; i < 12; i++)
        {
            GameObject card = Instantiate(CardPrefab, transform, false);
            card.name = Cards[i].Rank + " of " + Cards[i].Suit; // Use Rank as name, for example
            card.tag = gameObject.tag;
            card.transform.localPosition =
                new Vector3((XOffset * (i - 5.5f)), (YOffset - i * YOffset), (ZOffset + i * ZOffset));
            card.transform.Rotate(Vector3.right, 35);
        }
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
            new Card
            {
                Rank = 1,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/1 " + gameObjectTag)
            },
            new Card
            {
                Rank = 1,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/1 " + gameObjectTag)
            },
            new Card
            {
                Rank = 1,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/1 " + gameObjectTag)
            },
            new Card
            {
                Rank = 1,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/1 " + gameObjectTag)
            },
            new Card
            {
                Rank = 2,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/2 " + gameObjectTag)
            },
            new Card
            {
                Rank = 2,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/2 " + gameObjectTag)
            },
            new Card
            {
                Rank = 2,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/2 " + gameObjectTag)
            },
            new Card
            {
                Rank = 3,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/3 " + gameObjectTag)
            },
            new Card
            {
                Rank = 3,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/3 " + gameObjectTag)
            },
            new Card
            {
                Rank = 4,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/4 " + gameObjectTag)
            },
            new Card
            {
                Rank = 4,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/4 " + gameObjectTag)
            },
            new Card
            {
                Rank = 5,
                Suit = gameObjectTag,
                Content = Resources.Load<Texture>("Textures/5 " + gameObjectTag)
            }
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
