using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     The GridManager class is responsible for managing the grid in the game.
///     It assigns tokens to the cells of the grid based on the cards played by the players.
/// </summary>
public class GridManager : MonoBehaviour
{
    /// <summary>
    ///     The static reference to the token prefab.
    /// </summary>
    private static GameObject _tokenPrefab;

    /// <summary>
    ///     The instance of the token prefab to be used in the game.
    /// </summary>
    public GameObject TokenPrefabInstance;

    /// <summary>
    ///     Awake is called when the script instance is being loaded.
    ///     It initializes the _tokenPrefab with the TokenPrefabInstance.
    /// </summary>
    private void Awake()
    {
        _tokenPrefab = TokenPrefabInstance;
    }

    /// <summary>
    ///     Assigns tokens to the cells of the winner's grid based on the cards played.
    /// </summary>
    /// <param name="winner">The player who won the round.</param>
    /// <param name="loser">The player who lost the round.</param>
    /// <param name="playedCards">The list of cards played in the round along with their scores.</param>
    public static void GiveTokens(Player winner, Player loser, List<(Card, float)> playedCards)
    {
        var cardsList = GetCardsList(winner, loser, playedCards);
        var winnerGrid = GameObject.Find(winner.Suit + "Grid");

        if (winnerGrid != null)
            AssignTokensToCells(winnerGrid, cardsList);
        else
            Debug.LogError("Could not find a grid named " + winner.Suit + "Grid");
    }

    /// <summary>
    ///     Creates a list of cards played by the winner and the loser.
    /// </summary>
    /// <param name="winner">The player who won the round.</param>
    /// <param name="loser">The player who lost the round.</param>
    /// <param name="playedCards">The list of cards played in the round along with their scores.</param>
    /// <returns>A list of cards played by the winner and the loser.</returns>
    private static List<Card> GetCardsList(Player winner, Player loser, List<(Card, float)> playedCards)
    {
        var cardsList = new List<Card>();

        foreach (var (card, _) in playedCards)
            if (card.Suit == winner.Suit || card.Suit == loser.Suit)
                cardsList.Add(card);

        return cardsList;
    }

    /// <summary>
    ///     Assigns tokens to the cells of the winner's grid that match the rank and suit of the cards in the cardsList.
    /// </summary>
    /// <param name="winnerGrid">The grid of the winner.</param>
    /// <param name="cardsList">The list of cards to match with the cells.</param>
    private static void AssignTokensToCells(GameObject winnerGrid, List<Card> cardsList)
    {
        foreach (Transform child in winnerGrid.transform)
        {
            var cell = child.GetComponent<Cell>();
            if (cell == null) continue;

            foreach (var tokenObject in from card in cardsList
                     where cell.Rank == card.Rank && cell.Suit == card.Suit && !cell.HasToken
                     select Instantiate(_tokenPrefab, cell.transform.position, Quaternion.identity))
            {
                tokenObject.transform.parent = cell.transform;
                cell.HasToken = true;
            }
        }
    }
}