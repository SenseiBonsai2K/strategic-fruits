﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public int currentPhase = 1;
    public int currentRound = 1;
    public List<GameObject> firstSlots;
    public List<GameObject> secondSlots;
    public bool isCoroutinesRunning;

    [FormerlySerializedAs("firstSlotsSolved")]
    public bool firstCardsSolved;

    private void Start()
    {
        Debug.Log("Phase: " + currentPhase + ", Round: " + currentRound);
    }

    private void Update()
    {
        if (isCoroutinesRunning) return;
        if (FirstSlotsHaveCard() && !firstCardsSolved)
            StartCoroutine(RotateAndDestroyFirstCard());
        else if (FirstSlotsHaveCard() && SecondSlotsHaveCard())
            StartCoroutine(RotateAndDestroySecondCard());
    }

    private void AdvanceRound()
    {
        currentRound++;
        firstCardsSolved = false;

        switch (currentPhase)
        {
            case 1 when currentRound > 2:
                currentPhase++;
                currentRound = 1;
                break;
            case 2 or 3 when currentRound > 6:
                currentPhase++;
                currentRound = 1;
                break;
        }

        if (currentPhase > 3)
        {
            Debug.Log("Game Over!");
            return;
        }

        Debug.Log("Phase: " + currentPhase + ", Round: " + currentRound);
    }

    private bool FirstSlotsHaveCard()
    {
        return firstSlots.All(slot => slot.transform.childCount > 0);
    }

    private bool SecondSlotsHaveCard()
    {
        return secondSlots.All(slot => slot.transform.childCount > 0);
    }

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

            //Distruggi le carte figlie di first slot e secondslot 
            foreach (var slot in firstSlots)
                Destroy(slot.transform.GetChild(0).gameObject);
            foreach (var slot in secondSlots)
                Destroy(slot.transform.GetChild(0).gameObject);

            AdvanceRound();

            isCoroutinesRunning = false;
        }
    }