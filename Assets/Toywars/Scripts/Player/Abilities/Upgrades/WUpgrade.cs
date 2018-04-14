using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class WUpgrade : AbilityUpgrade {
        public float cdReduce;
        public float durationInc;
        public float buffInc;
        public override void activate(Player player) {
            W w = (W)player.W;
            w.cooldown -= cdReduce;
            w.duration += durationInc;
            w.pct += buffInc;
            w.level++;
        }
    }
}
