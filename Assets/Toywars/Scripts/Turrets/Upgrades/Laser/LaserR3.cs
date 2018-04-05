using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class LaserR3 : TowerUpgrade {
        public float pctDamageModifierDec;

        public override void activate(Turret turret) {
            turret.laserR3Upgrade(pctDamageModifierDec);
        }
    }
}
