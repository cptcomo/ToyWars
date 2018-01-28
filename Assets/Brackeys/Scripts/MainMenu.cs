using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brackey {
    public class MainMenu : MonoBehaviour {
        public string levelToLoad = "Game";
        public void play() {
            SceneManager.LoadScene(levelToLoad);
        }

        public void quit() {
            Application.Quit();
        }
    }
}
