using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class LaserL2 : TowerUpgrade {
        public override void activate(Turret turret) {
            turret.laserL2Upgrade();
        }
    }
}
