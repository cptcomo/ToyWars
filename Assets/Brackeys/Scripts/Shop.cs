using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Shop : MonoBehaviour {
        BuildManager buildManager;

        private void Start() {
            buildManager = BuildManager.instance;
        }

        public void purchaseStandardTurret() {
            Debug.Log("Standard turret selected");
            buildManager.setTurretToBuild(buildManager.standardTurretPrefab);
        }
        public void purchaseMissileLauncher() {
            Debug.Log("MissileLauncher selected");
            buildManager.setTurretToBuild(buildManager.missileLauncherPrefab);
        }
    }
}
