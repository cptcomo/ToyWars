using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerInteractivity {
    public class Enemy : MonoBehaviour {
        GameManager gm;
        public float startHealth = 100f;
        private float health;
        public int worth = 50;
        public float damage;
        public float attackRadius = 2f;

        public GameObject deathEffect;

        private void Start() {
            gm = GameManager.getInstance();
            health = startHealth;
        }

        private void Update() {
            Collider[] cols = Physics.OverlapSphere(this.transform.position, attackRadius);
            foreach(Collider c in cols) {
                if(c.transform.tag.Equals("Player")) {
                    c.GetComponent<Player>().takeDamage(damage * Time.deltaTime);
                }
            }
        }

        public void takeDamage(float damage) {
            this.health -= damage;
            if(health <= 0f) {
                die();
            }
        }

        void die() {
            gm.enemiesAlive--;
            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
            Destroy(gameObject);
        }
    }
}