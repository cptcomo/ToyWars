using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Enemy : MonoBehaviour {
        public float speed = 10f;
        public int health = 100;
        public int value = 50;
        public GameObject deathEffect;
        private Transform target;
        private int wpIndex = 0;
        private const double wpTolerance = 0.4f; //How close to waypoint is close enough

        private void Start() {
            target = Waypoints.wps[0];
        }

        private void Update() {
            Vector3 dir = (target.position - this.transform.position).normalized;
            transform.Translate(dir * speed * Time.deltaTime, Space.World);

            if(Vector3.Distance(transform.position, target.position) <= wpTolerance) {
                nextWaypoint();
            }
        }

        public void takeDamage(int damage) {
            this.health -= damage;
            if(health <= 0f) {
                die();
            }
        }

        void die() {
            PlayerStats.money += value;
            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);          
            Destroy(gameObject);
        }

        void nextWaypoint() {
            if(wpIndex >= Waypoints.wps.Length - 1) {
                endPath();
                return;
            }
            wpIndex++;
            target = Waypoints.wps[wpIndex];
        }

        void endPath() {
            PlayerStats.lives--;
            Destroy(gameObject);
        }
    }
}
