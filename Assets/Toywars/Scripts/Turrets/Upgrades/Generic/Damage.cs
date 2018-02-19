using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class Damage : TowerUpgrade {
        public bool isPctBuff;
        public float pct;
        public int flatDmg;
        public override void activate(Turret turret) {
            if(isPctBuff)
                turret.damage.modifyPct(pct);
            else
                turret.damage.modifyFlat(flatDmg);
        }
    }
}
