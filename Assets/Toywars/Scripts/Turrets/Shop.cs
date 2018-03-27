using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class Shop : MonoBehaviour {
        public GameObject shop;

        public TurretBlueprint standardTurret;
        public TurretBlueprint missileLauncher;
        public TurretBlueprint laserBeamer;

        GameManager gm;
        BuildManager buildManager;

        private void Start() {
            gm = GameManager.getInstance();
            buildManager = BuildManager.getInstance();
            gm.EndWaveEvent += toggleShop;
            gm.AIStartTurnEvent += toggleShop;
        }

        private void OnDisable() {
            gm.EndWaveEvent -= toggleShop;
            gm.AIStartTurnEvent -= toggleShop;
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

        void toggleShop() {
            shop.SetActive(!shop.activeSelf);
        }
    }
}
