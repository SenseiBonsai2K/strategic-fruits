using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentPhase = 1;
    public int currentRound = 1;
    public List<GameObject> firstSlots;
    public List<GameObject> secondSlots;
    public bool isCoroutinesRunning;

    private void Start()
    {
        Debug.Log("Phase: " + currentPhase + ", Round: " + currentRound);
    }

    private void Update()
    {
        if (!CheckFirstSlots() || isCoroutinesRunning) return;
        StartCoroutine(RotateAndDestroyCards());
    }

    public void AdvanceRound()
    {
        currentRound++;

        if (currentPhase == 1 && currentRound > 2)
        {
            currentPhase++;
            currentRound = 1;
        }
        else if (currentPhase is 2 or 3 && currentRound > 6)
        {
            currentPhase++;
            currentRound = 1;
        }

        if (currentPhase > 3)
        {
            Debug.Log("Game Over!");
            return;
        }

        Debug.Log("Phase: " + currentPhase + ", Round: " + currentRound);
    }

    private bool CheckFirstSlots()
    {
        return firstSlots.All(slot => slot.transform.childCount > 0);
    }
    
    private bool CheckSecondSlots()
    {
        return secondSlots.All(slot => slot.transform.childCount > 0);
    }

    private IEnumerator RotateAndDestroyCards()
    {
        isCoroutinesRunning = true;

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

        // Destroy cards only in the first phase
        if (currentPhase == 1)
            foreach (var slot in firstSlots)
                Destroy(slot.transform.GetChild(0).gameObject);

        AdvanceRound();

        isCoroutinesRunning = false;
    }
}