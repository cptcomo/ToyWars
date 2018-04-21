using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class StandardR4 : TowerUpgrade {
        public float flatRangeInc;
        public float pctDamageInc;
        public float pctHeadshot;
        public float fireRateDec;
        public override void activate(Turret turret) {
            turret.turretR4Upgrade(pctHeadshot);
            turret.range.modifyFlat(flatRangeInc);
            turret.damage.modifyPct(pctDamageInc);
            turret.fireRate.modifyPct(-fireRateDec, 0.05f, turret.fireRate.get());
            turret.projectileSpeed.modifyPct(200);
        }
    }
}
