using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class SlowDebuff : Buff {
        float duration, pct, startTime;

        Minion minion;

        public SlowDebuff(float dur, float pct) {
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

            List<Buff> buffsToRemove = new List<Buff>();

            foreach(Buff b in minion.buffs) {
                if(b is SlowDebuff) {
                    SlowDebuff sb = (SlowDebuff)b;
                    if(this.pct <= sb.pct) {
                        return;
                    }
                    else {
                        buffsToRemove.Add(sb);
                    }
                }
            }

            buffsToRemove.ForEach(buff => buff.finish());

            minion.addBuff(this);
        }

        public Buff copy() {
            return new SlowDebuff(this.duration, this.pct);
        }

        public void tick() {
            minion.getMinionMovement().speed.buffPct(-pct);
        }

        public void finish() {
            minion.removeBuff(this);
        }
    }
}
