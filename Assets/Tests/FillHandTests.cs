#region

using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

#endregion

public sealed class FillHandTests
{
    private FillHand _fillHand;
    private GameObject _hand;

    [SetUp]
    public void SetUp()
    {
        _hand = new GameObject();
        _fillHand = _hand.AddComponent<FillHand>();
        _fillHand.CardPrefab = new GameObject();
        _fillHand.CardOffset = 1f;
        _fillHand.YOffset = 0.5f;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_hand);
    }

    [Test]
    public void Start_FillsHandWithCorrectNumberOfCards()
    {
        _fillHand.Start();

        Assert.AreEqual(12, _fillHand.CardsInHand.Count);
    }

    [Test]
    public void Start_CardsInHandAreCorrectlyPositioned()
    {
        _fillHand.Start();

        for (var i = 0; i < 12; i++)
        {
            Vector3 expectedPosition = new Vector3(_fillHand.CardOffset * (i - 5.5f), i * _fillHand.YOffset, -(i * _fillHand.YOffset / 2));
            Assert.AreEqual(expectedPosition, _fillHand.CardsInHand[i].transform.localPosition);
        }
    }

    [Test]
    public void Start_CardsInHandAreCorrectlyRotated()
    {
        _fillHand.Start();

        foreach (GameObject card in _fillHand.CardsInHand)
        {
            Assert.AreEqual(40, card.transform.rotation.eulerAngles.x);
        }
    }

    [Test]
    public void Start_CardsInHandHaveCorrectParent()
    {
        _fillHand.Start();

        foreach (GameObject card in _fillHand.CardsInHand)
        {
            Assert.AreEqual(_hand.transform, card.transform.parent);
        }
    }

    [Test]
    public void Start_CardsInHandHaveCardControllerComponent()
    {
        _fillHand.Start();

        foreach (GameObject card in _fillHand.CardsInHand)
        {
            Assert.IsNotNull(card.GetComponent<CardController>());
        }
    }

    [Test]
    public void Start_CardsInHandCardControllerHasCorrectCardsInHandReference()
    {
        _fillHand.Start();

        foreach (CardController cardController in _fillHand.CardsInHand.Select(static card => card.GetComponent<CardController>()))
        {
            Assert.AreEqual(_fillHand.CardsInHand, cardController.CardsInHand);
        }
    }
}