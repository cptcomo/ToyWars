using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class ExplosionRadius : TowerUpgrade {
        public bool isPctBuff;
        public float pct;
        public int flatInc;
        public override void activate(Turret turret) {
            if(isPctBuff)
                turret.missileExplosionRadius.modifyPct(pct);
            else
                turret.missileExplosionRadius.modifyFlat(flatInc);
        }
    }
}
