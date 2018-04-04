using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class MissileR3 : TowerUpgrade {
        public float flatArmorShred;

        public override void activate(Turret turret) {
            turret.missileR3Upgrade(flatArmorShred);
        }
    }
}

