using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<Camera> cameras;
    private int _currentCameraIndex;

    private void Start()
    {
        _currentCameraIndex = 0;
        cameras.ForEach(camera => camera.gameObject.SetActive(camera == cameras[_currentCameraIndex]));
    }

    public Camera GetCurrentCamera()
    {
        return cameras[_currentCameraIndex];
    }

    public IEnumerator NextCameraWithDelay()
    {
        yield return new WaitForSeconds(1f);
        cameras[_currentCameraIndex].gameObject.SetActive(false);

        foreach (CardController card in FindObjectsOfType<CardController>())
        {
            card.ResetHover();
        }

        _currentCameraIndex = (_currentCameraIndex + 1) % cameras.Count;
        cameras[_currentCameraIndex].gameObject.SetActive(true);
    }
}