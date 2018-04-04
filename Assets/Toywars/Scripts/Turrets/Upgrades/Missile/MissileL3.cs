using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class MissileL3 : TowerUpgrade {
        public float pctExplosionRadiusInc;
        public float flatDmgInc;
        public float flatRangeInc;

        public override void activate(Turret turret) {
            turret.missileExplosionRadius.modifyPct(pctExplosionRadiusInc);
            turret.damage.modifyFlat(flatDmgInc);
            turret.range.modifyFlat(flatRangeInc);
        }
    }
}
