using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class Range : TowerUpgrade {
        public bool isPctBuff;
        public float pct;
        public int flat;

        public override void activate(Turret turret) {
            if(isPctBuff)
                turret.range.modifyPct(pct);
            else
                turret.range.modifyFlat(flat);
        }
    }
}
