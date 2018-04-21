using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class TileUI : MonoBehaviour {
        public GameObject tileUI;
        public GameObject upgradeUI;
        public GameObject upgradeButtonPrefab;
        public Image towerSpriteImage;
        public Text towerDamageText;

        [Header("Upgrade Button Prefab Settings")]
        [SerializeField]
        string upgradeNameText = "UpgradeName";
        [SerializeField]
        string upgradeCostText = "UpgradeCost";

        private GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();
            gm.AIStartTurnEvent += gm.callEventDeselectTile;
            gm.ShowAbilityUpgradeEvent += hide;
            gm.SelectTileEvent += show;
            gm.DeselectTileEvent += hide;
        }

        private void OnDisable() {
            gm.AIStartTurnEvent -= gm.callEventDeselectTile;
            gm.ShowAbilityUpgradeEvent -= hide;
            gm.SelectTileEvent -= show;
            gm.DeselectTileEvent -= hide;
        }

        void show(Tile tile) {
            gm.callEventHideAbilityUpgrade();
            tileUI.SetActive(true);
            updateUI(tile);
        }

        void updateUI(Tile tile) {
            destroyAllChildren(upgradeUI.transform);
            Turret turret = tile.turret.GetComponent<Turret>();
            this.towerDamageText.text = "" + Mathf.Round(turret.damageDone);
            TowerUpgrade[] upgrades = turret.towerUpgradePath.getAvailableUpgrades();
            for(int i = 0; i < upgrades.Length; i++) {
                GameObject newButton = Instantiate(upgradeButtonPrefab);
                if(upgrades[i] != null) {
                    List<GameObject> buttonChildren = new List<GameObject>();
                    foreach(Transform child in newButton.transform)
                        buttonChildren.Add(child.gameObject);
                    buttonChildren.ForEach(child => {
                        if(child.name.Equals(upgradeNameText))
                            child.GetComponent<Text>().text = upgrades[i].upgradeName;
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
                            child.GetComponent<Text>().text = "UPGRADE NOT AVAILABLE";
                        }
                        if(child.name.Equals(upgradeCostText))
                            child.GetComponent<Text>().text = "";
                    });
                }
                newButton.transform.SetParent(upgradeUI.transform, false);
            }
            towerSpriteImage.sprite = turret.towerSprite;
        }

        void onButtonUpgradeClick(int i) {
            gm.callEventUpgradeTurret(i);
        }

        void hide() {
            tileUI.SetActive(false);
        }

        void destroyAllChildren(Transform t) {
            List<GameObject> children = new List<GameObject>();
            foreach(Transform child in t)
                children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }
    }
}
