using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class ArmorShredBuff : Buff {
        private float duration;
        private float flat;

        float startTime;
        Minion minion;

        public ArmorShredBuff(float dur, float flat) {
            this.duration = dur;
            this.flat = flat;
        }

        public bool finished
        {
            get {
                return Time.time > startTime + duration;
            }
        }

        public void apply(Component target) {         
            startTime = Time.time;
            minion = target.GetComponent<Minion>();
            minion.addBuff(this);
        }

        public void tick() {
            minion.armor.buffFlat(-flat, -25, 1000);
        }

        public Buff copy() {
            return new ArmorShredBuff(duration, flat);
        }

        public void finish() {
            minion.removeBuff(this);
        }
    }
}

