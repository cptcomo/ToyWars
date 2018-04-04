using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class MissileR4 : TowerUpgrade {
        public float pctRateInc;
        public float pctRangeInc;

        public override void activate(Turret turret) {
            turret.fireRate.modifyPct(pctRateInc);
            turret.range.modifyPct(pctRangeInc);
        }
    }
}
