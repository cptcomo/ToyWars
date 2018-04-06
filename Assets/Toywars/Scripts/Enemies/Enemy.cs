﻿using System.Collections;
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

