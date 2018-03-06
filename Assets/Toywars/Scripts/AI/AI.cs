using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class AI : MonoBehaviour {
        GameManager gm;
        EnemiesManager em;

        public GameObject enemySide;
        AITile[] tiles;

        public TurretBlueprint standardTurretBlueprint;
        public TurretBlueprint missileLauncherBlueprint;
        public TurretBlueprint laserBeamerBlueprint;

        private void Start() {
            gm = GameManager.getInstance();
            em = EnemiesManager.getInstance();
            gm.AIStartTurnEvent += takeTurn;
            initReferences();
        }

        void initReferences() {
            tiles = enemySide.GetComponentsInChildren<AITile>();
        }

        private void OnDisable() {
            gm.AIStartTurnEvent -= takeTurn;
        }

        void takeTurn() {
            gm.gameState = GameManager.GameState.AI;
            if(em.money >= standardTurretBlueprint.cost) {
                tiles[624].buildTurret(standardTurretBlueprint);
                tiles[624].turret.GetComponent<Turret>().init();
                tiles[624].upgradeTurret(0);
                tiles[624].upgradeTurret(0);
                tiles[624].upgradeTurret(0);
                tiles[624].upgradeTurret(0);
                tiles[624].upgradeTurret(1);
                tiles[624].upgradeTurret(1);
            }
        }
    }
}
