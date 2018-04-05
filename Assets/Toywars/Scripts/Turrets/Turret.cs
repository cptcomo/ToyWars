using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars{
    public class Turret : MonoBehaviour {
        public Sprite towerSprite;
        public Attribute range;
        public Attribute fireRate;
        public Attribute damage;
        public Attribute projectileSpeed;
        private float fireCooldown;

        public TowerType towerType;

        [Header("Turret")]
        public GameObject turretBulletPrefab;
        bool turretL3Unlock;
        int turretL3NumberOfHits;
        int turretL3HitCount;
        float turretL3PctDmg;
        bool turretR4Unlock;
        float turretR4PctDmg;

        [Header("Missile")]
        public Attribute missileExplosionRadius;
        public GameObject missilePrefab;
        bool missileR3Unlock;
        float missileR3ArmorShred;

        [Header("Laser")]
        public Attribute laserDOT;
        public Attribute laserSlowPct;
        public LineRenderer laserLineRenderer;
        public ParticleSystem laserImpactEffect;
        public Light laserImpactLight;
        bool laserIgnoreArmor;
        bool laserL3Unlock;
        float laserL3PctMissing;
        bool laserL4Unlock;
        float laserL4PctMax;
        bool laserR3Unlock;
        float laserR3PctModifier;

        [Header("Fire")]
        public Attribute fireFireRate;
        public Attribute fireDOT;
        public Attribute fireDamage;
        private float fireFireCooldown;

        [Header("Beacon")]
        public Attribute placeHolder;

        [Header("Unity Setup Fields")]
        public float turnSpeed = 10f;
        public string targetTag;
        private Transform target;
        private Minion targetMinion;
        private MinionMovement targetMovement;
        public Transform partToRotate;
        public Transform firePoint;

        public TowerUpgradePath towerUpgradePath;

        bool isInit = false;

        private void Start() {
            if(!isInit)
                init();
        }

        public void init() {
            isInit = true;
            range.init();
            fireRate.init();
            damage.init();
            projectileSpeed.init();
            if(towerType == TowerType.Turret) {
            } else if(towerType == TowerType.Missile) {
                missileExplosionRadius.init();
            } else if(towerType == TowerType.Laser) {
                laserDOT.init();
                laserSlowPct.init();
                laserIgnoreArmor = false;
            } else if(towerType == TowerType.Fire) {
                fireFireRate.init();
                fireDOT.init();
                fireDamage.init();
            } else {
                placeHolder.init();
            }
            turretL3Unlock = false;
            towerUpgradePath.init();
            InvokeRepeating("updateTarget", 0f, 0.25f);
        }

        private void Update() {
            if(target == null) {
                if(towerType == TowerType.Laser) {
                    if(laserLineRenderer.enabled) {
                        laserLineRenderer.enabled = false;
                        laserImpactEffect.Stop();
                        laserImpactLight.enabled = false;
                    }
                }
                return;
            }

            lockOnTarget();

            switch(towerType) {
                case TowerType.Turret:
                    turret();
                    break;
                case TowerType.Missile:
                    missile();
                    break;
                case TowerType.Laser:
                    laser();
                    break;
                case TowerType.Fire:
                    fire();
                    break;
                case TowerType.Beacon:
                    beacon();
                    break;
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

        void shootTargetProjectile(GameObject prefab, float damage, float explosionRadius, Buff buffToApply) {
            GameObject bulletGO = (GameObject)Instantiate(prefab, firePoint.position, firePoint.rotation);
            TargetBullet bullet = bulletGO.GetComponent<TargetBullet>();
            if(bullet != null) {
                bullet.seek(target);
                bullet.setDamage(damage);
                bullet.setExplosionRadius(explosionRadius);
                bullet.setSpeed(projectileSpeed.get());
                bullet.setBuffToApply(buffToApply);
                bullet.setTargetTag(targetTag);
            } 
        }

        void turret() {
            if(fireCooldown <= 0f) {
                float dam = damage.get();

                if(turretL3Unlock) {
                    turretL3HitCount = (turretL3HitCount + 1) % turretL3NumberOfHits;
                    if(turretL3HitCount == 0) {
                        bool debug = false;
                        if(debug) {
                            Debug.Log("Minion Health: " + targetMinion.health.get());
                            Debug.Log("Minion Start: " + targetMinion.health.getStart());
                            Debug.Log("Minion % Max: " + targetMinion.health.getPctStart(turretL3PctDmg));
                            Debug.Log("Normal Damage: " + dam);
                        }
                        dam += targetMinion.health.getPctStart(turretL3PctDmg);
                        if(debug)
                            Debug.Log("Now Damage: " + dam);
                    }
                }

                if(turretR4Unlock) {
                    dam += targetMinion.health.getPctStart(turretR4PctDmg);
                }

                shootTargetProjectile(turretBulletPrefab, dam, 0, null);
                fireCooldown = 1f / fireRate.get();
            }
        }

        void missile() {
            if(fireCooldown <= 0f) {
                float dam = damage.get();

                Buff b = null;

                if(missileR3Unlock) {
                    b = new ArmorShredBuff(5000, missileR3ArmorShred);
                }

                shootTargetProjectile(missilePrefab, dam, missileExplosionRadius.get(), b);
                fireCooldown = 1f / fireRate.get();
            }
        }

        void laser() {
            float dam = laserDOT.get();

            if(laserL3Unlock) {
                dam += targetMinion.health.getMissing() * laserL3PctMissing / 100f;
            }

            if(laserL4Unlock) {
                dam += targetMinion.health.getPctStart(laserL4PctMax);
            }

            targetMinion.takeDamage(dam * Time.deltaTime, false, laserIgnoreArmor);

            if(laserR3Unlock) {
                targetMinion.damageModifier.modifyFlat(-laserR3PctModifier * Time.deltaTime, 50, 200);
            }

            if(laserSlowPct.get() > 0) {
                Buff slowBuff = new SlowDebuff(.5f, laserSlowPct.get());
                slowBuff.apply(targetMinion);
            }
            
            if(!laserLineRenderer.enabled) {
                laserLineRenderer.enabled = true;
                laserImpactEffect.Play();
                laserImpactLight.enabled = true;
            }
            laserLineRenderer.SetPosition(0, firePoint.position);
            laserLineRenderer.SetPosition(1, target.position);

            Vector3 dir = firePoint.position - target.position;
            laserImpactEffect.transform.rotation = Quaternion.LookRotation(dir);
            laserImpactEffect.transform.position = target.position + dir.normalized;
        }

        void fire() {

        }

        void beacon() {

        }

        public void upgrade(int upgradeIndex, bool playerTurret) {
            towerUpgradePath.upgrade(upgradeIndex, this, playerTurret);
        }

        public void turretL3Upgrade(int numberOfHits, float pctDamage) {
            this.turretL3Unlock = true;
            this.turretL3NumberOfHits = numberOfHits;
            this.turretL3PctDmg = pctDamage;
            this.turretL3HitCount = 0;
        }

        public void turretR4Upgrade(float pctDamage) {
            this.turretR4Unlock = true;
            this.turretR4PctDmg = pctDamage;
        }

        public void missileR3Upgrade(float armorShred) {
            this.missileR3Unlock = true;
            this.missileR3ArmorShred = armorShred;
        }

        public void laserL2Upgrade() {
            this.laserIgnoreArmor = true;
        }

        public void laserL3Upgrade(float pct) {
            this.laserL3Unlock = true;
            this.laserL3PctMissing = pct;
        }

        public void laserL4Upgrade(float pct) {
            this.laserL4Unlock = true;
            this.laserL4PctMax = pct;
        }

        public void laserR3Upgrade(float pct) {
            this.laserR3Unlock = true;
            this.laserR3PctModifier = pct;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawWireSphere(this.transform.position, range.getStart());
        }

        [HideInInspector]
        public enum TowerType {
            Turret, Missile, Laser, Fire, Beacon
        }
    }
}

