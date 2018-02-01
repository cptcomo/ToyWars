using System.Collections;
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
            Collider[] cols = Physics.OverlapSphere(this.transform.position, attackRadius);
            foreach(Collider c in cols) {
                if(c.transform.tag.Equals("Enemy")) {
                    c.GetComponent<Enemy>().takeDamage(damage * Time.deltaTime, false);
                }
            }
        }

        public void takeDamage(float damage) {
            this.health -= damage;
            if(health <= 0f)
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

