﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class BuildManager : MonoBehaviour {
        public static BuildManager instance;
        public GameObject buildEffect;
        public GameObject sellEffect;

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
            nodeUI.setTarget(node);
        }

        public void deselectNode() {
            selectedNode = null;
            nodeUI.hide();
        }
    }
}
