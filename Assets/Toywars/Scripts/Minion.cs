using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class Minion : MonoBehaviour, Damageable {
        protected GameManager gm;

        public Sprite sprite;
        public string minionName;

        public Attribute health;
        public Attribute armor;
        public Attribute damage;
        public Attribute attackRadius;
        public int moneyValue;

        public GameObject deathEffect;
        public GameObject hpBarPrefab;
        public string hpCanvasName;
        private GameObject hpCanvas;
        public float hpBarOffset;
        private GameObject hpBar;
        private Image hpBarImage;

        private MinionMovement minionMovement;

        public float mobPct, dpsPct;

        protected List<Buff> buffs;
        List<Attribute> attrs;

        public virtual void Start() {
            gm = GameManager.getInstance();
            buffs = new List<Buff>();
            attrs = new List<Attribute>();
            attrs.Add(health);
            attrs.Add(damage);
            attrs.Add(armor);
            attrs.Add(attackRadius);
            attrs.ForEach(attr => attr.init());
            hpCanvas = GameObject.Find(hpCanvasName);
            hpBar = (GameObject)Instantiate(hpBarPrefab);
            hpBar.transform.SetParent(hpCanvas.transform, false);
            hpBarImage = hpBar.GetComponent<Image>();
            if(hpBarImage == null)
                Debug.LogWarning("Minion Health Bar does not have an Image component");
            minionMovement = (MinionMovement)GetComponent<MinionMovement>();
        }

        public virtual void takeDamage(float damage, bool playerShot) { }

        protected float armorDamageMultiplier(float armor) {
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
                    buffs.Remove(buff);
                else
                    buff.tick();
            });
        }

        public virtual void OnDestroy() {
            Destroy(hpBar);
        }

        protected virtual void attack() {
            GameObject target = minionMovement.getTarget();
            if(target != null) {
                Damageable component = (Damageable)target.GetComponent(typeof(Damageable));
                if(component != null && Vector3.Distance(this.transform.position, target.transform.position) < attackRadius.get()) {
                    component.takeDamage(damage.get() * Time.deltaTime, false);
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
    }
}
