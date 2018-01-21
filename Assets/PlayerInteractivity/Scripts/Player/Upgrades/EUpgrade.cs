using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class EUpgrade : AbilityUpgrade {
        public float cdReduce;
        public float shotsIncrease;
        public float dmgPerBulletIncrease;
        public override void activate(Player player) {
            E E = (E)player.E;
            E.cooldown -= cdReduce;
            E.numOfCasts += shotsIncrease;
            E.damagePerBullet += dmgPerBulletIncrease;
        }
    }
}

