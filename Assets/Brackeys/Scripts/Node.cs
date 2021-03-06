﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Brackeys {
    public class Node : MonoBehaviour {
        public Color hoverColor;
        public Color notEnoughMoneyColor;
        public Vector3 posOffset;

        [HideInInspector]
        public GameObject turret;
        [HideInInspector]
        public TurretBlueprint turretBlueprint;
        [HideInInspector]
        public bool isUpgraded;

        private Renderer rend;
        private Color startColor;

        private BuildManager buildManager;

        private void Start() {
            this.isUpgraded = false;
            rend = GetComponent<Renderer>();
            startColor = rend.material.color;
            buildManager = BuildManager.instance;
        }

        private void OnMouseEnter() {
            if(EventSystem.current.IsPointerOverGameObject())
                return;

            if(!buildManager.canBuild)
                return;

            if(buildManager.hasMoney)
                rend.material.color = hoverColor;
            else
                rend.material.color = notEnoughMoneyColor;
        }

        private void OnMouseExit() {
            rend.material.color = startColor;
        }

        private void OnMouseDown() {
            if(EventSystem.current.IsPointerOverGameObject())
                return;

            if(turret != null) {
                buildManager.selectNode(this);
                return;
            }

            if(!buildManager.canBuild)
                return;

            buildTurret(buildManager.getTurretToBuild());
        }

        void buildTurret(TurretBlueprint blueprint) {
            if(PlayerStats.money < blueprint.cost) {
                Debug.Log("Not enough money");
                return;
            }

            GameObject turret = (GameObject)Instantiate(blueprint.prefab, getBuildPosition(), Quaternion.identity);
            this.turret = turret;

            GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, getBuildPosition(), Quaternion.identity);
            Destroy(effect, 5);

            this.turretBlueprint = blueprint;
            this.isUpgraded = false;
            PlayerStats.money -= blueprint.cost;
        }

        public void upgradeTurret() {
            if(PlayerStats.money < turretBlueprint.upgradeCost) {
                Debug.Log("Not enough money to upgrade");
                return;
            }

            Destroy(this.turret); //Get rid of old turret

            GameObject turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, getBuildPosition(), Quaternion.identity);
            this.turret = turret;

            GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, getBuildPosition(), Quaternion.identity);
            Destroy(effect, 5f);

            isUpgraded = true;

            PlayerStats.money -= turretBlueprint.upgradeCost;
        }

        public void sellTurret() {
            PlayerStats.money += turretBlueprint.getSellAmount();
            GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, getBuildPosition(), Quaternion.identity);
            Destroy(effect, 5f);
            Destroy(turret);
            turretBlueprint = null;
        }

        public Vector3 getBuildPosition() {
            return transform.position + posOffset;
        }
    }
}
