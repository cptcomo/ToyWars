using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class GameManager_GameOver : MonoBehaviour {
        private GameManager gm;

        [SerializeField]
        private GameObject panelGameOver;

        private void OnEnable() {
            gm = GameManager.getInstance();
            gm.GameOverEvent += turnOnGameOverPanel;
        }

        private void OnDisable() {
            gm.GameOverEvent -= turnOnGameOverPanel;
        }

        void turnOnGameOverPanel() {
            if(panelGameOver != null) {
                panelGameOver.SetActive(true);
            } else Debug.LogWarning("Please make a reference to a Game Over UI panel in the GameOver script");
        }
    }
}
