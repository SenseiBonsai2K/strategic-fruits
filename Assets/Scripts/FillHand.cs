#region

using JetBrains.Annotations;
using UnityEngine;

#endregion
public sealed class FillHand : MonoBehaviour
{
    private const float XOffset = 0.15f;
    private const float YOffset = 0.008f;
    private const float ZOffset = 0.002f;

    public GameObject CardPrefab;
    public Card[] Cards
    {
        get;
        private set;
    }

    public void Start()
    {
        Fill();
    }

    public void Fill()
    {
        Cards = GenerateCard();

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

    [NotNull]
    private Card[] GenerateCard()
    {
        return new[]
        {
            new Card
            {
                Rank = 1,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/1 " + gameObject.tag)
            },
            new Card
            {
                Rank = 1,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/1 " + gameObject.tag)
            },
            new Card
            {
                Rank = 1,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/1 " + gameObject.tag)
            },
            new Card
            {
                Rank = 1,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/1 " + gameObject.tag)
            },
            new Card
            {
                Rank = 2,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/2 " + gameObject.tag)
            },
            new Card
            {
                Rank = 2,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/2 " + gameObject.tag)
            },
            new Card
            {
                Rank = 2,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/2 " + gameObject.tag)
            },
            new Card
            {
                Rank = 3,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/3 " + gameObject.tag)
            },
            new Card
            {
                Rank = 3,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/3 " + gameObject.tag)
            },
            new Card
            {
                Rank = 4,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/4 " + gameObject.tag)
            },
            new Card
            {
                Rank = 4,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/4 " + gameObject.tag)
            },
            new Card
            {
                Rank = 5,
                Suit = gameObject.tag,
                Content = Resources.Load<Texture>("Textures/5 " + gameObject.tag)
            }
        };
    }

    public static float GetXOffset()
    {
        return XOffset;
    }

    public static float GetYOffset()
    {
        return YOffset;
    }

    public static float GetZOffset()
    {
        return ZOffset;
    }
}
