using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class Enemy : Minion {
        public int expValue;

        EnemiesManager em;
        PlayerManager pm;
        
        public override void Start() {
            base.Start();
            em = EnemiesManager.getInstance();
            pm = PlayerManager.getInstance();
            em.enemiesAlive++;
        }

        public override void Update() {
            base.Update();
        }

        protected override void attack() {
            base.attack();
        }

        public override void takeDamage(float damage, bool playershot, bool ignoreArmor) {
            this.health.modifyFlat(-damage * armorDamageMultiplier(ignoreArmor, armor.get()) * Random.Range(0.9f, 1.1f), -1, health.getStart());
            if(health.get() <= 0f)
                die(playershot);
        }

        protected void die(bool playerShot) {
            if(minionType == MinionType.Divide) {
                GameObject min1 = (GameObject)Instantiate(meleePrefab, this.transform.position, this.transform.rotation);
                min1.GetComponent<MinionMovement>().waypoints = this.getMinionMovement().waypoints;
                min1.GetComponent<MinionMovement>().setWaypointIndex(this.getMinionMovement().getWaypointIndex());
                GameObject min2 = (GameObject)Instantiate(meleePrefab, this.transform.position - this.transform.forward, this.transform.rotation);
                min2.GetComponent<MinionMovement>().waypoints = this.getMinionMovement().waypoints;
                min2.GetComponent<MinionMovement>().setWaypointIndex(this.getMinionMovement().getWaypointIndex());
            }

            pm.changeMoney(moneyValue);
            if(playerShot) {
                pm.changeExp(expValue);
            }

            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
            Destroy(gameObject);
        }

        public override void OnDestroy() {
            base.OnDestroy();
            em.enemiesAlive--;
        }
    }
}

