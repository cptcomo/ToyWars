using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class Shop : MonoBehaviour {
        public GameObject shop;

        public TurretBlueprint standardTurret;
        public TurretBlueprint missileLauncher;
        public TurretBlueprint laserBeamer;
        public TurretBlueprint fireTower;
        public TurretBlueprint supportTower;

        public Image turretBackground;
        public Image missileBackground;
        public Image laserBackground;
        public Image fireBackground;
        public Image supportBackground;

        GameManager gm;
        BuildManager buildManager;

        Color startColor;
        Color highlightColor;

        private void Start() {
            gm = GameManager.getInstance();
            buildManager = BuildManager.getInstance();
            gm.EndWaveEvent += toggleShop;
            gm.AIStartTurnEvent += toggleShop;
            startColor = turretBackground.color;
            highlightColor = new Color(startColor.r, startColor.g, startColor.b, .4f);
        }

        private void OnDisable() {
            gm.EndWaveEvent -= toggleShop;
            gm.AIStartTurnEvent -= toggleShop;
        }

        private void Update() {
            
        }

        public void selectStandardTurret() {
            buildManager.selectTurretToBuild(standardTurret);
            turretBackground.color = highlightColor;
            missileBackground.color = startColor;
            laserBackground.color = startColor;
            fireBackground.color = startColor;
            supportBackground.color = startColor;
        }

        public void selectMissileLauncher() {
            buildManager.selectTurretToBuild(missileLauncher);
            turretBackground.color = startColor;
            missileBackground.color = highlightColor;
            laserBackground.color = startColor;
            fireBackground.color = startColor;
            supportBackground.color = startColor;
        }

        public void selectLaserBeamer() {
            buildManager.selectTurretToBuild(laserBeamer);
            turretBackground.color = startColor;
            missileBackground.color = startColor;
            laserBackground.color = highlightColor;
            fireBackground.color = startColor;
            supportBackground.color = startColor;
        }

        public void selectFireTower() {
            buildManager.selectTurretToBuild(fireTower);
            turretBackground.color = startColor;
            missileBackground.color = startColor;
            laserBackground.color = startColor;
            fireBackground.color = highlightColor;
            supportBackground.color = startColor;
        }

        public void selectSupportTower() {
            buildManager.selectTurretToBuild(supportTower);
            turretBackground.color = startColor;
            missileBackground.color = startColor;
            laserBackground.color = startColor;
            fireBackground.color = startColor;
            supportBackground.color = highlightColor;
        }

        public void selectNone() {
            turretBackground.color = startColor;
            missileBackground.color = startColor;
            laserBackground.color = startColor;
            fireBackground.color = startColor;
            supportBackground.color = startColor;
        }

        void toggleShop() {
            shop.SetActive(!shop.activeSelf);
        }
    }
}
