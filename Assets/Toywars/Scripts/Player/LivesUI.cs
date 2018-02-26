using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class LivesUI : MonoBehaviour {
        public Text livesText;

        PlayerManager pm;

        private void Start() {
            pm = PlayerManager.getInstance();
        }

        private void Update() {
            livesText.text = "Lives: " + pm.baseHealth;
        }
    }
}

