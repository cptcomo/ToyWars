using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class GameManager_LoseLives : MonoBehaviour {
        private GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();

            gm.LoseLivesEvent += loseLives;
        }

        private void OnDisable() {
            gm.LoseLivesEvent -= loseLives;
        }

        void loseLives(int lives) {
            gm.playerStats.lives -= lives;
            if(gm.playerStats.lives <= 0)
                gm.callEventGameOver(false);
        }
    }
}

