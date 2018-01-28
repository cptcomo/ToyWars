using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class GameManager : MonoBehaviour {
        private static GameManager instance = null;

        public delegate void GameManagerEventHandler();
        public event GameManagerEventHandler StartNextWaveEvent;
        public event GameManagerEventHandler EndWaveEvent;
        public event GameManagerEventHandler PauseToggleEvent;
        public event GameManagerEventHandler RestartLevelEvent;
        public event GameManagerEventHandler GoToMenuSceneEvent;
        public event GameManagerEventHandler GameOverEvent;

        [HideInInspector]
        public int waveIndex;

        [HideInInspector]
        public int playerMinionsAlive, enemyMinionsAlive;

        [HideInInspector]
        public enum GameState {
            Play, Build, Pause, GameOver
        }

        [HideInInspector]
        public enum GameResult {
            Win, Lose
        }

        [HideInInspector]
        public GameState gameState;

        [HideInInspector]
        public GameResult gameResult;

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
            waveIndex = 0;
            gameState = GameState.Build;
        }

        public static GameManager getInstance() {
            return instance;
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

        public bool isWin() {
            return gameResult == GameResult.Win;
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
            if(PauseToggleEvent != null) {
                PauseToggleEvent();
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

        public void callEventGameOver(bool win) {
            if(GameOverEvent != null) {
                if(win)
                    gameResult = GameResult.Win;
                else
                    gameResult = GameResult.Lose;

                GameOverEvent();
            }
        }
    }
}

