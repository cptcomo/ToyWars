using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class Minion : MonoBehaviour, Damageable {
        protected GameManager gm;

        public Sprite sprite;
        public string minionName;
        public GameObject bulletPrefab;
        public GameObject meleePrefab;

        public MinionType minionType;

        public Attribute health;
        public Attribute armor;
        public Attribute damage;
        public Attribute attackRadius;
        public Attribute damageModifier;
        public int moneyValue;
        public float rangeFireRate;
        float rangeLastFire;

        public GameObject deathEffect;
        public GameObject hpBarPrefab;
        public string hpCanvasName;
        private GameObject hpCanvas;
        public float hpBarOffset;
        private GameObject hpBar;
        private Image hpBarImage;

        private MinionMovement minionMovement;

        public float mobPct, dpsPct;

        [HideInInspector]
        public List<Buff> buffs;
        List<Attribute> attrs;

        public virtual void Start() {
            gm = GameManager.getInstance();
            minionMovement = (MinionMovement)GetComponent<MinionMovement>();
            buffs = new List<Buff>();
            attrs = new List<Attribute>();
            attrs.Add(health);
            attrs.Add(damage);
            attrs.Add(armor);
            attrs.Add(attackRadius);
            attrs.Add(damageModifier);
            attrs.Add(minionMovement.detectionRadius);
            attrs.Add(minionMovement.speed);
            attrs.ForEach(attr => attr.init());
            hpCanvas = GameObject.Find(hpCanvasName);
            hpBar = (GameObject)Instantiate(hpBarPrefab);
            hpBar.transform.SetParent(hpCanvas.transform, false);
            hpBarImage = hpBar.GetComponent<Image>();
            minionMovement.setMinionType(minionType);
            minionMovement.setKiteRange(attackRadius.get() * .9f);
            if(hpBarImage == null)
                Debug.LogWarning("Minion Health Bar does not have an Image component");
            if(minionType == MinionType.Range)
                rangeLastFire = Time.time;
        }

        public virtual void takeDamage(float damage, bool playerShot, bool ignoreArmor) { }

        protected float armorDamageMultiplier(bool ignoreArmor, float armor) {
            if(ignoreArmor)
                return 1;

            if(armor > 0) {
                return 100 / (100 + armor);
            } else {
                return 2 - 100 / (100 - armor);
            }
        }

        public virtual void Update() {
            resetAttributes();
            updateBuffs();
            attack();
            hpBar.transform.position = (Vector3.up * hpBarOffset) + transform.position;
            hpBarImage.fillAmount = health.get() / health.getStart();    
        }

        protected virtual void resetAttributes() {
            attrs.ForEach(attr => attr.reset());
        }

        protected virtual void updateBuffs() {
            buffs.ForEach(buff => {
                if(buff.finished)
                    buff.finish();
                else
                    buff.tick();
            });
        }
       
        public void removeBuff(Buff buff) {
            buffs.Remove(buff);
        }

        public virtual void OnDestroy() {
            Destroy(hpBar);
            buffs.ForEach(buff => buff.finish());
        }

        protected virtual void attack() {
            GameObject target = minionMovement.getTarget();
            if(target != null) {
                if(minionType == MinionType.Range){
                    if(Vector3.Distance(this.transform.position, target.transform.position) < attackRadius.get() && Time.time >= rangeLastFire + rangeFireRate) {
                        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
                        TargetBullet bullet = bulletGO.GetComponent<TargetBullet>();
                        if(bullet != null) {
                            bullet.seek(target.transform);
                            bullet.setDamage(damage.get());
                            bullet.setSpeed(70f);
                            bullet.setBuffToApply(null);
                            bullet.setTargetTag(target.tag);
                        }
                        rangeLastFire = Time.time;
                    }
                }
                else {
                    Damageable component = (Damageable)target.GetComponent(typeof(Damageable));
                    if(component != null && Vector3.Distance(this.transform.position, target.transform.position) < attackRadius.get()) {
                        component.takeDamage(damage.get() * damageModifier.get() / 100f * Time.deltaTime, false, false);
                    }
                }
            }
        }

        public virtual Vector2 calculateScore() {
            float rawScore = health.getStart() / 5f + damage.getStart() * 10 + attackRadius.getStart() * 40 + minionMovement.speed.getStart() * 30;
            return new Vector2(rawScore * (mobPct / 100f), rawScore * (dpsPct / 100f));
        }

        public void addBuff(Buff buff) {
            buffs.Add(buff);
        }

        public MinionMovement getMinionMovement() {
            return this.minionMovement;
        }

    }

    public enum MinionType {
        Melee, Range, Fast, Tank, Divide
    }
}
