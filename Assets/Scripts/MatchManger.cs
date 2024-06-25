using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player
{
    public Player(string suit, int rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public string Suit { get; set; }
    public int Rank { get; set; }
}

public class MatchManager
{
    private static readonly Dictionary<int, List<(string, string)>> Matchups = new()
    {
        { 1, new List<(string, string)> { ("Apple", "Pear"), ("Orange", "Banana") } },
        { 2, new List<(string, string)> { ("Apple", "Banana"), ("Orange", "Pear") } },
        { 3, new List<(string, string)> { ("Pear", "Banana"), ("Orange", "Apple") } }
    };

    private static List<Player> _players = new();

    private static List<Player> CreatePlayers(int currentPhase)
    {
        _players.Clear();

        switch (currentPhase)
        {
            case 1:
            {
                foreach (var (card, _) in CardTracker.PlayedCards)
                {
                    var player = new Player(card.Suit, card.Rank);
                    _players.Add(player);
                }

                break;
            }
            case 2:
            {
                // Get the last four cards
                var lastFourCards = CardTracker.PlayedCards.Skip(Math.Max(0, CardTracker.PlayedCards.Count - 4));

                foreach (var (card, _) in lastFourCards)
                {
                    var player = new Player(card.Suit, card.Rank);
                    _players.Add(player);
                }

                break;
            }
            case 3:
            {
                var groupedCards = CardTracker.PlayedCards.GroupBy(card => card.Item1.Suit);

                foreach (var group in groupedCards)
                {
                    var player = new Player(group.Key, group.Sum(card => card.Item1.Rank));
                    _players.Add(player);
                }

                break;
            }
        }

        return _players;
    }

    public static void GetRoundWinners(int matchupRound, int currentPhase)
    {
        _players = CreatePlayers(currentPhase);
        foreach (var (suit1, suit2) in Matchups[matchupRound])
        {
            var player1 = _players.Find(player => player.Suit == suit1);
            var player2 = _players.Find(player => player.Suit == suit2);

            var winner = WhoWins(player1, player2, currentPhase).Suit;
            if (winner == "Tie")
            {
                Debug.Log($"The match between {suit1} and {suit2} is a {winner}.");
                continue;
            }

            Debug.Log($"The winner of the match between {suit1} and {suit2} is {winner}.");
        }
    }

    private static Player WhoWins(Player player1, Player player2, int currentPhase)
    {
        switch (currentPhase)
        {
            case 1:
                return CheckPhase1And2Winner(player1, player2);
            case 2:
                return CheckPhase1And2Winner(player1, player2);
            case 3:
                return CheckPhase3Winner(player1, player2);
            default: return null;
        }
    }

    private static Player CheckPhase1And2Winner(Player player1, Player player2)
    {
        if (player1.Rank == 5 && player2.Rank == 1) return player2;

        if (player2.Rank == 5 && player1.Rank == 1) return player1;

        if (player1.Rank == player2.Rank)
            return new Player("Tie", 0);

        return player1.Rank > player2.Rank ? player1 : player2;
    }

    private static Player CheckPhase3Winner(Player player1, Player player2)
    {
        if (player1.Rank == player2.Rank)
            return new Player("Tie", 0);

        return player1.Rank > player2.Rank ? player1 : player2;
    }
}