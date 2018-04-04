using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class StandardR3 : TowerUpgrade {
        public float flatDamageInc;
        public float pctFireRateDec;
        public float pctRangeInc;
        public override void activate(Turret turret) {
            turret.damage.modifyFlat(flatDamageInc);
            turret.fireRate.modifyPct(-pctFireRateDec, 0.01f, 1000);
            turret.range.modifyPct(pctRangeInc);
        }
    }
}
