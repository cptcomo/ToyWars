using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    [System.Serializable]
    public class TowerUpgradePath {
        private int leftIndex = 0, rightIndex = 0;
        public TowerUpgrade[] leftPath;
        public TowerUpgrade[] rightPath;

        GameManager gm;

        public void init() {
            gm = GameManager.getInstance();
        }

        public TowerUpgrade[] getAvailableUpgrades() {
            if(leftIndex > 2) {
                if(leftIndex != leftPath.Length)
                    return new TowerUpgrade[] { leftPath[leftIndex], null };
                else return new TowerUpgrade[] { null, null };
            } else if(rightIndex > 2) {
                if(rightIndex != rightPath.Length)
                    return new TowerUpgrade[] { null, rightPath[rightIndex] };
                else return new TowerUpgrade[] { null, null };
            } else return new TowerUpgrade[] { leftPath[leftIndex], rightPath[rightIndex] }; 
        }

        public void upgrade(int upgradeIndex, Turret turret) {
            if(upgradeIndex == 0) {
                TowerUpgrade upgrade = leftPath[leftIndex];
                if(gm.playerStats.money >= upgrade.cost) {
                    gm.playerStats.money -= upgrade.cost;
                    leftIndex++;
                    upgrade.activate(turret);
                }              
            }
            else {
                TowerUpgrade upgrade = rightPath[rightIndex];
                if(gm.playerStats.money >= upgrade.cost) {
                    gm.playerStats.money -= upgrade.cost;
                    rightIndex++;
                    upgrade.activate(turret);
                }               
            }
        }
    }
}
