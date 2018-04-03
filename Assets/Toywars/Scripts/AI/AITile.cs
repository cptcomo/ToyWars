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
        private AI ai;

        PathWrapper[] leftLane, centerLane, rightLane;

        private void Start() {
            em = EnemiesManager.getInstance();
            ai = AI.getInstance();
            initialize();
        }

        void initialize() {
            leftLane = new PathWrapper[ai.leftPaths.Length];
            centerLane = new PathWrapper[ai.centerPaths.Length];
            rightLane = new PathWrapper[ai.rightPaths.Length];
            for(int i = 0; i < leftLane.Length; i++) {
                leftLane[i] = new PathWrapper(ai.leftPaths[i], Vector3.Distance(this.transform.position, ai.leftPaths[i].transform.position));
            }
            for(int i = 0; i < centerLane.Length; i++) {
                centerLane[i] = new PathWrapper(ai.centerPaths[i], Vector3.Distance(this.transform.position, ai.centerPaths[i].transform.position));
            }
            for(int i = 0; i < rightLane.Length; i++) {
                rightLane[i] = new PathWrapper(ai.rightPaths[i], Vector3.Distance(this.transform.position, ai.rightPaths[i].transform.position));
            }
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
class PathWrapper {
    GameObject path;
    float dist;
    public PathWrapper(GameObject path, float dist) {
        this.path = path;
        this.dist = dist;
    }
}
