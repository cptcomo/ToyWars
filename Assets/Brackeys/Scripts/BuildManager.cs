using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class BuildManager : MonoBehaviour {
        public static BuildManager instance;

        private void Awake() {
            if(instance != null) {
                Debug.LogError("More than one build manager in scene!");
                return;
            }
            instance = this;
        }

        public GameObject standardTurretPrefab;

        private void Start() {
            turretToBuild = standardTurretPrefab;
        }

        private GameObject turretToBuild;

        public GameObject getTurretToBuild() {
            return turretToBuild;
        }
    }
}
