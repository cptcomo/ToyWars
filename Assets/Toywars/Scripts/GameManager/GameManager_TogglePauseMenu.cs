using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class GameManager_TogglePauseMenu : MonoBehaviour {

        private GameManager gm;
        public GameObject menu;

        private GameManager.GameState lastStateBeforePause;

        private void OnEnable() {
            gm = GameManager.getInstance();
            gm.PauseToggleEvent += toggleMenu;
        }

        private void OnDisable() {
            gm.PauseToggleEvent -= toggleMenu;
        }

        private void Update() {
            checkForMenuToggleRequest();
        }

        void checkForMenuToggleRequest() {
            if(Input.GetKeyUp(KeyCode.Escape) && !gm.isGameOver()) {
                gm.callEventMenuToggle();
            }
        }

        void toggleMenu() {
            if(menu != null) {
                menu.SetActive(!menu.activeSelf);
                if(gm.isPaused()) {
                    gm.gameState = lastStateBeforePause;
                } else {
                    lastStateBeforePause = gm.gameState;
                    gm.gameState = GameManager.GameState.Pause;
                }
            } else {
                Debug.LogWarning("Menu not referenced in GameManager_TogglePauseMenu script");
            }
        }
    }

}
