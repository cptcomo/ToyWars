using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class Bullet : MonoBehaviour {
        private Transform target;
        public float speed = 70f;

        private float damage;
        private float explosionRadius;
        private bool playerShot;
        public GameObject impactEffect;

        private void Start() {
            if(!playerShot)
                playerShot = false;
        }

        public void seek(Transform target) {
            this.target = target;
        }

        private void Update() {
            if(this.target == null) {
                Destroy(gameObject);
                return;
            }
            Vector3 dir = (target.position - transform.position);
            float distToMove = speed * Time.deltaTime;

            if(dir.magnitude <= distToMove) {
                hitTarget();
                return;
            }

            transform.Translate(dir.normalized * distToMove, Space.World);
            transform.LookAt(target);
        }

        public void setDamage(float damage)
        {
            this.damage = damage;
        }

        public void setExplosionRadius(float explosionRadius)
        {
            this.explosionRadius = explosionRadius;
        }

        public void setPlayerShot(bool playerShot) {
            this.playerShot = playerShot;
        }

        void hitTarget() {
            GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectInstance, 5f);

            if(explosionRadius > 0f) {
                explode();
            } else {
                doDamage(target);
            }

            Destroy(gameObject);
        }

        void doDamage(Transform enemy) {
            Enemy e = enemy.GetComponent<Enemy>();

            if(e != null)
                e.takeDamage(damage, playerShot);
        }

        void explode() {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach(Collider col in colliders) {
                if(col.tag == "Enemy") {
                    doDamage(col.transform);
                }
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }

}
