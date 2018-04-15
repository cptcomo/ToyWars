using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class ELevel1 : AbilityUpgrade {
        public override void activate(Player player) {
            E e = (E)player.E;
            e.level++;
        }
    }
}
