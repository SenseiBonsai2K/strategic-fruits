using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     Represents a player in the game.
/// </summary>
public class Player
{
    /// <summary>
    ///     Initializes a new instance of the Player class.
    /// </summary>
    /// <param name="suit">The suit of the player.</param>
    /// <param name="rank">The rank of the player.</param>
    public Player(string suit, int rank)
    {
        Suit = suit;
        Rank = rank;
    }

    /// <summary>
    ///     Gets or sets the suit of the player.
    /// </summary>
    public string Suit { get; set; }

    /// <summary>
    ///     Gets or sets the rank of the player.
    /// </summary>
    public int Rank { get; set; }
}

/// <summary>
///     Manages the matches in the game.
/// </summary>
public class MatchManager
{
    /// <summary>
    ///     Defines the matchups for each round.
    /// </summary>
    private static readonly Dictionary<int, List<(string, string)>> Matchups = new()
    {
        { 1, new List<(string, string)> { ("Apple", "Pear"), ("Orange", "Banana") } },
        { 2, new List<(string, string)> { ("Apple", "Banana"), ("Orange", "Pear") } },
        { 3, new List<(string, string)> { ("Pear", "Banana"), ("Orange", "Apple") } }
    };

    /// <summary>
    ///     List of players in the game.
    /// </summary>
    private static List<Player> _players = new();

    /// <summary>
    ///     Creates players for the current phase.
    /// </summary>
    /// <param name="currentPhase">The current phase of the game.</param>
    /// <returns>A list of players.</returns>
    private static List<Player> CreatePlayers(int currentPhase)
    {
        _players.Clear();

        var cards = CardTracker.PlayedCards;
        if (currentPhase == 2) cards = cards.Skip(Math.Max(0, cards.Count - 4)).ToList();

        if (currentPhase != 3)
            _players.AddRange(cards.Select(card => new Player(card.Item1.Suit, card.Item1.Rank)));
        else
            _players.AddRange(cards.GroupBy(card => card.Item1.Suit)
                .Select(group => new Player(group.Key, group.Sum(card => card.Item1.Rank))));

        return _players;
    }

    /// <summary>
    ///     Determines the winners of the round based on the matchups and the current phase of the game.
    /// </summary>
    /// <param name="matchupRound">The current round of matchups.</param>
    /// <param name="currentPhase">The current phase of the game.</param>
    public static void GetRoundWinners(int matchupRound, int currentPhase)
    {
        // Create players for the current phase
        _players = CreatePlayers(currentPhase);

        // Iterate through each matchup in the current round
        foreach (var (suit1, suit2) in Matchups[matchupRound])
        {
            // Find the players for the current matchup
            var player1 = FindPlayer(suit1);
            var player2 = FindPlayer(suit2);

            // Determine the winner of the matchup
            var winner = DetermineWinner(player1, player2, currentPhase);

            // If there is a winner, give tokens and log the result
            if (winner != null)
            {
                var loser = winner == player1 ? player2 : player1;
                GridManager.GiveTokens(winner, loser, CardTracker.PlayedCards);
            }

            // Log the result of the matchup
            Debug.Log($"{suit1} vs {suit2}: {winner?.Suit ?? "Tie"}");
        }
    }

    /// <summary>
    ///     Finds a player based on their suit.
    /// </summary>
    /// <param name="suit">The suit of the player to find.</param>
    /// <returns>The player with the matching suit, or null if no player is found.</returns>
    private static Player FindPlayer(string suit)
    {
        // Find and return the player with the matching suit
        return _players.Find(player => player.Suit == suit);
    }

    /// <summary>
    ///     Determines the winner between two players based on the current phase of the game.
    /// </summary>
    /// <param name="player1">The first player.</param>
    /// <param name="player2">The second player.</param>
    /// <param name="currentPhase">The current phase of the game.</param>
    /// <returns>The winning player, or null if there is a tie.</returns>
    private static Player DetermineWinner(Player player1, Player player2, int currentPhase)
    {
        // Determine the suit of the winning player
        var winnerSuit = WhoWins(player1, player2, currentPhase).Suit;

        // Return the player with the winning suit, or null if there is a tie
        return winnerSuit == player1.Suit ? player1 : winnerSuit == player2.Suit ? player2 : null;
    }

    /// <summary>
    ///     Determines the winner between two players.
    /// </summary>
    /// <param name="player1">The first player.</param>
    /// <param name="player2">The second player.</param>
    /// <param name="currentPhase">The current phase of the game.</param>
    /// <returns>The winning player.</returns>
    private static Player WhoWins(Player player1, Player player2, int currentPhase)
    {
        switch (currentPhase)
        {
            case 1:
            case 2:
                return CheckPhase1And2Winner(player1, player2);
            case 3:
                return CheckPhase3Winner(player1, player2);
            default: return null;
        }
    }

    /// <summary>
    ///     Determines the winner between two players in phase 1 and 2.
    /// </summary>
    /// <param name="player1">The first player.</param>
    /// <param name="player2">The second player.</param>
    /// <returns>The winning player.</returns>
    private static Player CheckPhase1And2Winner(Player player1, Player player2)
    {
        if (player1.Rank == 5 && player2.Rank == 1) return player2;

        if (player2.Rank == 5 && player1.Rank == 1) return player1;

        if (player1.Rank == player2.Rank)
            return new Player("Tie", 0);

        return player1.Rank > player2.Rank ? player1 : player2;
    }

    /// <summary>
    ///     Determines the winner between two players in phase 3.
    /// </summary>
    /// <param name="player1">The first player.</param>
    /// <param name="player2">The second player.</param>
    /// <returns>The winning player.</returns>
    private static Player CheckPhase3Winner(Player player1, Player player2)
    {
        if (player1.Rank == player2.Rank)
            return new Player("Tie", 0);

        return player1.Rank > player2.Rank ? player1 : player2;
    }

    // public static string GetOpponent(int currentRound, string suit)
    // {
    //     if (Matchups.ContainsKey(currentRound))
    //         foreach (var matchup in Matchups[currentRound])
    //             if (matchup.Item1 == suit)
    //                 return matchup.Item2;
    //             else if (matchup.Item2 == suit) return matchup.Item1;
    //
    //     throw new ArgumentException($"No matchups found for round {currentRound} and suit {suit}");
    // }
}