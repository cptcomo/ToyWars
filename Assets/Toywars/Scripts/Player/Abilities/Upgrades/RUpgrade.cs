using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class RUpgrade : AbilityUpgrade {
        public float cdReduce;
        public float shotsInc;
        public float dmgPerBulletInc;
        public float rangeInc;
        public override void activate(Player player) {
            R r = (R)player.R;
            r.cooldown -= cdReduce;
            r.numOfCasts += shotsInc;
            r.damagePerProjectile += dmgPerBulletInc;
            r.range += rangeInc;
        }
    }
}

