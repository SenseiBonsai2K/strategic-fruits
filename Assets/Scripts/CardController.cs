using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private float _hoverAmount; // The scale to apply when hovering
    private bool _isDragging;
    private Vector3 _offset;
    private Vector3 _originalPosition; // The target position of the GameObject
    private Vector3 _originalScale; // The original scale of the GameObject
    private Vector3 _screenPoint;
    public GameObject firstCardSlot;
    public GameObject secondCardSlot;
    public List<GameObject> cardsInHand; // The list of cards in hand

    private void Start()
    {
        // Store the original scale
        _originalScale = transform.localScale;
        _hoverAmount = 0.35f; // Set the hover amount
        _originalPosition = transform.position; // Store the original position
        firstCardSlot = GameObject.Find("FirstCardSlot");
        secondCardSlot = GameObject.Find("SecondCardSlot");
    }

    private void OnMouseDown()
    {
        _isDragging = true;
        transform.localScale = _originalScale;
        _screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        _offset = gameObject.transform.position -
                  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                      _screenPoint.z));
    }

    private void OnMouseDrag()
    {
        _isDragging = true;
        var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
        var curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + _offset;
        transform.position = curPosition;
    }

    private void OnMouseExit()
    {
        if (!_isDragging)
        {
            // Restore the original scale when the mouse is not over it
            transform.position = _originalPosition;
            transform.localScale = _originalScale;
        }
    }

    private void OnMouseOver()
    {
        if (!_isDragging)
        {
            // Scale the GameObject up when the mouse is over it
            transform.position = _originalPosition + new Vector3(0, _hoverAmount / 2, 0);
            transform.localScale = _originalScale + new Vector3(_hoverAmount / 3, _hoverAmount / 3, 0);
        }
    }

    private void OnMouseUp()
    {
        _isDragging = false;

        // If the card is released in the upper half of the screen, move it to the table slot
        if (Input.mousePosition.y > Screen.height / 2)
        {
            //unparent the card
            transform.parent = null;

            // Move and rotate the card
            transform.position = firstCardSlot.transform.position + new Vector3(0,firstCardSlot.transform.localScale.z,0);
            transform.rotation = firstCardSlot.transform.rotation;

            // Update the original position to the new position
            _originalPosition = transform.position;

            // Remove the card from the hand
            cardsInHand.Remove(gameObject); // Add this line
        }
        else
        {
            // If drop is not allowed, return the card to its original position
            transform.position = _originalPosition;
        }
    }
}