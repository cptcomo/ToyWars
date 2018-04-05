using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class Slow : TowerUpgrade {
        public float setSlowPct;
        public float flatDmgDec;

        public override void activate(Turret turret) {
            turret.laserSlowPct.set(setSlowPct);
            turret.laserDOT.modifyFlat(-flatDmgDec);
        }
    }
}
