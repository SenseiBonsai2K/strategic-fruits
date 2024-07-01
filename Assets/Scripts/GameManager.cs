#region

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

/// <summary>
///     Manages the game state, including the current phase and round, and handles the game logic.
/// </summary>
public sealed class GameManager : MonoBehaviour
{
    /// <summary>
    ///     The current phase of the game.
    /// </summary>
    public static int CurrentPhase = 1;

    /// <summary>
    ///     The current round of the game.
    /// </summary>
    public static int CurrentRound = 1;

    /// <summary>
    ///     List of the first set of slots in the game.
    /// </summary>
    public List<GameObject> FirstSlots;

    /// <summary>
    ///     List of the second set of slots in the game.
    /// </summary>
    public List<GameObject> SecondSlots;

    /// <summary>
    ///     Indicates whether coroutines are currently running.
    /// </summary>
    public bool IsCoroutinesRunning;

    /// <summary>
    ///     Indicates whether the first set of cards have been solved.
    /// </summary>
    public bool FirstCardsSolved;
    
    public List<GameObject> Hands;

    /// <summary>
    ///     Reference to the FillHand objects in the game.
    /// </summary>
    private FillHand[] _fillHand;

    /// <summary>
    ///     Initializes the GameManager at the start of the game.
    /// </summary>
    private void Start()
    {
        _fillHand = FindObjectsOfType<FillHand>();
        Debug.Log("Phase: " + CurrentPhase + ", Round: " + CurrentRound);
        CardTracker.ClearRoundData();
    }

    private void Update()
    {
        if (IsCoroutinesRunning)
        {
            return;
        }
        CheckSlots();
    }

    /// <summary>
    ///     Checks the slots and starts the card rotation and destruction process if they are all filled.
    /// </summary>
    private void CheckSlots()
    {
        if (FirstSlotsHaveCard() && !FirstCardsSolved)
        {
            StartCoroutine(RotateAndDestroyFirstCard());
        }
        if (FirstSlotsHaveCard() && SecondSlotsHaveCard())
        {
            StartCoroutine(RotateAndDestroySecondCard());
        }
    }

    /// <summary>
    ///     Advances the round of the game and checks if the phase should be advanced.
    /// </summary>
    private void AdvanceRound()
    {
        CurrentRound++;
        FirstCardsSolved = false;

        if (ShouldAdvancePhase())
        {
            AdvancePhase();
        }

        if (CurrentPhase > 3)
        {
            Debug.Log("Game Over!");
            return;
        }

        Debug.Log($"Phase: {CurrentPhase}, Round: {CurrentRound}");
        CardTracker.ClearRoundData();
    }

    /// <summary>
    ///     Determines whether the phase of the game should be advanced.
    /// </summary>
    /// <returns>True if the phase should be advanced, false otherwise.</returns>
    private bool ShouldAdvancePhase()
    {
        return (CurrentPhase == 1 && CurrentRound > 12) ||
            (CurrentPhase is 2 or 3 && CurrentRound > 6);
    }

    /// <summary>
    ///     Advances the phase of the game and refills the hands.
    /// </summary>
    private void AdvancePhase()
    {
        CurrentPhase++;
        CurrentRound = 1;
        foreach (Transform card in Hands.SelectMany(static hand => hand.transform.Cast<Transform>())) Destroy(card.gameObject);

        FillHands();
    }

    /// <summary>
    ///     Fills the hands with cards.
    /// </summary>
    private void FillHands()
    {
        foreach (FillHand fillHand in _fillHand) fillHand.Fill();
    }

    /// <summary>
    ///     Checks if all the first slots have a card.
    /// </summary>
    /// <returns>True if all the first slots have a card, false otherwise.</returns>
    private bool FirstSlotsHaveCard()
    {
        return FirstSlots.All(static slot => slot.transform.childCount > 0);
    }

    /// <summary>
    ///     Checks if all the second slots have a card.
    /// </summary>
    /// <returns>True if all the second slots have a card, false otherwise.</returns>
    private bool SecondSlotsHaveCard()
    {
        return SecondSlots.All(static slot => slot.transform.childCount > 0);
    }

    private IEnumerator RotateCards(List<GameObject> slots)
    {
        yield return new WaitForSeconds(2);

        // Move cards up by 2f
        foreach (Transform child in slots.Select(static slot => slot.transform.GetChild(0)))
        {
            child.position += new Vector3(0, 0.1f, 0);
        }

        yield return new WaitForSeconds(1);

        // Rotate cards around the long side
        foreach (Transform child in slots.Select(static slot => slot.transform.GetChild(0))) child.Rotate(0, 180, 0);

        yield return new WaitForSeconds(1);

        // Move cards down by 2f
        foreach (Transform child in slots.Select(static slot => slot.transform.GetChild(0)))
        {
            child.position -= new Vector3(0, 0.1f, 0);
        }

        yield return new WaitForSeconds(3);
    }

    /// <summary>
    ///     Rotates and destroys the first set of cards after a delay.
    /// </summary>
    /// <returns>An IEnumerator to be used in a coroutine.</returns>
    private IEnumerator RotateAndDestroyFirstCard()
    {
        IsCoroutinesRunning = true;
        FirstCardsSolved = true;

        yield return RotateCards(FirstSlots);

        // Destroy the cards in the first slot
        if (CurrentPhase == 1)
        {
            foreach (GameObject slot in FirstSlots)
                Destroy(slot.transform.GetChild(0).gameObject);
            AdvanceRound();
        }

        IsCoroutinesRunning = false;
    }


    /// <summary>
    ///     Rotates and destroys the second set of cards after a delay.
    /// </summary>
    /// <returns>An IEnumerator to be used in a coroutine.</returns>
    private IEnumerator RotateAndDestroySecondCard()
    {
        IsCoroutinesRunning = true;

        yield return RotateCards(SecondSlots);

        //Destroy the cards in the slots
        foreach (GameObject slot in FirstSlots)
            Destroy(slot.transform.GetChild(0).gameObject);
        foreach (GameObject slot in SecondSlots)
            Destroy(slot.transform.GetChild(0).gameObject);

        AdvanceRound();

        IsCoroutinesRunning = false;
    }
}
