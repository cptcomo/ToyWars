﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Turret : MonoBehaviour {   
        
        [Header("General")]
        public float range = 15f;

        [Header("Use Bullets (default)")]
        public float fireRate = 1f;
        private float fireCountdown = 0;
        public GameObject bulletPrefab;

        [Header("Use Laser")]
        public bool useLaser = false;
        public int damageOverTime = 40;
        public float slowPct = .3f;
        public LineRenderer lineRenderer;
        public ParticleSystem laserImpactEffect;
        public Light impactLight;

        [Header("Unity Setup Fields")]
        public float turnSpeed = 10f;
        public string enemyTag = "Enemy";
        private Transform target;
        private Enemy targetEnemy;
        public Transform partToRotate;     
        public Transform firePoint;

        private void Start() {
            InvokeRepeating("updateTarget", 0f, 0.5f);
        }

        private void Update() {
            if(target == null) {
                if(useLaser)
                    if(lineRenderer.enabled) {
                        lineRenderer.enabled = false;
                        laserImpactEffect.Stop();
                        impactLight.enabled = false;
                    }
                return;
            }

            lockOnTarget();

            if(useLaser) {
                laser();
            }
            else {
                if(fireCountdown <= 0f) {
                    shoot();
                    fireCountdown = 1f / fireRate;
                }
            }

            fireCountdown -= Time.deltaTime;
        }

        void laser() {
            targetEnemy.takeDamage(damageOverTime * Time.deltaTime);
            targetEnemy.slow(slowPct);
            if(!lineRenderer.enabled) {
                lineRenderer.enabled = true;
                laserImpactEffect.Play();
                impactLight.enabled = true;
            }
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, target.position);

            Vector3 dir = firePoint.position - target.position;
            laserImpactEffect.transform.rotation = Quaternion.LookRotation(dir);
            laserImpactEffect.transform.position = target.position + dir.normalized;
        }

        void lockOnTarget() {
            Vector3 dir = target.position - this.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
        }

        void updateTarget() {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            float minDist = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach(GameObject enemy in enemies) {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if(dist < minDist) {
                    minDist = dist;
                    nearestEnemy = enemy;
                }
            }
            if(nearestEnemy != null && minDist <= range) {
                target = nearestEnemy.transform;
                targetEnemy = target.GetComponent<Enemy>();
            } else target = null;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }

        void shoot() {
            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if(bullet != null) {
                bullet.seek(target);
            }
        }
    }
}
