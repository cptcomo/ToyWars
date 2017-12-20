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
        public GameObject missileLauncherPrefab;

        private GameObject turretToBuild;

        public GameObject getTurretToBuild() {
            return turretToBuild;
        }

        public void setTurretToBuild(GameObject turret) {
            turretToBuild = turret;
        }
    }
}
