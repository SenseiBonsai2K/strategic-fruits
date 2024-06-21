using UnityEngine;

public class FillHand : MonoBehaviour
{
    public GameObject cardPrefab;
    public float cardOffset, yOffset, zOffset;

    private void Start()
    {
        var cardNames = GenerateCardNames();

        for (var i = 0; i < 12; i++)
        {
            var card = Instantiate(cardPrefab, transform, false);
            card.name = cardNames[i];
            card.tag = gameObject.tag;
            card.transform.localPosition =
                new Vector3(cardOffset * (i - 5.5f), yOffset - i * yOffset, zOffset + i * zOffset);
            card.transform.Rotate(Vector3.right, 35);
        }
    }

    private string[] GenerateCardNames()
    {
        return new[]
        {
            "1 of " + gameObject.tag,
            "1 of " + gameObject.tag,
            "1 of " + gameObject.tag,
            "1 of " + gameObject.tag,
            "2 of " + gameObject.tag,
            "2 of " + gameObject.tag,
            "2 of " + gameObject.tag,
            "3 of " + gameObject.tag,
            "3 of " + gameObject.tag,
            "4 of " + gameObject.tag,
            "4 of " + gameObject.tag,
            "5 of " + gameObject.tag
        };
    }
}