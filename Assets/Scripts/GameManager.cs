using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
///     Manages the game state, including the current phase and round, and handles the game logic.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    ///     The current phase of the game.
    /// </summary>
    public int currentPhase = 1;

    /// <summary>
    ///     The current round of the game.
    /// </summary>
    public int currentRound = 1;

    /// <summary>
    ///     List of the first set of slots in the game.
    /// </summary>
    public List<GameObject> firstSlots;

    /// <summary>
    ///     List of the second set of slots in the game.
    /// </summary>
    public List<GameObject> secondSlots;

    /// <summary>
    ///     Indicates whether coroutines are currently running.
    /// </summary>
    public bool isCoroutinesRunning;

    /// <summary>
    ///     Indicates whether the first set of cards have been solved.
    /// </summary>
    [FormerlySerializedAs("firstSlotsSolved")]
    public bool firstCardsSolved;

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
        Debug.Log("Phase: " + currentPhase + ", Round: " + currentRound);
    }

    /// <summary>
    ///     Updates the GameManager every frame.
    /// </summary>
    private void Update()
    {
        if (isCoroutinesRunning) return;
        CheckFirstSlots();
        CheckSecondSlots();
    }

    /// <summary>
    ///     Checks the first set of slots and starts the card rotation and destruction process if they are all filled.
    /// </summary>
    private void CheckFirstSlots()
    {
        if (FirstSlotsHaveCard() && !firstCardsSolved)
            StartCoroutine(RotateAndDestroyFirstCard());
    }

    /// <summary>
    ///     Checks the second set of slots and starts the card rotation and destruction process if they are all filled.
    /// </summary>
    private void CheckSecondSlots()
    {
        if (FirstSlotsHaveCard() && SecondSlotsHaveCard())
            StartCoroutine(RotateAndDestroySecondCard());
    }

    /// <summary>
    ///     Advances the round of the game and checks if the phase should be advanced.
    /// </summary>
    private void AdvanceRound()
    {
        currentRound++;
        firstCardsSolved = false;

        if (ShouldAdvancePhase()) AdvancePhase();

        if (currentPhase > 3)
        {
            Debug.Log("Game Over!");
            return;
        }

        Debug.Log($"Phase: {currentPhase}, Round: {currentRound}");
    }

    /// <summary>
    ///     Determines whether the phase of the game should be advanced.
    /// </summary>
    /// <returns>True if the phase should be advanced, false otherwise.</returns>
    private bool ShouldAdvancePhase()
    {
        return (currentPhase == 1 && currentRound > 2) ||
               ((currentPhase == 2 || currentPhase == 3) && currentRound > 6);
    }

    /// <summary>
    ///     Advances the phase of the game and refills the hands.
    /// </summary>
    private void AdvancePhase()
    {
        currentPhase++;
        currentRound = 1;
        FillHands();
    }

    /// <summary>
    ///     Fills the hands with cards.
    /// </summary>
    private void FillHands()
    {
        foreach (var fillHand in _fillHand) fillHand.Fill();
    }

    /// <summary>
    ///     Checks if all the first slots have a card.
    /// </summary>
    /// <returns>True if all the first slots have a card, false otherwise.</returns>
    private bool FirstSlotsHaveCard()
    {
        return firstSlots.All(slot => slot.transform.childCount > 0);
    }

    /// <summary>
    ///     Checks if all the second slots have a card.
    /// </summary>
    /// <returns>True if all the second slots have a card, false otherwise.</returns>
    private bool SecondSlotsHaveCard()
    {
        return secondSlots.All(slot => slot.transform.childCount > 0);
    }

    /// <summary>
    ///     Rotates and destroys the first set of cards after a delay.
    /// </summary>
    /// <returns>An IEnumerator to be used in a coroutine.</returns>
    private IEnumerator RotateAndDestroyFirstCard()
    {
        isCoroutinesRunning = true;
        firstCardsSolved = true;

        yield return new WaitForSeconds(2);

        // Move cards up by 2f
        foreach (var child in firstSlots.Select(slot => slot.transform.GetChild(0)))
            child.position += new Vector3(0, 0.1f, 0);

        yield return new WaitForSeconds(1);

        // Rotate cards around the long side
        foreach (var child in firstSlots.Select(slot => slot.transform.GetChild(0))) child.Rotate(0, 180, 0);

        yield return new WaitForSeconds(1);

        // Move cards down by 2f
        foreach (var child in firstSlots.Select(slot => slot.transform.GetChild(0)))
            child.position -= new Vector3(0, 0.1f, 0);

        yield return new WaitForSeconds(3);

        if (currentPhase == 1)
        {
            foreach (var slot in firstSlots)
                Destroy(slot.transform.GetChild(0).gameObject);
            AdvanceRound();
        }

        isCoroutinesRunning = false;
    }


    /// <summary>
    ///     Rotates and destroys the second set of cards after a delay.
    /// </summary>
    /// <returns>An IEnumerator to be used in a coroutine.</returns>
    private IEnumerator RotateAndDestroySecondCard()
    {
        isCoroutinesRunning = true;

        yield return new WaitForSeconds(2);

        // Move cards up by 2f
        foreach (var child in secondSlots.Select(slot => slot.transform.GetChild(0)))
            child.position += new Vector3(0, 0.1f, 0);

        yield return new WaitForSeconds(1);

        // Rotate cards around the long side
        foreach (var child in secondSlots.Select(slot => slot.transform.GetChild(0))) child.Rotate(0, 180, 0);

        yield return new WaitForSeconds(1);

        // Move cards down by 2f
        foreach (var child in secondSlots.Select(slot => slot.transform.GetChild(0)))
            child.position -= new Vector3(0, 0.1f, 0);

        yield return new WaitForSeconds(3);

        //Destroy the cards in the slots
        foreach (var slot in firstSlots)
            Destroy(slot.transform.GetChild(0).gameObject);
        foreach (var slot in secondSlots)
            Destroy(slot.transform.GetChild(0).gameObject);

        AdvanceRound();

        isCoroutinesRunning = false;
    }
}