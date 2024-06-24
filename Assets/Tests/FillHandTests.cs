#region

using System.Linq;
using NUnit.Framework;
using UnityEngine;

#endregion
namespace Tests
{
    public sealed class FillHandTests
    {
        private FillHand _fillHand;

        [SetUp]
        public void SetUp()
        {
            GameObject fillHandGameObject = new GameObject();
            _fillHand = fillHandGameObject.AddComponent<FillHand>();
            _fillHand.CardPrefab = Resources.Load<GameObject>("Prefabs/Card");
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_fillHand.gameObject);
        }

        [Test]
        public void Start_GeneratesCorrectNumberOfCards()
        {
            _fillHand.Start();
            Assert.AreEqual(12, _fillHand.Cards.Length);
        }

        [Test]
        public void Start_GeneratesCardsWithCorrectRanksAndSuits()
        {
            _fillHand.Start();

            for (int i = 0; i < _fillHand.Cards.Length; i++)
            {
                int expectedRank = i < 4 ? 1 : i < 7 ? 2 : i < 9 ? 3 : i < 11 ? 4 : 5;
                Assert.AreEqual(expectedRank, _fillHand.Cards[i].Rank);
                Assert.AreEqual(_fillHand.gameObject.tag, _fillHand.Cards[i].Suit);
            }
        }

        [Test]
        public void Start_CreatesCardGameObjectsWithCorrectNamesAndTags()
        {
            _fillHand.Start();

            foreach (GameObject cardGameObject in _fillHand.Cards.Select(static t => GameObject.Find(t.Rank + " of " + t.Suit)))
            {
                Assert.IsNotNull(cardGameObject);
                Assert.AreEqual(_fillHand.gameObject.tag, cardGameObject.tag);
            }
        }

        [Test]
        public void Start_CreatesCardGameObjectsWithCorrectPositionsAndRotations()
        {
            _fillHand.Start();

            for (int i = 0; i < _fillHand.Cards.Length; i++)
            {
                GameObject cardGameObject = GameObject.Find(_fillHand.Cards[i].Rank + " of " + _fillHand.Cards[i].Suit);
                Vector3 expectedPosition = CalculateExpectedPosition(i);

                Vector3 actualPosition = cardGameObject.transform.localPosition;

                Assert.That(actualPosition.x, Is.EqualTo(expectedPosition.x).Within(0.5f));
                Assert.That(actualPosition.y, Is.EqualTo(expectedPosition.y).Within(0.1f));
                Assert.That(actualPosition.z, Is.EqualTo(expectedPosition.z).Within(0.1f));

                Assert.That(cardGameObject.transform.rotation.eulerAngles.x, Is.EqualTo(35).Within(0.1f));
            }
        }

        private static Vector3 CalculateExpectedPosition(int index)
        {
            return new Vector3((FillHand.GetXOffset() * (index - 5.5f)), (FillHand.GetYOffset() - index * FillHand.GetYOffset()), (FillHand.GetZOffset() + index * FillHand.GetZOffset()));
        }
    }
}
