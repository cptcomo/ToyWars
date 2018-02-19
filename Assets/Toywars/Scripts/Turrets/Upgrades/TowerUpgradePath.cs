using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    [System.Serializable]
    public class TowerUpgradePath {
        private int leftIndex = 0, rightIndex = 0;
        public TowerUpgrade[] leftPath;
        public TowerUpgrade[] rightPath;

        GameManager gm;
        PlayerManager pm;

        public void init() {
            gm = GameManager.getInstance();
            pm = PlayerManager.getInstance();
        }

        public TowerUpgrade[] getAvailableUpgrades() {
            if(leftIndex > 2) {
                if(leftIndex != leftPath.Length) {
                    if(rightIndex >= 2)
                        return new TowerUpgrade[] { leftPath[leftIndex], null };
                    else return new TowerUpgrade[] { leftPath[leftIndex], rightPath[rightIndex] };
                } else {
                    if(rightIndex >= 2)
                        return new TowerUpgrade[] { null, null };
                    else
                        return new TowerUpgrade[] { null, rightPath[rightIndex] };
                }

            }
            if(rightIndex > 2) {
                if(rightIndex != rightPath.Length) {
                    if(leftIndex >= 2)
                        return new TowerUpgrade[] { null, rightPath[rightIndex] };
                    else
                        return new TowerUpgrade[] { leftPath[leftIndex], rightPath[rightIndex] };
                } else {
                    if(leftIndex >= 2)
                        return new TowerUpgrade[] { null, null };
                    else return new TowerUpgrade[] { leftPath[leftIndex], null };
                }
            }
            return new TowerUpgrade[] { leftPath[leftIndex], rightPath[rightIndex] };
        }

        public void upgrade(int upgradeIndex, Turret turret) {
            if(upgradeIndex == 0) {
                TowerUpgrade upgrade = leftPath[leftIndex];
                if(pm.money >= upgrade.cost) {
                    pm.money -= upgrade.cost;
                    leftIndex++;
                    upgrade.activate(turret);
                }
            } else {
                TowerUpgrade upgrade = rightPath[rightIndex];
                if(pm.money >= upgrade.cost) {
                    pm.money -= upgrade.cost;
                    rightIndex++;
                    upgrade.activate(turret);
                }
            }
        }
    }
}
