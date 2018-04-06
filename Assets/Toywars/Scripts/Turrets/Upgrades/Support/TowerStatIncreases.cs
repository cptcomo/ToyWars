using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class TowerStatIncreases : TowerUpgrade {
        public float fireRate, range, damage, laserDOT, fireDOT;
        public override void activate(Turret turret) {
            turret.beaconTurretFireRate.modifyFlat(fireRate);
            turret.beaconTurretRange.modifyFlat(range);
            turret.beaconTurretDamage.modifyFlat(damage);
            turret.beaconTurretLaserDOT.modifyFlat(laserDOT);
            turret.beaconTurretFireDOT.modifyFlat(fireDOT);
        }
    }
}
