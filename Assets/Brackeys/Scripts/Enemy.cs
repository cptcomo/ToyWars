﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brackeys {
    public class Enemy : MonoBehaviour {
        public float startSpeed = 10f;

        [HideInInspector]
        public float speed;

        public float startHealth = 100f;
        private float health;
        public int worth = 50;

        public GameObject deathEffect;

        [Header("Unity Stuff")]
        public Image healthBar;

        private void Start() {
            speed = startSpeed;
            health = startHealth;
        }

        public void takeDamage(float damage) {
            this.health -= damage;
            healthBar.fillAmount = health / startHealth;
            if(health <= 0f) {
                die();
            }
        }

        public void slow(float slowPct) {
            speed = Mathf.Min(speed, startSpeed * (1f - slowPct));
        }

        void die() {
            WaveSpawner.enemiesAlive--;
            PlayerStats.money += worth;
            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);          
            Destroy(gameObject);
        }
    }
}
