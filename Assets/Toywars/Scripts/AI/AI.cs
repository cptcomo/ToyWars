﻿using System.Collections;
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
                Debug.Log("Left: " + wsm.allyLeftScore);
                Debug.Log("Center: " + wsm.allyCenterScore);
                Debug.Log("Right: " + wsm.allyRightScore);
                float range = dummyStandard.range.get();
                float bestTileScore = 0;
                int index = -1;
                for(int i = 0; i < tiles.Length; i++) {
                    float tileScore = 0;
                    if(tiles[i].turret != null) {
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

                    Collider[] cols = Physics.OverlapSphere(tiles[i].transform.position, range);

                    if(tileScore > bestTileScore) {
                        bestTileScore = tileScore;
                        index = i;
                    }
                }
                
                tiles[index].buildTurret(standardTurretBlueprint);
                tiles[index].turret.GetComponent<Turret>().init();
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
