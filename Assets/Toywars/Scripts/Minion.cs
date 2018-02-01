using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class Minion : MonoBehaviour {
        protected GameManager gm;
        public float startHealth;
        protected float health;
        public float damage;
        public float attackRadius;

        public GameObject deathEffect;
        public GameObject hpBarPrefab;
        public string hpCanvasName;
        private GameObject hpCanvas;
        public float hpBarOffset;
        private GameObject hpBar;
        private Image hpBarImage;

        public virtual void Start() {
            gm = GameManager.getInstance();
            health = startHealth;
            hpCanvas = GameObject.Find(hpCanvasName);
            hpBar = (GameObject)Instantiate(hpBarPrefab);
            hpBar.transform.SetParent(hpCanvas.transform, false);
            hpBarImage = hpBar.GetComponent<Image>();
            if(hpBarImage == null)
                Debug.LogWarning("Minion Health Bar does not have an Image component");
        }

        public virtual void Update() {
            attack();
            hpBar.transform.position = (Vector3.up * hpBarOffset) + transform.position;
            hpBarImage.fillAmount = health / startHealth;    
        }

        public virtual void OnDestroy() {
            Destroy(hpBar);
        }

        protected virtual void attack() {}
    }
}
