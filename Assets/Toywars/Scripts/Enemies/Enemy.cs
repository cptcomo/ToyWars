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

        public override void takeDamage(float damage, bool playershot) {
            this.health.change(-damage);
            if(health.get() <= 0f)
                die(playershot);
        }

        protected void die(bool playerShot) {   
            pm.money += moneyValue;
            if(playerShot) {
                pm.exp += expValue;
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

