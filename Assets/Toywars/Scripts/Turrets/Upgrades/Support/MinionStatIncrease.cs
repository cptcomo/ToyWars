using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class MinionStatIncrease : TowerUpgrade {
        public float damage, speed;

        public override void activate(Turret turret) {
            turret.beaconMinionDamage.modifyFlat(damage);
            turret.beaconMinionMovespeed.modifyFlat(speed);
        }
    }
}

