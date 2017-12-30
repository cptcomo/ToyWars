using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class GameManager : MonoBehaviour {
        private static GameManager instance = null;

        [SerializeField]
        private MapData mapData;

        [HideInInspector]
        public int enemiesAlive;

        public PlayerStats playerStats;

        public delegate void GameManagerEventHandler();
        public event GameManagerEventHandler MenuToggleEvent;
        public event GameManagerEventHandler RestartLevelEvent;
        public event GameManagerEventHandler GoToMenuSceneEvent;
        public event GameManagerEventHandler GameOverEvent;

        [HideInInspector]
        public bool isGameOver;
        [HideInInspector]
        public bool isMenuOn;

        private void Awake() {
            if(instance == null) {
                instance = this;
            }
            else if(instance != this) {
                Destroy(gameObject);
            }
        }

        private void Start() {
            Time.timeScale = 1;
            enemiesAlive = 0;
        }

        public static GameManager getInstance() {
            return instance;
        }

        public MapData getMapData() {
            return this.mapData;
        }

        public void callEventMenuToggle() {
            if(MenuToggleEvent != null) {
                MenuToggleEvent();
            }
        }

        public void callEventRestartLevel() {
            if(RestartLevelEvent != null) {
                RestartLevelEvent();
            }
        }

        public void callEventGoToMenuScene() {
            if(GoToMenuSceneEvent != null) {
                GoToMenuSceneEvent();
            }
        }

        public void callEventGameOver() {
            if(GameOverEvent != null) {
                isGameOver = true;
                GameOverEvent();
            }
        }
    }
}
