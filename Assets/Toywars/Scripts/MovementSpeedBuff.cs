using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class MovementSpeedBuff : Buff {
        private float duration;
        private float pct;

        float startTime;
        Player player;

        public MovementSpeedBuff(float dur, float pct) {
            this.duration = dur;
            this.pct = pct;
        }

        public bool finished
        {
            get {
                return Time.time > startTime + duration;
            }
        }

        public void apply(Component target) {
            startTime = Time.time;
            if(target as Player != null) {
                player = (Player)target;
            }
        }

        public void tick() {
            player.speed.modifyPct(pct);
        }
    }

}
