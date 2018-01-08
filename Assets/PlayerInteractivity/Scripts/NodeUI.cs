using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class NodeUI : MonoBehaviour {
        public GameObject nodeUI;
        public GameObject upgradeUI;
        private GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();
            gm.SelectNodeEvent += show;
            gm.DeselectNodeEvent += hide;
        }

        private void OnDisable() {
            gm.SelectNodeEvent -= show;
            gm.DeselectNodeEvent -= hide;
        }

        void show(Node node) {
            nodeUI.SetActive(true);
            Turret turret = node.turret.GetComponent<Turret>();
            TowerUpgrade[] upgrades = turret.towerUpgradePath.getAvailableUpgrades();
            for(int i = 0; i < upgrades.Length; i++) {
                
            }
        }

        void hide() {
            nodeUI.SetActive(false);
            destroyAllChildren(upgradeUI.transform);
        }

        void destroyAllChildren(Transform t) {
            List<GameObject> children = new List<GameObject>();
            foreach(Transform child in transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }
    }
}
