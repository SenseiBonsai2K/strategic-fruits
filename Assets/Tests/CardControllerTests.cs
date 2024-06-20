#region

using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

#endregion
public sealed class CardControllerTests
{
    private GameObject _card;
    private CardController _cardController;
    private GameObject _firstCardSlot;
    private GameObject _hand;
    private GameObject _secondCardSlot;

    [SetUp]
    public void SetUp()
    {
        _hand = new GameObject("Hand");
        _firstCardSlot = new GameObject("FirstCardSlot");
        _secondCardSlot = new GameObject("SecondCardSlot");
        _card = new GameObject();
        _cardController = _card.AddComponent<CardController>();
        _cardController.FirstCardSlot = _firstCardSlot;
        _cardController.SecondCardSlot = _secondCardSlot;
        _card.transform.parent = _hand.transform;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_card);
        Object.DestroyImmediate(_hand);
        Object.DestroyImmediate(_firstCardSlot);
        Object.DestroyImmediate(_secondCardSlot);
    }

    [Test]
    public void OnMouseDown_IsChildOfHand_SetsIsDraggingToTrue()
    {
        _cardController.OnMouseDown();

        Assert.IsTrue(_cardController.GetIsDragging());
    }

    [Test]
    public void OnMouseDrag_IsChildOfHand_ChangesPosition()
    {
        Vector3 originalPosition = _card.transform.position;

        _cardController.OnMouseDrag();

        Assert.AreNotEqual(originalPosition, _card.transform.position);
    }

    [Test]
    public void OnMouseUp_IsChildOfHandAndMouseInUpperHalf_ResetsPosition()
    {
        Vector3 originalPosition = _card.transform.position;
        // Cannot set Input.mousePosition directly as it's a read-only property

        _cardController.OnMouseUp();

        Assert.AreEqual(originalPosition, _card.transform.position);
    }

    [Test]
    public void OnMouseUp_IsChildOfHandAndMouseInLowerHalfAndFirstSlotEmpty_MovesCardToFirstSlot()
    {
        // Cannot set Input.mousePosition directly as it's a read-only property

        _cardController.OnMouseUp();

        Assert.AreEqual(_firstCardSlot.transform, _card.transform.parent);
    }

    [Test]
    public void OnMouseUp_IsChildOfHandAndMouseInLowerHalfAndFirstSlotNotEmpty_ResetsPosition()
    {
        Vector3 originalPosition = _card.transform.position;
        // Cannot set Input.mousePosition directly as it's a read-only property
        new GameObject().transform.parent = _firstCardSlot.transform;

        _cardController.OnMouseUp();

        Assert.AreEqual(originalPosition, _card.transform.position);
    }
}
