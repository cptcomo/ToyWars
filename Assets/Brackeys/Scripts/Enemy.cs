using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Enemy : MonoBehaviour {
        public float startSpeed = 10f;

        [HideInInspector]
        public float speed;

        public float health = 100f;
        public int worth = 50;
        public GameObject deathEffect;

        private void Start() {
            speed = startSpeed;
        }

        public void takeDamage(float damage) {
            this.health -= damage;
            if(health <= 0f) {
                die();
            }
        }

        public void slow(float slowPct) {
            speed = Mathf.Min(speed, startSpeed * (1f - slowPct));
        }

        void die() {
            PlayerStats.money += worth;
            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);          
            Destroy(gameObject);
        }
    }
}
