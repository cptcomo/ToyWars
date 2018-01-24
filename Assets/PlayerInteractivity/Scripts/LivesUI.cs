using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerInteractivity {
    public class LivesUI : MonoBehaviour {
        public Text livesText;

        GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();    
        }

        private void Update() {
            livesText.text = "" + gm.playerStats.lives;
        }
    }
}
