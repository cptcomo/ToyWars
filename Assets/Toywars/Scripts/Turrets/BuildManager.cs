using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class BuildManager : MonoBehaviour {
        private static BuildManager instance;
        private GameManager gm;
        private PlayerManager pm;
        public GameObject buildEffect;

        private void Awake() {
            if(instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start() {
            gm = GameManager.getInstance();
            pm = PlayerManager.getInstance();
            gm.SelectTileEvent += selectTile;
            gm.DeselectTileEvent += deselectTile;
           
        }

        private void OnDisable() {
            gm.SelectTileEvent -= selectTile;
            gm.DeselectTileEvent -= deselectTile;
        }

        public static BuildManager getInstance() {
            return instance;
        }

        private TurretBlueprint turretToBuild;
        private Tile selectedTile;

        public bool canBuild { get { return turretToBuild != null; } }
        public bool hasMoney { get { return pm.money >= turretToBuild.cost; } }

        public void selectTurretToBuild(TurretBlueprint turret) {
            turretToBuild = turret;
            gm.callEventDeselectTile();
        }

        public TurretBlueprint getTurretToBuild() {
            return turretToBuild;
        }

        void selectTile(Tile tile) {
            selectedTile = tile;
            turretToBuild = null;
        }

        void deselectTile() {
            selectedTile = null;
        }
    }
}
