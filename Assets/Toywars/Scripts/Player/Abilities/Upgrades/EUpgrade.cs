using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class EUpgrade : AbilityUpgrade {
        public float cdReduce;
        public float shotsInc;
        public float dmgPerbulletInc;
        public override void activate(Player player) {
            E e = (E)player.E;
            e.cooldown -= cdReduce;
            e.numOfCasts += shotsInc;
            e.damagePerBullet += dmgPerbulletInc;
        }
    }
}
