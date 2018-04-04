using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class WaveSpawner : MonoBehaviour {
        public bool isEnemy;
        public Vector3[] minionWaypoints;

        [HideInInspector]
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
                spawnMinion(section.minion);
                yield return new WaitForSeconds(1 / section.rate);
            }
        }

        void spawnMinion(GameObject minion) {
            GameObject obj = (GameObject)Instantiate(minion, this.transform.position, this.transform.rotation);
            obj.GetComponent<MinionMovement>().waypoints = minionWaypoints;
        }

        public Vector2 calculateScore(Dictionary<string, Vector2> minionPowerScores) {
            Vector2 scores = new Vector2(0, 0);
            foreach(WaveSection ws in wave.sections) {
                Minion m = ws.minion.GetComponent<Minion>();
                scores += minionPowerScores[m.minionName] * ws.count;
            }
            return scores; 
        }
    }
}

