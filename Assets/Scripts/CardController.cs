#region

using System.Collections.Generic;
using UnityEngine;

#endregion
public sealed class CardController : MonoBehaviour
{
    public GameObject FirstCardSlot;
    public GameObject SecondCardSlot;
    public List<GameObject> CardsInHand; // The list of cards in hand
    private float _hoverAmount; // The scale to apply when hovering
    private bool _isDragging;
    private Vector3 _offset;
    private Vector3 _originalPosition; // The target position of the GameObject
    private Vector3 _originalScale; // The original scale of the GameObject
    private Vector3 _screenPoint;

    private void Start()
    {
        // Store the original scale
        _originalScale = transform.localScale;
        _hoverAmount = 0.35f; // Set the hover amount
        _originalPosition = transform.position; // Store the original position
        FirstCardSlot = GameObject.Find("FirstCardSlot");
        SecondCardSlot = GameObject.Find("SecondCardSlot");
    }

    internal void OnMouseDown()
    {
        if (!IsChildOfHand())
        {
            return;
        }
        _isDragging = true;
        transform.localScale = _originalScale;
        _screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        _offset = gameObject.transform.position -
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                _screenPoint.z));
    }

    public void OnMouseDrag()
    {
        if (!IsChildOfHand())
        {
            return;
        }
        _isDragging = true;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + _offset;
        transform.position = curPosition;
    }

    private void OnMouseExit()
    {
        if (_isDragging || !IsChildOfHand())
        {
            return;
        }
        // Restore the original scale when the mouse is not over it
        transform.position = _originalPosition;
        transform.localScale = _originalScale;
    }

    private void OnMouseOver()
    {
        if (_isDragging || !IsChildOfHand())
        {
            return;
        }
        {
            // Scale the GameObject up when the mouse is over it
            transform.position = _originalPosition + new Vector3(0, _hoverAmount / 2, 0);
            transform.localScale = _originalScale + new Vector3(_hoverAmount / 3, _hoverAmount / 3, 0);
        }
    }

    public void OnMouseUp()
    {
        _isDragging = false;

        // If the card is released in the upper half of the screen or the first slot already has a child, return the card on the hand
        if (Input.mousePosition.y <= Screen.height / 2 || FirstCardSlot.transform.childCount > 0)
        {
            // If firstCardSlot already has a child, return the card to its original position
            transform.position = _originalPosition;
            return;
        }

        // Store the global scale of the card
        Vector3 globalScale = transform.lossyScale;

        // Change the parent
        transform.SetParent(FirstCardSlot.transform, false);

        // Calculate a new local scale for the card that maintains its global scale
        transform.localScale = new Vector3(globalScale.x / transform.parent.lossyScale.x,
            globalScale.y / transform.parent.lossyScale.y,
            globalScale.z / transform.parent.lossyScale.z);

        // Move and rotate the card
        transform.position = FirstCardSlot.transform.position +
            new Vector3(0, FirstCardSlot.transform.localScale.z, 0);
        transform.rotation = FirstCardSlot.transform.rotation;

        // Update the original position to the new position
        _originalPosition = transform.position;

        // Remove the card from the hand
        CardsInHand.Remove(gameObject);
    }

    internal bool GetIsDragging()
    {
        return _isDragging;
    }

    private bool IsChildOfHand()
    {
        // Check if the parent is "Hand"
        return transform.parent != null && transform.parent.name == "Hand";
    }
}
