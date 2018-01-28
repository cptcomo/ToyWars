using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class WaveSpawnerManager : MonoBehaviour {
        public GameObject[] spawnPoints;
        private WaveSpawner[] spawners;
        private GameManager gm;

        private void OnDisable() {
            gm.GameOverEvent -= disableScript;
            gm.StartNextWaveEvent -= startNextWave;
            gm.EndWaveEvent -= endWave;
        }

        void startNextWave() {
            gm.gameState = GameManager.GameState.Play;
            foreach(WaveSpawner spawner in spawners) {
                spawner.spawnWave(gm.waveIndex);
            }
            gm.waveIndex++;
        }

        void endWave() {
            gm.gameState = GameManager.GameState.Build;
        }

        private void Start() {
            gm = GameManager.getInstance();
            gm.GameOverEvent += disableScript;
            gm.StartNextWaveEvent += startNextWave;
            gm.EndWaveEvent += endWave;

            spawners = new WaveSpawner[spawnPoints.Length];
            for(int i = 0; i < spawners.Length; i++) {
                spawners[i] = spawnPoints[i].GetComponent<WaveSpawner>();
            }
        }

        private void Update() {
            if(gm.gameState == GameManager.GameState.Build && gameOver()) {
                Debug.Log("1");
                gm.callEventGameOver(true);
                return;
            }

            if(gm.gameState == GameManager.GameState.Play && doneWithWave() && gm.enemiesAlive == 0) {
                gm.callEventEndWave();
                return;
            }
      
            if(gm.gameState == GameManager.GameState.Build && Input.GetKeyDown(KeyCode.S)) {
                gm.callEventStartNextWave();
                return;
            }
        }

        bool gameOver() {
            foreach(WaveSpawner spawner in spawners) {
                if(!spawner.outOfWaves())
                    return false;
            }
            return true;
        }

        bool doneWithWave() {
            foreach(WaveSpawner spawner in spawners) {
                if(!spawner.doneWithWave)
                    return false;
            }
            return true;
        }

        void disableScript() {
            this.enabled = false;
        }
    }
}