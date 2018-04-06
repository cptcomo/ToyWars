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
        float laserR3lastFire;
        float laserR3tickRate;

        [Header("Fire")]
        public GameObject fireballPrefab;
        public GameObject ablazeEffect;
        public Attribute fireDOT;
        bool fireAblazeUnlock;
        float fireDotTickInterval;
        float ablazeDuration;
        bool fireL3Unlock;
        float fireAblazePctHealth;
        bool fireR3Unlock;
        float fireExplosionRadius;
        bool fireR4Unlock;
        float fireR4rad;
        float fireR4dps;

        static int globalBeaconIDs = 0;
        int beaconID;
        [Header("Beacon")]
        public string beaconTurretTag;
        public string beaconMinionTag;

        public bool isPlayer;
        public Attribute beaconTurretFireRate;
        public Attribute beaconTurretRange;
        public Attribute beaconTurretDamage;
        public Attribute beaconTurretLaserDOT;
        public Attribute beaconTurretFireDOT;
        public Attribute beaconMinionDamage;
        public Attribute beaconMinionMovespeed;
        public Attribute beaconMinionHeal;
        public Attribute beaconPlayerHeal;
        public Attribute beaconMoneyPerRound;
        public Attribute beaconMoneyRate;
        public Attribute beaconExpRate;

        [Header("Unity Setup Fields")]
        public float turnSpeed = 10f;
        public string targetTag;
        private Transform target;
        private Minion targetMinion;
        private MinionMovement targetMovement;
        public Transform partToRotate;
        public Transform firePoint;

        public TowerUpgradePath towerUpgradePath;

        [HideInInspector]
        public List<Buff> buffs;
        List<Attribute> attrs;

        bool isInit = false;

        private void Start() {
            if(!isInit)
                init();
        }

        public void init() {
            isInit = true;
            attrs = new List<Attribute>();
            attrs.Add(range);
            attrs.Add(fireRate);
            attrs.Add(damage);
            attrs.Add(projectileSpeed);
            if(towerType == TowerType.Turret) {
            } else if(towerType == TowerType.Missile) {
                attrs.Add(missileExplosionRadius);
            } else if(towerType == TowerType.Laser) {
                attrs.Add(laserDOT);
                attrs.Add(laserSlowPct);
                laserIgnoreArmor = false;
            } else if(towerType == TowerType.Fire) {
                attrs.Add(fireDOT);
            } else if(towerType == TowerType.Beacon) {
                beaconID = globalBeaconIDs++;
                attrs.Add(beaconTurretFireRate);
                attrs.Add(beaconTurretRange);
                attrs.Add(beaconTurretDamage);
                attrs.Add(beaconTurretLaserDOT);
                attrs.Add(beaconTurretFireDOT);
                attrs.Add(beaconMinionDamage);
                attrs.Add(beaconMinionMovespeed);
                attrs.Add(beaconMinionHeal);
                attrs.Add(beaconPlayerHeal);
                attrs.Add(beaconMoneyPerRound);
                attrs.Add(beaconMoneyRate);
                attrs.Add(beaconExpRate);
            }
            attrs.ForEach(attr => attr.init());
            buffs = new List<Buff>();
            towerUpgradePath.init();
            if(towerType != TowerType.Beacon)
                InvokeRepeating("updateTarget", 0f, 0.25f);
            else if(towerType == TowerType.Beacon) {
                GameManager.getInstance().EndWaveEvent += beaconMoney;
                this.transform.Translate(0, 2, 0);
                InvokeRepeating("beacon", 0f, 0.5f);
            }
            if(towerType == TowerType.Fire)
                InvokeRepeating("fireR4Check", 0f, 0.5f);
        }

        private void OnDisable() {
            if(towerType == TowerType.Beacon) {
                GameManager.getInstance().EndWaveEvent -= beaconMoney;
            }
        }

        void fireR4Check() {
            if(fireR4Unlock) {
                Collider[] cols = Physics.OverlapSphere(this.transform.position, fireR4rad);
                foreach(Collider col in cols) {
                    if(col.tag.Equals(targetTag)) {
                        Minion m = col.GetComponent<Minion>();
                        m.takeDamage(fireR4dps * 0.5f, false, true);
                        if(fireAblazeUnlock) {
                            AblazeBuff buff = new AblazeBuff(ablazeDuration, fireDOT.get(), fireDotTickInterval, fireAblazePctHealth, ablazeEffect);
                            buff.apply(col.transform);
                        }
                    }
                }
            }
        }

        private void Update() {
            resetAttributes();
            updateBuffs();

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

        void resetAttributes() {
            attrs.ForEach(attr => attr.reset());
        }

        void updateBuffs() {
            buffs.ForEach(buff => {
                if(buff.finished)
                    buff.finish();
                else
                    buff.tick();
            });
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
                    b = new ArmorShredBuff(6, missileR3ArmorShred);
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

            if(laserR3Unlock && Time.time >= laserR3lastFire + laserR3tickRate) {
                DamageModifierBuff dmb = new DamageModifierBuff(4, -laserR3PctModifier * laserR3tickRate);
                dmb.apply(targetMinion);
                laserR3lastFire = Time.time;
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
            if(fireCooldown <= 0f) {
                float dam = damage.get();

                Buff b = null;

                if(fireAblazeUnlock) {
                    b = new AblazeBuff(ablazeDuration, fireDOT.get(), fireDotTickInterval, fireAblazePctHealth, ablazeEffect);
                }

                bool toExplode = false;

                if(fireR3Unlock) {
                    foreach(Buff buff in targetMinion.buffs) {
                        if(buff is AblazeBuff) {
                            toExplode = true;
                        }
                    }
                }

                shootTargetProjectile(fireballPrefab, dam, toExplode ? fireExplosionRadius : 0, b);
                fireCooldown = 1f / fireRate.get();
            }
        }

        void beacon() {
            BeaconTurretBuff turretBuff = new BeaconTurretBuff(beaconID, 1f, beaconTurretFireRate.get(),
                beaconTurretRange.get(), beaconTurretDamage.get(), beaconTurretLaserDOT.get(), beaconTurretFireDOT.get());

            BeaconMinionBuff minionBuff = new BeaconMinionBuff(beaconID, 3f, beaconMinionDamage.get(), beaconMinionMovespeed.get());

            Collider[] cols = Physics.OverlapSphere(this.transform.position, range.get());
            foreach(Collider col in cols) {
                if(col.tag.Equals(beaconTurretTag) && col.gameObject != this.gameObject) {
                    if(col.GetComponent<Turret>().towerType != Turret.TowerType.Beacon)
                        turretBuff.copy().apply(col.transform);
                }
                if(col.tag.Equals(beaconMinionTag) && col.gameObject != this.gameObject) {
                    minionBuff.copy().apply(col.transform);
                    Minion m = col.GetComponent<Minion>();
                    m.takeDamage(-beaconMinionHeal.get() / 100f * m.health.getMissing() / .5f, false, true);
                }
                if(col.tag.Equals("Player") && isPlayer) {
                    col.GetComponent<Damageable>().takeDamage(-beaconPlayerHeal.get() / 100f * col.GetComponent<Player>().health.getMissing() / .5f, false, true);
                }
            }
        }

        void beaconMoney() {
            if(isPlayer) {
                PlayerManager.getInstance().changeMoney((int)beaconMoneyPerRound.get());
            }
            else {
                EnemiesManager.getInstance().changeMoney((int)beaconMoneyPerRound.get());
            }
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
            this.laserR3tickRate = 0.25f;
            this.laserR3lastFire = Time.time;
        }

        public void fireL2Upgrade(float tickInterval, float ablazeDuration) {
            this.fireAblazeUnlock = true;
            this.fireDotTickInterval = tickInterval;
            this.ablazeDuration = ablazeDuration;
        }

        public void fireL3Upgrade(float pct) {
            this.fireL3Unlock = true;
            this.fireAblazePctHealth = pct;
        }

        public void fireL4Upgrade(float factor) {
            this.fireDotTickInterval /= factor;
            this.fireDOT.set(fireDOT.get() / Mathf.Log(factor));
            this.fireAblazePctHealth /= Mathf.Log(factor);
        }

        public void fireR3Upgrade(float rad) {
            this.fireR3Unlock = true;
            this.fireExplosionRadius = rad;
        }

        public void fireR4Upgrade(float rad, float dps) {
            this.fireR4Unlock = true;
            this.fireR4rad = rad;
            this.fireR4dps = dps;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawWireSphere(this.transform.position, range.getStart());
        }

        public void addBuff(Buff b) {
            buffs.Add(b);
        }

        [HideInInspector]
        public enum TowerType {
            Turret, Missile, Laser, Fire, Beacon
        }
    }
}

