using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class SkillshotBullet : MonoBehaviour {
        private Vector3 dir;
        private Vector3 startPos;
        public float speed = 5f;

        private GameObject source;
        private string targetTag;
        private float damage;
        private float range = 10000;
        private float explosionRadius;
        private float pctMissingHealthDamage;
        private float pctDistanceDamage;
        private float chanceToImpactEffect = 100f;
        private bool playerShot;
        private bool ignoreArmor;
        private Buff buffToApply;
        public GameObject impactEffect;

        [Header("Fireball")]
        public bool isFireball;
        public GameObject fireballParticle;
        GameObject fireballParticleInstance;

        [Header("Rocket")]
        public bool isRocket;

        private void Start() {
            this.startPos = this.transform.position;
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

        public void seek(Vector3 dir) {
            this.dir = dir;
        }

        public void setTargetTag(string tag) {
            this.targetTag = tag;
        }

        public void setPlayerShot(bool playerShot) {
            this.playerShot = playerShot;
        }

        public void setFireball(bool fireball) {
            this.isFireball = fireball; 
        }

        public void setPctMissingHealthDamage(float pct) {
            this.pctMissingHealthDamage = pct;
        }

        public void setPctDistanceDamage(float pct) {
            this.pctDistanceDamage = pct;
        }

        public void setChanceToImpactEffect(float pct) {
            this.chanceToImpactEffect = pct;
        }

        private void Update() {
            if(source == null)
                Debug.LogWarning("Source is null in SkillshotBullet");

            if(Vector3.Distance(this.transform.position, startPos) > range) {
                destroy();
                return;
            }

            transform.Translate(Time.deltaTime * dir * speed, Space.World);

            if(isRocket) {
                this.transform.LookAt(this.transform.position + dir);
            }
            

            if(isFireball) {
                fireballParticleInstance.transform.position = this.transform.position;
                Quaternion rotation = Quaternion.LookRotation(dir);
                rotation *= Quaternion.Euler(0, 90, 0);
                fireballParticleInstance.transform.rotation = Quaternion.Slerp(fireballParticleInstance.transform.rotation, rotation, Time.deltaTime * 100);
            }
        }

        public void setDamage(float dmg) {
            this.damage = dmg;
        }

        public void setExplosionRadius(float radius) {
            this.explosionRadius = radius;
        }

        public void setRange(float rng) {
            this.range = rng;
        }

        public void setIgnoreArmor(bool ignoreArmor) {
            this.ignoreArmor = ignoreArmor;
        }

        public void setBuffToApply(Buff buff) {
            this.buffToApply = buff;
        }

        void OnCollisionEnter(Collision collision) {
            if(collision.transform.tag.Equals(targetTag)) {
                hitTarget(collision.transform);
            }
        }

        void hitTarget(Transform target) {
            if(Random.Range(0f, 1f) < chanceToImpactEffect) {
                GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(effectInstance, 5f);
            }
            if(explosionRadius > 0f) {
                explode();
            }
            else 
                doDamage(target);

            destroy();
        }

        void doDamage(Transform minion) {
            Minion m = minion.GetComponent<Minion>();

            if(m != null)
                m.takeDamage(damage + m.health.getMissing() * pctMissingHealthDamage / 100f + Mathf.Clamp(Vector3.Distance(this.transform.position, startPos) * pctDistanceDamage / 100f, 0f, 5 * damage), source, this.ignoreArmor);

            if(buffToApply != null) {
                try {
                    buffToApply.copy().apply(m);
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
