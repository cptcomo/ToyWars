using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class Minion : MonoBehaviour {
        protected GameManager gm;
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

        protected virtual void attack() {}
    }
}
