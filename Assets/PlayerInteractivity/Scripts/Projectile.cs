using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class Projectile : MonoBehaviour {
        private Vector3 dir;
        private Vector3 startPos;
        public float speed = 5f;

        private float damage;
        private float range = 10000;
        public GameObject impactEffect;

        private void Start() {
            this.startPos = this.transform.position;
        }

        public void seek(Vector3 dir) {
            this.dir = dir;
        }

        private void Update() {
            if(Vector3.SqrMagnitude(this.transform.position - startPos) > range) {
                Destroy(gameObject);
                return;
            }
            transform.Translate(Time.deltaTime * dir * speed, Space.World);
        }

        public void setDamage(float dmg) {
            this.damage = dmg;
        }

        public void setRange(float rng) {
            this.range = rng;
        }

        void OnCollisionEnter(Collision collision) {
            if(collision.transform.tag.Equals("Enemy")) {
                hitTarget(collision.transform);
            }
        }

        void hitTarget(Transform target) {
            GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectInstance, 5f);
            doDamage(target);
            Destroy(gameObject);
        }

        void doDamage(Transform enemy) {
            Enemy e = enemy.GetComponent<Enemy>();

            if(e != null)
                e.takeDamage(damage);
        }
    }
}
