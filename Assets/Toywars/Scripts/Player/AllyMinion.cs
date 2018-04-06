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

