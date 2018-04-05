using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class FireL2 : TowerUpgrade {
        public float dotInterval;
        public float duration;

        public override void activate(Turret turret) {
            turret.fireL2Upgrade(dotInterval, duration);
        }
    }
}

