#region

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#endregion

namespace Tests
{
    public sealed class CardManagerTests : MonoBehaviour
    {
        private CardManager _cardManager;
        private GameManager _gameManager;
        public Card Card { get; set; }

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
            _cardManager = cardControllerObject.AddComponent<CardManager>();
            _cardManager.GameManager = _gameManager;
            _cardManager.transform.SetParent(handObject.transform, false);
        }

        [TearDown]
        public void TearDown()
        {
            Destroy(_cardManager.gameObject);
            Destroy(_gameManager.gameObject);
        }

        [UnityTest]
        public IEnumerator OnMouseDown_WhenIsChildOfCurrentHand_IsDraggingIsTrue()
        {
            _cardManager.InvokeOnMouseDown();

            yield return null;

            Assert.IsTrue(_cardManager.IsDragging());
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
            Vector3 originalScale = _cardManager.GetOriginalScale();

            _cardManager.InvokeOnMouseOver();

            yield return null;

            Assert.AreNotEqual(originalScale, _cardManager.transform.localScale);
        }

        [UnityTest]
        public IEnumerator OnMouseExit_WhenNotDraggingAndIsChildOfCurrentHand_ResetsCard()
        {
            Vector3 originalPosition = _cardManager.transform.position;
            Vector3 originalScale = _cardManager.GetOriginalScale();

            _cardManager.InvokeOnMouseOver();
            _cardManager.InvokeOnMouseExit();

            yield return null;

            Assert.AreEqual(originalPosition, _cardManager.transform.position);
            Assert.AreEqual(originalScale, _cardManager.transform.localScale);
        }

        [UnityTest]
        public IEnumerator ResetHover_WhenNotDraggingAndIsChildOfCurrentHand_ResetsCard()
        {
            Vector3 originalPosition = _cardManager.transform.position;
            Vector3 originalScale = _cardManager.GetOriginalScale();

            _cardManager.InvokeOnMouseOver();
            _cardManager.InvokeResetHover();

            yield return null;

            Assert.AreEqual(originalPosition, _cardManager.transform.position);
            Assert.AreEqual(originalScale, _cardManager.transform.localScale);
        }

        //[UnityTest]
        //public IEnumerator SaveCardAndTime_SavesCardAndTime()
        //{
        //CardTracker.SaveCardAndTime(Card);
        //yield return null;
        //Assert.AreEqual(1, CardTracker._playedCards.Count);
        //Assert.AreEqual(Card, CardTracker._playedCards[0].Item1);
        //Assert.AreEqual(0, CardTracker._playedCards[0].Item2);
        //}
    }
}