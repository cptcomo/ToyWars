using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class AllyMinion : Minion {
        PlayerManager pm;
        EnemiesManager em;
        public override void Start() {
            base.Start();
            pm = PlayerManager.getInstance();
            em = EnemiesManager.getInstance();
            pm.alliesAlive++;
        }

        public override void Update() {
            base.Update();

        }

        protected override void attack() {
            base.attack();
        }

        public override void takeDamage(float damage, bool playerShot, bool ignoreArmor) {
            this.health.modifyFlat(-damage * armorDamageMultiplier(ignoreArmor, armor.get()) * Random.Range(0.9f, 1.1f), -1, health.getStart());
            if(health.get() <= 0f)
                die();
        }


        protected void die() {
            if(minionType == MinionType.Divide) {
                GameObject min1 = (GameObject)Instantiate(meleePrefab, this.transform.position + this.transform.forward, this.transform.rotation);
                min1.GetComponent<MinionMovement>().waypoints = this.getMinionMovement().waypoints;
                min1.GetComponent<MinionMovement>().setWaypointIndex(this.getMinionMovement().getWaypointIndex());
                GameObject min2 = (GameObject)Instantiate(meleePrefab, this.transform.position - this.transform.forward, this.transform.rotation);
                min2.GetComponent<MinionMovement>().waypoints = this.getMinionMovement().waypoints;
                min2.GetComponent<MinionMovement>().setWaypointIndex(this.getMinionMovement().getWaypointIndex());
            }
            em.changeMoney(moneyValue);
            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
            Destroy(gameObject);
        }

        public override void OnDestroy() {
            base.OnDestroy();
            pm.alliesAlive--;
        }
    }
}

