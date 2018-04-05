using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class AblazeBuff : Buff {
        float duration, interval, startTime, damage, pctHealth;

        float lastDotTick;

        Minion minion;

        GameObject ablazeEffect;
        GameObject ablazeEffectInstance;

        public AblazeBuff(float dur, float damage, float interval, float pctHealth, GameObject ablazeEffect) {
            this.duration = dur;
            this.interval = interval;
            this.damage = damage;
            this.pctHealth = pctHealth;
            this.ablazeEffect = ablazeEffect;
        }

        public bool finished
        {
            get {
                return Time.time > startTime + duration;
            }
        }

        public void apply(Component target) {
            startTime = lastDotTick = Time.time;
            minion = target.GetComponent<Minion>();

            List<Buff> buffsToRemove = new List<Buff>();

            foreach(Buff b in minion.buffs) {
                if(b is AblazeBuff) {
                    AblazeBuff ab = (AblazeBuff)b;
                    if(getActualDamage() / interval < ab.getActualDamage() / ab.interval)
                        return;
                    else buffsToRemove.Add(ab);
                }
            }

            buffsToRemove.ForEach(buff => buff.finish());

            minion.addBuff(this);
            ablazeEffectInstance = (GameObject)MonoBehaviour.Instantiate(ablazeEffect);
        }

        public Buff copy() {
            return new AblazeBuff(duration, damage, interval, pctHealth, ablazeEffect);
        }

        public void finish() {
            minion.removeBuff(this);
            MonoBehaviour.Destroy(ablazeEffectInstance, .4f);
        }

        public void tick() {
            ablazeEffectInstance.transform.position = minion.transform.position;
            if(Time.time >= lastDotTick + interval) {
                minion.takeDamage(getActualDamage(), false, true);
                lastDotTick = Time.time;
            }
        }

        public float getActualDamage() {
            return damage + pctHealth / 100f * minion.health.get();
        }
    }
}

