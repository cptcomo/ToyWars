using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class GameManager : MonoBehaviour {
        private static GameManager instance = null;

        [SerializeField]
        private MapData mapData;

        [HideInInspector]
        public int waveIndex;

        [HideInInspector]
        public int enemiesAlive;

        public PlayerStats playerStats;

        public delegate void GameManagerEventHandler();
        public event GameManagerEventHandler StartNextWaveEvent;
        public event GameManagerEventHandler EndWaveEvent;
        public event GameManagerEventHandler MenuToggleEvent;
        public event GameManagerEventHandler RestartLevelEvent;
        public event GameManagerEventHandler GoToMenuSceneEvent;
        public event GameManagerEventHandler GameOverEvent;

        [HideInInspector]
        public enum GameState {
            Play, Build, Pause, GameOver
        }

        [HideInInspector]
        public GameState gameState;

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
            gameState = GameState.Build;
        }

        private void Update() {
            Debug.Log(gameState);
        }

        public static GameManager getInstance() {
            return instance;
        }

        public MapData getMapData() {
            return this.mapData;
        }

        public bool isPlaying() {
            return gameState == GameState.Play;
        }

        public bool isBuilding() {
            return gameState == GameState.Build;
        }

        public bool isPaused() {
            return gameState == GameState.Pause;
        }

        public bool isGameOver() {
            return gameState == GameState.GameOver;
        }

        public void callEventStartNextWave() {
            if(StartNextWaveEvent != null) {
                StartNextWaveEvent();
            }
        }

        public void callEventEndWave() {
            if(EndWaveEvent != null) {
                EndWaveEvent();
            }
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
                GameOverEvent();
            }
        }
    }
}
