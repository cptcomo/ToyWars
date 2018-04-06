using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class DamageModifierBuff : Buff {
        float duration, pct, startTime;

        Minion minion;

        public DamageModifierBuff(float dur, float pct) {
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
            minion = target.GetComponent<Minion>();
            minion.addBuff(this);
        }

        public Buff copy() {
            return new DamageModifierBuff(duration, pct);
        }

        public void finish() {
            minion.removeBuff(this);
        }

        public void tick() {
            minion.damageModifier.buffFlat(pct, 50, 150);
        }
    }
}
