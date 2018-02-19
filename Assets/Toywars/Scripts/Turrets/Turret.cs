using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars{
    public class Turret : MonoBehaviour {
        public Attribute range;

        [Header("Use Bullets (Default)")]
        public Attribute fireRate;
        public Attribute explosionRadius;
        public Attribute damage;
        private float fireCooldown;
        public GameObject bulletPrefab;

        [Header("Use Laser")]
        public bool useLaser = false;
        public Attribute dot;
        public Attribute slowPct;
        public LineRenderer lineRenderer;
        public ParticleSystem laserImpactEffect;
        public Light impactLight;

        [Header("Unity Setup Fields")]
        public float turnSpeed = 10f;
        public string targetTag;
        private Transform target;
        private Minion targetMinion;
        private MinionMovement targetMovement;
        public Transform partToRotate;
        public Transform firePoint;

        public TowerUpgradePath towerUpgradePath;

        private void Start() {
            range.init();
            fireRate.init();
            explosionRadius.init();
            damage.init();
            dot.init();
            slowPct.init();
            towerUpgradePath.init();
            InvokeRepeating("updateTarget", 0f, 0.25f);
        }

        private void Update() {
            if(target == null) {
                if(useLaser) {
                    if(lineRenderer.enabled) {
                        lineRenderer.enabled = false;
                        laserImpactEffect.Stop();
                        impactLight.enabled = false;
                    }
                }
                return;
            }

            lockOnTarget();

            if(useLaser) {
                laser();
            } else {
                if(fireCooldown <= 0f) {
                    shoot();
                    fireCooldown = 1f / fireRate.get();
                }
            }

            fireCooldown -= Time.deltaTime;
        }

        void updateTarget() {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            float minDist = Mathf.Infinity;
            GameObject nearestTarget = null;
            foreach(GameObject target in targets) {
                float dist = Vector3.Distance(transform.position, target.transform.position);
                if(dist < minDist) {
                    minDist = dist;
                    nearestTarget = target;
                }
            }
            if(nearestTarget != null && minDist <= range.get()) {
                target = nearestTarget.transform;
                targetMinion = target.GetComponent<Minion>();
                targetMovement = target.GetComponent<MinionMovement>();
            }
            else {
                target = null;
            }
        }

        void lockOnTarget() {
            Vector3 dir = target.position - this.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
        }

        void laser() {
            targetMinion.takeDamage(dot.get() * Time.deltaTime, false);
            targetMovement.speed.modifyPct(-slowPct.get());
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

        void shoot() {
            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            TargetBullet bullet = bulletGO.GetComponent<TargetBullet>();
            if(bullet != null) {
                bullet.seek(target);
                bullet.setDamage(damage.get());
                bullet.setExplosionRadius(explosionRadius.get());
                bullet.setTargetTag(targetTag);
            } 
        }

        public void upgrade(int upgradeIndex) {
            Debug.Log("Hi");
            towerUpgradePath.upgrade(upgradeIndex, this);
        }
    }
}

