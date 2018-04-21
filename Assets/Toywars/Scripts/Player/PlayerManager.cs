using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class PlayerManager : MonoBehaviour {
        private static PlayerManager instance = null;

        GameManager gm;

        [HideInInspector]
        public int alliesAlive;

        public int baseHealth;

        public int lastBaseHealth;

        [SerializeField]
        int money;

        [SerializeField]
        int exp;

        [HideInInspector]
        public float moneyAmplify;
        [HideInInspector]
        public float expAmplify;

        private void Awake() {
            if(instance == null) {
                instance = this;
            } else if(instance != this) {
                Destroy(gameObject);
            }
        }

        private void Start() {
            gm = GameManager.getInstance();
            gm.StartNextWaveEvent += assertion;
            gm.EndWaveEvent += assertion;
            lastBaseHealth = baseHealth;
            alliesAlive = 0;
        }

        private void OnDisable() {
            gm.StartNextWaveEvent -= assertion;
            gm.EndWaveEvent -= assertion;
        }

        void assertion() {
            if(alliesAlive != 0) {
                Debug.LogError("Minion count non-zero on wave start or end");
            }
        }

        public int getMoney() {
            return money;
        }

        public void changeMoney(int d) {
            if(d > 0) d += (int)(d * moneyAmplify / 100f);
            this.money += d;
        }

        public int getExp() {
            return exp;
        }

        public void changeExp(int d) {
            if(d > 0) d += (int)(d * expAmplify / 100f);
            this.exp += d;
        }

        public static PlayerManager getInstance() {
            return instance;
        }
    }
}
