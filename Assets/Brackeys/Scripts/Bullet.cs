using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Bullet : MonoBehaviour {
        private Transform target;
        public float speed = 70f;
        public float explosionRadius = 0f;
        public GameObject impactEffect;

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

        void hitTarget() {
            GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectInstance, 5f);

            if(explosionRadius > 0f) {
                explode();
            }
            else {
                damage(target);
            }
            
            Destroy(gameObject);
        }

        void damage(Transform enemy) {
            Destroy(enemy.gameObject);
        }

        void explode() {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach(Collider col in colliders) {
                if(col.tag == "Enemy") {
                    damage(col.transform);
                }
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
