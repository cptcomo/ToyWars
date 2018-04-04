using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class StandardL3 : TowerUpgrade {
        public int numberOfHits;
        public float damagePct;
        public override void activate(Turret turret) {
            turret.turretL3Upgrade(numberOfHits, damagePct);
        }
    }
}

