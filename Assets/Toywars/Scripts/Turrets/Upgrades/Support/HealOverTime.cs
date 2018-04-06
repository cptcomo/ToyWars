using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class HealOverTime : TowerUpgrade {
        public float pctMissingPlayer, pctMissingMinion;

        public override void activate(Turret turret) {
            turret.beaconMinionHeal.set(pctMissingMinion);
            turret.beaconPlayerHeal.set(pctMissingPlayer);
        }
    }
}
