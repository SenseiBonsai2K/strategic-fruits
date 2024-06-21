using UnityEngine;

public class CardController : MonoBehaviour
{
    private GameObject _firstCardSlot;
    private float _hoverAmount; // The scale to apply when hovering
    private bool _isDragging;
    private Vector3 _offset;
    private Vector3 _originalPosition; // The target position of the GameObject
    private Vector3 _originalScale; // The original scale of the GameObject
    private Vector3 _screenPoint;
    private GameObject _secondCardSlot;
    public GameManager gameManager;

    private void Start()
    {
        // Store the original scale
        _originalScale = transform.localScale;
        _hoverAmount = 0.35f; // Set the hover amount
        _originalPosition = transform.position; // Store the original position
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
        _offset = gameObject.transform.position -
                  currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                      _screenPoint.z));
    }

    private void OnMouseDrag()
    {
        if (!IsChildOfCurrentHand()) return;
        _isDragging = true;
        var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
        var currentCamera = FindObjectOfType<CameraManager>().GetCurrentCamera();
        var curPosition = currentCamera.ScreenToWorldPoint(curScreenPoint) + _offset;
        transform.position = curPosition;
    }

    private void OnMouseExit()
    {
        if (_isDragging || !IsChildOfCurrentHand()) return;
        // Restore the original scale when the mouse is not over it
        transform.position = _originalPosition;
        transform.localScale = _originalScale;
    }

    private void OnMouseOver()
    {
        if (_isDragging || !IsChildOfCurrentHand()) return;
        {
            // Scale the GameObject up when the mouse is over it
            transform.position = _originalPosition + new Vector3(0, _hoverAmount / 2, 0);
            transform.localScale = _originalScale + new Vector3(_hoverAmount / 3, _hoverAmount / 3, 0);
        }
    }

    private void OnMouseUp()
    {
        _isDragging = false;

        if (ShouldReturnToHand())
        {
            transform.position = _originalPosition;
        }
        else
        {
            if (!gameManager.firstCardsSolved)
                MoveCardToFirstSlot();
            else
                MoveCardToSecondSlot();
            _originalPosition = transform.position;
            StartCoroutine(FindObjectOfType<CameraManager>().NextCameraWithDelay());
        }
    }

    private bool ShouldReturnToHand()
    {
        return ((gameManager.currentPhase == 1 && _firstCardSlot.transform.childCount > 0) ||
                (gameManager.currentPhase != 1 && _secondCardSlot.transform.childCount > 0 &&
                 gameManager.firstCardsSolved) ||
                Input.mousePosition.y <= Screen.height / 3);
    }

    private void MoveCardToFirstSlot()
    {
        var globalScale = transform.lossyScale;
        transform.SetParent(_firstCardSlot.transform, false);
        transform.localScale = new Vector3(globalScale.x / transform.parent.lossyScale.x,
            globalScale.y / transform.parent.lossyScale.y,
            globalScale.z / transform.parent.lossyScale.z);
        transform.position = _firstCardSlot.transform.position +
                             new Vector3(0, _firstCardSlot.transform.localScale.z, 0);
        transform.rotation = _firstCardSlot.transform.rotation;
    }

    private void MoveCardToSecondSlot()
    {
        var globalScale = transform.lossyScale;
        transform.SetParent(_secondCardSlot.transform, false);
        transform.localScale = new Vector3(globalScale.x / transform.parent.lossyScale.x,
            globalScale.y / transform.parent.lossyScale.y,
            globalScale.z / transform.parent.lossyScale.z);
        transform.position = _secondCardSlot.transform.position +
                             new Vector3(0, _secondCardSlot.transform.localScale.z, 0);
        transform.rotation = _secondCardSlot.transform.rotation;
    }

    private bool IsChildOfCurrentHand()
    {
        // Get the tag of the current camera
        var cameraManager = FindObjectOfType<CameraManager>();
        var currentCameraTag = cameraManager.GetCurrentCamera().tag;

        // Check if the parent's name is equal to the current camera tag + "Hand"
        return transform.parent != null && transform.parent.name == currentCameraTag + "Hand";
    }

    public void ResetHover()
    {
        if (_isDragging) return;

        // Check if the parent's name ends with "Hand"
        if (!transform.parent || !transform.parent.name.EndsWith("Hand")) return;
        transform.position = _originalPosition;
        transform.localScale = _originalScale;
    }
}