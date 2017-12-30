using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class WaveSpawnerManager : MonoBehaviour {
        //public Text countdownText;
        public GameObject[] spawnPoints;
        private WaveSpawner[] spawners;

        public float timeBetweenWaves = 2f;

        private float countdown = 2f;

        private int waveIndex = 0;

        private GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();
            waveIndex = 0;
            spawners = new WaveSpawner[spawnPoints.Length];
            for(int i = 0; i < spawners.Length; i++) {
                spawners[i] = spawnPoints[i].GetComponent<WaveSpawner>();
            }
            gm.GameOverEvent += disableScript;
        }

        private void OnDisable() {
            gm.GameOverEvent -= disableScript;
        }

        private void Update() {
            if(gm.enemiesAlive > 0)
                return;

            if(gameOver()) {
                gm.callEventGameOver();
                return;
            }

            if(countdown <= 0f) {
                nextWave();
                countdown = timeBetweenWaves;
                return;
            }
            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0, Mathf.Infinity);
            //countdownText.text = string.Format("{0:00.00}", countdown);
        }

        void nextWave() {
            foreach(WaveSpawner spawner in spawners) {
                spawner.spawnWave(waveIndex);
            }
            waveIndex++;
        }

        bool gameOver() {
            foreach(WaveSpawner spawner in spawners) {
                if(!spawner.outOfWaves())
                    return false;
            }
            return true;
        }

        void disableScript() {
            this.enabled = false;
        }
    }
}