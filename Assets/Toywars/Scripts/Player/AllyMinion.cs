﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class AllyMinion : Minion {
        PlayerManager pm;

        public override void Start() {
            base.Start();
            pm = PlayerManager.getInstance();
            pm.alliesAlive++;
        }

        public override void Update() {
            base.Update();

        }

        protected override void attack() {
            base.attack();
            Collider[] cols = Physics.OverlapSphere(this.transform.position, attackRadius.get());
            foreach(Collider c in cols) {
                if(c.transform.tag.Equals("Enemy")) {
                    c.GetComponent<Enemy>().takeDamage(damage.get() * Time.deltaTime, false);
                }
            }
        }

        public override void takeDamage(float damage, bool playerShot) {
            this.health.change(-damage);
            if(health.get() <= 0f)
                die();
        }


        protected void die() {
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
