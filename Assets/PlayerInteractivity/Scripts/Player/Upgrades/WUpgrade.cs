using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class WUpgrade : AbilityUpgrade {
        public float cdReduce;
        public float durationInc;
        public float buffInc;
        public override void activate(Player player) {
            W W = (W)player.W;
            W.cooldown -= cdReduce;
            W.duration += durationInc;
            W.pct += buffInc;
        }
    }
}
