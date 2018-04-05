using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class FireL3 : TowerUpgrade {
        public float pctHealth;

        public override void activate(Turret turret) {
            turret.fireL3Upgrade(pctHealth);
        }
    }
}
