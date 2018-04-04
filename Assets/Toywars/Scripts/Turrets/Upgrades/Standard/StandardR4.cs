using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class StandardR4 : TowerUpgrade {
        public float flatRangeInc;
        public float pctDamageInc;
        public float pctHeadshot;
        public float fireRateDecFactor;
        public override void activate(Turret turret) {
            turret.turretR4Upgrade(pctHeadshot);
            turret.range.modifyFlat(flatRangeInc);
            turret.damage.modifyPct(pctDamageInc);
            turret.fireRate.modifyFlat(-(turret.fireRate.get() - turret.fireRate.get() / fireRateDecFactor));
            turret.projectileSpeed.modifyPct(200);
        }
    }
}
