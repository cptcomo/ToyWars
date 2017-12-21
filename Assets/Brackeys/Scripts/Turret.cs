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
        public LineRenderer lineRenderer;

        [Header("Unity Setup Fields")]
        public float turnSpeed = 10f;
        public string enemyTag = "Enemy";
        private Transform target;
        public Transform partToRotate;     
        public Transform firePoint;

        private void Start() {
            InvokeRepeating("updateTarget", 0f, 0.5f);
        }

        private void Update() {
            if(target == null) {
                if(useLaser)
                    if(lineRenderer.enabled)
                        lineRenderer.enabled = false;
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
            if(!lineRenderer.enabled)
                lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, target.position);
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
