using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class W : InstantAbility {
        public float pct;
        public float duration;
        public override void activate(Player player) {
            MovementSpeedBuff msb = new MovementSpeedBuff(duration, pct);
            msb.apply(player);
            player.addBuff(msb);
            nextFire = Time.time + cooldown;
        }
    }
}

