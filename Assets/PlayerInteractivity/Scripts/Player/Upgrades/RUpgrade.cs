using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class RUpgrade : AbilityUpgrade {
        public float cdReduce;
        public float shotsIncrease;
        public float dmgPerBulletIncrease;
        public float rangeIncrease;
        public override void activate(Player player) {
            R R = (R)player.R;
            R.cooldown -= cdReduce;
            R.numOfCasts += shotsIncrease;
            R.damagePerProjectile += dmgPerBulletIncrease;
            R.range += rangeIncrease;
        }
    }
}

