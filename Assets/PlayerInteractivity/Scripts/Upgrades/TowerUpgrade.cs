using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public abstract class TowerUpgrade : MonoBehaviour {
        public string upgradeName;
        public int cost;

        public abstract void activate(Turret turret);
    }
}
