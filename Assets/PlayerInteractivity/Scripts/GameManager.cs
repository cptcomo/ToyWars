using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class GameManager : MonoBehaviour {
        private static GameManager instance = null;

        [SerializeField]
        private MapData mapData;

        [HideInInspector]
        public int enemiesAlive;

        public PlayerStats playerStats;

        private void Awake() {
            if(instance == null) {
                instance = this;
            }
            else if(instance != this) {
                Destroy(gameObject);
            }
        }

        private void Start() {
            enemiesAlive = 0;
        }

        public static GameManager getInstance() {
            return instance;
        }

        public MapData getMapData() {
            return this.mapData;
        }
    }
}
