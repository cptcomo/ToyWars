using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerInteractivity {
    public class GameManager_GameOver : MonoBehaviour {
        private GameManager gm;

        [SerializeField]
        private GameObject panelGameOver;

        public Text resultText;
        public Text roundsText;

        private void OnEnable() {
            gm = GameManager.getInstance();
            gm.GameOverEvent += turnOnGameOverPanel;
            gm.GameOverEvent += updateState;
        }

        private void OnDisable() {
            gm.GameOverEvent -= turnOnGameOverPanel;
            gm.GameOverEvent -= updateState;
        }

        void updateState() {
            gm.gameState = GameManager.GameState.GameOver;
        }

        void turnOnGameOverPanel() {
            if(panelGameOver != null) {
                panelGameOver.SetActive(true);
            } else {
                Debug.LogWarning("Please make a reference to a Game Over UI panel in the GameOver script");
            }

            updateUI();
        }

        void updateUI() {
            if(gm.isWin()) {
                resultText.text = "You Win!";
                roundsText.text = "" + gm.waveIndex;
            } else {
                resultText.text = "You Lose!";
                roundsText.text = "" + (gm.waveIndex - 1);
            }
        }
    }
}
