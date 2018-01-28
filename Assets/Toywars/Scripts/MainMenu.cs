using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Toywars {
    public class MainMenu : MonoBehaviour {
        [SerializeField]
        private string levelToLoad;

        private void Start() {
            Time.timeScale = 1;
        }

        public void play() {
            SceneManager.LoadScene(levelToLoad);
        }

        public void quit() {
            Application.Quit();
        }
    }
}
