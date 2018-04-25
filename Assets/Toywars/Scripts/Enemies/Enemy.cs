using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class Enemy : Minion {
        public int expValue;

        EnemiesManager em;
        PlayerManager pm;

        float lastPlayerHit;

        public override void Start() {
            base.Start();
            em = EnemiesManager.getInstance();
            pm = PlayerManager.getInstance();
            int pmDeltaLives = pm.baseHealth - pm.lastBaseHealth;
            int emDeltaLives = em.baseHealth - em.lastBaseHealth;
            float multiplier = Mathf.Clamp((emDeltaLives - pmDeltaLives) / 4f, 0f, 10f);
            float handicap = GameManager.getInstance().difficulty == Difficulty.hard ? 2f : -1f;
            health.setStart(health.getStart() + health.getStart() * (GameManager.getInstance().waveIndex) * (6f + handicap + multiplier) / 100f);
            armor.setStart(armor.getStart() + armor.getStart() * (GameManager.getInstance().waveIndex) * (6f + handicap + multiplier) / 100f);
            damage.setStart(damage.getStart() + damage.getStart() * (GameManager.getInstance().waveIndex) * (6f + handicap + multiplier) / 100f);
            initialize();
            em.enemiesAlive++;
            lastPlayerHit = 0;
        }

        public override void initialize() {
            base.initialize();
        }

        public override void Update() {
            base.Update();
        }

        protected override void attack() {
            base.attack();
        }

        public override void takeDamage(float damage, GameObject source, bool ignoreArmor) {
            float dam = damage * armorDamageMultiplier(ignoreArmor, armor.get()) * Random.Range(0.9f, 1.1f);
            this.health.modifyFlat(-dam);
            bool playerShot = false;
            if(source.tag.Equals("Tower")) {
                source.GetComponent<Turret>().damageDone += dam;
            }
            else if(source.tag.Equals("Player")) {
                source.GetComponent<Player>().damageDone += dam;
                playerShot = true;
                lastPlayerHit = Time.time;
            }
            if(health.get() <= 0f)
                die(playerShot);
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
            try {
                pm.changeMoney(moneyValue);
            } catch (System.Exception e) {
                Debug.LogWarning("Couldn't give money with " + this.name);
            }
            
            if(Time.time <= lastPlayerHit + (GameManager.getInstance().difficulty == Difficulty.hard ? .5f : 2f)) {
                pm.changeExp(expValue);
            }
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

