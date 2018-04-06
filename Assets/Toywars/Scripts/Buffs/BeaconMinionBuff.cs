using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class BeaconMinionBuff : Buff {
        int id;
        float startTime, duration, pctDamage, pctMovespeed;

        Minion minion;

        public BeaconMinionBuff(int id, float duration, float pctDamage, float pctMovespeed) {
            this.id = id;
            this.duration = duration;
            this.pctDamage = pctDamage;
            this.pctMovespeed = pctMovespeed;
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
                if(b is BeaconMinionBuff) {
                    BeaconMinionBuff sb = (BeaconMinionBuff)b;
                    if(this.id == sb.id) {
                        buffsToRemove.Add(sb);
                    }
                }
            }

            buffsToRemove.ForEach(buff => buff.finish());

            minion.addBuff(this);
        }

        public Buff copy() {
            return new BeaconMinionBuff(this.id, this.duration, this.pctDamage, this.pctMovespeed);
        }

        public int getId() {
            return this.id;
        }

        public void finish() {
            minion.buffs.Remove(this);
        }

        public void tick() {
            minion.damage.buffPct(pctDamage);
            minion.getMinionMovement().speed.buffPct(pctMovespeed);
        }
    }
}

