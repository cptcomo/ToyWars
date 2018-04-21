using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class TargetBullet : MonoBehaviour {
        private Transform target;
        public float speed = 70f;

        private GameObject source;
        private float damage;
        private float explosionRadius;
        private bool playerShot;
        private bool ignoreArmor;
        private string targetTag;
        private Buff buffToApply;
        public GameObject impactEffect;

        [Header("Fireball")]
        public bool isFireball;
        public GameObject fireballParticle;
        GameObject fireballParticleInstance;

        private void Start() {
            if(!playerShot)
                playerShot = false;

            if(!ignoreArmor)
                ignoreArmor = false;

            if(isFireball) {
                this.fireballParticleInstance = (GameObject)Instantiate(fireballParticle, transform.position, transform.rotation);
            }
        }

        public void setSource(GameObject source) {
            this.source = source;
        }

        public void seek(Transform target) {
            this.target = target;
        }

        private void Update() {
            if(this.target == null) {
                destroy();
                return;
            }

            Vector3 dir = target.position - transform.position;
            float distToMove = speed * Time.deltaTime;

            if(dir.magnitude <= distToMove) {
                hitTarget();
                return;
            }

            transform.Translate(dir.normalized * distToMove, Space.World);
            transform.LookAt(target);

            if(isFireball) {
                fireballParticleInstance.transform.position = this.transform.position;

                Vector3 lookDir = target.position - this.transform.position;
                lookDir.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookDir);
                rotation *= Quaternion.Euler(0, 90, 0);
                fireballParticleInstance.transform.rotation = Quaternion.Slerp(fireballParticleInstance.transform.rotation, rotation, Time.deltaTime * 100);
            }
        }

        public void setDamage(float damage) {
            this.damage = damage;
        }

        public void setExplosionRadius(float explRadius) {
            this.explosionRadius = explRadius;
        }

        public void setSpeed(float speed) {
            this.speed = speed;
        }

        public void setPlayerShot(bool playerShot) {
            this.playerShot = playerShot;
        }

        public void setIgnoreArmor(bool ignoreArmor) {
            this.ignoreArmor = ignoreArmor;
        }

        public void setTargetTag(string tag) {
            this.targetTag = tag;
        }

        public void setBuffToApply(Buff buff) {
            buffToApply = buff;
        }

        void hitTarget() {
            GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectInstance, 5f);

            if(explosionRadius > 0f) {
                explode();
            } else {
                doDamage(target);
            }

            destroy();
        }

        void doDamage(Transform t) {
            Minion m = t.GetComponent<Minion>();

            if(m != null)
                m.takeDamage(damage, source, this.ignoreArmor);

            if(buffToApply != null) {
                try {
                    buffToApply.copy().apply(t);
                }
                catch(System.Exception) {
                    Debug.LogWarning("Tried to add buff to an object without a Minion script");
                }
            }
        }

        void explode() {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach(Collider col in colliders) {
                if(col.tag.Equals(targetTag)) {
                    doDamage(col.transform);
                }
            }
        }

        void destroy() {
            if(isFireball) {
                Destroy(fireballParticleInstance);
            }

            Destroy(gameObject);     
        }
    }
}
