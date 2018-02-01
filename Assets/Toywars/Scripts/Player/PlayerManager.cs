using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class PlayerManager : MonoBehaviour {
        private static PlayerManager instance = null;

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
            alliesAlive = 0;
        }

        public static PlayerManager getInstance() {
            return instance;
        }
    }
}
