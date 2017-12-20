using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class GameManager : MonoBehaviour {
        private bool gameEnded;

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
            Debug.Log("Game end");
            gameEnded = true;
        }
    }
}

