using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Shop : MonoBehaviour {
        public TurretBlueprint standardTurret;
        public TurretBlueprint missileLauncher;

        BuildManager buildManager;

        private void Start() {
            buildManager = BuildManager.instance;
        }

        public void selectStandardTurret() {
            buildManager.selectTurretToBuild(standardTurret);
        }
        public void selectMissileLauncher() {
            Debug.Log("MissileLauncher selected");
            buildManager.selectTurretToBuild(missileLauncher);
        }
    }
}
