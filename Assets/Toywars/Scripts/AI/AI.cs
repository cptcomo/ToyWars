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
        public GameObject leftLane, centerLane, rightLane, middle;

        [HideInInspector]
        public GameObject[] leftPaths, centerPaths, rightPaths, middlePaths;

        public TurretBlueprint standardTurretBlueprint;
        public TurretBlueprint missileLauncherBlueprint;
        public TurretBlueprint laserBeamerBlueprint;
        public TurretBlueprint fireBlueprint;
        public TurretBlueprint supportBlueprint;

        public Turret dummyStandard, dummyMissile, dummyLaser, dummyFire, dummySupport;

        bool hasBuiltATower;

        List<AITile> tilesWithTowers;
        List<AITile> tilesWithDPSTowers;
        List<AITile> tilesWithMobTowers;

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
            middlePaths = getChildren(middle);
            hasBuiltATower = false;
            tilesWithTowers = new List<AITile>();
            tilesWithDPSTowers = new List<AITile>();
            tilesWithMobTowers = new List<AITile>();
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

        int randomIndex(float[] percentages) {
            float r = Random.Range(0f, 100f);
            float min = 0;
            float max = 0;
            for(int i = 0; i < percentages.Length; i++) {
                max = min + percentages[i];
                if(r >= min && r < max)
                    return i;
                min = max;
            }
            return -1;
        }

        void takeTurn() {
            gm.gameState = GameManager.GameState.AI;

            bool isDebug = true;

            GameObject[] minionGOs = wsm.getMinionsAvailable();
            Minion[] minions = new Minion[minionGOs.Length];
            for(int i = 0; i < minionGOs.Length; i++) {
                minions[i] = minionGOs[i].GetComponent<Minion>();
            }
            int numMinionsUnlocked = minionGOs.Length;
            if(isDebug)
                Debug.Log("Unlock slots available: " + numMinionsUnlocked);
            GameObject[] leftLane = new GameObject[numMinionsUnlocked];
            GameObject[] centerLane = new GameObject[numMinionsUnlocked];
            GameObject[] rightLane = new GameObject[numMinionsUnlocked];
            float[] weightedProbabilities;
            Debug.Log("Wave Index: " + gm.waveIndex);
            Debug.Log("Unlock Count: " + numMinionsUnlocked);
            if(numMinionsUnlocked == 1) {
                weightedProbabilities = new float[] { 100 };
            } else if(numMinionsUnlocked == 2) {
                weightedProbabilities = new float[] { 75, 25 };
            } else if(numMinionsUnlocked == 3) {
                weightedProbabilities = new float[] { 60, 35, 15 };
            } else if(numMinionsUnlocked == 4)
                weightedProbabilities = new float[] {15, 40, 5, 50};
            else {
                weightedProbabilities = new float[] { 0, 30, 10, 30, 30 };
            }
            for(int i = 0; i < leftLane.Length; i++) {
                leftLane[i] = minionGOs[randomIndex(weightedProbabilities)];
                centerLane[i] = minionGOs[randomIndex(weightedProbabilities)];
                rightLane[i] = minionGOs[randomIndex(weightedProbabilities)];
            }
            Debug.Log("Left: ");
            foreach(GameObject go in leftLane) {
                Debug.Log(go);
            }
            Debug.Log("Center: ");
            foreach(GameObject go in centerLane) {
                Debug.Log(go);
            }
            Debug.Log("Right: ");
            foreach(GameObject go in rightLane) {
                Debug.Log(go);
            }
            wsm.retrieveAIWaveComposition(leftLane, centerLane, rightLane);

            bool isDone = false;
            int actionsRemaining = 100;
            while(!isDone && actionsRemaining > 0) {
                Lane vulnerableLane;
                if(sumScore(wsm.allyLeftScore) > Mathf.Max(sumScore(wsm.allyCenterScore), sumScore(wsm.allyRightScore)))
                    vulnerableLane = Lane.left;
                else if(sumScore(wsm.allyCenterScore) > Mathf.Max(sumScore(wsm.allyLeftScore), sumScore(wsm.allyRightScore)))
                    vulnerableLane = Lane.center;
                else if(sumScore(wsm.allyRightScore) > Mathf.Max(sumScore(wsm.allyLeftScore), sumScore(wsm.allyCenterScore)))
                    vulnerableLane = Lane.right;
                else {
                    float rng = Random.Range(0f, 100f);
                    if(rng < 33) {
                        vulnerableLane = Lane.left;
                    } else if(rng < 67) {
                        vulnerableLane = Lane.center;
                    } else
                        vulnerableLane = Lane.right;
                }

                if(isDebug)
                    Debug.Log("Vulnerable Lane: " + vulnerableLane);

                Vector2 score;
                if(vulnerableLane == Lane.left) {
                    score = wsm.allyLeftScore;
                } else if(vulnerableLane == Lane.center) {
                    score = wsm.allyCenterScore;
                } else {
                    score = wsm.allyRightScore;
                }
                float ratio = score.y / score.x;

                if(isDebug)
                    Debug.Log("Vulnerable Lane Ratio: " + ratio);

                if(Random.Range(0f, 1f) < .8f && hasBuiltATower) { //Upgrade
                    if(isDebug)
                        Debug.Log("In Upgrade Branch");

                    List<int> availableIndexes = new List<int>();
                    AITile tile;
                    bool specialCase = false;
                    float money = em.getMoney();
                    if(ratio > 1.5f) {
                        tile = getBestTile(tilesWithDPSTowers.ToArray(), -1);
                        if(tile != null) {
                            specialCase = true;
                            Turret tu = tile.turret.GetComponent<Turret>();
                            TowerUpgrade[] upgs = tu.towerUpgradePath.getAvailableUpgrades();
                            //All the tank shredding upgrades are on the leftPath
                            if(upgs[0] != null && money >= upgs[0].cost)
                                availableIndexes.Add(0);
                            if(upgs[1] != null && tu.towerUpgradePath.rightIndex < 1 && money >= upgs[1].cost)
                                availableIndexes.Add(1);
                        }
                    }
                    else if(ratio < 0.5f) {
                        tile = getBestTile(tilesWithMobTowers.ToArray(), -1);
                        if(tile != null) {
                            specialCase = true;
                            Turret tu = tile.turret.GetComponent<Turret>();
                            TowerUpgrade[] upgs = tu.towerUpgradePath.getAvailableUpgrades();
                            if(tu.towerType == Turret.TowerType.Fire) { //Fire's mob killing is on the right
                                if(upgs[1] != null && money >= upgs[1].cost)
                                    availableIndexes.Add(1);
                                if(upgs[0] != null && tu.towerUpgradePath.leftIndex < 1 && money >= upgs[0].cost)
                                    availableIndexes.Add(0);
                            }
                            else {
                                if(upgs[0] != null && money >= upgs[0].cost)
                                    availableIndexes.Add(0);
                                if(upgs[1] != null && money >= upgs[1].cost)
                                    availableIndexes.Add(1);
                            }
                        }
                    }
                    else {
                        tile = getBestTile(tilesWithTowers.ToArray(), -1);
                    }

                    if(!specialCase) {
                        if(tile != null) {
                            Turret tower = tile.turret.GetComponent<Turret>();
                            TowerUpgrade[] upgrades = tower.towerUpgradePath.getAvailableUpgrades();

                            if(upgrades[0] != null && upgrades[1] != null) {
                                if(money >= upgrades[0].cost)
                                    availableIndexes.Add(0);
                                if(money >= upgrades[1].cost)
                                    availableIndexes.Add(1);
                            } else if(upgrades[0] != null) {
                                if(money >= upgrades[0].cost)
                                    availableIndexes.Add(0);
                            } else if(upgrades[1] != null) {
                                if(money >= upgrades[1].cost)
                                    availableIndexes.Add(1);
                            }
                        } else continue;
                    }
                    
                    if(availableIndexes.Count > 0) {
                        if(availableIndexes.Count == 1) {
                            tile.upgradeTurret(availableIndexes[0]);
                        }
                        else
                            tile.upgradeTurret(availableIndexes[Random.Range(0, availableIndexes.Count)]);
                    }
                    else {
                        actionsRemaining -= 20;
                    }
                    actionsRemaining--;
                } else { //Build
                    if(isDebug)
                        Debug.Log("Build branch");

                    Turret turret = null;
                    TurretBlueprint blueprint = null;
                    List<TurretBlueprint> availableBlueprints = new List<TurretBlueprint>();
                    if(ratio > 1.5f) { // The 3 DPS focused towers are the turret, laser, and fire
                        if(isDebug)
                            Debug.Log("In DPS ratio branch");

                        if(em.getMoney() >= standardTurretBlueprint.cost)
                            availableBlueprints.Add(standardTurretBlueprint);
                        if(em.getMoney() >= laserBeamerBlueprint.cost)
                            availableBlueprints.Add(laserBeamerBlueprint);
                        if(em.getMoney() >= fireBlueprint.cost)
                            availableBlueprints.Add(fireBlueprint);
                    } else if(ratio < 0.5f) { //The group focused towers are primarily the missile launcher and the fire tower
                        if(isDebug)
                            Debug.Log("In Group focused branch");
                        if(em.getMoney() >= missileLauncherBlueprint.cost)
                            availableBlueprints.Add(missileLauncherBlueprint);
                        if(em.getMoney() >= fireBlueprint.cost)
                            availableBlueprints.Add(fireBlueprint);
                    } else { //Otherwise, any of the towers would do.
                        if(em.getMoney() >= standardTurretBlueprint.cost)
                            availableBlueprints.Add(standardTurretBlueprint);
                        if(em.getMoney() >= missileLauncherBlueprint.cost)
                            availableBlueprints.Add(missileLauncherBlueprint);
                        if(em.getMoney() >= laserBeamerBlueprint.cost)
                            availableBlueprints.Add(laserBeamerBlueprint);
                        if(em.getMoney() >= fireBlueprint.cost)
                            availableBlueprints.Add(fireBlueprint);
                        if(em.getMoney() >= supportBlueprint.cost)
                            availableBlueprints.Add(supportBlueprint);
                    }

                    if(availableBlueprints.Count > 0) {
                        float[] percentages = new float[availableBlueprints.Count];
                        for(int i = 0; i < percentages.Length; i++) {
                            percentages[i] = 100f / percentages.Length;
                        }
                        int index = randomIndex(percentages);
                        blueprint = availableBlueprints[index];
                        if(blueprint == standardTurretBlueprint)
                            turret = dummyStandard;
                        else if(blueprint == missileLauncherBlueprint)
                            turret = dummyMissile;
                        else if(blueprint == laserBeamerBlueprint)
                            turret = dummyLaser;
                        else if(blueprint == fireBlueprint)
                            turret = dummyFire;
                        else if(blueprint == supportBlueprint)
                            turret = dummySupport;
                        else Debug.LogError("Did not match a blueprint to a turret.");

                        if(isDebug)
                            Debug.Log("Chose: " + turret.towerType);
                    }

                    if(blueprint == null) {
                        if(isDebug) {
                            Debug.Log("Did not choose a tower");
                        }
                        actionsRemaining -= 20;
                        continue;
                    }


                    if(em.getMoney() < blueprint.cost) {
                        isDone = true;
                        continue;
                    }

                    float range = turret.range.get();
                    AITile t = getBestTile(tiles, range);
                    if(t == null)
                        continue;
                    t.buildTurret(blueprint);
                    Turret tu = t.turret.GetComponent<Turret>();
                    tu.init();
                    tilesWithTowers.Add(t);
                    if(tu.towerType == Turret.TowerType.Turret || tu.towerType == Turret.TowerType.Laser || tu.towerType == Turret.TowerType.Fire)
                        tilesWithDPSTowers.Add(t);
                    if(tu.towerType == Turret.TowerType.Missile || tu.towerType == Turret.TowerType.Fire)
                        tilesWithMobTowers.Add(t);
                    hasBuiltATower = true;
                    actionsRemaining--;
                }
            }
            
        }

        AITile getBestTile(AITile[] tiles, float range) {
            Dictionary<AITile, float> tilesScore = new Dictionary<AITile, float>();
            for(int i = 0; i < tiles.Length; i++) {
                float tileScore = 0;

                if(range != -1 && tiles[i].turret != null) {
                    continue;
                }

                if(range == -1) {
                    if(!tiles[i].turret.GetComponent<Turret>().towerUpgradePath.hasRemainingUpgrades())
                        continue;
                    range = tiles[i].turret.GetComponent<Turret>().range.get();
                }

                for(int j = 0; j < tiles[i].leftWrappers.Length; j++) {
                    if(tiles[i].leftWrappers[j].getDistance() < range) {
                        tileScore += (range - tiles[i].leftWrappers[j].getDistance()) * sumScore(wsm.allyLeftScore) * Random.Range(0.8f, 1.2f);
                    }
                }

                for(int j = 0; j < tiles[i].centerWrappers.Length; j++) {
                    if(tiles[i].centerWrappers[j].getDistance() < range) {
                        tileScore += (range - tiles[i].centerWrappers[j].getDistance()) * sumScore(wsm.allyCenterScore) * Random.Range(0.8f, 1.2f);
                    }
                }

                for(int j = 0; j < tiles[i].rightWrappers.Length; j++) {
                    if(tiles[i].rightWrappers[j].getDistance() < range) {
                        tileScore += (range - tiles[i].rightWrappers[j].getDistance()) * sumScore(wsm.allyRightScore) * Random.Range(0.8f, 1.2f);
                    }
                }

                for(int j = 0; j < tiles[i].middleWrappers.Length; j++) {
                    if(tiles[i].middleWrappers[j].getDistance() < range) {
                        tileScore += (range - tiles[i].middleWrappers[j].getDistance()) * (sumScore(wsm.allyLeftScore) + sumScore(wsm.allyCenterScore) + sumScore(wsm.allyRightScore)) * 3f * Random.Range(0.8f, 1.2f);
                    }
                }
                if(tilesScore.ContainsKey(tiles[i]))
                    tilesScore[tiles[i]] += tileScore;
                else tilesScore[tiles[i]] = tileScore;
            }

            if(tilesScore.Count > 0) {
                AITile t = tilesScore.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                return t;
            }
            else {
                return null;
            }
        }

        float sumScore(Vector2 score) {
            return score.x + score.y;
        }

        enum Lane {
            left, center, right
        }
    }
}
