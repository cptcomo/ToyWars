using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class ELevel3 : AbilityUpgrade {
        public float cdIncrease;
        public override void activate(Player player) {
            E e = (E)player.E;
            e.range += 1000f;
            e.cooldown += cdIncrease;
            e.level++;
        }
    }
}
