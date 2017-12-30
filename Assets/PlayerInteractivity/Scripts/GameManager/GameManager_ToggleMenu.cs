using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class GameManager_ToggleMenu : MonoBehaviour {
        private GameManager gm;
        public GameObject menu;

        private void OnEnable() {
            gm = GameManager.getInstance();
            gm.MenuToggleEvent += toggleMenu;
        }

        private void OnDisable() {
            gm.MenuToggleEvent -= toggleMenu;
        }

        private void Update() {
            checkForMenuToggleRequest();
        }

        void checkForMenuToggleRequest() {
            if(Input.GetKeyUp(KeyCode.Escape) && !gm.isGameOver) {
                gm.callEventMenuToggle();
            }
        }

        void toggleMenu() {
            if(menu != null) {
                menu.SetActive(!menu.activeSelf);
                gm.isMenuOn = !gm.isMenuOn;
            }
            else {
                Debug.LogWarning("Menu not referenced in GameManager_ToggleMenu script");
            }
        }
    }
}
