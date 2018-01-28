using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    [System.Serializable]
    public class TurretBlueprint {
        public GameObject prefab;
        public int cost;
        
        public int getSellAmount() {
            return this.cost / 2;
        }
    }
}
