﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Toywars {
    public class GameManager_GoToMenuScene : MonoBehaviour {
        private GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();
            gm.GoToMenuSceneEvent += goToMenuScene;
        }

        void goToMenuScene() {
            SceneManager.LoadScene(0);
        }
    }
}

