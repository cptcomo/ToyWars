using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brackeys {
    public class NodeUI : MonoBehaviour {
        private Node target;
        public GameObject ui;
        public Text upgradeCost;
        public Text sellAmount;
        public Button upgradeButton;

        public void setTarget(Node target) {
            this.target = target;
            transform.position = target.getBuildPosition();

            if(!target.isUpgraded) {   
                upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
                upgradeButton.interactable = true;
            }
            else {
                upgradeCost.text = "DONE";
                upgradeButton.interactable = false;
            }

            sellAmount.text = "$" + target.turretBlueprint.getSellAmount();

            ui.SetActive(true);
        }

        public void hide() {
            ui.SetActive(false);
        }

        public void upgrade() {
            target.upgradeTurret();
            BuildManager.instance.deselectNode();
        }

        public void sell() {
            target.sellTurret();
            BuildManager.instance.deselectNode();
        }
    }
}

