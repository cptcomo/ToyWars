using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class StandardL4 : TowerUpgrade {
        public float flatRateInc;
        public float flatDamageDec;
        public float pctRangeInc;
        public override void activate(Turret turret) {
            turret.fireRate.modifyFlat(flatRateInc);
            turret.damage.modifyFlat(-flatDamageDec);
            turret.range.modifyPct(pctRangeInc);
        }
    }
}

