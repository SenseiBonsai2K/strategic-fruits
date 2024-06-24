using UnityEngine;

public class FillHand : MonoBehaviour
{
    public GameObject cardPrefab;
    public float cardOffset, yOffset, zOffset;
    public Card[] Cards { get; private set; }

    private void Start()
    {
        Fill();
    }

    public void Fill()
    {
        Cards = GenerateCard();

        for (var i = 0; i < 12; i++)
        {
            var card = Instantiate(cardPrefab, transform, false);
            card.name = Cards[i].Rank + " of " + Cards[i].Suit; // Use Rank as name for example
            card.tag = gameObject.tag;
            card.transform.localPosition =
                new Vector3(cardOffset * (i - 5.5f), yOffset - i * yOffset, zOffset + i * zOffset);
            card.transform.Rotate(Vector3.right, 35);
        }
    }

    private Card[] GenerateCard()
    {
        return new[]
        {
            new Card
            {
                Rank = 1, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/1 " + gameObject.tag)
            },
            new Card
            {
                Rank = 1, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/1 " + gameObject.tag)
            },
            new Card
            {
                Rank = 1, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/1 " + gameObject.tag)
            },
            new Card
            {
                Rank = 1, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/1 " + gameObject.tag)
            },
            new Card
            {
                Rank = 2, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/2 " + gameObject.tag)
            },
            new Card
            {
                Rank = 2, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/2 " + gameObject.tag)
            },
            new Card
            {
                Rank = 2, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/2 " + gameObject.tag)
            },
            new Card
            {
                Rank = 3, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/3 " + gameObject.tag)
            },
            new Card
            {
                Rank = 3, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/3 " + gameObject.tag)
            },
            new Card
            {
                Rank = 4, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/4 " + gameObject.tag)
            },
            new Card
            {
                Rank = 4, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/4 " + gameObject.tag)
            },
            new Card
            {
                Rank = 5, Suit = gameObject.tag, Content = Resources.Load<Texture>("Textures/5 " + gameObject.tag)
            }
        };
    }
}