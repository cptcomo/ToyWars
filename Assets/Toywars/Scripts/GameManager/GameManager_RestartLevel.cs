using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Toywars {
    public class GameManager_RestartLevel : MonoBehaviour {
        private GameManager gm;

        private void OnEnable() {
            gm = GameManager.getInstance();
            gm.RestartLevelEvent += restartLevel;
        }

        void restartLevel() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
