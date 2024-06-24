using UnityEngine;

/// <summary>
///     Controls the behavior of a card in the game.
/// </summary>
public class CardController : MonoBehaviour
{
    public GameManager gameManager;
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
        gameManager = FindObjectOfType<GameManager>();
    }
    
    private void OnMouseDown()
    {
        if (!IsChildOfCurrentHand()) return;
        _isDragging = true;
        transform.localScale = _originalScale;
        var currentCamera = FindObjectOfType<CameraManager>().GetCurrentCamera();
        _screenPoint = currentCamera.WorldToScreenPoint(gameObject.transform.position);
        _offset = gameObject.transform.position - GetWorldPositionFromMouse(currentCamera);
    }
    
    private void OnMouseDrag()
    {
        if (!IsChildOfCurrentHand()) return;
        _isDragging = true;
        var currentCamera = FindObjectOfType<CameraManager>().GetCurrentCamera();
        var curPosition = GetWorldPositionFromMouse(currentCamera) + _offset;
        transform.position = curPosition;
    }
    
    private void OnMouseExit()
    {
        if (_isDragging || !IsChildOfCurrentHand()) return;
        transform.position = _originalPosition;
        transform.localScale = _originalScale;
    }
    
    private void OnMouseOver()
    {
        if (_isDragging || !IsChildOfCurrentHand()) return;
        UpdateTransform(_originalPosition + new Vector3(0, _hoverAmount / 2, 0),
            _originalScale + new Vector3(_hoverAmount / 3, _hoverAmount / 3, 0));
    }
    
    private void OnMouseUp()
    {
        _isDragging = false;

        if (ShouldReturnToHand())
        {
            UpdateTransform(_originalPosition, _originalScale);
            return;
        }

        if (gameManager.isCoroutinesRunning) return;

        MoveCardToSlot();
        _originalPosition = transform.position;
        StartCoroutine(FindObjectOfType<CameraManager>().NextCameraWithDelay());
    }

    /// <summary>
    ///     Converts the mouse position to world position.
    /// </summary>
    /// <param name="currentCamera">The current camera.</param>
    /// <returns>The world position of the mouse.</returns>
    private Vector3 GetWorldPositionFromMouse(Camera currentCamera)
    {
        var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
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
    private void MoveCardToSlot()
    {
        if (!gameManager.firstCardsSolved)
            MoveCardToSlot(_firstCardSlot);
        else
            MoveCardToSlot(_secondCardSlot);
    }

    /// <summary>
    ///     Determines whether the card should return to the hand.
    /// </summary>
    /// <returns>True if the card should return to the hand, false otherwise.</returns>
    private bool ShouldReturnToHand()
    {
        return (gameManager.currentPhase == 1 && _firstCardSlot.transform.childCount > 0) ||
               (gameManager.currentPhase != 1 && _secondCardSlot.transform.childCount > 0 &&
                gameManager.firstCardsSolved) ||
               Input.mousePosition.y <= Screen.height / 3;
    }

    /// <summary>
    ///     Moves the card to the specified slot.
    /// </summary>
    /// <param name="slot">The slot to move the card to.</param>
    private void MoveCardToSlot(GameObject slot)
    {
        var globalScale = transform.lossyScale;
        transform.SetParent(slot.transform, false);
        transform.localScale = new Vector3(globalScale.x / transform.parent.lossyScale.x,
            globalScale.y / transform.parent.lossyScale.y,
            globalScale.z / transform.parent.lossyScale.z);
        transform.position = slot.transform.position +
                             new Vector3(0, slot.transform.localScale.z, 0);
        transform.rotation = slot.transform.rotation;
    }

    /// <summary>
    ///     Determines whether the card is a child of the current hand.
    /// </summary>
    /// <returns>True if the card is a child of the current hand, false otherwise.</returns>
    private bool IsChildOfCurrentHand()
    {
        var cameraManager = FindObjectOfType<CameraManager>();
        var currentCameraTag = cameraManager.GetCurrentCamera().tag;
        return transform.parent != null && transform.parent.name == currentCameraTag + "Hand";
    }

    /// <summary>
    ///     Resets the hover state of the card.
    /// </summary>
    public void ResetHover()
    {
        if (_isDragging) return;
        if (!transform.parent || !transform.parent.name.EndsWith("Hand")) return;
        transform.position = _originalPosition;
        transform.localScale = _originalScale;
    }
}