using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Persistence
{
    [Serializable]
    public class GameState
    {
        private const int TotalCollectables = 30;

        public int collectedCount;
        public Vector3 playerPosition = Vector3.zero;
        public List<Collectable> collectables = new();
        public long lastUpdatedAt;

        public int PercentageComplete =>
            Convert.ToInt32(decimal.Divide(collectedCount, TotalCollectables) * 100);
    }

    [Serializable]
    public class Collectable
    {
        public string id;
        public bool collected;
    }
}
