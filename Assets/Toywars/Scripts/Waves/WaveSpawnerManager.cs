using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class WaveSpawnerManager : MonoBehaviour {
        private static WaveSpawnerManager instance;
        public GameObject[] spawnPoints;
        private WaveSpawner[] spawners;
        private GameManager gm;
        private PlayerManager pm;
        private EnemiesManager em;

        private void Awake() {
            if(instance == null) {
                instance = this;
            } else if(instance != this) {
                Destroy(gameObject);
            }
        }

        private void Start() {
            gm = GameManager.getInstance();
            pm = PlayerManager.getInstance();
            em = EnemiesManager.getInstance();
            gm.GameOverEvent += disableScript;
            gm.StartNextWaveEvent += startNextWave;
            gm.EndWaveEvent += endWave;

            spawners = new WaveSpawner[spawnPoints.Length];
            for(int i = 0; i < spawners.Length; i++) {
                spawners[i] = spawnPoints[i].GetComponent<WaveSpawner>();
            }
        }

        private void OnDisable() {
            gm.GameOverEvent -= disableScript;
            gm.StartNextWaveEvent -= startNextWave;
            gm.EndWaveEvent -= endWave;
        }

        private void Update() {
            if(gm.gameState == GameManager.GameState.Play) {
                checkGameOver();
                checkEndWave();
            }

            if(gm.gameState == GameManager.GameState.Build && Input.GetKeyDown(KeyCode.S)) {
                gm.callEventAIStartTurn();
                return;
            }

            if(gm.gameState == GameManager.GameState.AI && Input.GetKeyDown(KeyCode.S)) {
                gm.callEventStartNextWave();
                return;
            }
        }

        public WaveSpawnerManager getInstance() {
            return instance;
        }

        void startNextWave() {
            gm.gameState = GameManager.GameState.Play;
            foreach(WaveSpawner spawner in spawners)
                spawner.spawnWave();
            gm.waveIndex++;
        }

        void endWave() {
            gm.gameState = GameManager.GameState.Build;
        }

        void checkGameOver() {
            if(em.baseHealth == 0) {
                gm.callEventGameOver(true);
                return;
            }

            if(pm.baseHealth == 0) {
                gm.callEventGameOver(false);
                return;
            }
        }

        void checkEndWave() {
            if(gm.minionsAlive > 0)
                return;

            foreach(WaveSpawner spawner in spawners)
                if(!spawner.doneWithWave)
                    return;

            gm.callEventEndWave();
        }

        void disableScript() {
            this.enabled = false;
        }
    }
}
