using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class QUpgrade : AbilityUpgrade {
        public float cdReduce;
        public float damageInc;
        public override void activate(Player player) {
            Q q = (Q)player.Q;
            q.damage += damageInc;
            q.cooldown -= cdReduce;
        }
    }
}
