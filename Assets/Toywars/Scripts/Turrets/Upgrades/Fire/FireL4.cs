using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class FireL4 : TowerUpgrade {
        public float ablazeTickFactor;

        public override void activate(Turret turret) {
            turret.fireL4Upgrade(ablazeTickFactor);
        }
    }
}

