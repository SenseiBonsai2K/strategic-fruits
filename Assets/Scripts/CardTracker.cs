using System;
using System.Collections.Generic;
using UnityEngine;

public class CardTracker : MonoBehaviour
{
    private static float _roundStartTime;
    private static List<(Card, float)> PlayedCards = new List<(Card, float)>();

    public static void SaveCardAndTime(Card card)
    {
        float timeElapsed = Time.time - _roundStartTime;
        PlayedCards.Add((card, timeElapsed));

        Debug.Log($"Card saved: {card.Rank} of {card.Suit}, Time elapsed: {timeElapsed}");
    }
    
    public static void ClearRoundData()
    {
        PlayedCards.Clear();
        _roundStartTime = Time.time;
        Debug.Log("Played cards cleared and time reset.");
    }
}