using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class DOT : TowerUpgrade {
        public int flatDmg;
        public float pct;
        public override void activate(Turret turret) {
            turret.updateDOT(pct);
            turret.updateDOT(flatDmg);
        }
    }
}
