using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Manages the cameras in the game.
/// </summary>
public class CameraManager : MonoBehaviour
{
    /// <summary>
    ///     List of cameras in the game.
    /// </summary>
    public List<Camera> cameras;

    /// <summary>
    ///     The index of the currently active camera.
    /// </summary>
    private int _currentCameraIndex;

    /// <summary>
    ///     Initializes the CameraManager at the start of the game.
    /// </summary>
    private void Start()
    {
        _currentCameraIndex = 0;
        SetActiveCamera(_currentCameraIndex);
    }

    /// <summary>
    ///     Returns the currently active camera.
    /// </summary>
    /// <returns>The currently active camera.</returns>
    public Camera GetCurrentCamera()
    {
        return cameras[_currentCameraIndex];
    }

    /// <summary>
    ///     Switches to the next camera after a delay.
    /// </summary>
    /// <returns>An IEnumerator to be used in a coroutine.</returns>
    public IEnumerator NextCameraWithDelay()
    {
        yield return new WaitForSeconds(1f);
        cameras[_currentCameraIndex].gameObject.SetActive(false);

        foreach (var card in FindObjectsOfType<CardController>()) card.ResetHover();

        _currentCameraIndex = (_currentCameraIndex + 1) % cameras.Count;
        SetActiveCamera(_currentCameraIndex);
    }

    /// <summary>
    ///     Sets the active camera based on the provided index.
    /// </summary>
    /// <param name="index">The index of the camera to be activated.</param>
    private void SetActiveCamera(int index)
    {
        foreach (var camera in cameras) camera.gameObject.SetActive(camera == cameras[index]);
    }
}