using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class WLevelIncreaser : AbilityUpgrade {
        public override void activate(Player player) {
            W w = (W)player.W;
            w.level++;
        }
    }

}
