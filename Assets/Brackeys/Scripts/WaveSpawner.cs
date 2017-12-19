﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brackeys {
    public class WaveSpawner : MonoBehaviour {
        public Transform enemyPrefab;
        public Transform spawnPoint;
        public Text countdownText;

        public float timeBetweenWaves = 5f;
        private float countdown = 2f;
        private int waveIndex = 0;

        private void Update() {
            if(countdown <= 0f) {
                StartCoroutine(spawnWave());
                countdown = timeBetweenWaves;
            }
            countdown -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(countdown).ToString();
        }

        IEnumerator spawnWave() {
            waveIndex++;
            for(int i = 0; i < waveIndex; i++) {
                spawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }       
        }
        
        void spawnEnemy() {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
