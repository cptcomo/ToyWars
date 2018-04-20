using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class LivesUI : MonoBehaviour {
        public Text livesText;

        PlayerManager pm;
        EnemiesManager em;

        private void Start() {
            pm = PlayerManager.getInstance();
            em = EnemiesManager.getInstance();
        }

        private void Update() {
            livesText.text = "Lives: " + pm.baseHealth + " vs " + em.baseHealth;
        }
    }
}

