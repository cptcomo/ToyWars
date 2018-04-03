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

        public int money;

        public int exp;

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

        public static PlayerManager getInstance() {
            return instance;
        }
    }
}
