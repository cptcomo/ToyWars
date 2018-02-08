using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class Enemy : Minion {
        public int moneyValue;
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
            Collider[] cols = Physics.OverlapSphere(this.transform.position, attackRadius.get());
            foreach(Collider c in cols) {
                if(c.transform.tag.Equals("Player")) {
                    //c.GetComponent<Player>().takeDamage(damage * Time.deltaTime);
                }
                else if(c.transform.tag.Equals("AllyMinion")) {
                    c.GetComponent<AllyMinion>().takeDamage(damage.get() * Time.deltaTime, false);
                }
            }
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

