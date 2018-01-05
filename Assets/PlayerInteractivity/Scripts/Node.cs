using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerInteractivity {
    public class Node : MonoBehaviour {
        public Vector3 posOffset;

        [HideInInspector]
        public GameObject turret;
        [HideInInspector]
        public TurretBlueprint turretBlueprint;

        private GameManager gm;
        private BuildManager buildManager;

        private void Start() {
            gm = GameManager.getInstance();
            buildManager = BuildManager.getInstance();
        }

        private void OnMouseDown() {
            if(gm.isBuilding()) {
                if(EventSystem.current.IsPointerOverGameObject())
                    return;
                if(turret != null) {
                    buildManager.selectNode(this);
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
            GameObject turret = (GameObject)Instantiate(blueprint.prefab, getBuildPosition(), Quaternion.identity);
            this.turret = turret;

            GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, getBuildPosition(), Quaternion.identity);
            Destroy(effect, 5f);

            this.turretBlueprint = blueprint;
            gm.playerStats.money -= turretBlueprint.cost;
        }

        public Vector3 getBuildPosition() {
            return transform.position;
        }
    }
}
