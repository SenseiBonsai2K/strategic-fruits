#region

using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

#endregion

namespace Tests
{
    public sealed class GameManagerTests
    {
        private List<GameObject> _firstSlots;
        private GameManager _gameManager;
        private List<GameObject> _secondSlots;

        [SetUp]
        public void SetUp()
        {
            _gameManager = new GameObject().AddComponent<GameManager>();

            _gameManager.Hands = new List<GameObject>();

            _firstSlots = new List<GameObject>
            {
                new GameObject(),
                new GameObject(),
                new GameObject()
            };
            _gameManager.FirstSlots = _firstSlots;

            _secondSlots = new List<GameObject>
            {
                new GameObject(),
                new GameObject(),
                new GameObject()
            };
            _gameManager.SecondSlots = _secondSlots;
        }

        [Test]
        public void AdvanceRound_IncrementsCurrentRound()
        {
            int initialRound = _gameManager.CurrentRound;
            _gameManager.InvokeAdvanceRound();
            Assert.AreEqual(initialRound + 1, _gameManager.CurrentRound);
        }
    }
}
