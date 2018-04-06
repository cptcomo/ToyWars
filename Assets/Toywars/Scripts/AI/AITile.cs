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

        [HideInInspector]
        public PathWrapper[] leftWrappers, centerWrappers, rightWrappers;

        private void Start() {
            em = EnemiesManager.getInstance();
            ai = AI.getInstance();
            initialize();
        }

        void initialize() {
            leftWrappers = new PathWrapper[ai.leftPaths.Length];
            centerWrappers = new PathWrapper[ai.centerPaths.Length];
            rightWrappers = new PathWrapper[ai.rightPaths.Length];
            for(int i = 0; i < leftWrappers.Length; i++) {
                leftWrappers[i] = new PathWrapper(ai.leftPaths[i], Vector3.Distance(this.transform.position, ai.leftPaths[i].transform.position));
            }
            for(int i = 0; i < centerWrappers.Length; i++) {
                centerWrappers[i] = new PathWrapper(ai.centerPaths[i], Vector3.Distance(this.transform.position, ai.centerPaths[i].transform.position));
            }
            for(int i = 0; i < rightWrappers.Length; i++) {
                rightWrappers[i] = new PathWrapper(ai.rightPaths[i], Vector3.Distance(this.transform.position, ai.rightPaths[i].transform.position));
            }
        }
        
        public void buildTurret(TurretBlueprint blueprint) {
            GameObject turret = (GameObject)Instantiate(blueprint.prefab, getBuildPosition(), Quaternion.identity);
            this.turret = turret;

            GameObject effect = (GameObject)Instantiate(blueprint.buildEffect, getBuildPosition(), Quaternion.identity);
            Destroy(effect, 5f);

            this.turretBlueprint = blueprint;
            em.changeMoney(-turretBlueprint.cost);
        }

        public void upgradeTurret(int upgradeIndex) {
            this.turret.GetComponent<Turret>().upgrade(upgradeIndex, false);
        }

        public Vector3 getBuildPosition() {
            return transform.position;
        }
    }
}
public class PathWrapper {
    GameObject path;
    float dist;
    public PathWrapper(GameObject path, float dist) {
        this.path = path;
        this.dist = dist;
    }
    public float getDistance() {
        return dist;
    }
}
