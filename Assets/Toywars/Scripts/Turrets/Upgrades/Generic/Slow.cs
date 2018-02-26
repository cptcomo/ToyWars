using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class Slow : TowerUpgrade {
        public bool isPctBuff;
        public float pct;
        public int flat;
        public override void activate(Turret turret) {
            if(isPctBuff)
                turret.slowPct.modifyPct(pct);
            else
                turret.slowPct.modifyFlat(flat);
        }
    }
}
