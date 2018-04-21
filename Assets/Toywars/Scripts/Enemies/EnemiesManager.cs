using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class EnemiesManager : MonoBehaviour {
        private static EnemiesManager instance;

        [HideInInspector]
        public int enemiesAlive;

        public int baseHealth;

        public int lastBaseHealth;

        [SerializeField]
        int money;

        [HideInInspector]
        public float moneyAmplify;

        private void Awake() {
            if(instance == null) {
                instance = this;
            } else if(instance != this) {
                Destroy(gameObject);
            }
        }

        private void Start() {
            enemiesAlive = 0;
            lastBaseHealth = baseHealth;
        }

        public int getMoney() {
            return money;
        }

        public void changeMoney(int d) {
            if(d > 0)
                d = (int)(d * 1.4f);
            this.money += d;
        }

        public static EnemiesManager getInstance() {
            return instance;
        }
    }
}
