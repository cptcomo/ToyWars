using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerInteractivity {
    public class UpgradeUI : MonoBehaviour {
        public GameObject playerUI;
        public GameObject upgradeUI;
        public GameObject upgradeButtonPrefab;
        public GameObject playerGO;

        private Player player;

        [Header("Upgrade Button Prefab Settings")]
        [SerializeField]
        private string upgradeNameText = "UpgradeName";
        [SerializeField]
        private string upgradeCostText = "UpgradeCost";

        private GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();
            player = playerGO.GetComponent<Player>();
            gm.ShowAbilityUpgradeEvent += show;
            gm.EndWaveEvent += gm.callEventShowAbilityUpgrade;
            gm.StartNextWaveEvent += gm.callEventHideAbilityUpgrade;
            gm.HideAbilityUpgradeEvent += hide;
            show();
        }

        private void OnDisable() {
            gm.ShowAbilityUpgradeEvent -= show;
            gm.EndWaveEvent -= gm.callEventShowAbilityUpgrade;
            gm.StartNextWaveEvent -= gm.callEventHideAbilityUpgrade;
            gm.HideAbilityUpgradeEvent -= hide;
        }

        void show() {
            playerUI.SetActive(true);
            updateUI();
        }

        void updateUI() {
            destroyAllChildren(upgradeUI.transform);
            AbilityUpgrade[] upgrades = player.abilityUpgradePath.getAvailableUpgrades();
            for(int i = 0; i < upgrades.Length; i++) {
                GameObject newButton;
                if(upgrades[i] != null) {
                    newButton = Instantiate(upgradeButtonPrefab);
                    List<GameObject> buttonChildren = new List<GameObject>();
                    foreach(Transform child in newButton.transform) buttonChildren.Add(child.gameObject);
                    buttonChildren.ForEach(child => {
                        if(child.name.Equals(upgradeNameText)) {
                            child.GetComponent<Text>().text = upgrades[i].upgradeName;
                        }

                        if(child.name.Equals(upgradeCostText))
                            child.GetComponent<Text>().text = "$" + upgrades[i].cost;
                    });
                    int index = i;
                    newButton.GetComponent<Button>().onClick.AddListener(delegate () { onButtonUpgradeClick(index); });
                }
                else {
                    newButton = Instantiate(upgradeButtonPrefab);
                    List<GameObject> buttonChildren = new List<GameObject>();
                    foreach(Transform child in newButton.transform) buttonChildren.Add(child.gameObject);
                    buttonChildren.ForEach(child => {
                        if(child.name.Equals(upgradeNameText)) {
                            child.GetComponent<Text>().text = "UPGRADE COMPLETED";
                        }
                        if(child.name.Equals(upgradeCostText))
                            child.GetComponent<Text>().text = "";
                    });
                }
                
                newButton.transform.SetParent(upgradeUI.transform, false); 
            } 
        }

        void onButtonUpgradeClick(int i) {
            gm.callEventUpgradePlayer(i);
        }

        void hide() {
            playerUI.SetActive(false);
        }

        void destroyAllChildren(Transform t) {
            List<GameObject> children = new List<GameObject>();
            foreach(Transform child in t) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }
    }
}
