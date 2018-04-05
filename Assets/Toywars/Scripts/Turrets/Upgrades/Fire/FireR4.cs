using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class FireR4 : TowerUpgrade {
        public float radius;
        public float dps;

        public override void activate(Turret turret) {
            turret.fireR4Upgrade(radius, dps);
        }
    }
}
