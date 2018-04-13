using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class W : InstantAbility {
        public float pct;
        public float duration;
        public override void activate(Player player) {
            MovementSpeedBuff msb = new MovementSpeedBuff(duration, pct);
            msb.apply(player);
            nextFire = Time.time + cooldown;
        }
    }

}
