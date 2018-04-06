using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class BeaconTurretBuff : Buff {
        int id;
        float startTime, duration, pctFireRate, pctRange, pctDamage, pctLaserDOT, pctFireDOT;

        Turret turret;

        public BeaconTurretBuff(int id, float duration, float pctFireRate, float pctRange, float pctDamage, float pctLaserDOT, float pctFireDOT) {
            this.id = id;
            this.duration = duration;
            this.pctFireRate = pctFireRate;
            this.pctRange = pctRange;
            this.pctDamage = pctDamage;
            this.pctLaserDOT = pctLaserDOT;
            this.pctFireDOT = pctFireDOT;
        }

        public bool finished
        {
            get {
                return Time.time > startTime + duration;
            }
        }

        public void apply(Component target) {
            startTime = Time.time;
            turret = target.GetComponent<Turret>();

            List<Buff> buffsToRemove = new List<Buff>();
            
            foreach(Buff b in turret.buffs) {
                if(b is BeaconTurretBuff) {
                    BeaconTurretBuff sb = (BeaconTurretBuff)b;
                    if(this.id == sb.id) {
                        buffsToRemove.Add(sb);
                    }
                }
            }

            buffsToRemove.ForEach(buff => buff.finish());

            turret.addBuff(this);
        }

        public Buff copy() {
            return new BeaconTurretBuff(this.id, this.duration, this.pctFireRate, this.pctRange, this.pctDamage, this.pctLaserDOT, this.pctFireDOT);
        }

        public int getId() {
            return this.id;
        }

        public void finish() {
            turret.buffs.Remove(this);
        }

        public void tick() {
            turret.fireRate.buffPct(pctFireRate);
            turret.range.buffPct(pctRange);
            turret.damage.buffPct(pctDamage);
            if(turret.towerType == Turret.TowerType.Laser)
                turret.laserDOT.buffPct(pctLaserDOT);
            if(turret.towerType == Turret.TowerType.Fire)
                turret.fireDOT.buffPct(pctFireDOT);
        }
    }
}
