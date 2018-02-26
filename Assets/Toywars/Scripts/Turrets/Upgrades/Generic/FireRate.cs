using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class FireRate : TowerUpgrade {
        public bool isPctBuff;
        public float pct;
        public int flatInc;
        public override void activate(Turret turret) {
            if(isPctBuff)
                turret.fireRate.modifyPct(pct);
            else
                turret.fireRate.modifyFlat(flatInc);
        }
    }
}

