using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class SkillshotBullet : MonoBehaviour {
        private Vector3 dir;
        private Vector3 startPos;
        public float speed = 5f;

        private string targetTag;
        private float damage;
        private float range = 10000;
        private bool playerShot;
        private bool ignoreArmor;
        public GameObject impactEffect;

        private void Start() {
            this.startPos = this.transform.position;
            if(!playerShot)
                playerShot = false;

            if(!ignoreArmor)
                ignoreArmor = false;
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

        public void setIgnoreArmor(bool ignoreArmor) {
            this.ignoreArmor = ignoreArmor;
        }

        void OnCollisionEnter(Collision collision) {
            if(collision.transform.tag.Equals(targetTag)) {
                hitTarget(collision.transform);
            }
        }

        void hitTarget(Transform target) {
            GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectInstance, 5f);

            doDamage(target);
            Destroy(gameObject);
        }

        void doDamage(Transform minion) {
            Minion m = minion.GetComponent<Minion>();
            if(m != null)
                m.takeDamage(damage, playerShot, this.ignoreArmor);
        }
    }

}
