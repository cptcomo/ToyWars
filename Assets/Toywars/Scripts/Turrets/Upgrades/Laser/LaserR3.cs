using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class LaserR3 : TowerUpgrade {
        public float pctDamageModifierPerSecond;

        public override void activate(Turret turret) {
            turret.laserR3Upgrade(pctDamageModifierPerSecond);
        }
    }
}
