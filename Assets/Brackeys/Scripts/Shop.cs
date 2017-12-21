using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Shop : MonoBehaviour {
        public TurretBlueprint standardTurret;
        public TurretBlueprint missileLauncher;
        public TurretBlueprint laserBeamer;

        BuildManager buildManager;

        private void Start() {
            buildManager = BuildManager.instance;
        }

        public void selectStandardTurret() {
            buildManager.selectTurretToBuild(standardTurret);
        }
        public void selectMissileLauncher() {
            buildManager.selectTurretToBuild(missileLauncher);
        }
        public void selectLaserBeamer() {
            buildManager.selectTurretToBuild(laserBeamer);
        }
    }
}
