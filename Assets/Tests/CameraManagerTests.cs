#region

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#endregion

namespace Tests
{
    public sealed class CameraManagerTests
    {
        private CameraManager _cameraManager;
        private List<Camera> _cameras;

        [SetUp]
        public void SetUp()
        {
            _cameraManager = new GameObject().AddComponent<CameraManager>();
            _cameras = new List<Camera>
            {
                new GameObject().AddComponent<Camera>(),
                new GameObject().AddComponent<Camera>(),
                new GameObject().AddComponent<Camera>()
            };
            _cameraManager.Cameras = _cameras;
        }

        [Test]
        public void GetCurrentCamera_ReturnsActiveCamera()
        {
            Camera activeCamera = _cameraManager.GetCurrentCamera();
            Assert.AreEqual(_cameras[0], activeCamera);
        }

        [UnityTest]
        public IEnumerator NextCameraWithDelay_SwitchesToNextCameraAfterDelay()
        {
            yield return _cameraManager.InvokeNextCameraWithDelay();

            Camera activeCamera = _cameraManager.GetCurrentCamera();
            Assert.AreEqual(_cameras[1], activeCamera);
        }

        [UnityTest]
        public IEnumerator NextCameraWithDelay_WrapsAroundToFirstCamera()
        {
            yield return _cameraManager.InvokeNextCameraWithDelay();
            yield return _cameraManager.InvokeNextCameraWithDelay();
            yield return _cameraManager.InvokeNextCameraWithDelay();

            Camera activeCamera = _cameraManager.GetCurrentCamera();
            Assert.AreEqual(_cameras[0], activeCamera);
        }

        [Test]
        public void SetActiveCamera_SetsCorrectCameraAsActive()
        {
            _cameraManager.InvokeSetActiveCamera(2);
            Camera activeCamera = _cameraManager.GetCurrentCamera();
            Assert.AreEqual(_cameras[2].ToString(), activeCamera.ToString());
        }
    }
}
