using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Bullet : MonoBehaviour {
        private Transform target;
        public float speed = 70f;
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
        }

        void hitTarget() {
            GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectInstance, 2f);
            Destroy(target.gameObject);
            Destroy(gameObject);
        }
    }
}
