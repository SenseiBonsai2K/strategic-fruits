#region

using System;
using JetBrains.Annotations;
using UnityEngine;

#endregion

/// <summary>
///     Controls the behavior of a card in the game.
/// </summary>
public sealed class CardController : MonoBehaviour
{
    public GameManager GameManager;
    private GameObject _firstCardSlot;
    private float _hoverAmount; // The scale to apply when hovering
    private bool _isDragging;
    private Vector3 _offset;
    private Vector3 _originalPosition; // The target position of the GameObject
    private Vector3 _originalScale; // The original scale of the GameObject
    private Vector3 _screenPoint;
    private GameObject _secondCardSlot;

    /// <summary>
    ///     Initializes the card controller at the start of the game.
    /// </summary>
    private void Start()
    {
        _originalScale = transform.localScale;
        _hoverAmount = 0.35f;
        _originalPosition = transform.position;
        _firstCardSlot = GameObject.Find(gameObject.tag + "FirstSlot");
        _secondCardSlot = GameObject.Find(gameObject.tag + "SecondSlot");
        GameManager = FindObjectOfType<GameManager>();
    }

    private void OnMouseDown()
    {
        if (!IsChildOfCurrentHand())
        {
            return;
        }

        _isDragging = true;
        transform.localScale = _originalScale;
        Camera currentCamera = FindObjectOfType<CameraManager>().GetCurrentCamera();
        _screenPoint = currentCamera.WorldToScreenPoint(gameObject.transform.position);
        _offset = gameObject.transform.position - GetWorldPositionFromMouse(currentCamera);
    }

    private void OnMouseDrag()
    {
        if (!IsChildOfCurrentHand())
        {
            return;
        }

        _isDragging = true;
        Camera currentCamera = FindObjectOfType<CameraManager>().GetCurrentCamera();
        Vector3 curPosition = GetWorldPositionFromMouse(currentCamera) + _offset;
        transform.position = curPosition;
    }

    private void OnMouseExit()
    {
        if (_isDragging || !IsChildOfCurrentHand())
        {
            return;
        }

        transform.position = _originalPosition;
        transform.localScale = _originalScale;
    }

    private void OnMouseOver()
    {
        if (_isDragging || !IsChildOfCurrentHand())
        {
            return;
        }

        UpdateTransform(_originalPosition + new Vector3(0, _hoverAmount / 2, 0),
            _originalScale + new Vector3(_hoverAmount / 3, _hoverAmount / 3, 0));
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        if (!IsChildOfCurrentHand()) return;
        if (ShouldReturnToHand())
        {
            UpdateTransform(_originalPosition, _originalScale);
            return;
        }

        if (GameManager.IsCoroutinesRunning)
        {
            return;
        }

        MoveCardToNextSlot();
        _originalPosition = transform.position;
        StartCoroutine(FindObjectOfType<CameraManager>().NextCameraWithDelay());
    }

    /// <summary>
    ///     Converts the mouse position to world position.
    /// </summary>
    /// <param name="currentCamera">The current camera.</param>
    /// <returns>The world position of the mouse.</returns>
    private Vector3 GetWorldPositionFromMouse([NotNull] Camera currentCamera)
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
        return currentCamera.ScreenToWorldPoint(curScreenPoint);
    }

    /// <summary>
    ///     Updates the transform of the card.
    /// </summary>
    /// <param name="position">The new position.</param>
    /// <param name="scale">The new scale.</param>
    private void UpdateTransform(Vector3 position, Vector3 scale)
    {
        transform.position = position;
        transform.localScale = scale;
    }

    /// <summary>
    ///     Moves the card to the appropriate slot.
    /// </summary>
    private void MoveCardToNextSlot()
    {
        MoveCardToSpecificSlot(!GameManager.FirstCardsSolved ? _firstCardSlot : _secondCardSlot);
    }

    /// <summary>
    ///     Determines whether the card should return to the hand.
    /// </summary>
    /// <returns>True if the card should return to the hand, false otherwise.</returns>
    private bool ShouldReturnToHand()
    {
        return (GameManager.CurrentPhase == 1 && _firstCardSlot.transform.childCount > 0) ||
               (GameManager.CurrentPhase != 1 && _secondCardSlot.transform.childCount > 0 &&
                GameManager.FirstCardsSolved) ||
               Input.mousePosition.y <= Screen.height / 3;
    }

    /// <summary>
    ///     Moves the card to the specified slot.
    /// </summary>
    /// <param name="slot">The slot to move the card to.</param>
    private void MoveCardToSpecificSlot([NotNull] GameObject slot)
    {
        Vector3 globalScale = transform.lossyScale;
        transform.SetParent(slot.transform, false);
        transform.localScale = new Vector3(globalScale.x / transform.parent.lossyScale.x,
            globalScale.y / transform.parent.lossyScale.y,
            globalScale.z / transform.parent.lossyScale.z);
        transform.position = slot.transform.position +
                             new Vector3(0, slot.transform.localScale.z, 0);
       transform.rotation = slot.transform.rotation * Quaternion.Euler(0, 180, 0);
    }

    /// <summary>
    ///     Determines whether the card is a child of the current hand.
    /// </summary>
    /// <returns>True if the card is a child of the current hand, false otherwise.</returns>
    private bool IsChildOfCurrentHand()
    {
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        string currentCameraTag = cameraManager.GetCurrentCamera().tag;
        return transform.parent != null && transform.parent.name == currentCameraTag + "Hand";
    }

    /// <summary>
    ///     Resets the hover state of the card.
    /// </summary>
    internal void ResetHover()
    {
        if (_isDragging)
        {
            return;
        }

        if (!transform.parent || !transform.parent.name.EndsWith("Hand", StringComparison.Ordinal))
        {
            return;
        }

        transform.position = _originalPosition;
        transform.localScale = _originalScale;
    }

    public void InvokeOnMouseDown()
    {
        OnMouseDown();
    }

    public bool? IsDragging()
    {
        return _isDragging;
    }

    public void InvokeOnMouseUp()
    {
        OnMouseUp();
    }

    public void InvokeOnMouseOver()
    {
        OnMouseOver();
    }

    public Vector3 GetOriginalScale()
    {
        return _originalScale;
    }

    public void InvokeOnMouseExit()
    {
        OnMouseExit();
    }

    public void InvokeResetHover()
    {
        ResetHover();
    }
}