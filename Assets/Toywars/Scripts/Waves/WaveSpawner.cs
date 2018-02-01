using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class WaveSpawner : MonoBehaviour {
        public bool isEnemy;

        public Wave wave;

        GameManager gm;
        PlayerManager pm;
        EnemiesManager em;

        [HideInInspector]
        public bool doneWithWave;

        private void Start() {
            gm = GameManager.getInstance();
            pm = PlayerManager.getInstance();
            em = EnemiesManager.getInstance();
        }

        public void spawnWave() {
            StartCoroutine(spawnNextWave());
        }

        IEnumerator spawnNextWave() {
            doneWithWave = false;
            for(int i = 0; i < wave.sections.Count; i++) {
                yield return StartCoroutine(spawnWaveSection(wave.sections[i]));
            }
            doneWithWave = true;
        }

        IEnumerator spawnWaveSection(WaveSection section) {
            for(int i = 0; i < section.count; i++) {
                spawnEnemy(section.enemy);
                yield return new WaitForSeconds(1 / section.rate);
            }
        }

        void spawnEnemy(GameObject enemy) {
            Instantiate(enemy, this.transform.position, this.transform.rotation);
            if(isEnemy)
                em.enemiesAlive++;
            else
                pm.alliesAlive++;
        }
    }
}

