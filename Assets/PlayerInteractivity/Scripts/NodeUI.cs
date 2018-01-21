using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerInteractivity {
    public class NodeUI : MonoBehaviour {
        public GameObject nodeUI;
        public GameObject upgradeUI;
        public GameObject upgradeButtonPrefab;

        [Header("Upgrade Button Prefab Settings")]
        [SerializeField]
        private string upgradeNameText = "UpgradeName";
        [SerializeField]
        private string upgradeCostText = "UpgradeCost";

        private GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();
            gm.StartNextWaveEvent += gm.callEventDeselectNode;
            gm.SelectNodeEvent += show;
            gm.DeselectNodeEvent += hide;
        }

        private void OnDisable() {
            gm.SelectNodeEvent -= show;
            gm.DeselectNodeEvent -= hide;
            gm.StartNextWaveEvent -= gm.callEventDeselectNode;
        }
        void show(Node node) {
            gm.callEventHideAbilityUpgrade();
            nodeUI.SetActive(true);
            updateUI(node);
        }

        void updateUI(Node node) {
            destroyAllChildren(upgradeUI.transform);
            Turret turret = node.turret.GetComponent<Turret>();
            TowerUpgrade[] upgrades = turret.towerUpgradePath.getAvailableUpgrades();
            for(int i = 0; i < upgrades.Length; i++) {
                GameObject newButton = Instantiate(upgradeButtonPrefab);
                List<GameObject> buttonChildren = new List<GameObject>();
                foreach(Transform child in newButton.transform) buttonChildren.Add(child.gameObject);
                buttonChildren.ForEach(child => {
                    if(child.name.Equals(upgradeNameText))
                        child.GetComponent<Text>().text = upgrades[i].upgradeName;
                    if(child.name.Equals(upgradeCostText))
                        child.GetComponent<Text>().text = "$" + upgrades[i].cost;
                });
                int index = i;
                newButton.GetComponent<Button>().onClick.AddListener(delegate() { onButtonUpgradeClick(index); });
                newButton.transform.SetParent(upgradeUI.transform, false);
            }
        }

        void onButtonUpgradeClick(int i) {
            gm.callEventUpgradeTurret(i);
        }

        void hide() {
            nodeUI.SetActive(false);
        }

        void destroyAllChildren(Transform t) {
            List<GameObject> children = new List<GameObject>();
            foreach(Transform child in t) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }
    }
}
