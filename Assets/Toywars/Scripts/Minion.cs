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
        public Attribute damage;
        public Attribute attackRadius;

        public GameObject deathEffect;
        public GameObject hpBarPrefab;
        public string hpCanvasName;
        private GameObject hpCanvas;
        public float hpBarOffset;
        private GameObject hpBar;
        private Image hpBarImage;

        private MinionMovement minionMovement;

        public virtual void Start() {
            gm = GameManager.getInstance();
            health.init();
            damage.init();
            attackRadius.init();
            hpCanvas = GameObject.Find(hpCanvasName);
            hpBar = (GameObject)Instantiate(hpBarPrefab);
            hpBar.transform.SetParent(hpCanvas.transform, false);
            hpBarImage = hpBar.GetComponent<Image>();
            if(hpBarImage == null)
                Debug.LogWarning("Minion Health Bar does not have an Image component");
            minionMovement = (MinionMovement)GetComponent<MinionMovement>();
        }

        public virtual void takeDamage(float damage, bool playerShot) { }

        public virtual void Update() {
            attack();
            hpBar.transform.position = (Vector3.up * hpBarOffset) + transform.position;
            hpBarImage.fillAmount = health.get() / health.getStart();    
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

        public virtual float calculateScore() {
            return health.getStart() / 5f + damage.getStart() * 10 + attackRadius.getStart() * 40 + minionMovement.speed.getStart() * 30;
        }
    }
}
