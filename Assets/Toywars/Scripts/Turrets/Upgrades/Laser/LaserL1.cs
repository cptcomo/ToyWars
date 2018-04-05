using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class LaserL1 : TowerUpgrade {
        public float flatDotIncrease;

        public override void activate(Turret turret) {
            turret.laserDOT.modifyFlat(flatDotIncrease);
        }
    }
}

