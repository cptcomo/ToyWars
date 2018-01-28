using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class Damage : TowerUpgrade {
        public bool isPctBuff;
        public float pct;
        public int flatDmg;
        public override void activate(Turret turret) {
            if(isPctBuff)
                turret.updateDamage(pct);
            else turret.updateDamage(pct);
        }
    }
}
