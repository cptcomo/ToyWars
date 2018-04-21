using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class DeathTimerUI : MonoBehaviour {
        GameManager gm;

        public GameObject timerPanel;
        public Text timerText;

        bool isDead;
        float timer;

        private void Start() {
            gm = GameManager.getInstance();
            gm.PlayerDeathEvent += onDeath;
            gm.EndWaveEvent += onRevive;
        }

        private void OnDisable() {
            gm.PlayerDeathEvent -= onDeath;
            gm.EndWaveEvent -= onRevive;
        }

        void onDeath(float deathTimer) {
            isDead = true;
            timer = deathTimer;
            timerPanel.SetActive(true);
            Invoke("onRevive", deathTimer);
        }

        void onRevive() {
            isDead = false;
            timerPanel.SetActive(false);
        }

        private void Update() {
            if(isDead) {
                timerText.text = "Respawning in: " + Mathf.Round(timer) + "s";
                timer -= Time.deltaTime;
            }
        }
    }
}
