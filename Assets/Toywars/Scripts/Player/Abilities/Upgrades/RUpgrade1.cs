using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class RUpgrade1 : AbilityUpgrade {
        public override void activate(Player player) {
            R r = (R)player.R;
            r.level++;
        }
    }
}
