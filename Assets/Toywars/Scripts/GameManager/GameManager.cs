using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class GameManager : MonoBehaviour {
        private static GameManager instance = null;
        private EnemiesManager em;
        private PlayerManager pm;

        public GameObject ablazeEffectsContainer;

        public delegate void GameManagerEventHandler();
        public event GameManagerEventHandler StartNextWaveEvent;
        public event GameManagerEventHandler EndWaveEvent;
        public event GameManagerEventHandler PauseToggleEvent;
        public event GameManagerEventHandler RestartLevelEvent;
        public event GameManagerEventHandler GoToMenuSceneEvent;
        public event GameManagerEventHandler GameOverEvent;

        public delegate void PlayerDeathEventHandler(float deathTime);
        public event PlayerDeathEventHandler PlayerDeathEvent;

        public delegate void TileSelectHandler(Tile tile);
        public event TileSelectHandler SelectTileEvent;

        public delegate void TileDeselectHandler();
        public event TileDeselectHandler DeselectTileEvent;

        public delegate void UpgradeTurretHandler(int upgradeIndex);
        public event UpgradeTurretHandler UpgradeTurretEvent;

        public delegate void TogglePlayerUIHandler();
        public event TogglePlayerUIHandler TogglePlayerUIEvent;

        public delegate void PlayerAbilityUpgradeHandler();
        public event PlayerAbilityUpgradeHandler ShowAbilityUpgradeEvent, HideAbilityUpgradeEvent;

        public delegate void UpgradePlayerHandler(int upgradeIndex);
        public event UpgradePlayerHandler UpgradePlayerEvent;

        public delegate void AIStartTurnHandler();
        public event AIStartTurnHandler AIStartTurnEvent;

        public delegate void MinionManagementOpenHandler();
        public event MinionManagementOpenHandler MinionManagementOpenEvent;

        public delegate void MinionManagementCloseHandler();
        public event MinionManagementCloseHandler MinionManagementCloseEvent;

        public delegate void SlotAddHandler(GameObject slot);
        public event SlotAddHandler SlotAddEvent;

        public int minionsAlive
        {
            get {
                return em.enemiesAlive + pm.alliesAlive;
            }
        }

        [HideInInspector]
        public int waveIndex;

        [HideInInspector]
        public int playerMinionsAlive, enemyMinionsAlive;

        [HideInInspector]
        public enum GameState {
            Play, Build, MinionManagement, AI, Pause, GameOver
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
            pm = PlayerManager.getInstance();
            em = EnemiesManager.getInstance();
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

        public bool isMinionManagement() {
            return gameState == GameState.MinionManagement;
        }

        public bool isAITurn() {
            return gameState == GameState.AI;
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

        public void callEventPlayerDeath(float deathTime) {
            if(PlayerDeathEvent != null) {
                PlayerDeathEvent(deathTime);
            }
        }

        public void callEventSelectTile(Tile tile) {
            if(SelectTileEvent != null) {
                SelectTileEvent(tile);
            }
        }

        public void callEventDeselectTile() {
            if(DeselectTileEvent != null) {
                DeselectTileEvent();
            }
        }

        public void callEventUpgradeTurret(int upgradeIndex) {
            if(UpgradeTurretEvent != null) {
                UpgradeTurretEvent(upgradeIndex);
            }
        }

        public void callEventTogglePlayerUI() {
            if(TogglePlayerUIEvent != null) {
                TogglePlayerUIEvent();
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

        public void callEventAIStartTurn() {
            if(AIStartTurnEvent != null) {
                AIStartTurnEvent();
            }
        }

        public void callEventMinionManagementOpen() {
            if(MinionManagementOpenEvent != null) {
                MinionManagementOpenEvent();
            }
        }

        public void callEventMinionManagementClose() {
            if(MinionManagementCloseEvent != null) {
                MinionManagementCloseEvent();
            }
        }

        public void callEventSlotAdd(GameObject slot) {
            if(SlotAddEvent != null) {
                SlotAddEvent(slot);
            }
        }
    }
}

