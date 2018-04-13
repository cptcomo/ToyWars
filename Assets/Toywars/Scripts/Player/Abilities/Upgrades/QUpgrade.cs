using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class QUpgrade : AbilityUpgrade {
        public float cdReduce;
        public float dmgInc;
        public float rangeInc;
        public override void activate(Player player) {
            Q q = (Q)player.Q;
            q.level++;
            q.damage += dmgInc;
            q.cooldown -= cdReduce;
            q.range += rangeInc;
        }
    }
}

