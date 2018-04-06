using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class SupportR4 : TowerUpgrade {
        public float money, exp;

        public override void activate(Turret turret) {
            if(turret.isPlayer) {
                PlayerManager.getInstance().moneyAmplify += money;
                PlayerManager.getInstance().expAmplify += exp;
            }
            else {
                EnemiesManager.getInstance().moneyAmplify += money + exp;
            }
        }
    }
}
