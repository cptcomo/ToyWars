using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class MissileL4 : TowerUpgrade {
        public float pctTowerRangeInc;
        public float pctDmgInc;
        public float pctFireRateDec;
        public float pctExplosionRadiusInc;
        public override void activate(Turret turret) {
            turret.range.modifyPct(pctTowerRangeInc);
            turret.damage.modifyPct(pctDmgInc);
            turret.fireRate.modifyPct(-pctFireRateDec, 0.05f, 1000);
            turret.missileExplosionRadius.modifyPct(pctExplosionRadiusInc);
        }
    }
}

