using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class AI : MonoBehaviour {
        private static AI instance;

        GameManager gm;
        EnemiesManager em;
        WaveSpawnerManager wsm;

        public GameObject enemySide;
        AITile[] tiles;
        public GameObject leftLane, centerLane, rightLane;

        [HideInInspector]
        public GameObject[] leftPaths, centerPaths, rightPaths;

        public TurretBlueprint standardTurretBlueprint;
        public TurretBlueprint missileLauncherBlueprint;
        public TurretBlueprint laserBeamerBlueprint;

        public static AI getInstance() {
            return instance;
        }

        private void Awake() {
            if(instance == null)
                instance = this;
            else if(instance != this)
                Destroy(gameObject);
        }

        private void Start() {
            gm = GameManager.getInstance();
            em = EnemiesManager.getInstance();
            wsm = WaveSpawnerManager.getInstance();
            gm.AIStartTurnEvent += takeTurn;
            initReferences();
        }

        void initReferences() {
            tiles = enemySide.GetComponentsInChildren<AITile>();
            leftPaths = getChildren(leftLane);
            centerPaths = getChildren(centerLane);
            rightPaths = getChildren(rightLane);
        }

        GameObject[] getChildren(GameObject go) {
            Transform[] ts = go.GetComponentsInChildren<Transform>();
            List<GameObject> children = new List<GameObject>();
            foreach(Transform t in ts)
                if(!t.name.Equals(go.name))
                    children.Add(t.gameObject);
            return children.ToArray();
        }

        private void OnDisable() {
            gm.AIStartTurnEvent -= takeTurn;
        }

        void takeTurn() {
            gm.gameState = GameManager.GameState.AI;
            if(em.money >= standardTurretBlueprint.cost) {
                Debug.Log("Left: " + wsm.allyLeftScore);
                Debug.Log("Center: " + wsm.allyCenterScore);
                Debug.Log("Right: " + wsm.allyRightScore);
                /*
                tiles[624].buildTurret(standardTurretBlueprint);
                tiles[624].turret.GetComponent<Turret>().init();
                tiles[624].upgradeTurret(0);
                tiles[624].upgradeTurret(0);
                tiles[624].upgradeTurret(0);
                tiles[624].upgradeTurret(0);
                tiles[624].upgradeTurret(1);
                tiles[624].upgradeTurret(1); */
            }
        }
    }
}
