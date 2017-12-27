using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brackeys {
    public class PauseMenu : MonoBehaviour {
        public GameObject pauseUI;
        private void Update() {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
                toggle();
            }
        }

        public void toggle() {
            pauseUI.SetActive(!pauseUI.activeSelf);
            if(pauseUI.activeSelf)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }

        public void restart() {
            toggle();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void menu() {
            SceneManager.LoadScene(0);
        }
    }
}

