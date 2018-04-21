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

            bool isDebug = false;

            GameObject[] minionGOs = wsm.getMinionsAvailable();
            Minion[] minions = new Minion[minionGOs.Length];
            for(int i = 0; i < minionGOs.Length; i++) {
                minions[i] = minionGOs[i].GetComponent<Minion>();
            }
            int unlockCount = wsm.getMinionsUnlockCount();
            if(isDebug)
                Debug.Log("Unlock slots available: " + unlockCount);
            GameObject[] leftLane = new GameObject[unlockCount];
            GameObject[] centerLane = new GameObject[unlockCount];
            GameObject[] rightLane = new GameObject[unlockCount];       
            for(int i = 0; i < leftLane.Length; i++) {
                if(PlayerManager.getInstance().baseHealth < 7f && Random.Range(0f, 1f) < .2f && unlockCount >= 3) { //Close to winning, rush! maybe
                    leftLane[i] = minionGOs[2]; //fast minion
                    centerLane[i] = minionGOs[2];
                    rightLane[i] = minionGOs[2];
                }
                else {
                    leftLane[i] = minionGOs[Random.Range(0, minions.Length)];
                    centerLane[i] = minionGOs[Random.Range(0, minions.Length)];
                    rightLane[i] = minionGOs[Random.Range(0, minions.Length)];
                }
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

                if(Random.Range(0f, 1f) < .8f && hasBuiltATower) { //Upgrade
                    if(isDebug)
                        Debug.Log("In Upgrade Branch");

                    AITile tile = getBestTile(tilesWithTowers.ToArray(), -1);
                    if(tile == null) {
                        continue;
                    }
                    Turret tower = tile.turret.GetComponent<Turret>();
                    TowerUpgrade[] upgrades = tower.towerUpgradePath.getAvailableUpgrades();
                    float money = em.getMoney();
                    List<int> availableIndexes = new List<int>();
                    if(upgrades[0] != null && upgrades[1] != null) {
                        if(money >= upgrades[0].cost)
                            availableIndexes.Add(0);
                        if(money >= upgrades[1].cost)
                            availableIndexes.Add(1);
                    }
                    else if(upgrades[0] != null) {
                        if(money >= upgrades[0].cost)
                            availableIndexes.Add(0);
                    }
                    else if(upgrades[1] != null) {
                        if(money >= upgrades[1].cost)
                            availableIndexes.Add(1);
                    }
                    if(availableIndexes.Count > 0) {
                        if(availableIndexes.Count == 1)
                            tile.upgradeTurret(availableIndexes[0]);
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
                    Vector2 score;
                    TurretBlueprint blueprint = null;
                    Turret turret = null;
                    if(vulnerableLane == Lane.left) {
                        score = wsm.allyLeftScore;
                    } else if(vulnerableLane == Lane.center) {
                        score = wsm.allyCenterScore;
                    } else {
                        score = wsm.allyRightScore;
                    }
                    float ratio = score.y / score.x;
                    if(isDebug) {
                        Debug.Log("Vulnerable lane score: " + score);
                        Debug.Log("DPS to group ratio: " + ratio);
                    }
                    if(ratio > 1.5f) { // The 2 DPS focused towers are the turret and laser beamer      
                        if(isDebug)
                            Debug.Log("In DPS ratio branch");
                        if(em.getMoney() > Mathf.Max(standardTurretBlueprint.cost, laserBeamerBlueprint.cost)) {
                            float rng = Random.Range(0f, 100f);
                            if(rng < 60f) {
                                if(isDebug)
                                    Debug.Log("Both turret and laser beamer were eligible, selected turret with: " + Mathf.Round(rng));
                                blueprint = standardTurretBlueprint;
                                turret = dummyStandard;
                            }
                            else {
                                if(isDebug)
                                    Debug.Log("Both turret and laser beamer were eligible, selected laser beamer with: " + Mathf.Round(rng));
                                blueprint = laserBeamerBlueprint;
                                turret = dummyLaser;
                            }
                        }
                        else if(em.getMoney() > Mathf.Min(standardTurretBlueprint.cost, laserBeamerBlueprint.cost)) {
                            if(isDebug)
                                Debug.Log("Only one was available, cheaper one chosen");
                            blueprint = standardTurretBlueprint.cost < laserBeamerBlueprint.cost ? standardTurretBlueprint : laserBeamerBlueprint;
                            turret = standardTurretBlueprint.cost < laserBeamerBlueprint.cost ? dummyStandard : dummyLaser;
                        }
                    }
                    else if(ratio < 0.5f) { //The group focused towers are primarily the missile launcher and maybe the fire tower
                        if(isDebug)
                            Debug.Log("In Group focused branch");
                        if(em.getMoney() > Mathf.Max(missileLauncherBlueprint.cost, fireBlueprint.cost)) {
                            float rng = Random.Range(0f, 100f);
                            if(rng < 75f) {
                                if(isDebug)
                                    Debug.Log("Both missile and fire tower were eligible, selected missile with: " + Mathf.Round(rng));
                                blueprint = missileLauncherBlueprint;
                                turret = dummyMissile;
                            }
                            else {
                                if(isDebug)
                                    Debug.Log("Both missile and fire tower were eligible, selected fire with: " + Mathf.Round(rng));
                                blueprint = fireBlueprint;
                                turret = dummyFire;
                            }
                        }
                        else if(em.getMoney() > Mathf.Min(missileLauncherBlueprint.cost, fireBlueprint.cost)) {
                            if(isDebug)
                                Debug.Log("Only one was available, cheaper one chosen");
                            blueprint = missileLauncherBlueprint.cost < fireBlueprint.cost ? missileLauncherBlueprint : fireBlueprint;
                            turret = missileLauncherBlueprint.cost < fireBlueprint.cost ? dummyMissile : dummyFire;
                        }
                    }
                    else { //Otherwise, any of the towers would do.
                        //23% chance for turret
                        float rng = Random.Range(0f, 100f);
                        if(isDebug)
                            Debug.Log("semi-randomly choosing tower, first rng: " + rng + " " + (rng < 23f));
                        bool chosenTurret = false;
                        if(rng < 23f) {
                            if(em.getMoney() >= standardTurretBlueprint.cost) {
                                blueprint = standardTurretBlueprint;
                                turret = dummyStandard;
                                chosenTurret = true;
                            }
                            else {
                                if(isDebug)
                                    Debug.Log("Did not have enough money: " + em.getMoney() + " vs " + standardTurretBlueprint.cost);
                            }
                        }
                        rng = Random.Range(0f, 100f);
                        if(isDebug && !chosenTurret)
                            Debug.Log("Second rng: " + rng + " " + (rng < 29.8701f));
                        if(!chosenTurret && rng < 29.8701f) {
                            if(em.getMoney() >= missileLauncherBlueprint.cost) {
                                blueprint = missileLauncherBlueprint;
                                turret = dummyMissile;
                                chosenTurret = true;
                            } else {
                                if(isDebug)
                                    Debug.Log("Did not have enough money: " + em.getMoney() + " vs " + missileLauncherBlueprint.cost);
                            }
                        }
                        rng = Random.Range(0f, 100f);
                        if(isDebug && !chosenTurret)
                            Debug.Log("Third rng: " + rng + " " + (rng < 42.55f));
                        if(!chosenTurret && rng < 32.55f) {
                            if(em.getMoney() >= laserBeamerBlueprint.cost) {
                                blueprint = laserBeamerBlueprint;
                                turret = dummyLaser;
                                chosenTurret = true;
                            } else {
                                if(isDebug)
                                    Debug.Log("Did not have enough money: " + em.getMoney() + " vs " + laserBeamerBlueprint.cost);
                            }
                        }
                        rng = Random.Range(0f, 100f);
                        if(isDebug && !chosenTurret)
                            Debug.Log("Fourth rng: " + rng + " " + (rng < 74.064f));
                        if(!chosenTurret && rng < 77.064f) {
                            if(em.getMoney() >= fireBlueprint.cost) {
                                blueprint = fireBlueprint;
                                turret = dummyFire;
                                chosenTurret = true;
                            } else {
                                if(isDebug)
                                    Debug.Log("Did not have enough money: " + em.getMoney() + " vs " + fireBlueprint.cost);
                            }
                        }
                        if(!chosenTurret) {
                            if(em.getMoney() >= supportBlueprint.cost) {
                                blueprint = supportBlueprint;
                                turret = dummySupport;
                                chosenTurret = true;
                            } else {
                                if(isDebug)
                                    Debug.Log("Did not have enough money: " + em.getMoney() + " vs " + supportBlueprint.cost);
                            }
                        }
                    }
                    if(blueprint == null) {
                        if(isDebug) {
                            Debug.Log("Did not choose a tower");
                        }
                        actionsRemaining -= 20;
                        continue;
                    }

                    if(isDebug) {
                        Debug.Log("Chose: " + turret.towerType);
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
                    t.turret.GetComponent<Turret>().init();
                    tilesWithTowers.Add(t);
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
