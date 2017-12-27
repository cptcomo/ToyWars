using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brackeys {
    public class WaveSpawner : MonoBehaviour {
        public Wave[] waves;
        public Transform spawnPoint;
        public Text countdownText;

        public float timeBetweenWaves = 2f;
        private float countdown = 2f;
        private int waveIndex = 0;

        public static int enemiesAlive = 0;

        private void Update() {
            if(enemiesAlive > 0)
                return;

            if(countdown <= 0f) {
                StartCoroutine(spawnWave());
                countdown = timeBetweenWaves;
                return;
            }
            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0, Mathf.Infinity);
            countdownText.text = string.Format("{0:00.00}", countdown);
        }

        IEnumerator spawnWave() {
            PlayerStats.rounds++;
            Wave wave = waves[waveIndex];
            for(int i = 0; i < wave.count; i++) {
                spawnEnemy(wave.enemy);
                yield return new WaitForSeconds(1 / wave.rate);
            }
            waveIndex++;
            if(waveIndex == waves.Length) {
                Debug.Log("LEVEL WON!");
                this.enabled = false;
            }
        }
        
        void spawnEnemy(GameObject enemy) {
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
            enemiesAlive++;
        }
    }
}
