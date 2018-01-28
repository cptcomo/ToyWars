using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    [System.Serializable]
    public class TurretBlueprint {
        public GameObject prefab;
        public int cost;
        public GameObject upgradedPrefab;
        public int upgradeCost;

        public int getSellAmount() {
            return this.cost / 2;
        }
    }
}
