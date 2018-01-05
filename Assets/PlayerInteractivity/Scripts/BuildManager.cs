using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class BuildManager : MonoBehaviour {
        private static BuildManager instance;
        private GameManager gm;

        public GameObject buildEffect;

        private void Awake() {
            if(instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start() {
            gm = GameManager.getInstance();
        }

        public static BuildManager getInstance() {
            return instance;
        }

        private TurretBlueprint turretToBuild;
        private Node selectedNode;

        public bool canBuild { get { return turretToBuild != null; } }
        public bool hasMoney { get { return gm.playerStats.money >= turretToBuild.cost; } }

        public void selectTurretToBuild(TurretBlueprint turret) {
            turretToBuild = turret;
            deselectNode();
        }

        public TurretBlueprint getTurretToBuild() {
            return turretToBuild;
        }

        public void selectNode(Node node) {
            if(selectedNode == node) {
                deselectNode();
                return;
            }
            selectedNode = node;
            turretToBuild = null;
        }

        public void deselectNode() {
            selectedNode = null;
        }
    }
}

