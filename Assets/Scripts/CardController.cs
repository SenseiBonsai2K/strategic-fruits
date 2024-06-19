using UnityEngine;

public class CardController : MonoBehaviour
{
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
        // If drop is not allowed, return the card to its original position
        transform.position = _originalPosition;
        OnMouseExit(); // Add this line
    }
}