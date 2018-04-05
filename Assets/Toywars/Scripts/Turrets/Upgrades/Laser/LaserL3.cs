using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class LaserL3 : TowerUpgrade {
        public float pctMissingHealth;
        public float pctRangeInc;

        public override void activate(Turret turret) {
            turret.laserL3Upgrade(pctMissingHealth);
            turret.range.modifyPct(pctRangeInc);
        }
    }
}
