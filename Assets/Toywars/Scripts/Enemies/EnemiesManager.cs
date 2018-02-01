using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class EnemiesManager : MonoBehaviour {
        private static EnemiesManager instance;

        [HideInInspector]
        public int enemiesAlive;

        public int baseHealth;

        private void Awake() {
            if(instance == null) {
                instance = this;
            } else if(instance != this) {
                Destroy(gameObject);
            }
        }

        private void Start() {
            enemiesAlive = 0;
        }

        public static EnemiesManager getInstance() {
            return instance;
        }
    }
}
