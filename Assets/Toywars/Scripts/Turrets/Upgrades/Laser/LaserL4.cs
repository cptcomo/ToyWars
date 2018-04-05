using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class LaserL4 : TowerUpgrade {
        public float pctMax;
        public float pctRangeInc;
        public override void activate(Turret turret) {
            turret.laserL4Upgrade(pctMax);
            turret.range.modifyPct(pctRangeInc);
        }
    }
}
