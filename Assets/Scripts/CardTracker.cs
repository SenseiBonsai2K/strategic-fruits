using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     The CardTracker class is responsible for tracking the cards played in a round and the time elapsed since the start
///     of the round.
/// </summary>
public class CardTracker : MonoBehaviour
{
    /// <summary>
    ///     The time at which the current round started.
    /// </summary>
    private static float _roundStartTime;

    /// <summary>
    ///     A list of tuples, each containing a Card that was played and the time elapsed since the start of the round when the
    ///     card was played.
    /// </summary>
    private static readonly List<(Card, float)> PlayedCards = new();

    /// <summary>
    ///     Saves a card and the time elapsed since the start of the round to the PlayedCards list.
    /// </summary>
    /// <param name="card">The card to be saved.</param>
    public static void SaveCardAndTime(Card card)
    {
        var timeElapsed = Time.time - _roundStartTime;
        PlayedCards.Add((card, timeElapsed));

        Debug.Log($"Card saved: {card.Rank} of {card.Suit}, Time elapsed: {timeElapsed}");
    }

    /// <summary>
    ///     Clears the PlayedCards list and resets the round start time to the current time.
    /// </summary>
    public static void ClearRoundData()
    {
        PlayedCards.Clear();
        _roundStartTime = Time.time;
        Debug.Log("Played cards cleared and time reset.");
    }
}