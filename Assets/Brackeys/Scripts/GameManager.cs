using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class GameManager : MonoBehaviour {
        public static bool gameEnded;
        public GameObject gameOverUI;

        private void Start() {
            gameEnded = false;
        }

        private void Update() {
            if(gameEnded)
                return;

            if(PlayerStats.lives <= 0) {
                endGame();
            }
        }

        void endGame() {
            gameOverUI.SetActive(true);
            gameEnded = true;
        }
    }
}

