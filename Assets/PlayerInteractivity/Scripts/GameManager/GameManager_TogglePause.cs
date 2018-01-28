using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class GameManager_TogglePause : MonoBehaviour {
        private GameManager gm;
        private bool isPaused;

        private void OnEnable() {
            gm = GameManager.getInstance();
            isPaused = false;
            gm.MenuToggleEvent += togglePause;
        }

        private void OnDisable() {
            gm.MenuToggleEvent -= togglePause;
        }

        void togglePause() {
            if(isPaused) {
                Time.timeScale = 1;
                isPaused = false;
            } else {
                Time.timeScale = 0;
                isPaused = true;
            } 
        }
    }
}
