using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class WaveSpawner : MonoBehaviour {
        public Wave[] waves;

        private int waveIndex;

        GameManager gm;

        [HideInInspector]
        public bool doneWithWave;

        private void Start() {
            gm = GameManager.getInstance();
            waveIndex = 0;
        }

        public void spawnWave(int waveIndex) {
            if(this.waveIndex != waveIndex) {
                Debug.Log("Manager and " + this.transform.name + " had a waveIndex desynchronization issue - Manager:" + waveIndex + "vs: " + this.waveIndex);
                this.waveIndex = waveIndex;
            }

            StartCoroutine(spawnNextWave());
        }

        IEnumerator spawnNextWave() {
            doneWithWave = false;
            Wave wave = waves[waveIndex];
            for(int i = 0; i < wave.sections.Length; i++) {
                yield return StartCoroutine(spawnWaveSection(wave.sections[i]));
            }
            doneWithWave = true;
            waveIndex++;
        }

        IEnumerator spawnWaveSection(WaveSection section) {
            for(int i = 0; i < section.count; i++) {
                spawnEnemy(section.enemy);
                yield return new WaitForSeconds(1 / section.rate);
            }
        }

        void spawnEnemy(GameObject enemy) {
            Instantiate(enemy, this.transform.position, this.transform.rotation);
            gm.enemiesAlive++;
        }

        public bool outOfWaves() {
            return this.waveIndex == waves.Length;
        }
    }
}
