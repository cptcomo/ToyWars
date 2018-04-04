using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public Turret dummyStandard, dummyMissile, dummyLaser;

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
            while(em.money >= standardTurretBlueprint.cost) {
                float range = dummyStandard.range.get();
                Dictionary<AITile, float> tilesScore = new Dictionary<AITile, float>();
                for(int i = 0; i < tiles.Length; i++) {
                    float tileScore = 0;
                    if(tiles[i].turret != null) {
                        Collider[] cols = Physics.OverlapSphere(tiles[i].transform.position, tiles[i].turret.GetComponent<Turret>().range.get());
                        foreach(Collider col in cols) {
                            if(col.gameObject.layer.Equals(11)) {
                                AITile ti = col.GetComponent<AITile>();
                                if(tilesScore.ContainsKey(ti)) {
                                    tilesScore[ti] -= Vector3.Distance(tiles[i].transform.position, ti.transform.position) * 5000;
                                } else {
                                    tilesScore[ti] = -Vector3.Distance(tiles[i].transform.position, ti.transform.position) * 5000;
                                }   
                            }
                        }
                        continue;
                    }

                    for(int j = 0; j < tiles[i].leftWrappers.Length; j++) {
                        if(tiles[i].leftWrappers[j].getDistance() < range) {
                            tileScore += (range - tiles[i].leftWrappers[j].getDistance()) * wsm.allyLeftScore.magnitude * Random.Range(0.8f, 1.2f);
                        }
                    }

                    for(int j = 0; j < tiles[i].centerWrappers.Length; j++) {
                        if(tiles[i].centerWrappers[j].getDistance() < range) {
                            tileScore += (range - tiles[i].centerWrappers[j].getDistance()) * wsm.allyCenterScore.magnitude * Random.Range(0.8f, 1.2f);
                        }
                    }

                    for(int j = 0; j < tiles[i].rightWrappers.Length; j++) {
                        if(tiles[i].rightWrappers[j].getDistance() < range) {
                            tileScore += (range - tiles[i].rightWrappers[j].getDistance()) * wsm.allyRightScore.magnitude * Random.Range(0.8f, 1.2f);
                        }
                    }
                    if(tilesScore.ContainsKey(tiles[i]))
                        tilesScore[tiles[i]] += tileScore;
                    else tilesScore[tiles[i]] = tileScore;
                }

                AITile t = tilesScore.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                t.buildTurret(standardTurretBlueprint);
                t.turret.GetComponent<Turret>().init();

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
