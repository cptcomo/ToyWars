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

        public delegate void NodeSelectHandler(Node node);
        public event NodeSelectHandler SelectNodeEvent;

        public delegate void NodeDeselectHandler();
        public event NodeDeselectHandler DeselectNodeEvent;

        public delegate void UpgradeTurretHandler(int upgradeIndex);
        public event UpgradeTurretHandler UpgradeTurretEvent;

        public delegate void PlayerAbilityUpgradeHandler();
        public event PlayerAbilityUpgradeHandler ShowAbilityUpgradeEvent, HideAbilityUpgradeEvent;

        public delegate void UpgradePlayerHandler(int upgradeIndex);
        public event UpgradePlayerHandler UpgradePlayerEvent;

        public delegate void TogglePlayerUIHandler();
        public event TogglePlayerUIHandler TogglePlayerUIEvent;

        public delegate void LoseLifeHandler(int livesLost);
        public event LoseLifeHandler LoseLivesEvent;

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
            enemiesAlive = 0;
            playerStats.init();
            gameState = GameState.Build;
        }

        private void Update() {
            //Debug.Log(gameState);
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

        public void callEventGameOver(bool win) {
            if(GameOverEvent != null) {
                if(win)
                    gameResult = GameResult.Win;
                else
                    gameResult = GameResult.Lose;

                GameOverEvent();
            }
        }

        public void callEventSelectNode(Node node) {
            if(SelectNodeEvent != null) {
                SelectNodeEvent(node);
            }
        }

        public void callEventDeselectNode() {
            if(DeselectNodeEvent != null) {
                DeselectNodeEvent();
            }
        }

        public void callEventUpgradeTurret(int upgradeIndex) {
            if(UpgradeTurretEvent != null) {
                UpgradeTurretEvent(upgradeIndex);
            }
        }

        public void callEventShowAbilityUpgrade() {
            if(ShowAbilityUpgradeEvent != null) {
                ShowAbilityUpgradeEvent();
            }
        }

        public void callEventHideAbilityUpgrade() {
            if(HideAbilityUpgradeEvent != null) {
                HideAbilityUpgradeEvent();
            }
        }

        public void callEventUpgradePlayer(int upgradeIndex) {
            if(UpgradePlayerEvent != null) {
                UpgradePlayerEvent(upgradeIndex);
            }
        }

        public void callEventTogglePlayerUI() {
            if(TogglePlayerUIEvent != null) {
                TogglePlayerUIEvent();
            }
        }

        public void callEventLoseLives(int livesLost) {
            if(LoseLivesEvent != null) {
                LoseLivesEvent(livesLost);
            }
        }
    }
}
