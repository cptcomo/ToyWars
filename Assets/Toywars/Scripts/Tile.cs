﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Toywars {
    public class Tile : MonoBehaviour {
        [HideInInspector]
        public GameObject turret;

        [HideInInspector]
        public TurretBlueprint turretBlueprint;

        private GameManager gm;
        private BuildManager buildManager;
        private PlayerManager pm;

        private void Start() {
            gm = GameManager.getInstance();
            buildManager = BuildManager.getInstance();
            pm = PlayerManager.getInstance();
        }

        private void OnMouseDown() {
            tileClick();
        }

        public void tileClick() {
            if(gm.isBuilding()) {
                if(EventSystem.current.IsPointerOverGameObject()) {
                    return;
                }

                if(turret != null) {
                    gm.callEventSelectTile(this);
                    return;
                }

                if(!buildManager.canBuild)
                    return;

                if(!buildManager.hasMoney)
                    return;

                buildTurret(buildManager.getTurretToBuild());
            }
        }

        void buildTurret(TurretBlueprint blueprint) {
            if(blueprint == null)
                return;

            GameObject turret = (GameObject)Instantiate(blueprint.prefab, getBuildPosition(), Quaternion.identity);
            this.turret = turret;
            turret.GetComponent<Turret>().setTile(this);

            GameObject effect = (GameObject)Instantiate(blueprint.buildEffect, getBuildPosition(), Quaternion.identity);
            Destroy(effect, 5f);

            this.turretBlueprint = blueprint;
            pm.changeMoney(-turretBlueprint.cost);
        }

        public void upgradeTurret(int upgradeIndex) {
            this.turret.GetComponent<Turret>().upgrade(upgradeIndex, true);
            gm.callEventSelectTile(this);
        }

        public Vector3 getBuildPosition() {
            return transform.position;
        }
    }
}
