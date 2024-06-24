using UnityEngine;

/// <summary>
///     Represents a playing card.
/// </summary>
public class Card
{
    /// <summary>
    ///     Gets or sets the rank of the card.
    /// </summary>
    /// <value>The rank of the card.</value>
    public int Rank { get; set; }

    /// <summary>
    ///     Gets or sets the suit of the card.
    /// </summary>
    /// <value>The suit of the card.</value>
    public string Suit { get; set; }

    /// <summary>
    ///     Gets or sets the content of the card.
    /// </summary>
    /// <value>The content of the card.</value>
    public Material Content { get; set; }
}