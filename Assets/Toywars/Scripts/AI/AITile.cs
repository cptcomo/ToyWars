using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class AITile : MonoBehaviour {
        [HideInInspector]
        public GameObject turret;

        [HideInInspector]
        public TurretBlueprint turretBlueprint;

        private EnemiesManager em;

        private void Start() {
            em = EnemiesManager.getInstance();
        }

        public void buildTurret(TurretBlueprint blueprint) {
            GameObject turret = (GameObject)Instantiate(blueprint.prefab, getBuildPosition(), Quaternion.identity);
            this.turret = turret;

            GameObject effect = (GameObject)Instantiate(blueprint.buildEffect, getBuildPosition(), Quaternion.identity);
            Destroy(effect, 5f);

            this.turretBlueprint = blueprint;
            em.money -= turretBlueprint.cost;
        }

        public void upgradeTurret(int upgradeIndex) {
            this.turret.GetComponent<Turret>().upgrade(upgradeIndex, false);
        }

        public Vector3 getBuildPosition() {
            return transform.position;
        }
    }
}
