using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class GameManager : MonoBehaviour {
        private static GameManager instance = null;

        [SerializeField]
        private MapData mapData;

        private void Awake() {
            if(instance == null) {
                instance = this;
            }
            else if(instance != this) {
                Destroy(gameObject);
            }
        }

        public static GameManager getInstance() {
            return instance;
        }

        public MapData getMapData() {
            return this.mapData;
        }
    }
}
