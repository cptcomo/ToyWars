using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class FireR3 : TowerUpgrade {
        public float explosionRadius;

        public override void activate(Turret turret) {
            turret.fireR3Upgrade(explosionRadius);
        }
    }
}
