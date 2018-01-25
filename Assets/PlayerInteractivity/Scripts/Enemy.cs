using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerInteractivity {
    public class Enemy : MonoBehaviour {
        GameManager gm;
        public float startHealth = 100f;
        private float health;
        public int moneyValue = 50;
        public int expValue = 20;
        public float damage;
        public float attackRadius = 2f;

        public GameObject deathEffect;
        public GameObject hpBarPrefab;
        public string hpCanvasName;
        private GameObject hpCanvas;
        public float hpBarOffset;
        private GameObject hpBar;
        private Image hpBarImage;

        private void Start() {
            gm = GameManager.getInstance();
            health = startHealth;
            hpCanvas = GameObject.Find(hpCanvasName);
            hpBar = (GameObject)Instantiate(hpBarPrefab);
            hpBar.transform.SetParent(hpCanvas.transform, false);
            hpBarImage = hpBar.GetComponent<Image>();
            if(hpBarImage == null)
                Debug.LogWarning("Enemy Health Bar does not have an Image component");
        }

        private void Update() {
            Collider[] cols = Physics.OverlapSphere(this.transform.position, attackRadius);
            foreach(Collider c in cols) {
                if(c.transform.tag.Equals("Player")) {
                    c.GetComponent<Player>().takeDamage(damage * Time.deltaTime);
                }
            }

            hpBar.transform.position = (Vector3.up * hpBarOffset) + transform.position;
            hpBarImage.fillAmount = health / startHealth;
        }

        public void takeDamage(float damage, bool playerShot) {
            this.health -= damage;
            if(health <= 0f) {
                die(playerShot);
            }
        }

        void die(bool playerShot) {
            gm.enemiesAlive--;
            gm.playerStats.money += moneyValue;
            if(playerShot) {
                gm.playerStats.exp += expValue;
            }
                
            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
            Destroy(gameObject);
        }

        private void OnDestroy() {
            Destroy(hpBar);
        }
    }
}