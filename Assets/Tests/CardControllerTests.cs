#region

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#endregion
namespace Tests
{
    public sealed class CardControllerTests : MonoBehaviour
    {
        private CardController _cardController;
        private GameManager _gameManager;

        [SetUp]
        public void SetUp()
        {
            GameObject gameManagerObject = new GameObject();
            _gameManager = gameManagerObject.AddComponent<GameManager>();
            _gameManager.FirstSlots = new List<GameObject>
            {
                new GameObject()
            };
            _gameManager.SecondSlots = new List<GameObject>
            {
                new GameObject()
            };

            GameObject cameraManagerObject = new GameObject();
            CameraManager cameraManager = cameraManagerObject.AddComponent<CameraManager>();
            cameraManager.Cameras = new List<Camera>
            {
                cameraManagerObject.AddComponent<Camera>()
            };

            GameObject handObject = new GameObject
            {
                name = cameraManager.GetCurrentCamera().tag + "Hand"
            };

            GameObject cardControllerObject = new GameObject();
            _cardController = cardControllerObject.AddComponent<CardController>();
            _cardController.GameManager = _gameManager;
            _cardController.transform.SetParent(handObject.transform, false);
        }

        [TearDown]
        public void TearDown()
        {
            Destroy(_cardController.gameObject);
            Destroy(_gameManager.gameObject);
        }

        [UnityTest]
        public IEnumerator OnMouseDown_WhenIsChildOfCurrentHand_IsDraggingIsTrue()
        {
            _cardController.InvokeOnMouseDown();

            yield return null;

            Assert.IsTrue(_cardController.IsDragging());
        }

        // [UnityTest]
        // public IEnumerator OnMouseUp_WhenShouldReturnToHand_IsDraggingIsFalse()
        // {
        //     _cardController.InvokeOnMouseDown();
        //     _cardController.InvokeOnMouseUp();
        //
        //     yield return null;
        //
        //     Assert.IsFalse(_cardController.IsDragging());
        // }

        [UnityTest]
        public IEnumerator OnMouseOver_WhenNotDraggingAndIsChildOfCurrentHand_ScalesCard()
        {
            Vector3 originalScale = _cardController.GetOriginalScale();

            _cardController.InvokeOnMouseOver();

            yield return null;

            Assert.AreNotEqual(originalScale, _cardController.transform.localScale);
        }

        [UnityTest]
        public IEnumerator OnMouseExit_WhenNotDraggingAndIsChildOfCurrentHand_ResetsCard()
        {
            Vector3 originalPosition = _cardController.transform.position;
            Vector3 originalScale = _cardController.GetOriginalScale();

            _cardController.InvokeOnMouseOver();
            _cardController.InvokeOnMouseExit();

            yield return null;

            Assert.AreEqual(originalPosition, _cardController.transform.position);
            Assert.AreEqual(originalScale, _cardController.transform.localScale);
        }

        [UnityTest]
        public IEnumerator ResetHover_WhenNotDraggingAndIsChildOfCurrentHand_ResetsCard()
        {
            Vector3 originalPosition = _cardController.transform.position;
            Vector3 originalScale = _cardController.GetOriginalScale();

            _cardController.InvokeOnMouseOver();
            _cardController.InvokeResetHover();

            yield return null;

            Assert.AreEqual(originalPosition, _cardController.transform.position);
            Assert.AreEqual(originalScale, _cardController.transform.localScale);
        }
    }
}
