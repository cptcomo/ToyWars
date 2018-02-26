using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class DOT : TowerUpgrade {
        public bool isPctBuff;
        public float pct;
        public int flatInc;
        public override void activate(Turret turret) {
            if(isPctBuff)
                turret.dot.modifyPct(pct);
            else
                turret.dot.modifyFlat(flatInc);
        }
    }
}
