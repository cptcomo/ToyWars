using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class QLevel1 : AbilityUpgrade {
        public override void activate(Player player) {
            Q q = (Q)player.Q;
            q.level++;
        }
    }
}
