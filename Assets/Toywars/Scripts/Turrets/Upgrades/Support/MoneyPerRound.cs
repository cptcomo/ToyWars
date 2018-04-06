using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class MoneyPerRound : TowerUpgrade {
        public float moneySetpoint;
        public override void activate(Turret turret) {
            turret.beaconMoneyPerRound.set(moneySetpoint);
        }
    }
}
