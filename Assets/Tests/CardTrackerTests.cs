#region

using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#endregion

namespace Tests
{
    public sealed class CardTrackerTests
    {
        private CardTracker _cardTracker;

        [SetUp]
        public void SetUp()
        {
            GameObject cardTrackerGameObject = new GameObject();
            _cardTracker = cardTrackerGameObject.AddComponent<CardTracker>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_cardTracker.gameObject);
        }
        
        [UnityTest]
        public IEnumerator ResetTimer_ResetsTimer()
        {
            CardTracker.ClearRoundData();
            yield return null;
            Assert.AreEqual(Time.time, 0, 0.3);
        }
    }
}