using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class AblazeBuff : Buff {
        private static int maxParticles = 40;

        float duration, interval, startTime, damage, pctHealth;

        float lastDotTick;

        Minion minion;

        GameObject source;
        GameObject ablazeEffectsContainer;
        GameObject ablazeEffect;
        GameObject ablazeEffectInstance;

        public AblazeBuff(GameObject source, float dur, float damage, float interval, float pctHealth, GameObject ablazeEffect) {
            this.source = source;
            this.duration = dur;
            this.interval = interval;
            this.damage = damage;
            this.pctHealth = pctHealth;
            this.ablazeEffect = ablazeEffect;
            this.ablazeEffectsContainer = GameManager.getInstance().ablazeEffectsContainer;
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

            Transform[] ts = ablazeEffectsContainer.GetComponentsInChildren<Transform>();

            if(ts.Length < maxParticles) {
                ablazeEffectInstance = (GameObject)MonoBehaviour.Instantiate(ablazeEffect);
                ablazeEffectInstance.transform.SetParent(ablazeEffectsContainer.transform);
                MonoBehaviour.Destroy(ablazeEffectInstance, duration);
            }
        }

        public Buff copy() {
            return new AblazeBuff(source, duration, damage, interval, pctHealth, ablazeEffect);
        }

        public void finish() {
            minion.removeBuff(this);
            if(ablazeEffectInstance != null)
                MonoBehaviour.Destroy(ablazeEffectInstance, .4f);
        }

        public void tick() {
            if(ablazeEffectInstance != null)
                ablazeEffectInstance.transform.position = minion.transform.position;
            if(Time.time >= lastDotTick + interval) {
                minion.takeDamage(getActualDamage(), source, true);
                lastDotTick = Time.time;
            }
        }

        public float getActualDamage() {
            return damage + pctHealth / 100f * minion.health.get();
        }
    }
}

