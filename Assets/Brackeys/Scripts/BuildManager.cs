using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class BuildManager : MonoBehaviour {
        public static BuildManager instance;
        public GameObject buildEffect;

        private void Awake() {
            if(instance != null) {
                Debug.LogError("More than one build manager in scene!");
                return;
            }
            instance = this;
        }

        private TurretBlueprint turretToBuild;
        private Node selectedNode;
        public NodeUI nodeUI;

        public bool canBuild { get { return turretToBuild != null; } }
        public bool hasMoney { get { return PlayerStats.money >= turretToBuild.cost; } }

        public void selectTurretToBuild(TurretBlueprint turret) {
            turretToBuild = turret;
            deselectNode();
        }

        public void selectNode(Node node) {
            if(selectedNode == node) {
                deselectNode();
                return;
            }
            selectedNode = node;
            turretToBuild = null;
            nodeUI.setTarget(node);
        }

        public void deselectNode() {
            selectedNode = null;
            nodeUI.hide();
        }

        public void buildTurretOn(Node node) {
            if(PlayerStats.money < turretToBuild.cost) {
                Debug.Log("Not enough money");
                return;
            }
        
            GameObject turret = (GameObject) Instantiate(turretToBuild.prefab, node.getBuildPosition(), Quaternion.identity);
            node.turret = turret;

            GameObject effect = (GameObject)Instantiate(buildEffect, node.getBuildPosition(), Quaternion.identity);
            Destroy(effect, 5);

            PlayerStats.money -= turretToBuild.cost;
            Debug.Log("Turret Build! Money left: " + PlayerStats.money);
        }
    }
}
